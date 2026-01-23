namespace DarksideApi.DarkOrbit.Commands.GalaxyGates
{
    public class GalaxyGateBuilderMultiGateProgressModule : Command //GalaxyGateMultiGateProgressModule
    {
        public new const short Id = 8128;
        public int current;
        public string name;
        public int total;

        public GalaxyGateBuilderMultiGateProgressModule()
        {
            this.name = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            this.total = reader.ReadShort();
            this.total = 65535 & ((65535 & this.total) << 12 | (65535 & this.total) >>> 4);
            this.total = this.total > 32767 ? this.total - 65536 : this.total;
            this.current = reader.ReadShort();
            this.current = 65535 & ((65535 & this.current) << 11 | (65535 & this.current) >>> 5);
            this.current = this.current > 32767 ? this.current - 65536 : this.current;
            this.name = reader.ReadUTF();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
