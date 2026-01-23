using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Commands.Auction;
using DarksideApi.Util.Extensions;

namespace DarksideApi.DarkOrbit.Handlers.Auction
{
    internal class AuctionBidHandler : ICommandHandler
    {
        private readonly Api api;

        public AuctionBidHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            AuctionBidCommand auctionBid = new();
            auctionBid.Read(@byte);

            if (!auctionBid.success)
            {
                this.api.Logging.Log("Bidding failed, info: " + auctionBid.stack.ToString());
                return;
            }
            var itemName = (CommandConstants.AUCTION_NAMES.TryGetKey(auctionBid.myBid.id, out var key) ? key : auctionBid.myBid.id);
            if (auctionBid.stack.enumBaseKey == Commands.MessageStack.BaseStackCommand.BaseKey.auction_bid_credits_to_little)
            {
                this.api.WriteLog("Not enough credits to bid on " + itemName);
                return;
            }
            if (auctionBid.stack.enumBaseKey == Commands.MessageStack.BaseStackCommand.BaseKey.auction_bid_minBid_not_reached)
            {
                this.api.WriteLog("Bid on " + itemName + " too low");
                return;
            }
            if (auctionBid.myBid.highestBidderId == this.api.Hero.UserId)
            {
                this.api.WriteLog("Successfully bid on " + itemName);
                return;
            }
            this.api.Logging.Log("Aucion Bid returned: " + auctionBid.stack.enumBaseKey + ", for " + auctionBid.myBid.ToString());
            //Debug.WriteLine(auctionBid.ToString());
        }
    }
}
