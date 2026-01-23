namespace DarksideApi.DarkOrbit.Commands.Map
{
    public class InstantRepairAssetInformationModule : AssetInformationModule  //2271
    {
        public new const short Id = 20098;
        public int instantRepairCount;

        public override void Read(ByteArray reader)
        {
            reader.ReadShort(); //class_1922 -> Empty
            base.Read(reader);

            this.instantRepairCount = reader.ReadInt();
            this.instantRepairCount = this.instantRepairCount << 13 | this.instantRepairCount >>> 19;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
