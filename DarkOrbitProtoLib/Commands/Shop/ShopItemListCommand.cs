namespace DarksideApi.DarkOrbit.Commands.Shop
{
    public class ShopItemListCommand : Command
    {
        public new const short Id = -8316;
        public List<ShopLabelPrompt> promptList;
        public string currentShipLootId;
        public string category;
        public List<ShopItemCommand> itemList;

        public ShopItemListCommand()
        {
            this.promptList = [];
            this.currentShipLootId = string.Empty;
            this.category = string.Empty;
            this.itemList = [];
        }

        public override void Read(ByteArray reader)
        {
            var i = 0;
            var max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                var e = new ShopItemCommand();
                e.Read(reader);
                this.itemList.Add(e);
                i++;
            }
            this.category = reader.ReadUTF();
            i = 0;
            max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                var e = new ShopLabelPrompt(string.Empty);
                e.Read(reader);
                this.promptList.Add(e);
                i++;
            }
            this.currentShipLootId = reader.ReadUTF();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            var itemListString = string.Join(", ", this.itemList.Select(item => item.ToString()));
            var promptListString = string.Join(", ", this.promptList.Select(prompt => prompt.ToString()));
            return $"ShopItemListCommand{{category={this.category}, currentShipLootId={this.currentShipLootId}, itemList={itemListString}, promptList={promptListString}}}";
        }
    }
}
