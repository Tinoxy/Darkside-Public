namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class LogoutCancelRequest : Command
    {
        public new const short Id = 15;

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
