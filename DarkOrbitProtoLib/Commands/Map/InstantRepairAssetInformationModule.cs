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
            this.@byte.WriteShort((short)Id);
            this.@byte.WriteShort((short)0);
            // TODO: konnte nicht automatisch konvertieren: base.Read(reader);
            this.@byte.WriteInt(this.instantRepairCount >>> 13 | this.instantRepairCount << 19);
            // TODO: konnte nicht automatisch konvertieren: this.instantRepairCount = this.instantRepairCount << 13 | this.instantRepairCount >>> 19;
            return this.@byte.ToArray();
        }
    }
}
