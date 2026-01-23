namespace DarksideApi.DarkOrbit.Commands.Action
{
    public class TradeRequest : Command   //TradeRequest
    {
        public new const short Id = 81;

        public TradeRequest()
        {
        }

        public override void Read(ByteArray reader)
        {
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            return this.@byte.ToArray();
        }
    }
}
