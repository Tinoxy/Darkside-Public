using DarksideApi.DarkOrbit.Commands.Pet;

namespace DarksideApi.DarkOrbit.Handlers.Pet
{
    public class PetLevelUpHandler : ICommandHandler
    {
        private readonly Api api;

        public PetLevelUpHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            PetLevelUpCommand petLevelUp = new();
            petLevelUp.Read(@byte);

            if (this.api.Hero.IsPetActive)
            {
                this.api.Hero.Pet.Level = petLevelUp.level;
                this.api.WriteLog($"Pet leveled up to {petLevelUp.level}.");
            }

            this.api.Logging.Log(petLevelUp.ToString());
        }
    }
}
