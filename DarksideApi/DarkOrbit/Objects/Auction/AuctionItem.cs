namespace DarksideApi.DarkOrbit.Objects.Auction
{
    public class AuctionItem
    {
        public string LootId { get; set; }
        public string Group { get; set; }
        public long HighestBid { get; set; }
        public long MyBid { get; set; }
        public int HighestBidderId { get; set; }

        public AuctionItem(string lootId, string group, long highestBid, long myBid, int highestBidderId)
        {
            this.LootId = lootId;
            this.Group = group;
            this.HighestBid = highestBid;
            this.MyBid = myBid;
            this.HighestBidderId = highestBidderId;
        }
    }
}
