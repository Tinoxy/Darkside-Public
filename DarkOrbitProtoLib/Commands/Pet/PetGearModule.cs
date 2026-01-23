namespace DarksideApi.DarkOrbit.Commands.Pet
{
    public class PetGearModule : Command //554
    {
        public new const short Id = 164;
        public short module;    //name_104

        public PetGearModule(short module)
        {
            this.module = module;
        }

        public PetGearModule()
        {
        }

        public override void Read(ByteArray reader)
        {
            this.module = reader.ReadShort();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteShort(this.module);
            return this.@byte.ToArray();
        }
    }
}
