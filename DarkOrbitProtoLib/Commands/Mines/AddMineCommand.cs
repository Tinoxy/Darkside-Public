using DarksideApi.DarkOrbit.Commands.Boxes;

namespace DarksideApi.DarkOrbit.Commands.Mines
{
    public class AddMineCommand : Command  //AddMineCommand
    {
        public new const short Id = 28996;
        private readonly AddMapEntityModule box;
        public int X => this.box!.x;
        public int Y => this.box!.y;
        public string Hash => this.box!.hash!;
        public bool maxSkill5b = false;
        public int typeId = 0;
        public bool maxSkill1 = false;
        //map.method_6408(new class_564(param1.x,param1.y,param1.typeId,param1.hash,param1.var_4462 ? 1 : 0,param1.var_4108 ? 1 : 0));

        public AddMineCommand()
        {
            this.box = new();
        }

        public override void Read(ByteArray reader)
        {
            reader.ReadShort();
            this.box.Read(reader);

            this.maxSkill1 = reader.ReadBool();
            this.maxSkill5b = reader.ReadBool();
            this.typeId = reader.ReadInt();
            this.typeId = this.typeId >>> 16 | this.typeId << 16;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
