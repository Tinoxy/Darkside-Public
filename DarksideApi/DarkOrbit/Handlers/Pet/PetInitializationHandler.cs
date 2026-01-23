using DarksideApi.DarkOrbit.Commands.Pet;

namespace DarksideApi.DarkOrbit.Handlers.Pet
{
    internal class PetInitializationHandler : ICommandHandler
    {
        private readonly Api api;

        public PetInitializationHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            PetInitializationCommand petInitialization = new();
            petInitialization.Read(@byte);

            api.Hero.IsPetOutOfFuel = !petInitialization.hasFuel;
        }
    }
}
