namespace DarksideApi.DarkOrbit.Commands.Shop
{
    public class ShopItemBuyRequest : Command
    {
        public new const short Id = 4216;
        public string lootId;
        public string category;
        public string buyCategory;
        public int amount;
        public string level;
        public string action;
        public string petName;

        public ShopItemBuyRequest(string lootId, string category, string buyCategory, int amount, string level, string action, string petName)
        {
            this.lootId = lootId;
            this.category = category;
            this.buyCategory = buyCategory;
            this.amount = amount;
            this.level = level;
            this.action = action;
            this.petName = petName;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteUTF(this.action);
            this.@byte.WriteUTF(this.lootId);
            this.@byte.WriteUTF(this.category);
            this.@byte.WriteUTF(this.buyCategory);
            this.@byte.WriteUTF(this.petName);
            this.@byte.WriteUTF(this.level);
            this.@byte.WriteInt(this.amount >>> 3 | this.amount << 29);
            return this.@byte.ToArray();
        }
    }
}
