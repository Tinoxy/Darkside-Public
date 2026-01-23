using DarksideApi.DarkOrbit.Commands.Dispatch;

namespace DarksideApi.DarkOrbit.Handlers.Dispatch
{
    internal class DispatchActiveEventHandler : ICommandHandler
    {
        private readonly Api api;

        public DispatchActiveEventHandler(Api api)
        {
            this.api = api;
        }
        public void Handle(ByteArray @byte)
        {
            DispatchActiveEventCommand dispatchActiveEvent = new();
            dispatchActiveEvent.Read(@byte);

            switch (dispatchActiveEvent.eventType)
            {
                case DispatchActiveEventCommand.EventType.DISPATCH_DAY:
                    if (!this.api.Hero.DispatchDay)
                        this.api.WriteLog($"Dispatch day active!");

                    this.api.Hero.DispatchDay = true;
                    break;
                case DispatchActiveEventCommand.EventType.GALAXY_GATE_DAY:
                    //api.Hero.GalaxyGateDay = true;
                    break;
                case DispatchActiveEventCommand.EventType.NONE:
                    this.api.Hero.DispatchDay = false;
                    break;
            }
            //api.Logging.WriteLine($"Active Dispatch Event: {dispatchActiveEvent.eventType}");
        }
    }
}
