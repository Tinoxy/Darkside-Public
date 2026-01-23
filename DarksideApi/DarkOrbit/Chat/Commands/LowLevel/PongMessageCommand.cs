namespace DarksideApi.DarkOrbit.Chat.Commands.LowLevel
{
    public class PongMessageCommand : Command
    {
        public long SendTime;

        public PongMessageCommand(long sendTime)
        {
            this.SendTime = sendTime;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteDouble(this.SendTime);
            return this.@byte.ToArray();
        }
    }
}
