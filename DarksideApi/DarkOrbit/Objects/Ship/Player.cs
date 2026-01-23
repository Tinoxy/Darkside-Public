using DarksideApi.DarkOrbit.Commands;

namespace DarksideApi.DarkOrbit.Objects.Ship
{
    public class Player(Api api, int userId, string username, int x, int y, string shipname, bool cloaked, int clanId, string clanTag, int factionId, int clanRelation) : Ship(api, userId, username, x, y, shipname), IGameObject
    {
        //Status
        public bool Cloaked { get; set; } = cloaked;

        // Social
        public int ClanId { get; private set; } = clanId;
        public string ClanTag { get; private set; } = clanTag;
        public int FactionId { get; private set; } = factionId;
        public int ClanRelation { get; private set; } = clanRelation;

        public bool IsEnemy => this.ClanTag != this.api.Hero.ClanTag && this.FactionId != this.api.Hero.FactionId && (this.ClanRelation == CommandConstants.CLAN_RELATION_AT_WAR || this.ClanRelation == CommandConstants.CLAN_RELATION_NONE);

        public static Player Empty()
        {
            return new Player(null, 0, string.Empty, 0, 0, string.Empty, false, 0, string.Empty, 0, CommandConstants.CLAN_RELATION_NONE);
        }

        public override string ToString() => $"{this.Username} (ID: {this.UserId}, Clan: {this.ClanTag}, Faction: {this.FactionId}, Relation: {this.ClanRelation}), [{base.ToString()}]";
    }
}
