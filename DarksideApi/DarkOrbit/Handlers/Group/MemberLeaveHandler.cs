using DarksideApi.DarkOrbit.Commands.Group;

namespace DarksideApi.DarkOrbit.Handlers.Group
{
    internal class MemberLeaveHandler : ICommandHandler
    {
        private readonly Api api;

        public MemberLeaveHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            MemberLeaveCommand memberLeave = new();
            memberLeave.Read(@byte);

            if (this.api.Group == null)
            {
                this.api.Logging.Log("Group is null, cannot handle member leave.");
                return;
            }
            if (!this.api.Group.Members.TryGetValue(memberLeave.playerId, out Objects.Group.GroupMemberObj? member))
            {
                this.api.Logging.Log($"Member with ID {memberLeave.playerId} not found in group.");
                return;
            }
            this.api.Group.Members.Remove(memberLeave.playerId);

            switch (memberLeave.reason)
            {
                case MemberLeaveCommand.LeaveReason.NONE:
                    this.api.WriteLog(member.Player.Username + " left group, reason: unknown.");
                    break;
                case MemberLeaveCommand.LeaveReason.LEAVE:
                    this.api.WriteLog(member.Player.Username + " left group.");
                    break;
                case MemberLeaveCommand.LeaveReason.KICKED:
                    this.api.WriteLog(member.Player.Username + " kicked from group!");
                    break;
            }
        }
    }
}
