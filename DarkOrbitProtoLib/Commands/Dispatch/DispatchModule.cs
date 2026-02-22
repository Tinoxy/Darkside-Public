namespace DarksideApi.DarkOrbit.Commands.Dispatch
{
    public class DispatchModule : Command    //DispatchModule
    {
        public new const short Id = -31536;
        public int slotId;
        public double duration;
        public int id;

        public override void Read(ByteArray reader)
        {
            this.duration = reader.ReadDouble();
            this.id = reader.ReadShort();
            this.id = 65535 & ((65535 & this.id) << 14 | (65535 & this.id) >>> 2);
            this.id = this.id > 32767 ? (this.id - 65536) : this.id;
            this.slotId = reader.ReadShort();
            this.slotId = 65535 & ((65535 & this.slotId) >>> 5 | (65535 & this.slotId) << 11);
            this.slotId = this.slotId > 32767 ? this.slotId - 65536 : this.slotId;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
