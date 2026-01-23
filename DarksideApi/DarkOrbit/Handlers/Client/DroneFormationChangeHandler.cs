using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Commands.Client;

namespace DarksideApi.DarkOrbit.Handlers.Client
{
    public class DroneFormationChangeHandler : ICommandHandler
    {
        private readonly Api api;

        public DroneFormationChangeHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            DroneFormationChangeCommand droneFormationChange = new();
            droneFormationChange.Read(@byte);

            if (!CommandConstants.DRONE_FORMATION_IDS.TryGetValue(droneFormationChange.selectedFormation, out var name))
            {
                this.api.Logging.Log(droneFormationChange.userId + " changed formation to an Unknown Formation (" + droneFormationChange.selectedFormation + ")");
                return;
            }
            if (droneFormationChange.userId == this.api.Hero.UserId)
            {
                this.api.Hero.DroneFormation = name;
                this.api.WriteLog("Changed Formation to: " + name);
                return;
            }
            //if (api.Ships.TryGetValue(droneFormationChange.userId, out var ship))
            //{
            //    
            //}
            //api.Logging.WriteLine(droneFormationChange.userId + " changed formation to: " + droneFormationChange.selectedFormation + " -> " + name);
        }
    }
}
