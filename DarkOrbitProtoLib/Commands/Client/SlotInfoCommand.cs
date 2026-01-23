namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class SlotInfoCommand : Command  //1944
    {
        public new const short Id = 11342;
        public string menuItemId;
        public int slotId = 0;

        public SlotInfoCommand()
        {
            this.menuItemId = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            this.menuItemId = reader.ReadUTF();
            this.slotId = reader.ReadInt();
            this.slotId = this.slotId << 4 | this.slotId >>> 28;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
