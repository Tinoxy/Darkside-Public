namespace DarksideApi.DarkOrbit.Commands.Pet
{
    public class PetLevelUpCommand : Command    //1053
    {
        public new const short Id = 160;
        public int level = 0;   //var_5005
        public double var_1555 = 0;
        public int name_184 = 0;
        public int var_2718 = 0;

        public override void Read(ByteArray reader)
        {
            this.level = reader.ReadShort();
            this.level = 65535 & ((65535 & this.level) << 14 % 16 | (65535 & this.level) >>> 16 - 14 % 16);
            this.level = this.level > 32767 ? this.level - 65536 : this.level;
            this.var_1555 = reader.ReadDouble();
            this.name_184 = reader.ReadShort();
            this.name_184 = 65535 & ((65535 & this.name_184) >>> 16 % 16 | (65535 & this.name_184) << 16 - 16 % 16);
            this.name_184 = this.name_184 > 32767 ? this.name_184 - 65536 : this.name_184;
            this.var_2718 = reader.ReadShort();
            this.var_2718 = 65535 & ((65535 & this.var_2718) >>> 3 % 16 | (65535 & this.var_2718) << 16 - 3 % 16);
            this.var_2718 = this.var_2718 > 32767 ? this.var_2718 - 65536 : this.var_2718;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
