namespace DarksideApi.DarkOrbit.Commands.Attack
{
    public class AttackHitCommand : Command   //1165
    {
        public new const short Id = 28;
        public AttackTypeModule attackType;
        public int attackerId;  //name_161
        public int victimId;  //var_1766
        public int vicitimNanonShield;  //var_5852
        public int victimShield;  //var_3841
        public long victimHitpoints;  //var_5008
        public int damage;
        public bool skilled;
        public bool strike;

        public AttackHitCommand()
        {
            this.attackType = new();
        }

        public override void Read(ByteArray reader)
        {
            if (reader.ReadShort() != 0) //1162
            {
                this.attackType.Read(reader);
            }
            this.attackerId = reader.ReadInt();
            this.attackerId = this.attackerId << 7 | this.attackerId >>> 25;
            this.victimId = reader.ReadInt();
            this.victimId = this.victimId >>> 3 | this.victimId << 29;
            this.victimHitpoints = (long)reader.ReadDouble();
            this.victimShield = reader.ReadInt();
            this.victimShield = this.victimShield << 13 | this.victimShield >>> 19;
            this.vicitimNanonShield = reader.ReadInt();
            this.vicitimNanonShield = this.vicitimNanonShield >>> 8 | this.vicitimNanonShield << 24;
            this.damage = reader.ReadInt();
            this.damage = this.damage >>> 8 | this.damage << 24;
            this.skilled = reader.ReadBool();
            this.strike = reader.ReadBool();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
