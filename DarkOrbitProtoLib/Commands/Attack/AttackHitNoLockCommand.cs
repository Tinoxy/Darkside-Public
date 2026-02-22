namespace DarksideApi.DarkOrbit.Commands.Attack
{
    public class AttackHitNoLockCommand : Command   //1165
    {
        public new const short Id = -100;
        public AttackTypeModule attackType;
        public int attackerId;  //name_161
        public int victimId;  //var_1766
        public int vicitimNanonShield;  //var_5852
        public int victimMaxNanonShield;  //var_5853
        public int victimShield;  //var_3841
        public int victimMaxShield;  //var_3842
        public long victimHitpoints;  //var_5008
        public long victimMaxHitpoints;  //var_5009
        public int damage;
        public bool skilled;
        public bool strike;

        public AttackHitNoLockCommand()
        {
            this.attackType = new();
        }

        public override void Read(ByteArray reader)
        {
            this.victimMaxShield = reader.ReadInt();
            this.victimMaxShield = this.victimMaxShield << 28 | this.victimMaxShield >>> 4;
            this.vicitimNanonShield = reader.ReadInt();
            this.vicitimNanonShield = this.vicitimNanonShield >>> 28 | this.vicitimNanonShield << 4;
            this.victimMaxHitpoints = (long)reader.ReadDouble();
            if (reader.ReadShort() != 0) //1162
            {
                this.attackType.Read(reader);
            }
            this.attackerId = reader.ReadInt();
            this.attackerId = this.attackerId << 11 | this.attackerId >>> 21;
            this.victimId = reader.ReadInt();
            this.victimId = this.victimId >>> 13 | this.victimId << 19;
            this.damage = reader.ReadInt();
            this.damage = this.damage >>> 26 | this.damage << 6;
            this.victimShield = reader.ReadInt();
            this.victimShield = this.victimShield << 6 | this.victimShield >>> 26;
            this.skilled = reader.ReadBool();
            this.strike = reader.ReadBool();
            this.victimMaxNanonShield = reader.ReadInt();
            this.victimMaxNanonShield = this.victimMaxNanonShield >>> 14 | this.victimMaxNanonShield << 18;
            this.victimHitpoints = (long)reader.ReadDouble();        
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
