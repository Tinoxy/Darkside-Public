namespace DarksideApi.DarkOrbit.Chat.Commands.LowLevel
{
    public class IntroductionMessageCommand : Command
    {
        public string AddressString;

        public IntroductionMessageCommand(string addressString)
        {
            this.AddressString = addressString;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteUTF(this.AddressString);
            return this.@byte.ToArray();
        }
    }
}
