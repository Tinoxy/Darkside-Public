using DarksideApi.DarkOrbit.Commands.Client;
using DarksideApi.Logic;

namespace DarksideApi.DarkOrbit.Handlers.Client
{
    internal class ReloginHandler : ICommandHandler
    {
        private readonly Api api;

        public ReloginHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            ReloginCommand mapUpdate = new();
            mapUpdate.Read(@byte);

            this.api.Hero.IsJumping = false;
            //GameObjects.Hero.MapId = mapUpdate.mapId;
            //param1.var_5821 / 1000,class_571.execute,[new class_166("0|i|" + param1.mapID)];
            Task.Delay(mapUpdate.delayInMs).ContinueWith(_ =>
            {
                if (this.api.Logic.State == LogicState.Revive)
                {
                    this.api.Hero.NextMapId = mapUpdate.mapId;
                    this.api.Logic.SetState(this, LogicState.None);
                }
                if (this.api.GameClient.GameState >= GameState.Ready)
                {
                    return;
                }
                this.api.Logging.Log("Channel Close Requested");
                this.api.GameMethods.ChannelClose(true);
            });
        }
    }
}
