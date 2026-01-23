namespace DarksideApi.DarkOrbit.Handlers.Pet
{
    internal class PetOutOfFuelHandler : ICommandHandler
    {
        private readonly Api api;

        public PetOutOfFuelHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            this.api.Hero.IsPetOutOfFuel = true;
        }
    }
}
