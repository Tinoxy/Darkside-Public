using DarksideApi.DarkOrbit.Commands.Client;
using System.Diagnostics;

namespace DarksideApi.DarkOrbit.Commands.Map
{
    public class MapAssetActionAvailableCommand : Command  //1300
    {
        public new const short Id = 6530;
        public int mapAssetId;
        public int state;
        public bool activatable;
        public MessageKeyListWildcardCommand toolTip;
        public AssetInformationModule additionalInformation;

        public MapAssetActionAvailableCommand()
        {
            this.toolTip = new();
            this.additionalInformation = new();
        }

        public override void Read(ByteArray reader)
        {
            this.mapAssetId = reader.ReadInt();
            this.mapAssetId = this.mapAssetId << 12 | this.mapAssetId >>> 20;
            this.state = reader.ReadShort();
            this.activatable = reader.ReadBool();
            if (reader.ReadShort() != 0)
            {
                this.toolTip.Read(reader);
            }
            this.additionalInformation = this.GetAssetInformationModule(reader.ReadShort());
            this.additionalInformation.Read(reader);
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        private AssetInformationModule GetAssetInformationModule(short id)
        {
            switch (id)
            {
                case InstantRepairAssetInformationModule.Id:
                    return new InstantRepairAssetInformationModule();
                case AssetInformationModule.Id:
                    return new AssetInformationModule();
                default:
                    Debug.WriteLine("AssetInformationModule not found: " + id);
                    return new AssetInformationModule();
            }
        }
    }
}
