using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Commands.Ship;
using DarksideApi.DarkOrbit.Objects.Group;
using DarksideApi.DarkOrbit.Objects.Ship;
using DarksideApi.Logic;
using System.Diagnostics;

namespace DarksideApi.DarkOrbit.Handlers.Ship
{
    internal class ShipCreateHandler : ICommandHandler
    {
        private readonly Api api;

        public ShipCreateHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            ShipCreateCommand shipInit = new();
            shipInit.Read(@byte);

            //Debug.WriteLine(shipInit.ToString());
            
            Objects.Ship.Ship ship = shipInit.npc
                ? new Npc(this.api, shipInit.userId, shipInit.userName, shipInit.x, shipInit.y, shipInit.typeId, shipInit.cloaked, shipInit.specialNpcType)
                : new Player(this.api, shipInit.userId, shipInit.userName, shipInit.x, shipInit.y, shipInit.typeId, shipInit.cloaked, shipInit.clanId, shipInit.clanTag, shipInit.factionId, shipInit.clanRelationModule.type);
            if (!this.api.Ships.TryAdd(shipInit.userId, ship))
            {
                this.api.Ships[shipInit.userId] = ship;
            }
            if (ship is Player && this.api.Settings.GroupInviteAll && !this.api.Cooldown.IsCooldownActive(DarkOrbit.Cooldown.CooldownType.LOGIN))
            {
                //this.api.Group ??= new GroupObj(this.api, this.api.Hero.Username, this.api.Hero.UserId);

                this.api.Group.Invite(shipInit.userName);
            }
            if (ship is Npc npc && !CommandConstants.NPC_NAMES.Contains(npc.Username))
            {
                CommandConstants.AddNpcName(npc.Username);
                return;
            }
            if (ship is Player player
                && this.api.Settings.OnAttackedShootback
                && (!this.api.AutoAttackKillAmounts.TryGetValue(player.UserId, out var killAmount) || killAmount < this.api.Settings.OnAttackedShootbackAutoAttackMaxKills)
                && this.api.Logic.State != LogicState.GotoRepair && this.api.Logic.State != LogicState.Repair && this.api.Logic.State != LogicState.RepairJump
                && this.api.Logic.State != LogicState.SellOre && this.api.Logic.State != LogicState.SellOreDone && this.api.Logic.State != LogicState.GotoSellOre)
            {
                var attackableClanless = (this.api.Settings.OnAttackedShootbackAutoAttackClanless && player.ClanId <= 0 && player.FactionId != this.api.Hero.FactionId);
                var attackableEnemy = (this.api.Settings.OnAttackedShootbackAutoAttackEnemies && (player.ClanRelation == CommandConstants.CLAN_RELATION_AT_WAR || (player.ClanRelation == CommandConstants.CLAN_RELATION_NONE && player.FactionId != this.api.Hero.FactionId)));
                if (attackableClanless || attackableEnemy)
                {
                    if (this.api.Logic.State == LogicState.GotoShootback || this.api.Logic.State == LogicState.Shootback || this.api.Logic.Target is Player)
                    {
                        return;
                    }
                    this.api.Logic.Target = null;
                    this.api.Logic.Target = player;
                    this.api.Logic.SetState(this, LogicState.GotoShootback);
                    this.api.WriteLog($"Spotted {(attackableClanless ? "Clan-Less" : attackableEnemy ? $"Enemy{(player.ClanId > 0 ? $" [{player.ClanTag}]" : "")}" : "")} Player: {player.Username}! Attacking...");
                    //api.AddLoot("Attacked Players", 1);
                    this.api.Statistics.AmountShotback += 1;
                    return;
                }
                return;
            }
        }
    }
}
