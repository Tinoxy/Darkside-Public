namespace DarksideApi.DarkOrbit.Commands.Boxes
{
    public class AddBoxCommand : Command
    {
        public new const short Id = -18701;
        private readonly AddMapEntityModule box;
        public string Hash => this.box.hash;
        public string type;
        public int X => this.box.x;
        public int Y => this.box.y;

        public AddBoxCommand()
        {
            this.box = new();
            this.type = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            reader.ReadShort();
            this.box!.Read(reader);
            this.type = reader.ReadUTF();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
