namespace DarksideApi.DarkOrbit.Commands.Attack
{
    public class AttackRocketRequest : Command    //1917 
    {
        public new const short Id = 89;
        public int userId;  //name_140
        public int x;   //name_156
        public int y;   //name_148

        public AttackRocketRequest(int rocketId, int x, int y)
        {
            this.userId = rocketId;
            this.x = x;
            this.y = y;
        }

        public override void Read(ByteArray reader)
        {
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteInt(this.userId << 2 | this.userId >>> 30);
            this.@byte.WriteInt(this.x << 12 | this.x >>> 20);
            this.@byte.WriteInt(this.y >>> 14 | this.y << 18);
            return this.@byte.ToArray();
        }
    }
}
