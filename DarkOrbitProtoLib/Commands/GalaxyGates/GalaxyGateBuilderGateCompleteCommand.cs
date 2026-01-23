namespace DarksideApi.DarkOrbit.Commands.GalaxyGates
{
    public class GalaxyGateBuilderGateCompleteCommand : Command
    {
        public new const short Id = -24697;
        public int currentPart;
        public byte gateId;

        public override void Read(ByteArray reader)
        {
            this.currentPart = reader.ReadShort();
            this.gateId = reader.ReadByte();
            this.gateId = (byte)(255 & ((255 & this.gateId) << 1 | (255 & this.gateId) >>> 7));
            this.gateId = (byte)(this.gateId > 127 ? (this.gateId - 256) : this.gateId);
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
