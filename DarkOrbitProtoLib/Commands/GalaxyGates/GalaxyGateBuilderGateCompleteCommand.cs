namespace DarksideApi.DarkOrbit.Commands.GalaxyGates
{
    public class GalaxyGateBuilderGateCompleteCommand : Command
    {
        public new const short Id = 2414;
        public int currentPart;
        public byte gateId;

        public override void Read(ByteArray reader)
        {
            this.currentPart = reader.ReadShort();
            this.currentPart = (byte)(65535 & ((65535 & this.currentPart) >>> 0 | (65535 & this.currentPart) << 16));
            this.currentPart = (byte)(this.currentPart > 32767 ? (this.currentPart - 65536) : this.currentPart);
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
