namespace DarksideApi.DarkOrbit.Commands.GalaxyGates
{
    public class GalaxyGateBuilderSaveSpinAmountRequest : Command //828
    {
        public new const short Id = 29446;
        public byte spinAmount;

        public GalaxyGateBuilderSaveSpinAmountRequest(byte spinAmount)
        {
            this.spinAmount = spinAmount;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteByte((byte)(255 & ((255 & this.spinAmount) >>> 0 | (255 & this.spinAmount) << 8)));
            return this.@byte.ToArray();
        }
    }
}
