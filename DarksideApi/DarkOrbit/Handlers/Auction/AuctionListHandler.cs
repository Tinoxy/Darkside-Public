using DarksideApi.DarkOrbit.Commands.Auction;
using DarksideApi.DarkOrbit.Objects.Auction;

namespace DarksideApi.DarkOrbit.Handlers.Auction
{
    internal class AuctionListHandler : ICommandHandler
    {
        private readonly Api api;

        public AuctionListHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            AuctionListCommand auctionList = new();
            auctionList.Read(@byte);

            this.api.Auction.ClearAuction();

            foreach (AuctionDataCommand auction in auctionList.auctionList)
            {
                var auctionItem = new AuctionItem(auction.id, auction.group, auction.highestBid, auction.myBid, auction.highestBidderId);
                this.api.Auction.AddAuctionItem(auctionItem);
            }
            //Debug.WriteLine(auctionList.ToString());

            if (this.api.Hero.IsCurrentlyBidding)
                return;

            this.api.Auction.Bid();
        }
    }
}
