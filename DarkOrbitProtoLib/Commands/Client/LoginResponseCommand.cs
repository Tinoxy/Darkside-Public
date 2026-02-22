namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class LoginResponseCommand : Command //LoginResponse
    {
        public new const short Id = 19206;
        public int status;

        public override void Read(ByteArray reader)
        {
            this.status = reader.ReadShort();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
