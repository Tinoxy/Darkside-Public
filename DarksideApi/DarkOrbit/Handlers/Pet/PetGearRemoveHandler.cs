using DarksideApi.DarkOrbit.Commands.Pet;

namespace DarksideApi.DarkOrbit.Handlers.Pet
{
    internal class PetGearRemoveHandler : ICommandHandler
    {
        private readonly Api api;

        public PetGearRemoveHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            PetGearRemoveCommand petGearRemove = new();
            petGearRemove.Read(@byte);

            if (!this.api.Hero.IsPetActive)
            {
                return;
            }
            this.api.Hero.Pet.Gears.Remove(petGearRemove.gearModule.module);
        }
    }
}
