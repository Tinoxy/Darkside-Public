namespace DarksideApi.DarkOrbit.Commands.Map
{
    public class MoveRequest : Command //504
    {
        public new const short Id = 82;
        public int y;   //name_148
        public int x;   //name_156
        public int currentX;    //var_1274
        public int currentY;    //var_1331

        public MoveRequest(int x, int y, int currentX, int currentY)
        {
            this.x = x;
            this.y = y;
            this.currentX = currentX;
            this.currentY = currentY;
        }

        public override void Read(ByteArray reader)
        {
            this.currentX = reader.ReadInt();
            this.currentX = this.currentX >>> 11 | this.currentX << 21;
            this.y = reader.ReadInt();
            this.y = this.y << 15 | this.y >>> 17;
            this.x = reader.ReadInt();
            this.x = this.x >>> 13 | this.x << 19;
            this.currentY = reader.ReadInt();
            this.currentY = this.currentY >>> 10 | this.currentY << 22;
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteInt(this.currentX << 11 | this.currentX >>> 21);
            this.@byte.WriteInt(this.y >>> 15 | this.y << 17);
            this.@byte.WriteInt(this.x << 13 | this.x >>> 19);
            this.@byte.WriteInt(this.currentY << 10 | this.currentY >>> 22);
            return this.@byte.ToArray();
        }
    }
}
