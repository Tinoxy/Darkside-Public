namespace DarksideApi.DarkOrbit.Commands.Group
{
    public class LocationModule : PlayerAttributeModule
    {
        public new const short Id = 17470;
        public int mapId;
        public int x;
        public int y;

        public override void Read(ByteArray reader)
        {
            reader.ReadShort();
            base.Read(reader);

            this.y = reader.ReadInt();
            this.y = this.y << 11 | this.y >>> 21;
            this.mapId = reader.ReadInt();
            this.mapId = this.mapId << 5 | this.mapId >>> 27;
            this.x = reader.ReadInt();
            this.x = this.x << 13 | this.x >>> 19;
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort((short)Id);
            this.@byte.WriteShort((short)0);
            this.@byte.WriteInt(this.y >>> 11 | this.y << 21);
            this.@byte.WriteInt(this.mapId >>> 5 | this.mapId << 27);
            this.@byte.WriteInt(this.x >>> 13 | this.x << 19);
            return this.@byte.ToArray();
        }
    }
}
