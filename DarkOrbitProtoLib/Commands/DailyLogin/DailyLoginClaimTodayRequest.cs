namespace DarksideApi.DarkOrbit.Commands.DailyLogin
{
    public class DailyLoginClaimTodayRequest : Command
    {
        public new const short Id = -10319;

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
