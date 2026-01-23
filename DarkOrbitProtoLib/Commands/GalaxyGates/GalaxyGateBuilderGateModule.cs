namespace DarksideApi.DarkOrbit.Commands.GalaxyGates
{
    public class GalaxyGateBuilderGateModule : Command   //GGBuilderGateModule
    {
        public new const short Id = 17995;
        public int multiplier;
        public List<GalaxyGateBuilderMultiGateProgressModule> multiGateProgress;
        public List<GalaxyGateBuilderItemModule> boosts;
        public List<GalaxyGateBuilderProbabilityModule> probabilityModules;
        public int currentWave;
        public int id;
        public int currentParts;    //var_5769
        public int lifePrice;   //name_96
        public int lifesLeft;       //name_95
        public List<GalaxyGateBuilderBonusGGRewardModule> bonusGGRewards;
        public string mode;
        public int state;

        public GalaxyGateBuilderGateModule()
        {
            this.multiGateProgress = [];
            this.boosts = [];
            this.probabilityModules = [];
            this.bonusGGRewards = [];
            this.mode = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            var i = 0;
            var max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                GalaxyGateBuilderBonusGGRewardModule bonusGGReward = new();
                bonusGGReward.Read(reader);
                this.bonusGGRewards.Add(bonusGGReward);
                i++;
            }
            this.lifesLeft = reader.ReadInt();
            this.lifesLeft = this.lifesLeft << 8 | this.lifesLeft >>> 24;
            this.currentWave = reader.ReadShort();
            this.currentWave = 65535 & ((65535 & this.currentWave) << 10 | (65535 & this.currentWave) >>> 6);
            this.currentWave = this.currentWave > 32767 ? (this.currentWave - 65536) : this.currentWave;
            i = 0;
            max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                GalaxyGateBuilderMultiGateProgressModule multiGateProgressModule = new();
                multiGateProgressModule.Read(reader);
                this.multiGateProgress.Add(multiGateProgressModule);
                i++;
            }
            i = 0;
            max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                GalaxyGateBuilderProbabilityModule probabilityModule = new();
                probabilityModule.Read(reader);
                this.probabilityModules.Add(probabilityModule);
                i++;
            }
            this.state = reader.ReadShort();
            this.mode = reader.ReadUTF();
            this.currentParts = reader.ReadShort();
            this.currentParts = 65535 & ((65535 & this.currentParts) >>> 10 | (65535 & this.currentParts) << 6);
            this.currentParts = this.currentParts > 32767 ? (this.currentParts - 65536) : this.currentParts;
            this.id = reader.ReadByte();
            this.id = 255 & ((255 & this.id) << 4 | (255 & this.id) >>> 4);
            this.id = this.id > 127 ? (this.id - 256) : this.id;
            this.lifePrice = reader.ReadInt();
            this.lifePrice = this.lifePrice >>> 7 | this.lifePrice << 25;
            i = 0;
            max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                GalaxyGateBuilderItemModule boost = new();
                boost.Read(reader);
                this.boosts.Add(boost);
                i++;
            }
            this.multiplier = reader.ReadByte();
            this.multiplier = 255 & ((255 & this.multiplier) << 2 | (255 & this.multiplier) >>> 6);
            this.multiplier = this.multiplier > 127 ? (this.multiplier - 256) : this.multiplier;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
