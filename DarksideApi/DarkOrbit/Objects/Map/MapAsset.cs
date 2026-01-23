using DarksideApi.DarkOrbit.Commands;
using DarksideApi.Render;
using DarksideApi.Util;
using System.Numerics;

namespace DarksideApi.DarkOrbit.Objects.Map
{
    public class MapAsset(Api api, int assetId, string name, int x, int y, AssetType assetType, int clanRelation, int factionId, string clanTag = "") : IGameObject, IRenderable
    {
        private readonly Api api = api;

        public int AssetId { get; private set; } = assetId;
        public string Name { get; private set; } = name;
        public int X { get; set; } = x;
        public int Y { get; set; } = y;
        public Vector2 Position => new(this.X, this.Y);
        public AssetType AssetType { get; private set; } = assetType;
        public int ClanRelation { get; private set; } = clanRelation;
        public int FactionId { get; private set; } = factionId;
        public string ClanTag { get; private set; } = clanTag;
        public double CurrentDistance => this.HasApi ? HelpTools.CalculateDistance(this.api.Hero.X, this.api.Hero.Y, this.X, this.Y) : double.MaxValue;
        public double TargetDistance => this.HasApi ? HelpTools.CalculateDistance(this.api.Hero.TargetX, this.api.Hero.TargetY, this.X, this.Y) : double.MaxValue;
        public double PathDistance => this.HasApi ? HelpTools.CalculateDistance(this.api.Hero.LastPathElement, this.Position) : double.MaxValue;

        public int TempAvoidTries { get; set; } = 0;
        public int InstantRepairCount { get; set; } = 0;

        public bool HasApi => this.api != null;
        public bool IsBaseBuilding => this.IsCompanyHQ
            || this.IsHangar
            || this.IsTrader
            || this.IsRepairDock;
        public bool IsCompanyHQ => this.AssetType == AssetType.COMPANY_HQ_HOME || this.AssetType == AssetType.COMPANY_HQ_OUTPOST;
        public bool IsClanBase => this.AssetType == AssetType.BATTLESTATION;// || this.AssetType == AssetType.ASTEROID;
        public bool IsQuestGiver => this.AssetType == AssetType.QUESTGIVER;
        public bool IsTrader => this.AssetType == AssetType.ORE_TRADE_HOME || this.AssetType == AssetType.ORE_TRADE_OUTPOST || this.AssetType == AssetType.ORE_TRADE_INVISIBLE;
        public bool IsHangar => this.AssetType == AssetType.HANGAR_HOME || this.AssetType == AssetType.HANGAR_OUTPOST;
        public bool IsRepairDock => this.AssetType == AssetType.INSTANT_REPAIR_HOME || this.AssetType == AssetType.INSTANT_REPAIR_OUTPOST;
        public bool IsWreck => this.AssetType == AssetType.WRECK;
        public bool IsInstantRepairAvialable => this.IsRepairDock && this.InstantRepairCount > 0;
        public bool IsGOPGeneratorPositiv => this.AssetType == AssetType.GOP_GENERATOR_GREEN;
        public bool IsGOPGeneratorNegativ => this.AssetType == AssetType.GOP_GENERATOR_RED;
        public bool IsStationTurrent => this.AssetType == AssetType.STATION_TURRET_LARGE_HOME || this.AssetType == AssetType.STATION_TURRET_LARGE_OUTPOST
            || this.AssetType == AssetType.STATION_TURRET_SMALL_HOME || this.AssetType == AssetType.STATION_TURRET_SMALL_OUTPOST;
        public bool IsHpBeacon => this.AssetType == AssetType.PET_HP_BEACON || this.AssetType == AssetType.HEALING_POD;

        public bool IsFriendly => (FactionId == this.api.Hero.FactionId
                        || ClanTag == this.api.Hero.ClanTag
                        || ClanRelation == CommandConstants.CLAN_RELATION_ALLIED
                        || ClanRelation == CommandConstants.CLAN_RELATION_NON_AGGRESSION_PACT);
        public bool IsEnemy => !IsFriendly
                        || this.ClanRelation == CommandConstants.CLAN_RELATION_AT_WAR
                        || (this.ClanRelation == CommandConstants.CLAN_RELATION_NONE && FactionId != this.api.Hero.FactionId);//this.ClanRelation == CommandConstants.CLAN_RELATION_AT_WAR || (FactionId != api.Hero.FactionId && ClanRelation != CommandConstants.CLAN_RELATION_ALLIED && ClanRelation != CommandConstants.CLAN_RELATION_NON_AGGRESSION_PACT && ClanRelation != CommandConstants.CLAN_RELATION_NONE);

        //Render
        public DateTime ActionStartTime { get; set; }
        public double Radius { get; set; }

        public static MapAsset Empty()
        {
            return new MapAsset(null, 0, string.Empty, 0, 0, 0, 0, 0);
        }
    }
}
