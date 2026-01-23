namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class LegacyCommand : Command    //166
    {
        public new const short Id = 1;
        public string message;

        public LegacyCommand()
        {
            this.message = string.Empty;
        }

        public LegacyCommand(string message)
        {
            this.message = message;
        }

        public override void Read(ByteArray reader)
        {
            this.message = reader.ReadUTF();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteUTF(this.message);
            return this.@byte.ToArray();
        }
    }
}
