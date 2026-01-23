using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Commands.Client;
using DarksideApi.Logic;

namespace DarksideApi.DarkOrbit.Handlers.Client
{
    internal class LoginResponseHandler : ICommandHandler
    {
        private readonly Api api;

        public LoginResponseHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            LoginResponseCommand loginResponse = new();
            loginResponse.Read(@byte);

            this.api.Logging.Log("Received Status: " + loginResponse.status);

            switch (loginResponse.status)
            {
                case CommandConstants.STATUS_WRONGSERVER:
                    if (this.api.Logic.State == LogicState.SellOreDone || this.api.Logic.State == LogicState.GotoSellOre || this.api.Logic.State == LogicState.Pausing || this.api.Logic.State == LogicState.PausingDeaths
                        || this.api.Hero.IsJumping
                        || this.api.Hero.IsLoggingOut)
                    {
                        this.api.GameMethods.StatusSelect(0);
                        this.api.GameMethods.ChannelClose(false);
                        break;
                    }
                    this.api.Logging.Log("Received incorrect Server, reconnecting!");
                    this.api.GameClient.GameState = GameState.Disconnected;
                    break;
                case CommandConstants.STATUS_DEFAULT:
                    this.api.GameMethods.StatusSelect(0);
                    this.api.GameMethods.ChannelClose(false);
                    break;
                case CommandConstants.STATUS_INVALID_DATA:
                    this.api.GameClient.GameState = GameState.Disconnected;
                    this.api.WriteLog("Received invalid data, will retry shortly...");
                    break;
                case CommandConstants.STATUS_INVALID_SID:
                    this.api.WriteLog("Invalid SessionId!");
                    this.api.BackpageApi.IsSessionValid(true);
                    this.api.GameClient.GameState = GameState.Disconnected;
                    //LOG OUT-> RELOG REQUIRED
                    break;
                case CommandConstants.STATUS_SHIP_DESTROYED:
                    this.api.Logic.SetState(this, LogicState.Revive);
                    if (this.api.GameClient.GameState < GameState.Ready)
                    {
                        this.api.Hero.HasSpawnedDestroyed = true;
                    }
                    if (this.api.Settings.AutoStart && !this.api.Settings.IsRunning)
                    {
                        this.api.Logic.StartLogic();
                    }
                    break;
                case CommandConstants.STATUS_WRONG_INSTANCEID:
                    api.Logging.Log("Wrong InstanceId!");
                    this.api.BackpageApi.IsSessionValid(true);
                    this.api.GameClient.GameState = GameState.Disconnected;
                    //LOG OUT-> RELOG REQUIRED
                    break;
                default:
                    this.api.Logging.Log($"Unknown Login Response: {loginResponse.status}");
                    break;
            }
        }
    }
}
