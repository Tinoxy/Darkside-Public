namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class ClientLoggedInCommand : Command
    {
        public new const short Id = 245;

        public override void Read(ByteArray reader)
        {
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
