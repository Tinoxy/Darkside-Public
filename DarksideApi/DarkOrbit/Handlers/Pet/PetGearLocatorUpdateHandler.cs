using DarksideApi.DarkOrbit.Commands.Pet;
using DarksideApi.Util;

namespace DarksideApi.DarkOrbit.Handlers.Pet
{
    internal class PetGearLocatorUpdateHandler : ICommandHandler
    {
        private readonly Api api;

        public PetGearLocatorUpdateHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            PetGearLocatorUpdateCommand petGearLocatorUpdate = new();
            petGearLocatorUpdate.Read(@byte);


            if (petGearLocatorUpdate.x <= 0 && petGearLocatorUpdate.y <= 0)
            {
                this.api.PointOfInterest = new(-1, -1);
                return;
            }

            var flag = (this.api.PointOfInterest.X <= 0 && this.api.PointOfInterest.Y <= 0) || HelpTools.CalculateDistance(petGearLocatorUpdate.x, petGearLocatorUpdate.y, this.api.Hero.TargetX, this.api.Hero.TargetY) >= 600;
            this.api.PointOfInterest = new(petGearLocatorUpdate.x, petGearLocatorUpdate.y);

            if (flag
                && this.api.Logic.State == Logic.LogicState.None
                && this.api.Logic.Target == null
                && this.api.Logic.IsInWorkArea(this.api.PointOfInterest))
            {
                this.api.Logic.SetState(this, Logic.LogicState.Roaming);
            }
        }
    }
}
