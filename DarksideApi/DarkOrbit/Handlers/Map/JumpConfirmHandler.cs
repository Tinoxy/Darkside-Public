using DarksideApi.DarkOrbit.Commands.Map;

namespace DarksideApi.DarkOrbit.Handlers.Map
{
    internal class JumpConfirmHandler : ICommandHandler
    {
        private readonly Api api;

        public JumpConfirmHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            JumpInitiatedCommand jumpConfirm = new();
            jumpConfirm.Read(@byte);

            this.api.Hero.NextMapId = jumpConfirm.mapId;
            this.api.Hero.IsJumping = true;
            this.api.Hero.LastJump = DateTime.UtcNow;
            this.api.Cooldown.AddCooldown(DarkOrbit.Cooldown.CooldownType.CPU_SPECIALJUMP, 2);

            if (this.api.GameClient.IPAddresses.TryGetValue(this.api.Hero.NextMapId, out System.Net.IPEndPoint? nextIp)
                && (nextIp.Address.ToString() != this.api.GameClient.IpAdress || nextIp.Port != this.api.GameClient.Port))
            {
                this.api.Logging.Log("Need connect to next Map: " + this.api.Hero.NextMapId + " -> " + nextIp);
                Task.Delay(3200).ContinueWith(_ =>
                {
                    this.api.GameClient.GameState = GameState.Reconnect;
                });
                return;
            }
            this.api.Logging.Log("Next MapId: " + this.api.Hero.NextMapId + ", has no new Address: " + nextIp);

            //api.AddLootResource(Statistics.StatisticsConstants.Jump, 1);
        }
    }
}
