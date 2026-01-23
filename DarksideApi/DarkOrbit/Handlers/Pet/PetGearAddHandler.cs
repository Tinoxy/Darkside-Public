using DarksideApi.DarkOrbit.Commands.Pet;
using DarksideApi.DarkOrbit.Objects.Pets;

namespace DarksideApi.DarkOrbit.Handlers.Pet
{
    internal class PetGearAddHandler : ICommandHandler
    {
        private readonly Api api;

        public PetGearAddHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            PetGearAddCommand petGearAdd = new();
            petGearAdd.Read(@byte);

            if (!this.api.Hero.IsPetActive)
            {
                return;
            }
            var gear = new PetGear(petGearAdd.gearModule.module, petGearAdd.level, petGearAdd.amount, petGearAdd.enabled);
            if (!this.api.Hero.Pet.Gears.TryAdd(gear.Id, gear))
            {
                this.api.Hero.Pet.Gears[gear.Id] = gear;
            }
        }
    }
}
