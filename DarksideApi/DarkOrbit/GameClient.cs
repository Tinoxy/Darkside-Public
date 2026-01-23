using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Handlers;
using DarksideApi.Logic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Timers;

namespace DarksideApi.DarkOrbit
{
    public class GameClient
    {
        public static int PORT = 5001;
        public static double PING_TIME => 15000;

        private readonly Api api;
        private readonly CommandHandler commandHandler;
        private readonly ByteArrayBuffer arrayBuffer;
        private Proxy? proxy;
        //private Socket socket;
        private TcpClient client;
        private DateTime lastPing;
        private DateTime lastPong;
        private DateTime lastPacket;
        private GameState gameState = GameState.Disconnected;
        private NetworkStream? stream;
        private System.Timers.Timer? timer;
        private int loginRetryCounter;

        public event EventHandler<GameState> GameStateChanged;

        public bool CanDisconnect { get; set; } = true;
        public bool PingAllowed { get; set; } = false;
        public string IpAdress { get; set; } = string.Empty;
        public int Port { get; set; } = PORT;
        public bool IsServerRestarting { get; set; } = false;
        public double PingTime => (this.LastPong - this.LastPing).TotalMilliseconds;
        public Dictionary<int, IPEndPoint> IPAddresses => this.api.BackpageApi.IpAdresses;
        public WebProxy? Proxy => this.proxy?.GetWebProxy();
        public GameState GameState
        {
            get => this.gameState;
            set
            {
                if (this.gameState != value)
                {
                    GameStateChanged?.Invoke(this, value);
                }
                this.gameState = value;
            }
        }
        public DateTime LastPing => this.lastPing;
        public DateTime LastPong { get => this.lastPong; set => this.lastPong = value; }

        public GameClient(Api api)
        {
            this.api = api;
            this.commandHandler = new CommandHandler(this.api);
            this.arrayBuffer = new();
            //socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.client = new()
            {
                NoDelay = true,
            };
            this.CanDisconnect = true;

            this.lastPing = DateTime.UtcNow;
            this.lastPong = DateTime.UtcNow;

            GameStateChanged += this.OnGameStateChanged;
        }

        public GameClient() { }

        public void Login(bool useSessionId = false)
        {
            this.ResetTimer();
            if (this.api.Settings.UseProxy && this.proxy == null)
            {
                var host = this.api.Settings.ProxyAddress.Split(':')[0];
                var port = int.Parse(this.api.Settings.ProxyAddress.Split(':')[0]);
                this.proxy = new(host, port, this.api.Settings.ProxyUser, this.api.Settings.ProxyPassword);
                this.api.BackpageApi.UpdateProxy(this.Proxy);
            }
            Task.Factory.StartNew(() =>
            {
                var flag = this.api.BackpageApi.IsSessionValid();
                this.GameState = GameState.LoggingIn;

                this.api.BackpageApi.UpdateUserAgent();

                if (!flag && (useSessionId ? this.api.BackpageApi.LoginSessionId(string.Empty) : this.api.BackpageApi.Login(CultureInfo.CurrentCulture.TwoLetterISOLanguageName)))
                {
                    this.api.WriteLog("Successfully logged in to Session: " + this.api.Hero.SessionId);
                    flag = true;
                }
                if (flag)
                {
                    Task.Factory.StartNew(() => this.api.BackpageApi.UpgradeSkylab());
                }
                if (flag && this.api.BackpageApi.GetServer())
                {
                    this.GameState = GameState.CollectingLoginBonus;
                    if (this.api.BackpageApi.CollectDailyLoginBonus())
                    {
                        this.api.WriteLog("Daily Login Bonus collected.");
                    }
                    return;
                }
            });
        }

        public void ChangeHangar(long hangarId)
        {
            this.api.BackpageApi.SwitchHangar(hangarId);
        }

        private void HandleData(ByteArray @byte)
        {
            this.lastPacket = DateTime.UtcNow;
            try
            {
                var id = @byte.ReadShort();
                this.commandHandler.Lookup(id).Handle(@byte);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                this.GameState = GameState.Disconnected;
            }
        }

        public void Connect(string ip, int port)
        {
            try
            {
                this.lastPacket = DateTime.UtcNow;
                if (this.proxy != null)
                {
                    this.client = this.proxy.ConnectToTarget(ip, port);
                }
                else
                {
                    this.client.Connect(ip, port);
                }

                if (this.client.Connected)
                {
                    this.stream = this.client.GetStream();
                    this.stream.WriteByte(0);
                    //socket.Send(new byte[1] { 0, });
                    this.api.GameMethods.VersionRequest();

                    //stream = new NetworkStream(this.socket);
                    Task.Factory.StartNew(this.Listen);
                }
                else
                {
                    this.GameState = GameState.Disconnected;
                }
            }
            catch (Exception x)
            {
                this.api.Logging.Log("X:" + x);
                this.GameState = GameState.Disconnected;
            }
        }

        private void Listen()
        {
            State state = new();
            try
            {
                //while (socket.Connected)
                while (this.client.Connected)
                {
                    this.Ping();

                    if ((DateTime.UtcNow - this.lastPacket).TotalSeconds > 60)
                    {
                        this.GameState = GameState.Disconnected;
                        throw new SocketException((int)SocketError.TimedOut, "Socket Response time is too high!");
                    }
                    if (this.PingTime > (PING_TIME * 2) && this.PingAllowed)
                    {
                        this.GameState = GameState.Disconnected;
                        throw new SocketException((int)SocketError.TimedOut, "Pong time too high!");
                    }
                    if (this.client.Available < 3)
                    //if (socket.Available < 3)
                    {
                        Thread.Sleep(1);
                        continue;
                    }

                    if (this.GameState < GameState.Loading || this.GameState == GameState.Reconnect)
                    {
                        this.GameState = GameState.Loading;
                        this.CanDisconnect = true;
                    }

                    this.stream!.Read(state.Buffer, 0, 3);
                    ByteArray headerArray = new(state.Buffer);
                    var length = (uint)(headerArray.ReadByte() << 16) | (headerArray.ReadUShort() & 65535u);
                    this.arrayBuffer.SetLength(length);

                    if (!this.arrayBuffer.IsReady)
                    {
                        continue;
                    }

                    var array = new byte[length];
                    var read = 0;
                    while (read < length)
                    {
                        read += this.stream!.Read(array, read, (int)(length - read));
                    }

                    this.HandleData(new ByteArray(array));
                    this.arrayBuffer.Reset();
                }
            }
            catch (Exception e)
            {
                if (e is SocketException s && s.ErrorCode == 10060)
                {
                    this.GameState = GameState.Disconnected;
                }
                this.api.Logging.Log("E:" + e);
            }
        }

        public void Reset()
        {
            try
            {
                this.Disconnect();
                this.api.GameObservers.SessionObserver.StopObserver();
                this.ResetSocket();
                this.ResetTimer();
            }
            catch (Exception x)
            {
                this.api.Logging.Log(x);
            }
        }

        private void ResetSocket()
        {
            /*socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                NoDelay = true,
                ExclusiveAddressUse = false,
            };*/
            this.client = new()
            {
                NoDelay = true,
            };
        }

        public void RestartTimer()
        {
            this.ResetTimer();
            this.timer!.Start();
            this.api.Logging.Log("Restarting Timer...");
        }

        public void StopTimer()
        {
            this.timer?.Stop();
            this.api.Logging.Log("Stopping Timer...");
        }

        private void ResetTimer()
        {
            this.timer?.Stop();
            this.timer = new System.Timers.Timer(TimeSpan.FromMinutes(this.IsServerRestarting ? 15 : 5));
            this.timer.Elapsed += this.LoginTimerExpired;
        }

        public void Disconnect(bool reconnect = false)
        {
            try
            {
                this.CanDisconnect = !reconnect;
                this.PingAllowed = false;
                //socket?.Close();
                this.client?.Close();
                this.stream?.Close();
                this.stream?.Dispose();
                this.timer?.Stop();
                this.api.GameObservers.GameTaskObserver.StopObserver();
                this.api.Movement.StopMovementUpdater();
            }
            catch (Exception)
            {
            }
        }

        public void Ping()
        {
            if (!this.PingAllowed)
            {
                return;
            }
            if ((DateTime.UtcNow - this.lastPing).TotalMilliseconds < PING_TIME)
            {
                return;
            }
            this.lastPing = DateTime.UtcNow;
            this.api.GameMethods.KeepAlive(false);
        }

        public void Send(Command command)
        {
            try
            {
                if (this.client == null || this.stream == null || (!this.client.Connected && this.GameState != GameState.Reconnect))
                //if (socket == null || (!socket.Connected && GameState != GameState.Reconnect))
                {
                    throw new Exception("Socket is non existent or not connected and state is unequal to reconnect!");
                }

                var commandBuffer = command.Write();
                var length = (short)commandBuffer.Length;

                ByteArray @byte = new();
                @byte.WriteByte((byte)((length & 16711680) >> 16));
                @byte.WriteShort((short)(length & 65535));
                @byte.WriteByteArray(commandBuffer);

                //socket.Send(@byte.ToByteArray());
                this.client.Client.Send(@byte.ToArray());
                Debug.WriteLine("Command: " + command);
            }
            catch (Exception x)
            {
                this.api.Logging.Log(x + "\n" + command);
                if (this.GameState < GameState.Ready)
                    return;

                this.GameState = GameState.Disconnected;
            }
        }

        private void LoginTimerExpired(object? sender, ElapsedEventArgs e)
        {
            if (this.loginRetryCounter > 3)
            {
                this.api.BackpageApi.IsSessionValid(true);
                this.loginRetryCounter = 0;
            }
            if (this.GameState < GameState.Ready)
            {
                this.loginRetryCounter += 1;
                this.CanDisconnect = true;
                if (this.api.Logic.State == LogicState.GotoSellOre)
                {
                    this.api.Logic.SetState(this, LogicState.None);
                }
                this.Login();
                this.api.WriteLog("Login took too long... Trying again!");
                return;
            }
        }

        private void OnGameStateChanged(object? sender, GameState e)
        {
            if (this.api.Logic.State == LogicState.GotoSellOre
                || this.api.Logic.State == LogicState.SellOreDone)
            {
                return;
            }
            this.api.Logging.Log("New GameState: " + e);
            if (e == GameState.LoggingIn)
            {
                this.api.WriteLog("Logging in...");
                this.Reset();
                this.timer?.Start();
                return;
            }
            if (e == GameState.CollectingLoginBonus)
            {
                this.api.WriteLog("Loading Map...");
                this.api.BackpageApi.GetInventory();
                this.api.GameObservers.SessionObserver.InitSessionObserver();
            }
            if (e < GameState.Loading && e > GameState.LoggingIn)
            {
                this.Connect(this.IpAdress!, this.Port);
                return;
            }
            if (e == GameState.Disconnected && this.CanDisconnect
                && (this.api.Settings.IsRunning || !this.timer.Enabled)
                && this.api.Logic.State != LogicState.Pausing
                && this.api.Logic.State != LogicState.PausingDeaths)
            {
                //api.AddLoot(Statistics.StatisticsConstants.Reconnect, 1);
                this.Login();
                return;
            }
            if (e == GameState.Disconnected
                && (this.api.Logic.State == LogicState.Pausing || this.api.Logic.State == LogicState.PausingDeaths))
            {
                this.Disconnect();
                return;
            }
            if (e >= GameState.Ready)
            {
                this.IsServerRestarting = false;
            }
            if (e == GameState.LoginFailed)
            {
                return;
            }
            if (e == GameState.Incompatible)
            {
                this.Disconnect();
                return;
            }
            if (e == GameState.Ready)
            {
                this.loginRetryCounter = 0;
                this.timer?.Stop();
                this.api.Settings.DisplayName = (this.api.Hero.ClanTag.Length > 0 ? $"[{this.api.Hero.ClanTag}] " : "") + this.api.Hero.Username;
                return;
            }
            if (e == GameState.Reconnect)
            {
                if (this.api.BackpageApi.IpAdresses.TryGetValue(this.api.Hero.NextMapId, out IPEndPoint? nextIp))
                {
                    this.api.Logging.Log("Reconnect to Map: " + CommandConstants.MAP_NAMES[this.api.Hero.NextMapId] + " (" + nextIp + ")");
                    this.IpAdress = nextIp.Address.ToString();
                    this.Port = nextIp.Port;
                    this.api.Hero.NextMapId = this.api.Map.Id;
                }
                this.Disconnect(true);
                this.ResetTimer();
                this.ResetSocket();
                this.timer?.Start();
                this.Connect(this.IpAdress, this.Port);
                return;
            }
            if (e == GameState.Loading)
            {
                this.api.Movement.StartMovementUpdater();
                this.api.GameObservers.GameTaskObserver.InitTaskObserver();
                this.api.WriteLog("Successfully connected to Game!");
                return;
            }
        }

        private class State
        {
            public byte[] Buffer { get; private set; }
            public int Length => this.Buffer.Length;

            public State()
            {
                this.Buffer = new byte[32768];
            }
        }
    }
}
