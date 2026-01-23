namespace DarksideApi.DarkOrbit.Commands.Dispatch
{
    public class DispatchGetRetrieverListRequest : Command  //DispatchGetRetrieverListRequest
    {
        public new const short Id = -31874;
        public short statusType;

        public DispatchGetRetrieverListRequest(short type)
        {
            this.statusType = type;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteShort(this.statusType);
            return this.@byte.ToArray();
        }
    }
}
