namespace DarksideApi.DarkOrbit.Commands.Auction
{
    public class AuctionDataCommand : Command
    {
        public new const short Id = -13339;
        public bool instantBuy;
        public long highestBid;
        public int amount;
        public string group;
        public string highestBidderName;
        public long myBid;
        public int highestBidderId;
        public string type;
        public string id;
        public long price;

        public AuctionDataCommand()
        {
            this.group = string.Empty;
            this.highestBidderName = string.Empty;
            this.type = string.Empty;
            this.id = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            this.instantBuy = reader.ReadBool();
            this.myBid = (long)reader.ReadDouble();
            this.highestBid = (long)reader.ReadDouble();
            this.highestBidderName = reader.ReadUTF();
            this.highestBidderId = reader.ReadInt();
            this.highestBidderId = this.highestBidderId << 3 | this.highestBidderId >>> 29;
            this.price = (long)reader.ReadDouble();
            this.id = reader.ReadUTF();
            this.group = reader.ReadUTF();
            this.amount = reader.ReadInt();
            this.amount = this.amount << 30 | this.amount >>> 2;
            this.type = reader.ReadUTF();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"{{\"instantBuy\":{this.instantBuy},\"highestBid\":{this.highestBid},\"amount\":{this.amount},\"group\":\"{this.group}\",\"highestBidderName\":\"{this.highestBidderName}\",\"myBid\":{this.myBid},\"highestBidderId\":{this.highestBidderId},\"type\":\"{this.type}\",\"id\":\"{this.id}\",\"price\":{this.price}}}";
        }
    }
}
