namespace DarksideApi.DarkOrbit.Commands.Dispatch
{
    public class DispatchCollectRetrieverRequest : Command  //DispatchCollectRetrieverRequest
    {
        public new const short Id = -63;
        public short slotId;

        public DispatchCollectRetrieverRequest(short slotId)
        {
            this.slotId = slotId;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteShort((short)(65535 & ((65535 & this.slotId) << 4 | (65535 & this.slotId) >>> 12)));
            return this.@byte.ToArray();
        }
    }
}
