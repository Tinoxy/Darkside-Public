namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class MessageWildcardReplacementModule : Command //MessageWildcardReplacementModule 568
    {
        public new const short Id = 48;
        public string wildcard;
        public string replacement;
        public LoginResponseCommand replacementType;

        public MessageWildcardReplacementModule()
        {
            this.wildcard = string.Empty;
            this.replacement = string.Empty;
            this.replacementType = new();
        }

        public override void Read(ByteArray reader)
        {
            this.wildcard = reader.ReadUTF();
            this.replacement = reader.ReadUTF();
            if (reader.ReadShort() != 0)
            {
                this.replacementType.Read(reader);
            }
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
