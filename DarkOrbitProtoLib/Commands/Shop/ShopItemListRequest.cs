namespace DarksideApi.DarkOrbit.Commands.Shop
{
    public class ShopItemListRequest : Command
    {
        public new const short Id = -19420;
        public string category;

        public ShopItemListRequest(string category)
        {
            this.category = category;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteUTF(this.category);
            return this.@byte.ToArray();
        }
    }
}
