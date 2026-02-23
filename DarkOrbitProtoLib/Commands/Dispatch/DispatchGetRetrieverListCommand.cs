namespace DarksideApi.DarkOrbit.Commands.Dispatch
{
    public class DispatchGetRetrieverListCommand : Command    //DispatchGetRetrieverListCommand
    {
        public new const short Id = 5871;
        public List<DispatchModule> dispatchers;
        public StatusType statusType;

        public DispatchGetRetrieverListCommand()
        {
            this.dispatchers = [];
        }

        public override void Read(ByteArray reader)
        {
            var i = 0;
            int max = reader.ReadByte();
            while (i < max)
            {
                if (reader.ReadShort() != 0)
                {
                    DispatchModule dispatcherInfo = new();
                    dispatcherInfo.Read(reader);
                    this.dispatchers.Add(dispatcherInfo);
                }
                i++;
            }
            this.statusType = (StatusType)reader.ReadShort();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public enum StatusType
        {
            AVAILABLE = 0,
            IN_PROGRESS = 1,
            READY_TO_COLLECT = 2
        }
    }
}
