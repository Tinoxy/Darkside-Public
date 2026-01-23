namespace DarksideApi.DarkOrbit.Commands.Attack
{
    public class AttackLaserRunCommand : Command    //1082
    {
        public new const int Id = 3;
        public int attackerId;    //name_161
        public int targetId;  //name_140
        public int laserColor;  //var_265
        public bool isDiminishedBySkilledShield;    //var_2841
        public bool skilledLaser;   //var_3551
        public int visualType;

        public override void Read(ByteArray reader)
        {
            this.attackerId = reader.ReadInt();
            this.attackerId = this.attackerId >>> 2 | this.attackerId << 30;
            this.targetId = reader.ReadInt();
            this.targetId = this.targetId << 1 | this.targetId >>> 31;
            this.laserColor = reader.ReadInt();
            this.laserColor = this.laserColor >>> 16 | this.laserColor << 16;
            this.isDiminishedBySkilledShield = reader.ReadBool();
            this.skilledLaser = reader.ReadBool();
            this.visualType = reader.ReadInt();
            this.visualType = this.visualType << 9 | this.visualType >>> 23;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
