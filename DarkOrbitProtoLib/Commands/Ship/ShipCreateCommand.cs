using System.Text;

namespace DarksideApi.DarkOrbit.Commands.Ship
{
    public class ShipCreateCommand : Command  //580
    {
        public new const short Id = 83;
        public int userId;
        public string typeId;
        public int expansionStage;
        public string clanTag;
        public string userName;
        public int x;
        public int y;
        public int factionId;
        public int clanId;
        public int dailyRank;
        public bool warnBox;
        public ClanRelationModule clanRelationModule;
        public int galaxyGatesDone;
        public bool useSystemFont;
        public bool npc;
        public bool cloaked;
        public int motherShipId;
        public int positionIndex;
        public string designSet;
        public List<VisualModifierCommand> modifier;
        public MinimapStrategyModule minimapStategyModule;
        public string specialNpcType;

        public ShipCreateCommand()
        {
            this.typeId = string.Empty;
            this.clanTag = string.Empty;
            this.userName = string.Empty;
            this.clanRelationModule = new();
            this.modifier = [];
            this.designSet = string.Empty;
            this.minimapStategyModule = new();
            this.specialNpcType = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            this.userId = reader.ReadInt();
            this.userId = this.userId >>> 15 | this.userId << 17;
            this.typeId = reader.ReadUTF();
            this.expansionStage = reader.ReadInt();
            this.expansionStage = this.expansionStage >>> 8 | this.expansionStage << 24;
            this.clanTag = reader.ReadUTF();
            this.userName = reader.ReadUTF();
            this.x = reader.ReadInt();
            this.x = this.x << 6 | this.x >>> 26;
            this.y = reader.ReadInt();
            this.y = this.y << 15 | this.y >>> 17;
            this.factionId = reader.ReadInt();
            this.factionId = this.factionId >>> 1 | this.factionId << 31;
            this.clanId = reader.ReadInt();
            this.clanId = this.clanId >>> 15 | this.clanId << 17;
            this.dailyRank = reader.ReadInt();
            this.dailyRank = this.dailyRank << 1 | this.dailyRank >>> 31;
            this.warnBox = reader.ReadBool();
            if (reader.ReadShort() != 0)
            {
                this.clanRelationModule.Read(reader);
            }
            this.galaxyGatesDone = reader.ReadInt();
            this.galaxyGatesDone = this.galaxyGatesDone >>> 12 | this.galaxyGatesDone << 20;
            this.useSystemFont = reader.ReadBool();
            this.npc = reader.ReadBool();
            this.cloaked = reader.ReadBool();
            this.motherShipId = reader.ReadInt();
            this.motherShipId = this.motherShipId >>> 10 | this.motherShipId << 22;
            this.positionIndex = reader.ReadInt();
            this.positionIndex = this.positionIndex >>> 1 | this.positionIndex << 31;
            this.designSet = reader.ReadUTF();
            this.modifier = [];
            var i = 0;
            int max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                var mod = new VisualModifierCommand();
                mod.Read(reader);
                this.modifier.Add(mod);
                i++;
            }
            if (reader.ReadShort() != 0)
            {
                this.minimapStategyModule.Read(reader);
            }
            this.specialNpcType = reader.ReadUTF();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"ShipCreateCommand: {this.userId}");
            sb.AppendLine($"TypeId: {this.typeId}");
            sb.AppendLine($"ExpansionStage: {this.expansionStage}");
            sb.AppendLine($"ClanTag: {this.clanTag}");
            sb.AppendLine($"UserName: {this.userName}");
            sb.AppendLine($"X: {this.x}");
            sb.AppendLine($"Y: {this.y}");
            sb.AppendLine($"FactionId: {this.factionId}");
            sb.AppendLine($"ClanId: {this.clanId}");
            sb.AppendLine($"DailyRank: {this.dailyRank}");
            sb.AppendLine($"WarnBox: {this.warnBox}");
            if (this.clanRelationModule != null)
                sb.AppendLine(this.clanRelationModule.ToString());
            sb.AppendLine($"GalaxyGatesDone: {this.galaxyGatesDone}");
            sb.AppendLine($"UseSystemFont: {this.useSystemFont}");
            sb.AppendLine($"Npc: {this.npc}");
            sb.AppendLine($"Cloaked: {this.cloaked}");
            sb.AppendLine($"MotherShipId: {this.motherShipId}");
            sb.AppendLine($"PositionIndex: {this.positionIndex}");
            sb.AppendLine($"DesignSet: {this.designSet}");
            sb.AppendLine($"SpecialNpcType: {this.specialNpcType}");
            foreach (VisualModifierCommand mod in this.modifier)
                sb.AppendLine(mod.ToString());
            if (this.minimapStategyModule != null)
                sb.AppendLine(this.minimapStategyModule.ToString());
            return sb.ToString();
        }
    }
}
