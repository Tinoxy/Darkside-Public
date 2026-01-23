using DarksideApi.DarkOrbit.Commands.Pet;

namespace DarksideApi.DarkOrbit.Handlers.Pet
{
    internal class PetInitHandler : ICommandHandler
    {
        private readonly Api api;

        public PetInitHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            PetActivationCommand petInit = new();
            petInit.Read(@byte);

            Objects.Pets.Pet pet = new(this.api, petInit.petId, petInit.ownerId, petInit.level, 0, petInit.x, petInit.y, petInit.username, petInit.clanTag, petInit.isVisible);
            if (!this.api.Pets.TryAdd(pet.UserId, pet))
            {
                this.api.Pets[pet.UserId] = pet;
            }

            //api.Logging.WriteLine(petInit.ToString());
        }
    }
}
