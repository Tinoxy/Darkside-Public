using DarksideApi.DarkOrbit.Commands.Boxes;

namespace DarksideApi.DarkOrbit.Commands.Mines
{
    public class FireworkInitCommand : Command  //1412 NOT USED //AddFireworkBoxCommand
    {
        public new const short Id = 5613;
        private readonly AddMapEntityModule box;
        public string Hash => this.box.hash!;
        public int X => this.box.x!;
        public int Y => this.box.y!;
        public string var_2697;
        public int var_3727;
        public int typeId;

        public FireworkInitCommand()
        {
            this.box = new();
            this.var_2697 = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            reader.ReadShort();
            this.box.Read(reader);
            this.var_2697 = reader.ReadUTF();
            this.var_3727 = reader.ReadInt();
            this.var_3727 = this.var_3727 << 7 | this.var_3727 >>> 25;
            this.typeId = reader.ReadInt();
            this.typeId = this.typeId << 16 | this.typeId >>> 16;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
