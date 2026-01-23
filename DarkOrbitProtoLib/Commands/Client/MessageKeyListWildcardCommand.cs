namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class MessageKeyListWildcardCommand : Command    //MessageKeyListWildcardCommand 399
    {
        public new const short Id = 10841;
        public List<MessageKeyListWildcardModule> commands;

        public MessageKeyListWildcardCommand()
        {
            this.commands = [];
        }

        public override void Read(ByteArray reader)
        {
            var i = 0;
            var max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                var command = new MessageKeyListWildcardModule();
                command.Read(reader);

                this.commands.Add(command);

                i++;
            }
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
