using DarksideApi.DarkOrbit.Commands.MessageStack;

namespace DarksideApi.DarkOrbit.Commands.Auction
{
    public class AuctionBidCommand : Command
    {
        public new const short Id = -9664;
        public AuctionDataCommand myBid;
        public BaseStackCommand stack;
        public AuctionDataCommand curBid;
        public bool success;

        public AuctionBidCommand()
        {
            this.myBid = new();
            this.stack = new();
            this.curBid = new();
        }

        public override void Read(ByteArray reader)
        {
            if (reader.ReadShort() != 0)
                this.curBid.Read(reader);
            this.success = reader.ReadBool();
            if (reader.ReadShort() != 0)
                this.stack.Read(reader);         
            if (reader.ReadShort() != 0)
                this.myBid.Read(reader);
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"{{\"myBid\":{this.myBid},\"stack\":{this.stack},\"curBid\":{this.curBid},\"success\":{this.success}}}";
        }
    }
}
