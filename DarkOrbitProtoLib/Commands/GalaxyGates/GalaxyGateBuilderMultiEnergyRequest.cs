namespace DarksideApi.DarkOrbit.Commands.GalaxyGates
{
    public class GalaxyGateBuilderMultiEnergyRequest : Command   //GGBuilderMultiEnegeryRequest
    {
        public new const short Id = 6588;
        public byte gateId;
        public byte spinAmount; //var_5912
        public bool useMultiplier;

        public GalaxyGateBuilderMultiEnergyRequest(byte gateId, byte amount, bool useMultiplier)
        {
            this.gateId = gateId;
            this.spinAmount = amount;
            this.useMultiplier = useMultiplier;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteByte((byte)(255 & ((255 & this.spinAmount) << 5 % 8 | (255 & this.spinAmount) >>> 8 - 5 % 8)));
            this.@byte.WriteBool(this.useMultiplier);
            this.@byte.WriteByte((byte)(255 & ((255 & this.gateId) >>> 3 | (255 & this.gateId) << 5)));
            return this.@byte.ToArray();
        }
    }
}
