namespace DarksideApi.DarkOrbit.Commands.Attack
{
    public class ShipSelectRequest : Command  //2179
    {
        public new const short Id = 165;
        public int targetId;  //name_140
        public int targetX;       //name_156
        public int targetY;       //name_148
        public int posX;  //var_2089
        public int posY;  //var_2738
        public int clickX;//var_2909
        public int clickY;//var_2656
        public int radius;

        public ShipSelectRequest(int targetId, int targetX, int targetY, int posX, int posY, int clickX, int clickY, int radius)
        {
            this.targetId = targetId;
            this.targetX = targetX;
            this.targetY = targetY;
            this.posX = posX;
            this.posY = posY;
            this.clickX = clickX;
            this.clickY = clickY;
            this.radius = radius;
        }

        public override void Read(ByteArray reader)
        {
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteInt(this.targetId << 5 | this.targetId >>> 27);
            this.@byte.WriteInt(this.targetX >>> 2 | this.targetX << 30);
            this.@byte.WriteInt(this.targetY >>> 10 | this.targetY << 22);
            this.@byte.WriteInt(this.posX >>> 1 | this.posX << 31);
            this.@byte.WriteInt(this.posY >>> 1 | this.posY << 31);
            this.@byte.WriteInt(this.clickX << 15 | this.clickX >>> 17);
            this.@byte.WriteInt(this.clickY << 2 | this.clickY >>> 30);
            this.@byte.WriteInt(this.radius >>> 6 | this.radius << 26);
            return this.@byte.ToArray();
        }
    }
}
