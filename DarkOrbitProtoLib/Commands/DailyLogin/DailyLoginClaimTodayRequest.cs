namespace DarksideApi.DarkOrbit.Commands.DailyLogin
{
    public class DailyLoginClaimTodayRequest : Command
    {
        public new const short Id = 13023;

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            return this.@byte.ToArray();
        }
    }
}
