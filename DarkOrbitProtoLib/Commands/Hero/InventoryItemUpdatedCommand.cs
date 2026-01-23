namespace DarksideApi.DarkOrbit.Commands.Hero
{
    public class InventoryItemUpdatedCommand : Command   //InventoryItemUpdateCommand
    {
        public new const short Id = -17578;
        public InventoryItemModule item;

        public InventoryItemUpdatedCommand()
        {
            this.item = new();
        }

        public override void Read(ByteArray reader)
        {
            if (reader.ReadShort() != 0)
            {
                this.item.Read(reader);
            }
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
