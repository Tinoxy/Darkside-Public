namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class ChannelCloseRequest : Command   //165
    {
        public new const short Id = 260;
        public bool close;

        public ChannelCloseRequest(bool close)
        {
            this.close = close;
        }

        public override void Read(ByteArray reader)
        {
            this.close = reader.ReadBool();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteBool(this.close);
            return this.@byte.ToArray();
        }
    }
}
