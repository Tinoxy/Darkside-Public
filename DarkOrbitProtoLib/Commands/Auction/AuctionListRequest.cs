namespace DarksideApi.DarkOrbit.Commands.Auction
{
    public class AuctionListRequest : Command
    {
        public new const short Id = 31981;
        public string auctionType;

        public AuctionListRequest(string auctionType)
        {
            this.auctionType = auctionType;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteUTF(this.auctionType);
            return this.@byte.ToArray();
        }
    }
}
