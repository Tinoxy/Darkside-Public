namespace DarksideApi.DarkOrbit.Commands.GalaxyGates
{
    public class GalaxyGateBuilderBuyLifeRequest : Command    //820
    {
        public new const short Id = -32288;
        public byte gateId;

        public GalaxyGateBuilderBuyLifeRequest(byte gateId)
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
            this.@byte.WriteByte((byte)(255 & ((255 & this.gateId) >>> 3 | (255 & this.gateId) << 5)));
            return this.@byte.ToArray();
        }
    }
}
