namespace DarksideApi.DarkOrbit.Commands.GalaxyGates
{
    public class GalaxyGateBuilderProbabilityModule : Command    //819
    {
        public new const short Id = -2325;
        public int category;
        public int percentage;

        public override void Read(ByteArray reader)
        {
            this.percentage = reader.ReadByte();
            this.percentage = 255 & ((255 & this.percentage) << 6 | (255 & this.percentage) >>> 2);
            this.percentage = this.percentage > 127 ? (this.percentage - 256) : this.percentage;
            this.category = reader.ReadShort();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
