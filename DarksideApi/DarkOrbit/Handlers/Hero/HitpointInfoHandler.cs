using DarksideApi.DarkOrbit.Commands.Hero;

namespace DarksideApi.DarkOrbit.Handlers.Hero
{
    internal class HitpointInfoHandler : ICommandHandler
    {
        private readonly Api api;

        public HitpointInfoHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            HitpointInfoCommand hitpointUpdate = new();
            hitpointUpdate.Read(@byte);

            this.api.Hero.Hitpoints = hitpointUpdate.hp;
            this.api.Hero.MaxHitpoints = hitpointUpdate.maxHp;
            this.api.Hero.NanoShield = hitpointUpdate.nanoShield;
            this.api.Hero.MaxNanoShield = hitpointUpdate.maxNanoShield;
        }
    }
}
