using DarksideApi.DarkOrbit.Commands.Group;
using System.Diagnostics;

namespace DarksideApi.DarkOrbit.Handlers.Group
{
    internal class GroupMemberUpdateHandler : ICommandHandler
    {
        private readonly Api api;

        public GroupMemberUpdateHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            GroupMemberUpdateCommand groupMemberUpdate = new();
            groupMemberUpdate.Read(@byte);

            Debug.WriteLine("[GroupMemberUpdateHandler] Handling GroupMemberUpdate for PlayerId: " + groupMemberUpdate.playerId);
            //this.api.Group ??= new(this.api, this.api.Hero.Username, this.api.Hero.UserId);

            if (this.api.Group == null)
            {
                Debug.WriteLine($"[GroupMemberUpdateHandler] Group is NULL!");
                return;
            }
            if (!this.api.Group.Members.TryGetValue(groupMemberUpdate.playerId, out Objects.Group.GroupMemberObj? groupMember))
            {
                Debug.WriteLine($"[GroupMemberUpdateHandler] Did not find {groupMemberUpdate.playerId} in Group!");
                return;
            }

            foreach (var update in groupMemberUpdate.updates)
            {
                if (update is HealthModule health)
                {
                    groupMember.Player.Hitpoints = health.hp;
                    groupMember.Player.MaxHitpoints = health.hpMax;
                    groupMember.Player.Shield = health.shield;
                    groupMember.Player.MaxShield = health.shieldMax;
                    groupMember.Player.NanoShield = health.nanoShield;
                    groupMember.Player.MaxNanoShield = health.nanoShieldMax;
                    Debug.WriteLine($"[GroupMemberUpdateHandler] Updated Health for {groupMember.Player.Username}: HP {health.hp}/{health.hpMax}, Shield {health.shield}/{health.shieldMax}, NanoShield {health.nanoShield}/{health.nanoShieldMax}");
                    continue;
                }
                if (update is LocationModule location)
                {
                    groupMember.Player.X = location.x;
                    groupMember.Player.Y = location.y;
                    groupMember.MapId = location.mapId;
                    Debug.WriteLine($"[GroupMemberUpdateHandler] Updated Location for {groupMember.Player.Username}: MapId {location.mapId}, X {location.x}, Y {location.y}");
                    continue;
                }
                if (update is TargetModule target)
                {
                    groupMember.Target = new Objects.Group.GroupTargetObj(this.api, target.name, target.health.hp, target.health.hpMax, target.health.shield, target.health.shieldMax, -1, -1);
                    Debug.WriteLine($"[GroupMemberUpdateHandler] Updated Target for {groupMember.Player.Username}: TargetName {target.name}, HP {target.health.hp}/{target.health.hpMax}, Shield {target.health.shield}/{target.health.shieldMax}");
                    continue;
                }
                if (update is RaidGateGameStageModule raidGateGameStage)
                {
                    groupMember.RaidGateStage = raidGateGameStage.gameStage;
                    groupMember.IsInRaidGate = raidGateGameStage.isInGate;
                    Debug.WriteLine($"[GroupMemberUpdateHandler] Updated RaidGateGameStage for {groupMember.Player.Username}: Stage {raidGateGameStage.gameStage}, IsInGate {raidGateGameStage.isInGate}");
                    continue;
                }
                continue;
            }
            groupMember.Updated();
            //if (!api.Group.MemberIds.Contains(groupMemberUpdate.playerId))
            //    api.Group.MemberIds.Add(groupMemberUpdate.playerId);
        }
    }
}
