namespace DarksideApi.DarkOrbit.Commands.Group
{
    public class HealthModule : PlayerAttributeModule
    {
        public new const short Id = 19647;
        public int shieldMax;
        public int shield;
        public long hp;
        public long hpMax;
        public int nanoShield;
        public int nanoShieldMax;

        public override void Read(ByteArray reader)
        {
            reader.ReadShort(); //PlayerAttribute/471 -> Empty
            base.Read(reader);

            this.nanoShield = reader.ReadInt();
            this.nanoShield = this.nanoShield << 26 | this.nanoShield >>> 6;
            this.nanoShieldMax = reader.ReadInt();
            this.nanoShieldMax = this.nanoShieldMax << 24 | this.nanoShieldMax >>> 8;
            this.shieldMax = reader.ReadInt();
            this.shieldMax = this.shieldMax << 14 | this.shieldMax >>> 18;
            this.hpMax = (long)reader.ReadDouble();
            this.hp = (long)reader.ReadDouble();
            this.shield = reader.ReadInt();
            this.shield = this.shield << 16 | this.shield >>> 16;      
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort((short)Id);
            this.@byte.WriteShort((short)0);
            this.@byte.WriteInt(this.nanoShield >>> 26 | this.nanoShield << 6);
            this.@byte.WriteInt(this.nanoShieldMax >>> 24 | this.nanoShieldMax << 8);
            this.@byte.WriteInt(this.shieldMax >>> 14 | this.shieldMax << 18);
            this.@byte.WriteInt(this.shield >>> 16 | this.shield << 16);
            return this.@byte.ToArray();
        }
    }
}
