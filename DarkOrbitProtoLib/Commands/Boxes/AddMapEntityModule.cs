namespace DarksideApi.DarkOrbit.Commands.Boxes
{
    internal class AddMapEntityModule : Command
    {
        public new const short Id = 716;
        public string hash;
        public int y;
        public int x;

        public AddMapEntityModule()
        {
            this.hash = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            this.y = reader.ReadInt();
            this.y = this.y >>> 11 | this.y << 21;
            this.x = reader.ReadInt();
            this.x = this.x << 1 | this.x >>> 31;
            this.hash = reader.ReadUTF();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
