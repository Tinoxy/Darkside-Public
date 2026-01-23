using DarksideApi.DarkOrbit.Commands.Group;

namespace DarksideApi.DarkOrbit.Handlers.Group
{
    public class InvitationInitalizationHandler : ICommandHandler
    {
        private readonly Api api;

        public InvitationInitalizationHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            InvitationInitializationCommand groupInvite = new();
            groupInvite.Read(@byte);

            if (groupInvite.candidateId != this.api.Hero.UserId)
            {
                return;
            }
            this.api.WriteLog("Group invite from: " + groupInvite.inviterUsername);
            if (this.api.Settings.GroupSettings.Any(g => g.IsEnabled && g.Username == groupInvite.inviterUsername) || this.api.Settings.GroupAcceptAll)
            {
                //this.api.Group = new(this.api, groupInvite.inviterUsername, groupInvite.inviterId);
                //this.api.Group.AcceptInvite();
                this.api.GameMethods.GroupAccept(groupInvite.inviterId);
                this.api.WriteLog("Accepting Group invite!");
                return;
            }
            this.api.GameMethods.GroupDecline(groupInvite.inviterId);
            this.api.WriteLog("Declining Group invite!");
        }
    }
}
