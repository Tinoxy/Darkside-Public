namespace DarksideApi.DarkOrbit.Commands.GalaxyGates
{
    public class GalaxyGateBuilderItemModule : Command  //GalaxyGateBuilderItemModule
    {
        public new const short Id = -7254;
        public double date;
        public int type;

        public GalaxyGateBuilderItemModule() { }

        public override void Read(ByteArray reader)
        {
            this.type = reader.ReadShort();
            this.date = reader.ReadDouble();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
