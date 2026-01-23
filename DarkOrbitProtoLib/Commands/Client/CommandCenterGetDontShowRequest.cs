namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class CommandCenterGetDontShowRequest : Command   //1020
    {
        public new const short Id = 4934;

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