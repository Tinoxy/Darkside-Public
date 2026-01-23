namespace DarksideApi.DarkOrbit.Commands.Autopilot
{
    public class AutopilotInfoCommand : Command
    {
        public new const short Id = 11873;
        public int laserRange;
        public string autoPilotInfos;
        public double remainingSeconds;
        public bool result;

        public AutopilotInfoCommand()
        {
            this.autoPilotInfos = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            this.autoPilotInfos = reader.ReadUTF();
            this.laserRange = reader.ReadInt();
            this.laserRange = this.laserRange << 14 | this.laserRange >>> 18;
            this.remainingSeconds = reader.ReadDouble();
            this.result = reader.ReadBool();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
