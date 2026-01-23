namespace DarksideApi.DarkOrbit.Commands.Group
{
    public class MemberLeaveCommand : Command
    {
        public new const short Id = 6434;
        public LeaveReason reason;
        public int playerId;

        public override void Read(ByteArray reader)
        {
            this.playerId = reader.ReadInt();
            this.playerId = this.playerId >>> 3 | this.playerId << 29;
            this.reason = (LeaveReason)reader.ReadShort();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public enum LeaveReason
        {
            NONE = 0,
            LEAVE = 1,
            KICKED = 2,
        }
    }
}
