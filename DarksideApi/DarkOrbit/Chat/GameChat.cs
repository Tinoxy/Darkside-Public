using DarksideApi.DarkOrbit.Chat.Commands;
using DarksideApi.DarkOrbit.Chat.Commands.Login;
using DarksideApi.DarkOrbit.Chat.Commands.LowLevel;
using DarksideApi.DarkOrbit.Chat.Commands.Messages;
using DarksideApi.DarkOrbit.Chat.Objects;
using System.Net.Sockets;

namespace DarksideApi.DarkOrbit.Chat
{
    public class GameChat
    {
        public string IP { get; private set; }
        public int Port { get; private set; }

        private readonly TcpClient chatClient;
        private NetworkStream? chatStream;
        private GameChatState state;
        private EventHandler<GameChatState> gameChatStateChanged;

        public GameChatState ChatState
        {
            get => this.state; set
            {
                if (this.state == value)
                    return;

                this.gameChatStateChanged?.Invoke(this, value);
                this.state = value;
            }
        }

        public GameChat(string ip, int port)
        {
            this.IP = ip;
            this.Port = port;

            this.chatClient = new();

            this.gameChatStateChanged += this.OnGameChatStateChanged;
        }

        private void OnGameChatStateChanged(object sender, GameChatState e)
        {
            if (e == GameChatState.Ready)
            {
                this.Send(new InitConnectionCommand());
                return;
            }
        }

        public void Disconnect()
        {
            this.chatClient?.Close();
            this.chatStream?.Dispose();
        }

        public void Send(Command command)
        {
            try
            {
                if (this.chatClient == null || !this.chatClient.Connected
                    || this.chatStream == null)
                {
                    this.ChatState = GameChatState.Disconnected;
                    return;
                }

                var commandBuffer = command.Write();
                var length = (short)commandBuffer.Length;

                ByteArray @byte = new();
                @byte.WriteByte((byte)((length & 16711680) >> 16));
                @byte.WriteShort((short)(length & 65535));
                @byte.WriteByteArray(commandBuffer);

                this.chatStream?.Write(@byte.ToArray());
                this.chatStream?.Flush();
            }
            catch (Exception ex)
            {
            }
        }

        private void Read(byte[] array)
        {
            object _loc4_ = new();
            ByteArray reader = new(array);
            while (reader.Length() > 0)
            {
                if ((_loc4_ = ProtocolCoder.Decode(reader)) == null)
                {
                    break;
                }
                if (_loc4_ is IntroductionMessageCommand)
                {
                    this.ChatState = GameChatState.Ready;
                }
                else if (_loc4_ is PongMessageCommand)
                {
                    this.Send(new PongMessageCommand(0L));
                }
                else if (_loc4_ is StringMessageCommand)
                {
                    //this.settings.Handler.onReceiveFromServer(_loc4_ as APlayStringMessage);
                }
            }
        }
    }
}
