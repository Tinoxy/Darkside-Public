using DarksideApi.DarkOrbit.Commands.Pet;

namespace DarksideApi.DarkOrbit.Handlers.Pet
{
    public class PetFuelUpdateHandler : ICommandHandler
    {
        private readonly Api api;

        public PetFuelUpdateHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            var petFuelUpdate = new PetFuelUpdateCommand();
            petFuelUpdate.Read(@byte);

            if (this.api.Hero.IsPetActive)
            {
                this.api.Hero.Pet.Fuel = petFuelUpdate.currentFuel;
                this.api.Hero.Pet.MaxFuel = petFuelUpdate.maxFuel;
            }
            this.api.AddLootResource("Pet Fuel", 0, petFuelUpdate.currentFuel);
        }
    }
}
