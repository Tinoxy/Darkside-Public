namespace DarksideApi.DarkOrbit.Commands.Hero
{
    public class InventoryItemAttributeModule : Command   //class_2025
    {
        public new const short Id = -11029;
        public string value;
        public InventoryItemAttributeKey key;

        public InventoryItemAttributeModule()
        {
            this.value = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            this.key = (InventoryItemAttributeKey)reader.ReadShort();
            this.value = reader.ReadUTF();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public enum InventoryItemAttributeKey
        {
            HP = 0,
            LEVEL = 1,
            AUTO_BUY_ITEM = 3,
        }
    }
}
