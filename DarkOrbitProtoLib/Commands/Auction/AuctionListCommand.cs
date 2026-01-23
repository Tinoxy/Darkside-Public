namespace DarksideApi.DarkOrbit.Commands.Auction
{
    public class AuctionListCommand : Command
    {
        public new const short Id = 10916;
        public List<AuctionHistoryCommand> historyList;
        public List<AuctionDataCommand> auctionList;
        public int hourSubSec;
        public int daySubSec;
        public List<AuctionDataCommand> myBidList;
        public int weekSubSec;

        public AuctionListCommand()
        {
            this.historyList = [];
            this.auctionList = [];
            this.myBidList = [];
        }

        public override void Read(ByteArray reader)
        {
            var i = 0;
            var max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                AuctionDataCommand auctionData = new();
                auctionData.Read(reader);
                this.myBidList.Add(auctionData);
                i++;
            }
            this.hourSubSec = reader.ReadInt();
            this.hourSubSec = this.hourSubSec << 3 | this.hourSubSec >>> 29;
            i = 0;
            max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                AuctionHistoryCommand auctionHistory = new();
                auctionHistory.Read(reader);
                this.historyList.Add(auctionHistory);
                i++;
            }
            i = 0;
            max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                AuctionDataCommand auctionData = new();
                auctionData.Read(reader);
                this.auctionList.Add(auctionData);
                i++;
            }
            this.weekSubSec = reader.ReadInt();
            this.weekSubSec = this.weekSubSec >>> 20 | this.weekSubSec << 12;
            this.daySubSec = reader.ReadInt();
            this.daySubSec = this.daySubSec >>> 4 | this.daySubSec << 28;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            string a = string.Empty, b = string.Empty, c = string.Empty;
            foreach (AuctionHistoryCommand auctionHistory in this.historyList)
            {
                a += auctionHistory.ToString() + "\n";
            }
            foreach (AuctionDataCommand auctionData in this.auctionList)
            {
                b += auctionData.ToString() + "\n";
            }
            foreach (AuctionDataCommand auctionData in this.myBidList)
            {
                c += auctionData.ToString() + "\n";
            }
            return $"AuctionListCommand: historyList: {a}, auctionList: {b}, myBidList: {c}, hourSubSec: {this.hourSubSec}, daySubSec: {this.daySubSec}, weekSubSec: {this.weekSubSec}";
        }
    }
}
