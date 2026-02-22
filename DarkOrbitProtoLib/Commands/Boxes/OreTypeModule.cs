namespace DarksideApi.DarkOrbit.Commands.Boxes
{
    public class OreTypeModule : Command    //OreTypeModule
    {
        public new const short Id = 1922;
        public short type;  //name_102

        public OreTypeModule(short type)
        {
            this.type = type;
        }

        public override void Read(ByteArray reader)
        {
            this.type = reader.ReadShort();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteShort(this.type);
            return this.@byte.ToArray();
        }
    }
}
