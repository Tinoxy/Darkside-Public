namespace DarksideApi.DarkOrbit.Commands.Attack
{
    public class AttackLaserRequest : Command    //1889
    {
        public new const short Id = 4;
        public int userId;    //name_140
        public int x;   //name_156
        public int y;   //name_148

        public AttackLaserRequest(int userId, int x, int y)
        {
            this.userId = userId;
            this.x = x;
            this.y = y;
        }

        public override void Read(ByteArray reader)
        {
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteInt(this.userId << 15 | this.userId >>> 17);
            this.@byte.WriteInt(this.x << 9 | this.x >>> 23);
            this.@byte.WriteInt(this.y << 9 | this.y >>> 23);
            return this.@byte.ToArray();
        }
    }
}
