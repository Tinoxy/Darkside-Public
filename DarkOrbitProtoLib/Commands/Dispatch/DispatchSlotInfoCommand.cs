namespace DarksideApi.DarkOrbit.Commands.Dispatch
{
    public class DispatchSlotInfoCommand : Command   //DispatchSlotInfoCommand
    {
        public new const short Id = 8936;
        public int availableSlotCount;
        public int currentMaxSlotCount;

        public override void Read(ByteArray reader)
        {   
            this.currentMaxSlotCount = reader.ReadShort();
            this.currentMaxSlotCount = 65535 & ((65535 & this.currentMaxSlotCount) << 2 | (65535 & this.currentMaxSlotCount) >>> 14);
            this.currentMaxSlotCount = this.currentMaxSlotCount > 32767 ? (this.currentMaxSlotCount - 65536) : this.currentMaxSlotCount;
            this.availableSlotCount = reader.ReadShort();
            this.availableSlotCount = 65535 & ((65535 & this.availableSlotCount) >>> 7 | (65535 & this.availableSlotCount) << 9);
            this.availableSlotCount = this.availableSlotCount > 32767 ? (this.availableSlotCount - 65536) : this.availableSlotCount;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
