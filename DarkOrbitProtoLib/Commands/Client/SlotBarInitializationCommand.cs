namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class SlotBarInitializationCommand : Command //2310
    {
        public new const short Id = -18116;
        public string slotBarId;
        public string slotBarOrder;
        public string slotBarPosition;
        public bool visible;
        public List<SlotInfoCommand> slots;

        public SlotBarInitializationCommand()
        {
            this.slotBarId = string.Empty;
            this.slotBarOrder = string.Empty;
            this.slotBarPosition = string.Empty;
            this.slots = [];
        }

        public override void Read(ByteArray reader)
        {
            this.slotBarPosition = reader.ReadUTF();
            var i = 0;
            var max = reader.ReadShort();
            while (i < max)
            {
                reader.ReadShort();
                var slotInfo = new SlotInfoCommand();
                slotInfo.Read(reader);
                this.slots.Add(slotInfo);
                i++;
            }
            this.visible = reader.ReadBool();
            this.slotBarId = reader.ReadUTF();
            this.slotBarOrder = reader.ReadUTF();

        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
