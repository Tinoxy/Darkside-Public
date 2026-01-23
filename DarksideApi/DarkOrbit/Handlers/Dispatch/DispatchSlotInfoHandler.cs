using DarksideApi.DarkOrbit.Commands.Dispatch;

namespace DarksideApi.DarkOrbit.Handlers.Dispatch
{
    internal class DispatchSlotInfoHandler : ICommandHandler
    {
        private readonly Api api;

        public DispatchSlotInfoHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            DispatchSlotInfoCommand dispatchSlots = new();
            dispatchSlots.Read(@byte);

            this.api.Hero.DispatcherSlotsTotal = dispatchSlots.currentMaxSlotCount;
            this.api.Hero.DispatcherSlotsFree = dispatchSlots.availableSlotCount;
        }
    }
}
