using DarksideApi.DarkOrbit.Commands.Client;
using DarksideApi.DarkOrbit.Objects.Client;

namespace DarksideApi.DarkOrbit.Handlers.Client
{
    public class ServerVersionCheckHandler : ICommandHandler
    {
        private readonly Api api;

        public ServerVersionCheckHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            ServerVersionCheck serverVersionCheck = new();
            serverVersionCheck.Read(@byte);

            if (serverVersionCheck.version != DarkOrbitProtoLib.Version.GameVersion)
            {
                this.api.GameClient.GameState = GameState.Incompatible;
                this.api.WriteLog("Game Version Incompatible!");
                this.api.Logging.Log("New Version: " + serverVersionCheck.version);
                return;
            }
            this.api.Logging.Log("Version compatible: " + serverVersionCheck.compatible);

            this.api.GameMethods.KeepAlive();
            this.api.GameMethods.GameLogin();

            this.api.GameMethods.CommandCenterHide();

            Task.Delay(10000).ContinueWith(_ => Skylab.UpgradeModules(this.api));
        }
    }
}
