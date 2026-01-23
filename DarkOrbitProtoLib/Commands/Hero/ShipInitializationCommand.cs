using DarksideApi.DarkOrbit.Commands.Ship;

namespace DarksideApi.DarkOrbit.Commands.Hero
{
    public class ShipInitializationCommand : Command  //585
    {
        public new const short Id = 49;
        public int userId;
        public string userName;
        public string typeId;
        public int speed;
        public int shield;
        public int maxShield;   //name_93
        public long currentHitpoints;    //var_673
        public long maxHitpoints;    //var_22
        public int freeCargoSpace;  //var_2081
        public int maxCargoSpace;   //var_5012
        public int nanoShield;  //var_4538
        public int maxNanoShield;   //var_4196
        public int x;
        public int y;
        public int mapId;
        public int factionId;
        public int clanId;  //name_89
        public int expansionStage;
        public bool premium;
        public double experiencePoints; //var_4001
        public double honor;    //var_5338
        public int level;
        public double credits;
        public double uridium;
        public float jackpot;
        public int dailyRank;    //name_38
        public string clanTag;
        public int galaxyGatesFinished; //var_1303
        public bool useSystemFont;
        public bool cloaked;
        public bool refreshMap; //var_2921
        public int captainEnergy;   //var_422
        public int maxCaptainEnergy;    //var_2660
        public List<VisualModifierCommand> modifier;

        public ShipInitializationCommand()
        {
            this.modifier = [];
            this.userName = string.Empty;
            this.clanTag = string.Empty;
            this.typeId = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            this.userId = reader.ReadInt();
            this.userId = this.userId >>> 14 | this.userId << 18;
            this.userName = reader.ReadUTF();
            this.typeId = reader.ReadUTF();
            this.speed = reader.ReadInt();
            this.speed = this.speed << 12 | this.speed >>> 20;
            this.shield = reader.ReadInt();
            this.shield = this.shield << 14 | this.shield >>> 18;
            this.maxShield = reader.ReadInt();
            this.maxShield = this.maxShield << 13 | this.maxShield >>> 19;
            this.currentHitpoints = (long)reader.ReadDouble();
            this.maxHitpoints = (long)reader.ReadDouble();
            this.freeCargoSpace = reader.ReadInt();
            this.freeCargoSpace = this.freeCargoSpace << 10 | this.freeCargoSpace >>> 22;
            this.maxCargoSpace = reader.ReadInt();
            this.maxCargoSpace = this.maxCargoSpace >>> 14 | this.maxCargoSpace << 18;
            this.nanoShield = reader.ReadInt();
            this.nanoShield = this.nanoShield >>> 11 | this.nanoShield << 21;
            this.maxNanoShield = reader.ReadInt();
            this.maxNanoShield = this.maxNanoShield >>> 15 | this.maxNanoShield << 17;
            this.x = reader.ReadInt();
            this.x = this.x >>> 14 | this.x << 18;
            this.y = reader.ReadInt();
            this.y = this.y >>> 4 | this.y << 28;
            this.mapId = reader.ReadInt();
            this.mapId = this.mapId << 11 | this.mapId >>> 21;
            this.factionId = reader.ReadInt();
            this.factionId = this.factionId << 5 | this.factionId >>> 27;
            this.clanId = reader.ReadInt();
            this.clanId = this.clanId >>> 3 | this.clanId << 29;
            this.expansionStage = reader.ReadInt();
            this.expansionStage = this.expansionStage >>> 5 | this.expansionStage << 27;
            this.premium = reader.ReadBool();
            this.experiencePoints = reader.ReadDouble();
            this.honor = reader.ReadDouble();
            this.level = reader.ReadInt();
            this.level = this.level >>> 15 | this.level << 17;
            this.credits = reader.ReadDouble();
            this.uridium = reader.ReadDouble();
            this.jackpot = reader.ReadFloat();
            this.dailyRank = reader.ReadInt();
            this.dailyRank = this.dailyRank << 14 | this.dailyRank >>> 18;
            this.clanTag = reader.ReadUTF();
            this.galaxyGatesFinished = reader.ReadInt();
            this.galaxyGatesFinished = this.galaxyGatesFinished << 5 | this.galaxyGatesFinished >>> 27;
            this.useSystemFont = reader.ReadBool();
            this.cloaked = reader.ReadBool();
            this.refreshMap = reader.ReadBool();
            this.captainEnergy = reader.ReadInt();
            this.captainEnergy = this.captainEnergy << 9 | this.captainEnergy >>> 23;
            this.maxCaptainEnergy = reader.ReadInt();
            this.maxCaptainEnergy = this.maxCaptainEnergy << 1 | this.maxCaptainEnergy >>> 31;
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
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
