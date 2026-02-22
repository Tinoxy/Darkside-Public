namespace DarksideApi.DarkOrbit.Commands.Shop
{
    public class ShopItemLevelDataCommand : Command
    {
        public new const short Id = -18599;
        public int isPaymentItem;
        public int price;
        public int isElite;
        public string cost = string.Empty;
        public string discountPercent = string.Empty;
        public string currency = string.Empty;

        public override void Read(ByteArray reader)
        {
            this.isPaymentItem = reader.ReadInt();
            this.isElite = reader.ReadInt();
            this.price = reader.ReadInt();
            this.discountPercent = reader.ReadUTF();
            this.cost = reader.ReadUTF();
            this.currency = reader.ReadUTF();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"ShopItemLevelDataCommand{{isPaymentItem={this.isPaymentItem}, price={this.price}, isElite={this.isElite}, cost='{this.cost}', discountPercent='{this.discountPercent}', currency='{this.currency}'}}";
        }
    }
}
