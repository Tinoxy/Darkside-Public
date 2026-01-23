namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class StayinAliveCommand : Command
    {
        public new const short Id = -25136;

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
