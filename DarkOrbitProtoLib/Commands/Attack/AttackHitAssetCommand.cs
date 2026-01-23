namespace DarksideApi.DarkOrbit.Commands.Attack
{
    public class AttackHitAssetCommand : Command
    {
        public new const short Id = 5;
        public int assetId;
        public long hitpointsMax;
        public long hitpointsNow;

        public override void Read(ByteArray reader)
        {
            this.assetId = reader.ReadInt();
            this.assetId = this.assetId >>> 3 | this.assetId << 29;
            this.hitpointsMax = (long)reader.ReadDouble();
            this.hitpointsNow = (long)reader.ReadDouble();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
