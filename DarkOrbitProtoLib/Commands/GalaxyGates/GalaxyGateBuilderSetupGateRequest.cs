namespace DarksideApi.DarkOrbit.Commands.GalaxyGates
{
    public class GalaxyGateBuilderSetupGateRequest : Command  //GGSetupGateRequest
    {
        public new const short Id = 25136;
        public byte gateId;

        public GalaxyGateBuilderSetupGateRequest(byte gateId)
        {
            this.gateId = gateId;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteByte((byte)(255 & ((255 & this.gateId) << 4 | (255 & this.gateId) >>> 4)));
            return this.@byte.ToArray();
        }
    }
}
