namespace DarksideApi.DarkOrbit.Commands.Dispatch
{
    public class DispatchSendRetrieverRequest : Command  //DispatchSendRetrieverRequest
    {
        public new const short Id = 30664;
        public int dispatcherId;

        public DispatchSendRetrieverRequest(int dispatcherId)
        {
            this.dispatcherId = dispatcherId;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteInt(this.dispatcherId >>> 1 | this.dispatcherId << 31);
            return this.@byte.ToArray();
        }
    }
}
