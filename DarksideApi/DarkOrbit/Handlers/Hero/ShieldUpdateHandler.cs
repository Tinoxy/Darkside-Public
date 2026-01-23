using DarksideApi.DarkOrbit.Commands.Hero;

namespace DarksideApi.DarkOrbit.Handlers.Hero
{
    internal class ShieldUpdateHandler : ICommandHandler
    {
        private readonly Api api;

        public ShieldUpdateHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            ShieldUpdateCommand shieldUpdate = new();
            shieldUpdate.Read(@byte);

            this.api.Hero.Shield = shieldUpdate.shield;
            this.api.Hero.MaxShield = shieldUpdate.maxShield;
        }
    }
}
