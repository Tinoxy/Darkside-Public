namespace DarksideApi.DarkOrbit.Commands.Hero
{
    public class HitpointInfoCommand : Command    //1169
    {
        public new const short Id = 1300;
        public long hp;
        public long maxHp;   //name_86
        public int nanoShield;  //var_4538
        public int maxNanoShield;   //var_393

        public override void Read(ByteArray reader)
        {
            this.hp = (long)reader.ReadDouble();
            this.maxHp = (long)reader.ReadDouble();
            this.nanoShield = reader.ReadInt();
            this.nanoShield = this.nanoShield << 13 | this.nanoShield >>> 19;
            this.maxNanoShield = reader.ReadInt();
            this.maxNanoShield = this.maxNanoShield >>> 6 | this.maxNanoShield << 26;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
