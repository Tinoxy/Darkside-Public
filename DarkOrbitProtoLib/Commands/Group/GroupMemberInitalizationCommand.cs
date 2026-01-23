namespace DarksideApi.DarkOrbit.Commands.Group
{
    public class GroupMemberInitalizationCommand : Command
    {
        public new const short Id = 3178;
        public HealthModule health;
        public bool cloaked;
        public bool active;
        public RaidGateGameStageModule raidGateGameStage;
        public int id;
        public TargetModule target;
        public ShipIconModule faction;
        public LocationModule location;
        public bool inFight;
        public int level;
        public bool blockingInvitations;
        public string name;
        public ClanModule clan;
        public ShipIconModule shipIcon;
        public bool loggedOut;

        public GroupMemberInitalizationCommand()
        {
            this.health = new();
            this.raidGateGameStage = new();
            this.target = new();
            this.faction = new();
            this.location = new();
            this.name = string.Empty;
            this.clan = new();
            this.shipIcon = new();
        }

        public override void Read(ByteArray reader)
        {
            this.blockingInvitations = reader.ReadBool();
            if (reader.ReadShort() != 0)
                this.location.Read(reader);
            this.id = reader.ReadInt();
            this.id = this.id << 10 | this.id >>> 22;
            if (reader.ReadShort() != 0)
                this.health.Read(reader);
            if (reader.ReadShort() != 0)
                this.shipIcon.Read(reader);
            this.inFight = reader.ReadBool();
            if (reader.ReadShort() != 0)
                this.raidGateGameStage.Read(reader);
            if (reader.ReadShort() != 0)
                this.clan.Read(reader);
            this.name = reader.ReadUTF();
            if (reader.ReadShort() != 0)
                this.target.Read(reader);
            this.loggedOut = reader.ReadBool();
            this.active = reader.ReadBool();
            this.cloaked = reader.ReadBool();
            this.level = reader.ReadInt();
            this.level = this.level << 11 | this.level >>> 21;
            if (reader.ReadShort() != 0)
                this.faction.Read(reader);
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
