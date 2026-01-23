using DarksideApi.DarkOrbit.Commands.Autopilot;
using System.Diagnostics;

namespace DarksideApi.DarkOrbit.Handlers.Autopilot
{
    internal class AutopilotInfoHandler : ICommandHandler
    {
        private readonly Api api;

        public AutopilotInfoHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            var autoPilotInfo = new AutopilotInfoCommand();
            autoPilotInfo.Read(@byte);

            Debug.WriteLine("Autopilot info: " + autoPilotInfo.autoPilotInfos + "\n" + autoPilotInfo.result + "\n" + autoPilotInfo.laserRange + "\n" + autoPilotInfo.remainingSeconds);
        }
    }
}
