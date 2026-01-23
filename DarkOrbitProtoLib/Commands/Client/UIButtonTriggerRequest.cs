namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class UIButtonTriggerRequest : Command  //UIButtonTriggerRequest
    {
        public new const short Id = -13772;
        public string itemId;

        public UIButtonTriggerRequest(string itemId)
        {
            this.itemId = itemId;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteUTF(this.itemId);
            return this.@byte.ToArray();
        }
    }
}
