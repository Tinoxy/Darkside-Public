namespace DarksideApi.DarkOrbit.Commands.Auction
{
    public class AuctionHistoryCommand : Command
    {
        public new const short Id = 1313;
        public string type;
        public string winnerName;
        public string lootId;
        public string auctionStartTime;
        public int winnerId;
        public long currentBid;
        public string group;

        public AuctionHistoryCommand()
        {
            this.type = string.Empty;
            this.winnerName = string.Empty;
            this.lootId = string.Empty;
            this.auctionStartTime = string.Empty;
            this.group = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            this.type = reader.ReadUTF();
            this.currentBid = (long)reader.ReadDouble();
            this.group = reader.ReadUTF();
            this.lootId = reader.ReadUTF();
            this.auctionStartTime = reader.ReadUTF();
            this.winnerId = reader.ReadInt();
            this.winnerId = this.winnerId << 19 | this.winnerId >>> 13;            
            this.winnerName = reader.ReadUTF();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"{{\"type\":\"{this.type}\",\"winnerName\":\"{this.winnerName}\",\"lootId\":\"{this.lootId}\",\"auctionStartTime\":\"{this.auctionStartTime}\",\"winnerId\":{this.winnerId},\"currentBid\":{this.currentBid},\"group\":\"{this.group}\"}}";
        }
    }
}
