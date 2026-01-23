namespace DarksideApi.DarkOrbit.Commands.Attack
{
    public class ShipSelectionCommand : Command    //1163
    {
        public new const short Id = 50;
        public int userId;
        public int shipType;        //var_2915
        public int shield;
        public int maxShield;       //name_93
        public long hitpoints;
        public long maxHitpoints;    //name_86
        public int nanoShield;      //var_4538
        public int maxNanoShield;   //var_4196
        public bool shieldSkill;    //var_3741

        public override void Read(ByteArray reader)
        {
            this.userId = reader.ReadInt();
            this.userId = this.userId << 8 | this.userId >>> 24;
            this.shipType = reader.ReadInt();
            this.shipType = this.shipType << 5 | this.shipType >>> 27;
            this.shield = reader.ReadInt();
            this.shield = this.shield << 7 | this.shield >>> 25;
            this.maxShield = reader.ReadInt();
            this.maxShield = this.maxShield << 15 | this.maxShield >>> 17;
            this.hitpoints = (long)reader.ReadDouble();
            this.maxHitpoints = (long)reader.ReadDouble();
            this.nanoShield = reader.ReadInt();
            this.nanoShield = this.nanoShield << 1 | this.nanoShield >>> 31;
            this.maxNanoShield = reader.ReadInt();
            this.maxNanoShield = this.maxNanoShield << 3 | this.maxNanoShield >>> 29;
            this.shieldSkill = reader.ReadBool();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
