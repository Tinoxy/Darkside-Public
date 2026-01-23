using DarksideApi.DarkOrbit.Commands.Hero;
using DarksideApi.DarkOrbit.Objects.Boxes;

namespace DarksideApi.DarkOrbit.Handlers.Hero
{
    internal class CargoUpdateHandler : ICommandHandler
    {
        private readonly Api api;

        public CargoUpdateHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            CargoUpdateCommand cargoUpdate = new();
            cargoUpdate.Read(@byte);

            foreach (OreCountModule cargoModule in cargoUpdate.cargoUpdates)
            {
                this.api.Hero.Cargo.Set((Ore.OreType)cargoModule.OreType, cargoModule.count);
            }

            this.api.Hero.Cargo.UsedCargoSpace = cargoUpdate.UsedCargoSpace;
        }
    }
}
