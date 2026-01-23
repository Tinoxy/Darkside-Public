namespace DarksideApi.DarkOrbit.Chat.Commands.Messages
{
    public class StringMessageCommand : Command
    {
        public string strData;
        public Dictionary<string, object> json;

        public StringMessageCommand(string strData, Dictionary<string, object> json)
        {
            this.strData = strData;
            this.json = json;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
