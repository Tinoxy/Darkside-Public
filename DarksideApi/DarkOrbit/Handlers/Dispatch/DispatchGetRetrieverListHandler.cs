using DarksideApi.DarkOrbit.Commands.Dispatch;
using DarksideApi.DarkOrbit.Objects.Client;

namespace DarksideApi.DarkOrbit.Handlers.Dispatch
{
    internal class DispatchGetRetrieverListHandler : ICommandHandler
    {
        private readonly Api api;

        public DispatchGetRetrieverListHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            DispatchGetRetrieverListCommand dispatcherUpdate = new();
            dispatcherUpdate.Read(@byte);

            //api.Logging.WriteLine($"Dispatcher Update: {dispatcherUpdate.type} ({dispatcherUpdate.dispatchers.Count}) => ");
            this.api.Hero.ActiveDispatchers.Clear();
            foreach (DispatchModule dispatch in dispatcherUpdate.dispatchers)
            {
                //api.Logging.WriteLine(dispatch.id + ", Slot: " + dispatch.slotId + ", time: " + dispatch.duration);
                DispatcherObj? dispatcherItem = this.api.Dispatchers.Where(d => d.Id == dispatch.id).FirstOrDefault();
                if (dispatcherItem == null)
                {
                    continue;
                }
                var dispatcherObj = DispatcherObj.From(dispatcherItem);
                dispatcherObj.SlotId = dispatch.slotId;
                dispatcherObj.Duration = dispatch.duration;
                if (this.api.Hero.ActiveDispatchers.TryAdd(dispatcherObj.SlotId, dispatcherObj))
                {
                    continue;
                }
                this.api.Hero.ActiveDispatchers[dispatcherObj.SlotId] = dispatcherObj;
            }
        }
    }
}
