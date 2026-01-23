namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class KillScreenRepairRequest : Command  //class_193
    {
        public new const short Id = 255;
        public KillScreenOptionTypeModule selection;
        public LoginRequest loginCommand;

        public KillScreenRepairRequest(short selection, int userId, short factionId, string sessionId, string version, int instanceId, bool client = true)
        {
            this.selection = new(selection);
            this.loginCommand = new(userId, factionId, sessionId, version, instanceId, client);
        }

        public override void Read(ByteArray reader)
        {
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteByteArray(this.selection.Write());
            this.@byte.WriteByteArray(this.loginCommand.Write());
            return this.@byte.ToArray();
        }
    }
}
