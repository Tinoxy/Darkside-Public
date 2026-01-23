namespace DarksideApi.DarkOrbit.Commands.Hero
{
    public class InventoryItemModule : Command   //InventoryItemModule
    {
        public new const short Id = -13778;
        public string lootId;
        public List<InventoryItemAttributeModule> inventoryItemAttributes;
        public double amount;
        public double itemId;
        public int upgradeLevel;
        public bool isNew;
        public double hangarId;

        public InventoryItemModule()
        {
            this.lootId = string.Empty;
            this.inventoryItemAttributes = [];
        }

        public override void Read(ByteArray reader)
        {
            this.upgradeLevel = reader.ReadInt();
            this.upgradeLevel = this.upgradeLevel << 8 % 32 | this.upgradeLevel >>> 32 - 8 % 32;
            this.isNew = reader.ReadBool();
            this.amount = reader.ReadDouble(); 
            this.hangarId = reader.ReadDouble();
            var i = 0;
            int max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                var c = new InventoryItemAttributeModule();
                c.Read(reader);
                this.inventoryItemAttributes.Add(c);
                i++;
            }
            this.itemId = reader.ReadDouble();
            this.lootId = reader.ReadUTF();
           

        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
