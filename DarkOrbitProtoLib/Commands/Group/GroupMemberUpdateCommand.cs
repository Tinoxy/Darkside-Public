namespace DarksideApi.DarkOrbit.Commands.Group
{
    public class GroupMemberUpdateCommand : Command //GroupMemberUpdateCommand
    {
        public new const short Id = -17087;
        public List<PlayerAttributeModule> updates;
        public int playerId;

        public GroupMemberUpdateCommand()
        {
            this.updates = [];
            this.playerId = 0;
        }

        public override void Read(ByteArray reader)
        { 
            var i = 0;
            var max = reader.ReadByte();
            while (i < max)
            {
                var module = GetPlayerAttributeModule(reader.ReadShort());
                module.Read(reader);
                this.updates.Add(module);
                i++;
            }
            this.playerId = reader.ReadInt();
            this.playerId = this.playerId << 9 | this.playerId >>> 23;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        private static PlayerAttributeModule GetPlayerAttributeModule(int id)
        {
            return id switch
            {
                ClanModule.Id => new ClanModule(),
                HealthModule.Id => new HealthModule(),
                LocationModule.Id => new LocationModule(),
                RaidGateGameStageModule.Id => new RaidGateGameStageModule(),
                ShipIconModule.Id => new ShipIconModule(),
                TargetModule.Id => new TargetModule(),
                _ => new PlayerAttributeModule(),
            };
        }
    }
}
