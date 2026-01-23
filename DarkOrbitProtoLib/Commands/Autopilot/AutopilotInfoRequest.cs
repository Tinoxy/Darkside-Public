namespace DarksideApi.DarkOrbit.Commands.Autopilot
{
    public class AutopilotInfoRequest : Command
    {
        public new const short Id = 8547;

        public AutopilotInfoRequest()
        {
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            return this.@byte.ToArray();
        }
    }
}
