namespace DarksideApi.DarkOrbit.Commands.Boxes
{
    public class OreInitCommand : Command
    {
        public new const short Id = 147; //AddOreCommand
        private readonly AddMapEntityModule box;
        private readonly OreTypeModule typeModule;
        public int X => this.box.x;
        public int Y => this.box.y;
        public string Hash => this.box.hash;
        public int Type => this.typeModule.type;

        public OreInitCommand()
        {
            this.box = new();
            this.typeModule = new(-1);
        }

        public override void Read(ByteArray reader)
        {
            reader.ReadShort();
            this.box!.Read(reader);

            reader.ReadShort(); //125
            this.typeModule!.Read(reader);
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
