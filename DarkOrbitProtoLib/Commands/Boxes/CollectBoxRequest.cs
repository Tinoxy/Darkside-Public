namespace DarksideApi.DarkOrbit.Commands.Boxes
{
    public class CollectBoxRequest : Command    //516
    {
        public new const short Id = 26;
        public string hash; //var_4860
        public int x;       //var_2089
        public int y;       //var_2738
        public int boxX;    //var_5364
        public int boxY;    //var_5470

        public CollectBoxRequest()
        {
            this.hash = string.Empty;
        }

        public CollectBoxRequest(string hash, int x, int y, int boxX, int boxY)
        {
            this.hash = hash;
            this.x = x;
            this.y = y;
            this.boxX = boxX;
            this.boxY = boxY;
        }

        public override void Read(ByteArray reader)
        {
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteUTF(this.hash!);
            this.@byte.WriteInt(this.boxX >>> 1 | this.boxX << 31);
            this.@byte.WriteInt(this.boxY >>> 6 | this.boxY << 26);
            this.@byte.WriteInt(this.x >>> 10 | this.x << 22);
            this.@byte.WriteInt(this.y << 7 | this.y >>> 25);
            return this.@byte.ToArray();
        }
    }
}
