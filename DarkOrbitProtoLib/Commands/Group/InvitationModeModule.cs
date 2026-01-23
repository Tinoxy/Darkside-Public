namespace DarksideApi.DarkOrbit.Commands.Group
{
    public class InvitationModeModule : Command
    {
        public new const short Id = 17008;
        public int mode;

        public override void Read(ByteArray reader)
        {
            this.mode = reader.ReadShort();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
