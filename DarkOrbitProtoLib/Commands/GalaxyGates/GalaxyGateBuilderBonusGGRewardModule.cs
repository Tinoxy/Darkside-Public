namespace DarksideApi.DarkOrbit.Commands.GalaxyGates
{
    public class GalaxyGateBuilderBonusGGRewardModule : Command  //2200
    {
        public new const short Id = -11063;
        public bool claimed;
        public int amount;
        public int countdown;
        public string lootId;

        public GalaxyGateBuilderBonusGGRewardModule()
        {
            this.lootId = string.Empty;
        }

        public override void Read(ByteArray reader)
        {    
            this.claimed = reader.ReadBool();
            this.countdown = reader.ReadInt();
            this.countdown = this.countdown >>> 8 | this.countdown << 24;
            this.lootId = reader.ReadUTF();
            this.amount = reader.ReadInt();
            this.amount = this.amount << 8 | this.amount >>> 24;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
