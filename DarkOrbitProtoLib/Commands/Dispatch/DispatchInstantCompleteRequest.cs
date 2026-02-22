namespace DarksideApi.DarkOrbit.Commands.Dispatch
{
    public class DispatchInstantCompleteRequest : Command    //DipatchInstantCompleteRequest
    {
        public new const short Id = 20585;
        public short slotId;

        public DispatchInstantCompleteRequest(short slotId)
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
            this.@byte.WriteShort((short)(65535 & ((65535 & this.slotId) << 14 | (65535 & this.slotId) >>> 2)));
            return this.@byte.ToArray();
        }
    }
}
