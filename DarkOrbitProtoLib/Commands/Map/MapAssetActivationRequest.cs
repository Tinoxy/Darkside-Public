namespace DarksideApi.DarkOrbit.Commands.Map
{
    public class MapAssetActivationRequest : Command
    {
        public new const short Id = 30282;
        public int mapAssetId;

        public MapAssetActivationRequest(int mapAssetId)
        {
            this.mapAssetId = mapAssetId;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteInt(this.mapAssetId >>> 27 | this.mapAssetId << 5);
            return this.@byte.ToArray();
        }
    }
}
