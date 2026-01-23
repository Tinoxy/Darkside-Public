namespace DarksideApi.DarkOrbit.Commands.Map
{
    public class JumpInitiatedCommand : Command   //1233
    {
        public new const short Id = 19;
        public int gateId;
        public int mapId;

        public JumpInitiatedCommand()
        {
        }

        public override void Read(ByteArray reader)
        {
            this.mapId = reader.ReadInt();
            this.mapId = this.mapId << 7 % 32 | this.mapId >>> 32 - 7 % 32;
            this.gateId = reader.ReadInt();
            this.gateId = this.gateId << 6 % 32 | this.gateId >>> 32 - 6 % 32;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
