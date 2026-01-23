using DarksideApi.DarkOrbit.Commands.Hero;

namespace DarksideApi.DarkOrbit.Handlers.Hero
{
    internal class EnergyUpdateHandler : ICommandHandler
    {
        private readonly Api api;

        public EnergyUpdateHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            EnergyUpdateCommand energyUpdate = new();
            energyUpdate.Read(@byte);

            this.api.Hero.CurrentCaptainEnergy = energyUpdate.currentCaptainEnergy;
            this.api.Hero.MaxCaptainEnergy = energyUpdate.maxCaptainEnergy;
        }
    }
}
