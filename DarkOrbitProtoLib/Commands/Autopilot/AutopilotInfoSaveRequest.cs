namespace DarksideApi.DarkOrbit.Commands.Autopilot
{
    public class AutopilotInfoSaveRequest : Command
    {
        public new const short Id = -14897;
        public string autoPilotInfos;

        public AutopilotInfoSaveRequest(string autoPilotInfos)
        {
            this.autoPilotInfos = autoPilotInfos;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteUTF(this.autoPilotInfos);
            return this.@byte.ToArray();
        }
    }
}
