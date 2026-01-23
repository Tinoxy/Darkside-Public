namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class KeepAlive : Command
    {
        public new const short Id = 2;
        public bool mouseClick;

        public KeepAlive(bool mouseClick)
        {
            this.mouseClick = mouseClick;
        }

        public override void Read(ByteArray reader)
        {
            this.mouseClick = this.@byte.ReadBool();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteBool(this.mouseClick);
            return this.@byte.ToArray();
        }
    }
}
