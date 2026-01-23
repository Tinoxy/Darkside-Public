using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Commands.Attack;
using DarksideApi.DarkOrbit.Objects.Map;
using DarksideApi.DarkOrbit.Objects.Ship;
using DarksideApi.Logic;

namespace DarksideApi.DarkOrbit.Handlers.Attack
{
    internal class AttackLaserRunHandler : ICommandHandler
    {
        private readonly Api api;

        public AttackLaserRunHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            AttackLaserRunCommand attackLaserRun = new();
            attackLaserRun.Read(@byte);

            if (attackLaserRun.attackerId == this.api.Hero.UserId)
            {
                this.api.Hero.LastAttackTime = DateTime.UtcNow;
                this.api.Hero.CurrentTarget = attackLaserRun.targetId;
                if (this.api.Ships.TryGetValue(attackLaserRun.targetId, out Objects.Ship.Ship? t))
                {
                    t.tempAttackTries = 0;
                }
                return;
            }
            if (attackLaserRun.targetId == this.api.Hero.UserId)
            {
                if (!this.api.EntitiesUnderAttackBy.TryAdd(attackLaserRun.attackerId, DateTime.UtcNow))
                {
                    this.api.EntitiesUnderAttackBy[attackLaserRun.attackerId] = DateTime.UtcNow;
                }
                if (this.api.Ships.TryGetValue(attackLaserRun.attackerId, out Objects.Ship.Ship? ship))
                {
                    if (ship is Player player)
                    {
                        if (this.api.Settings.OnAttackedFlee)
                        {
                            if (this.api.Logic.State != LogicState.GotoFlee && this.api.Logic.State != LogicState.FleeJump && (!this.api.Map.IsPirateMap || this.api.Logic.State != LogicState.GotoFleeSpotted))
                            {
                                this.api.Logic.Target = player;
                                this.api.Logic.SetState(this, LogicState.GotoFlee);
                                this.api.WriteLog($"Under Attack by {player.Username}! Fleeing...");
                                this.api.Statistics.AmountFled += 1;
                            }
                            return;
                        }
                        if (this.api.Settings.OnAttackedShootback)
                        {
                            if (this.api.Logic.State != LogicState.GotoShootback && this.api.Logic.State != LogicState.Shootback && this.api.Logic.Target is not Player)
                            {
                                this.api.Logic.Target = player;
                                this.api.Logic.SetState(this, LogicState.GotoShootback);
                                this.api.WriteLog($"Under Attack by {player.Username}! Shooting back...");
                                this.api.Statistics.AmountShotback += 1;
                            }
                            return;
                        }
                    }
                    if (ship is Npc npc)
                    {
                        if (this.api.Settings.GeneralPassiveShootNpcs)
                        {
                            if (this.api.Logic.State != LogicState.GotoNpc && this.api.Logic.State != LogicState.Npc
                                && !this.api.Map.IsGalaxyGate
                                && this.api.Logic.Target is not Gate
                                && this.api.Logic.Target is not Player
                                && this.api.Logic.State != LogicState.GotoRepair
                                && this.api.Logic.State != LogicState.GotoSellOre)
                            {
                                this.api.Logic.Target = npc;
                                this.api.Logic.SetState(this, LogicState.GotoNpc);
                                this.api.WriteLog($"Under Attack by {npc.Username}, will attack...");
                            }
                        }
                    }
                }
                if (this.api.Pets.TryGetValue(attackLaserRun.attackerId, out Objects.Pets.Pet? pet))
                {
                    if (this.api.Settings.OnAttackedFlee)
                    {
                        if (this.api.Logic.State != LogicState.GotoFlee && this.api.Logic.State != LogicState.FleeJump && (!this.api.Map.IsPirateMap || this.api.Logic.State != LogicState.GotoFleeSpotted))
                        {
                            this.api.Logic.Target = pet;
                            this.api.Logic.SetState(this, LogicState.GotoFlee);
                            this.api.WriteLog($"Under Attack by {pet.Name}! Fleeing...");
                            this.api.Statistics.AmountFled += 1;
                        }
                        return;
                    }
                }
            }
            if (this.api.Hero.IsPetActive && attackLaserRun.targetId == this.api.Hero.Pet.UserId)
            {
                if (this.api.Ships.TryGetValue(attackLaserRun.attackerId, out Objects.Ship.Ship? petAttackerShip) && petAttackerShip is Player petAttacker)
                {
                    if (!this.api.Settings.OnAttackedShootPetKillers
                        || !this.api.Settings.OnAttackedShootback
                        || petAttacker.ClanRelation == CommandConstants.CLAN_RELATION_ALLIED)
                    {
                        return;
                    }
                    if (this.api.Logic.State != LogicState.GotoShootback && this.api.Logic.State != LogicState.Shootback && this.api.Logic.Target is not Player)
                    {
                        this.api.Logic.Target = petAttacker;
                        this.api.Logic.SetState(this, LogicState.GotoShootback);
                        this.api.WriteLog($"PET is under Attack by {petAttacker.Username}! Shooting back...");
                        this.api.Statistics.AmountShotback += 1;
                    }
                    return;
                }
            }
            if (this.api.Ships.TryGetValue(attackLaserRun.attackerId, out Objects.Ship.Ship? attackerShip) && attackerShip is Player attackerPlayer
                && this.api.Ships.TryGetValue(attackLaserRun.targetId, out Objects.Ship.Ship? defenderShip) && defenderShip is Player defenderPlayer
                && this.api.Settings.OnAttackedShootbackSupportAllies)
            {
                if (this.api.Logic.State == LogicState.GotoShootback || this.api.Logic.State == LogicState.Shootback)
                {
                    return;
                }
                if (defenderPlayer == null || attackerPlayer == null)
                {
                    return;
                }
                if (attackerPlayer.ClanRelation == CommandConstants.CLAN_RELATION_ALLIED)
                {
                    this.api.Logic.Target = defenderPlayer;
                    this.api.Logic.SetState(this, LogicState.GotoShootback);
                    this.api.WriteLog($"Ally {attackerPlayer.Username} is in fight, going to support against {defenderPlayer.Username}...");
                    return;
                }
                if (defenderPlayer.ClanRelation == CommandConstants.CLAN_RELATION_ALLIED)
                {
                    this.api.Logic.Target = attackerPlayer;
                    this.api.Logic.SetState(this, LogicState.GotoShootback);
                    this.api.WriteLog($"Ally {defenderPlayer.Username} is under attack, going to support against {attackerPlayer.Username}...");
                    return;
                }
                if (api.Group != null && api.Group.IsInGroup(defenderPlayer.UserId))
                {
                    this.api.Logic.Target = attackerPlayer;
                    this.api.Logic.SetState(this, LogicState.GotoShootback);
                    this.api.WriteLog($"Group Member {defenderPlayer.Username} is under attack, going to support against {attackerPlayer.Username}...");
                    return;
                }
                if (api.Group != null && api.Group.IsInGroup(attackerPlayer.UserId))
                {
                    this.api.Logic.Target = defenderPlayer;
                    this.api.Logic.SetState(this, LogicState.GotoShootback);
                    this.api.WriteLog($"Group Member {attackerPlayer.Username} is in fight, going to support against {defenderPlayer.Username}...");
                    return;
                }
            }
            if (this.api.Pets.TryGetValue(attackLaserRun.attackerId, out Objects.Pets.Pet? _pet))
            {
                _pet.LastAttackTime = DateTime.UtcNow;
                return;
            }
            if (this.api.Ships.TryGetValue(attackLaserRun.targetId, out Objects.Ship.Ship? n) && n is Npc _npc
                && (this.api.Ships.TryGetValue(attackLaserRun.attackerId, out Objects.Ship.Ship? p) && p is Player
                || attackLaserRun.attackerId == this.api.Hero.UserId))
            {
                foreach (Npc npc in this.api.Ships.Values.OfType<Npc>())
                {
                    if (npc.LastAttackerId != attackLaserRun.attackerId)
                        continue;
                    if (npc.UserId == _npc.UserId)
                        continue;

                    npc.LastAttackerId = 0;
                }
                _npc.LastAttackerId = attackLaserRun.attackerId;
            }
        }
    }
}
