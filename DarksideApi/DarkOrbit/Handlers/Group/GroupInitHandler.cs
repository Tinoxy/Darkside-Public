using DarksideApi.DarkOrbit.Commands.Group;
using DarksideApi.DarkOrbit.Objects.Group;
using DarksideApi.DarkOrbit.Objects.Ship;

namespace DarksideApi.DarkOrbit.Handlers.Group
{
    internal class GroupInitHandler : ICommandHandler
    {
        private readonly Api api;

        public GroupInitHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            GroupInitalizationCommand groupInit = new();
            groupInit.Read(@byte);

            GroupMemberInitalizationCommand? leader = groupInit.members.FirstOrDefault(m => m.id == groupInit.leaderId);
            var leaderName = leader != null ? leader.name : this.api.Hero.Username;

            if (this.api.Group != null)
            {
                this.api.Group.LeaderUsername = leaderName;
                this.api.Group.LeaderUserId = groupInit.leaderId;

                foreach (GroupMemberInitalizationCommand member in groupInit.members)
                {
                    if (this.api.Group.Members.Contains(member.id))
                        continue;
                    this.AddGroupMember(member);
                }
                foreach (var member in this.api.Group.Members)
                {
                    if (groupInit.members.Any(m => m.id == member.Id))
                    {
                        continue;
                    }
                    this.api.Group.Members.Remove(member);
                }

                return;
            }

            //this.api.Group = new(this.api, leaderName, groupInit.leaderId);

            foreach (GroupMemberInitalizationCommand member in groupInit.members)
            {
                this.AddGroupMember(member);
            }

            this.api.Logging.Log("Created Group with " + groupInit.members.Count + " Members");
        }

        private void AddGroupMember(GroupMemberInitalizationCommand member)
        {
            var memberShip = new Player(this.api, member.id, member.name, member.location.x, member.location.y, member.shipIcon.shipIcon.ToString(), member.cloaked, member.clan.clanId, member.clan.clanTag, member.faction.shipIcon, 0);
            var memberTarget = new GroupTargetObj(this.api, member.target.name, member.target.health.hp, member.target.health.hpMax, member.target.health.shield, member.target.health.shieldMax);
            var groupMember = new GroupMemberObj(this.api, memberShip, memberTarget, member.location.mapId);

            this.api.AddGroupMembers([groupMember]);
        }
    }
}
