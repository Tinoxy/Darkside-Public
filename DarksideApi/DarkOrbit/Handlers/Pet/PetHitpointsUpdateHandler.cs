using System.Diagnostics;

namespace DarksideApi.DarkOrbit.Handlers.Pet
{
    internal class PetHitpointsUpdateHandler : ICommandHandler
    {
        private readonly Api api;

        public PetHitpointsUpdateHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            Commands.Pet.PetHitpointsUpdateCommand petHitpointsUpdate = new();
            petHitpointsUpdate.Read(@byte);

            if (!this.api.Hero.IsPetActive)
                return;

            this.api.Hero.Pet.HitPoints = petHitpointsUpdate.hitpointsNow;
            this.api.Hero.Pet.MaxHitPoints = petHitpointsUpdate.hitpointsMax;

            Debug.WriteLine($"Pet Hitpoints Update: Now: {petHitpointsUpdate.hitpointsNow}, Max: {petHitpointsUpdate.hitpointsMax}, Use Repair Gear: {petHitpointsUpdate.useRepairGear}");
        }
    }
}
