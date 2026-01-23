using DarksideApi.DarkOrbit.Commands.Autopilot;

namespace DarksideApi.DarkOrbit.Handlers.Autopilot
{
    internal class AutopilotStatusHandler : ICommandHandler
    {
        private readonly Api api;

        public AutopilotStatusHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            AutoPilotStatus status = new AutoPilotStatus();
            status.Read(@byte);

            this.api.Logging.Log("[Autopilot] Status: " + status.status);
        }
    }
}
