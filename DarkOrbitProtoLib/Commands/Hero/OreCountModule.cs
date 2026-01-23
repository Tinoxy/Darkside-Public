using DarksideApi.DarkOrbit.Commands.Boxes;

namespace DarksideApi.DarkOrbit.Commands.Hero
{
    public class OreCountModule : Command    //569
    {
        public new const short Id = 105;
        private readonly Command typeModule;
        public int OreType => ((OreTypeModule)this.typeModule).type;
        public long count;

        public OreCountModule()
        {
            this.typeModule = new OreTypeModule(-1);
        }

        public OreCountModule(short oreType, long count)
        {
            this.typeModule = new OreTypeModule(oreType);
            this.count = count;
        }

        public override void Read(ByteArray reader)
        {
            reader.ReadShort();
            this.typeModule.Read(reader);
            this.count = (long)reader.ReadDouble();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteByteArray(this.typeModule.Write());
            this.@byte.WriteDouble(this.count);
            return this.@byte.ToArray();
        }
    }
}
