namespace DarksideApi.DarkOrbit.Commands.Auction
{
    public class AuctionBidRequest : Command
    {
        public new const short Id = -23475;
        public string lootId;
        public long price;
        public string type;

        public AuctionBidRequest(string lootId, long price, string type)
        {
            this.lootId = lootId;
            this.price = price;
            this.type = type;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteUTF(this.lootId);
            this.@byte.WriteUTF(this.type);
            this.@byte.WriteDouble(this.price);
            return this.@byte.ToArray();
        }
    }
}
