namespace DarksideApi.DarkOrbit.Commands.Hero
{
    public class CurrencyUpdated : Command  //AttributeCreditsUpdateCommand
    {
        public new const short Id = 9;
        public int credits;
        public int uridium;
        public float jackpot;

        public override void Read(ByteArray reader)
        {
            this.credits = reader.ReadInt();
            this.credits = this.credits >>> 2 | this.credits << 30;
            this.uridium = reader.ReadInt();
            this.uridium = this.uridium >>> 13 | this.uridium << 19;
            this.jackpot = reader.ReadFloat();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
