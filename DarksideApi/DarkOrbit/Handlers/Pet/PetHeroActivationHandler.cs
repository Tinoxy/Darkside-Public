using DarksideApi.DarkOrbit.Commands.Pet;

namespace DarksideApi.DarkOrbit.Handlers.Pet
{
    internal class PetHeroActivationHandler : ICommandHandler
    {
        private readonly Api api;

        public PetHeroActivationHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            PetHeroActivationCommand petInit = new();
            petInit.Read(@byte);

            Objects.Pets.Pet pet = new(this.api, petInit.userId, petInit.ownerId, petInit.level, 0, petInit.x, petInit.y, petInit.name, petInit.clanTag, !this.api.Hero.IsCloaked);
            if (!this.api.Pets.TryAdd(petInit.userId, pet))
            {
                this.api.Pets[petInit.userId] = pet;
            }
            this.api.Hero.PetStartTries = 0;
        }
    }
}
