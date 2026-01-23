namespace DarksideApi.DarkOrbit.Commands.GalaxyGates
{
    public class GalaxyGateBuilderInfoCommand : Command   //GalaxyGateBuilderInfoCommand
    {
        public new const short Id = -8501;
        public bool galaxyGateDay;
        public List<GalaxyGateBuilderGateModule> gates;
        public int energyCost;
        public double samples;  //Uridium
        public bool bonusRewardsDay;
        public int spinAmountSelect;
        public bool spinOnSale;
        public List<int> spinAmounts;
        public double money;  //var_4340  //uridium

        public GalaxyGateBuilderInfoCommand()
        {
            this.gates = [];
            this.spinAmounts = [];
        }

        public override void Read(ByteArray reader)
        {
            var i = 0;
            var max = reader.ReadByte();
            while (i < max)
            {
                int v = reader.ReadByte();
                v = 255 & ((255 & v) >>> 5 | (255 & v) << 3);
                v = (255 & ((255 & v) >>> 5 | (255 & v) << 3)) > 127 ? (v - 256) : v;
                this.spinAmounts.Add(v);
                i++;
            }
            this.spinAmountSelect = reader.ReadByte();
            this.spinAmountSelect = 255 & ((255 & this.spinAmountSelect) << 7 | (255 & this.spinAmountSelect) >>> 1);
            this.spinAmountSelect = this.spinAmountSelect > 127 ? (this.spinAmountSelect - 256) : this.spinAmountSelect;
            i = 0;
            max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                GalaxyGateBuilderGateModule galaxyGateModule = new();
                galaxyGateModule.Read(reader);
                this.gates.Add(galaxyGateModule);
                i++;
            }
            this.money = reader.ReadDouble();
            this.galaxyGateDay = reader.ReadBool();
            this.spinOnSale = reader.ReadBool();
            this.bonusRewardsDay = reader.ReadBool();
            this.energyCost = reader.ReadShort();
            this.energyCost = 65535 & ((65535 & this.energyCost) << 13 | (65535 & this.energyCost) >>> 3);
            this.energyCost = this.energyCost > 32767 ? (this.energyCost - 65536) : this.energyCost;
            this.samples = reader.ReadDouble();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
