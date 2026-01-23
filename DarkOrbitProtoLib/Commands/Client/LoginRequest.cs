namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class LoginRequest : Command
    {
        public new const short Id = 7;
        public int userId;
        public short factionId;
        public string sessionId;
        public string version;
        public int instanceId;
        public bool fromClient; //var_5674

        public LoginRequest(int userID, short factionID, string sessionID, string version, int instanceId, bool fromClient)
        {
            this.userId = userID;
            this.factionId = factionID;
            this.sessionId = sessionID;
            this.version = version;
            this.instanceId = instanceId;
            this.fromClient = fromClient;
        }

        public override void Read(ByteArray reader)
        {
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteInt(this.userId << 3 | this.userId >>> 29);
            this.@byte.WriteShort((short)(65535 & ((65535 & this.factionId) >>> 9 | (65535 & this.factionId) << 7)));
            this.@byte.WriteUTF(this.sessionId);
            this.@byte.WriteUTF(this.version);
            this.@byte.WriteInt(this.instanceId << 12 | this.instanceId >>> 20);
            this.@byte.WriteBool(this.fromClient);
            return this.@byte.ToArray();
        }
    }
}
