using DarksideApi.DarkOrbit.Commands.Client;
using DarksideApi.DarkOrbit.Commands.Ship;

namespace DarksideApi.DarkOrbit.Commands.Map
{
    public class AssetCreateCommand : Command //AssetCreateCommand
    {
        public new const short Id = -27112;
        public int expansionStage;  //var_2718
        public bool showBubble;   //var_3800
        public ClanRelationModule clanRelation; //name_122
        public string clanTag;
        public List<VisualModifierCommand> modifier;
        public bool invisible;  //var_3139
        public int assetId;
        public int x;   //var_2088
        public bool visibleOnWarnRadar; //var_5550
        public int y;   //var_2738
        public int designId;    //name_184
        public bool detectedByWarnRadar;    //var_4162
        public DestructionTypeModule type;
        public int clanId;  //name_89
        public string userName;
        public int factionId;

        public AssetCreateCommand()
        {
            this.clanRelation = new();
            this.modifier = [];
            this.clanTag = string.Empty;
            this.type = new();
            this.userName = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            this.y = reader.ReadInt();
            this.y = this.y << 11 | this.y >>> 21;
            this.invisible = reader.ReadBool();
            this.detectedByWarnRadar = reader.ReadBool();
            if (reader.ReadShort() != 0)
            {
                this.type.Read(reader);
            }
            this.clanTag = reader.ReadUTF();
            var i = 0;
            var max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                var visualModifier = new VisualModifierCommand();
                visualModifier.Read(reader);
                this.modifier.Add(visualModifier);
                i++;
            }
            this.clanId = reader.ReadInt();
            this.clanId = this.clanId >>> 2 | this.clanId << 30;
            this.factionId = reader.ReadInt();
            this.factionId = this.factionId >>> 1 | this.factionId << 31;
            this.showBubble = reader.ReadBool();
            this.visibleOnWarnRadar = reader.ReadBool();
            if (reader.ReadShort() != 0)
            {
                this.clanRelation.Read(reader);
            }
            this.assetId = reader.ReadInt();
            this.assetId = this.assetId >>> 1 | this.assetId << 31;
            this.userName = reader.ReadUTF();
            this.x = reader.ReadInt();
            this.x = this.x << 15 | this.x >>> 17;
            this.designId = reader.ReadInt();
            this.designId = this.designId << 5 | this.designId >>> 27;
            this.expansionStage = reader.ReadInt();
            this.expansionStage = this.expansionStage << 15 | this.expansionStage >>> 17;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
