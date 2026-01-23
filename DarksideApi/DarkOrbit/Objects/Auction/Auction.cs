namespace DarksideApi.DarkOrbit.Objects.Auction
{
    public class Auction
    {
        private readonly Api api;
        private readonly Dictionary<string, AuctionItem> auctionItems;
        private readonly List<string> bidOn;
        private readonly List<AuctionQueue> auctionQueue;

        struct AuctionQueue
        {
            public string LootId { get; set; }
            public long CreditsAmount { get; set; }
            public string AuctionType { get; set; }
        }

        public Auction(Api api)
        {
            this.api = api;
            this.auctionItems = [];
            this.bidOn = [];
            this.auctionQueue = [];
        }

        public void AddAuctionItem(AuctionItem item)
        {
            if (this.auctionItems.ContainsKey(item.LootId))
            {
                this.auctionItems[item.LootId] = item;
                return;
            }
            this.auctionItems.Add(item.LootId, item);
        }

        public void ClearAuction()
        {
            this.auctionItems.Clear();
        }

        public void ClearBids()
        {
            this.auctionQueue.Clear();
            this.bidOn.Clear();
            this.api.Logging.Log("Current Bids Cleared.");
        }

        public void InitializeAuction()
        {
            if (this.api.Hero.IsCurrentlyBidding)
            {
                return;
            }
            if (this.api.Settings.IsUnity)
            {
                this.api.GameMethods.AuctionGetList();
                return;
            }
            this.api.BackpageApi.AuctionBidInit();
        }

        public void Bid(string auctionType = "hour")
        {
            this.api.Hero.IsCurrentlyBidding = true;

            foreach (KeyValuePair<string, AuctionItem> item in this.auctionItems)
            {
                if (!this.api.Settings.AuctionSettings.TryGetValue(item.Key, out Settings.AuctionSetting? setting)
                    || !setting.IsEnabled
                    || setting.CreditsAmount > this.api.Inventory.GetCredits)
                {
                    continue;
                }
                if (item.Value.HighestBid >= setting.CreditsAmount
                    || item.Value.MyBid == item.Value.HighestBid
                    || item.Value.HighestBidderId == this.api.Hero.UserId)
                {
                    continue;
                }
                if (this.bidOn.Contains(item.Value.LootId))
                {
                    continue;
                }
                this.auctionQueue.Add(new AuctionQueue
                {
                    LootId = item.Value.LootId,
                    CreditsAmount = setting.CreditsAmount,
                    AuctionType = auctionType,
                });
                this.bidOn.Add(item.Value.LootId);
            }

            Task.Delay(250).ContinueWith(_ =>
            {
                foreach (AuctionQueue item in this.auctionQueue)
                {
                    this.api.GameMethods.AuctionBid(item.LootId, item.CreditsAmount, item.AuctionType);
                    this.api.Logging.Log("Bidding on " + item.LootId);

                    Thread.Sleep(Logic.Logic.GetRandomSleepTime(200, 300));
                }

                this.auctionQueue.Clear();
                this.api.Hero.IsCurrentlyBidding = false;
            });
        }
    }
}
