namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class MessageKeyListWildcardModule : Command //MessageKeyListWildcardModule 567
    {
        public new const short Id = -1992;
        public string baseKey;
        public LoginResponseCommand baseKeyType;
        public List<MessageWildcardReplacementModule> wildcardReplacements;
        public int textStyle;

        public MessageKeyListWildcardModule()
        {
            this.baseKey = string.Empty;
            this.baseKeyType = new();
            this.wildcardReplacements = [];
        }

        public override void Read(ByteArray reader)
        { 
            if (reader.ReadShort() != 0)
            {
                this.baseKeyType.Read(reader);
            }
            var i = 0;
            var max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                var module = new MessageWildcardReplacementModule();
                module.Read(reader);
                this.wildcardReplacements.Add(module);
                i++;
            }
            this.textStyle = reader.ReadShort();
            this.baseKey = reader.ReadUTF();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
