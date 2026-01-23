using DarksideApi.DarkOrbit.Commands.Pet;

namespace DarksideApi.DarkOrbit.Handlers.Pet
{
    internal class PetIsDestroyedHandler : ICommandHandler
    {
        private readonly Api api;

        public PetIsDestroyedHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            PetIsDestroyedCommand petIsDestroyed = new();
            petIsDestroyed.Read(@byte);

            api.Hero.IsPetDestroyed = true;
            switch (petIsDestroyed.destroyedType)
            {
                case PetIsDestroyedCommand.DestroyedType.NONE:
                    this.api.Logic.TryPetRevive();
                    break;
                case PetIsDestroyedCommand.DestroyedType.USED_KAMIKAZE:
                    this.api.Cooldown.RemoveCooldown(DarkOrbit.Cooldown.CooldownType.PET_KAMIKAZE_GEAR);
                    this.api.Cooldown.AddCooldown(DarkOrbit.Cooldown.CooldownType.PET_KAMIKAZE_GEAR, 30);
                    this.api.Logic.TryPetRevive();
                    break;
                default:
                    this.api.Logging.Log("Pet Status: " + petIsDestroyed.destroyedType);
                    break;
            }
        }
    }
}
