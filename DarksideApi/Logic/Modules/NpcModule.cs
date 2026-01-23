using DarksideApi.DarkOrbit;
using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Objects.Ship;
using DarksideApi.DarkOrbit.Settings;
using DarksideApi.Util;
using System.Numerics;

namespace DarksideApi.Logic.Modules
{
    internal class NpcModule(Api api) : ILogicModule
    {
        private const double TargetToCloseDiv = 0.65;
        private const int TargetInRange = 1172;
        private const int TargetMaxVariation = 50;
        private const int TargetMaxSelectTries = 3;
        private const int InterceptorRadius = 620;
        private const int InterceptorMinCount = 3;
        private const int HitpointsPercentageThreshold = 75;
        private const int RsbCooldownMs = 3050;
        private const int RsbSwitchBackSeconds = 3;
        private const int AttackTriesThreshold = 2;
        private const int AttackTriesRemove = 3;
        private const int RocketLauncherMinCount = 5;
        private const int AttackTryCooldownSeconds = 2;
        private const int FlyingTimeMsThreshold = 300;
        private const int CircleMoveCooldownSeconds = 2;
        private const int BoxDistanceThreshold = 1;
        private const int BoxCollectSleepMin = 100;
        private const int BoxCollectSleepMax = 200;

        public bool Run()
        {
            if (api.Logic.State == LogicState.GotoNpc)
            {
                if (api.Logic.Target is not Npc targetNpc)
                {
                    api.Logic.SetState(this, LogicState.None);
                    return true;
                }

                if (!api.Ships.TryGetValue(targetNpc.UserId, out var _))
                {
                    api.Hero.LastAttackTime = DateTime.MinValue;
                    api.Logic.Target = null;
                    api.Logic.SetState(this, LogicState.None);
                    return true;
                }

                var npcSetting = api.Logic.GetNpcSetting(targetNpc);
                if (!targetNpc.IsValid(npcSetting)
                    && !api.Settings.GeneralPassiveShootNpcs
                    && !targetNpc.IsMinion)
                {
                    api.Logic.Target = null;
                    api.Logic.SetState(this, LogicState.None);
                    return false;
                }

                if ((!api.Hero.IsFlying && targetNpc.GetMotherShip.CurrentDistance > npcSetting.Radius) 
                    || (targetNpc.GetMotherShip.CurrentDistance > npcSetting.Radius
                    && targetNpc.GetMotherShip.TargetDistance > npcSetting.Radius
                    && targetNpc.GetMotherShip.PathDistance > npcSetting.Radius))
                {
                    var npcPosition = targetNpc.GetMotherShip.Position;
                    Vector2 newPosition = targetNpc.GetMotherShip.IsFleeing ? targetNpc.GetMotherShip.TargetPosition : api.Logic.GetCirclePosition((int)npcPosition.X, (int)npcPosition.Y, Random.Shared.Next((int)(targetNpc.GetRadius(npcSetting.Radius) * 0.1), (int)targetNpc.GetRadius(npcSetting.Radius)), 0.0);

                    //if (targetNpc.IsHitacMinion)
                        //newPosition = targetNpc.GetMotherShip.Position;

                    api.GameMethods.Move(newPosition);
                    Thread.Sleep(Logic.GetRandomSleepTime());
                    return true;
                }

                // Refactored: Get and switch to higher priority target if needed
                SwitchToHigherPriorityTarget(ref targetNpc, ref npcSetting);

                HandleWeaponSwitching(npcSetting);

                if (targetNpc.GetMotherShip.CurrentDistance <= TargetInRange
                    && !targetNpc.IsSelected
                    && targetNpc.tempSelectTries < TargetMaxSelectTries)
                {
                    api.WriteLog("Locking " + targetNpc.Username);
                    api.GameMethods.TargetSelect(targetNpc);
                    Thread.Sleep(Logic.GetRandomSleepTime());
                }
                if (targetNpc.GetMotherShip.CurrentDistance >= npcSetting.Radius)
                {
                    return true;
                }
                targetNpc.tempLastCircleMove = DateTime.MinValue;
                targetNpc.tempCircleClockwise = HelpTools.GetChance();
                api.Logic.SetState(this, LogicState.Npc);
            }
            if (api.Logic.State == LogicState.Npc)
            {
                if (api.Logic.Target is not Npc targetNpc)
                {
                    api.Logic.SetState(this, LogicState.None);
                    return true;
                }

                if (!api.Ships.TryGetValue(targetNpc.UserId, out var _))
                {
                    api.Logic.Target = null;
                    api.Logic.SetState(this, LogicState.None);
                    api.Hero.LastAttackTime = DateTime.MinValue;
                    return true;
                }

                if (targetNpc.GetMotherShip.CurrentDistance > TargetInRange
                    && ((!targetNpc.IsSelected
                    && targetNpc.tempSelectTries < TargetMaxSelectTries) || targetNpc.tempSelectTries >= TargetMaxSelectTries))
                {
                    api.Logging.Log("Range exceeded for: " + targetNpc.Username);
                }

                if (!targetNpc.IsSelected && targetNpc.tempSelectTries < TargetMaxSelectTries)
                {
                    api.WriteLog("Locking " + targetNpc.Username);
                    api.GameMethods.TargetSelect(targetNpc);
                    return true;
                }

                if (targetNpc.IsProtected)
                {
                    var interceptors = api.Logic.GetNpcsByNameInRadius(CommandConstants.NPC_INTERCEPTOR, targetNpc.X, targetNpc.Y, InterceptorRadius, false).OrderBy(s => s.CurrentDistance).ToList();
                    if (interceptors.Count > AttackTriesThreshold && targetNpc.HitpointsPercentage > HitpointsPercentageThreshold)
                    {
                        targetNpc.MinioinNpcs.Clear();
                        interceptors.ForEach(i => targetNpc.MinioinNpcs.Add(i.UserId));

                        targetNpc.tempAttackTries = 0;
                        targetNpc = interceptors.First();

                        api.Logic.Target = targetNpc;
                    }
                    var hitacMinions = api.Logic.GetNpcsByNameInRadius(CommandConstants.NPC_HITAC_MINION, targetNpc.X, targetNpc.Y, InterceptorRadius, false).OrderBy(s => s.CurrentDistance).ToList();
                    if (hitacMinions.Count > 0)
                    {
                        targetNpc.MinioinNpcs.Clear();
                        hitacMinions.ForEach(h => targetNpc.MinioinNpcs.Add(h.UserId));

                        targetNpc.tempAttackTries = 0;
                        targetNpc = hitacMinions.First();

                        api.Logic.Target = targetNpc;
                    }
                }

                if (targetNpc.tempAttackTries > AttackTriesRemove)
                {
                    lock (api.ObjectLocker)
                        api.Ships.Remove(targetNpc.UserId, out _);
                }

                if (!api.Ships.TryGetValue(targetNpc.UserId, out var _))
                {
                    api.Logic.Target = null;
                    api.Hero.LastAttackTime = DateTime.MinValue;
                    api.Logic.SetState(this, LogicState.None);
                    return true;
                }

                var npcSetting = api.Logic.GetNpcSetting(targetNpc);

                if (api.Hero.Config != npcSetting.Config)
                {
                    api.GameMethods.ConfigurationChange(npcSetting.Config);
                }
                HandleShieldPercentageFormationChange(ref targetNpc, ref npcSetting);

                if (targetNpc.ShieldPercentage <= 0 
                    && api.Hero.CurrentLaser.Id == CommandConstants.AMMO_SAB_50 
                    && npcSetting.LaserId != CommandConstants.AMMO_SAB_50)
                {
                    api.GameMethods.LaserChange(npcSetting.LaserId!);
                }

                if (npcSetting.UseRSB 
                    && (!npcSetting.UseSAB || targetNpc.ShieldPercentage <= 0) 
                    && (DateTime.UtcNow - targetNpc.tempRsbLastUsed).TotalMilliseconds > RsbCooldownMs)
                {
                    api.GameMethods.LaserChange(CommandConstants.AMMO_RSB);
                    targetNpc.tempRsbLastUsed = DateTime.UtcNow;
                    Thread.Sleep(Logic.GetRandomSleepTime(BoxCollectSleepMin, BoxCollectSleepMax));
                }
                else if (api.Hero.CurrentLaser.Id == CommandConstants.AMMO_RSB 
                    && (DateTime.UtcNow - targetNpc.tempRsbLastUsed).TotalSeconds < RsbSwitchBackSeconds)
                {
                    api.GameMethods.LaserChange(npcSetting.UseSAB && targetNpc.ShieldPercentage > 0 ? CommandConstants.AMMO_SAB_50 : npcSetting.LaserId!);
                }

                if (!api.Hero.IsAttacking && targetNpc.GetMotherShip.CurrentDistance < npcSetting.Radius)
                {
                    if (targetNpc.tempAttackTries > AttackTriesThreshold)
                    {
                        api.GameMethods.Move(targetNpc.GetMotherShip, (int)(npcSetting.Radius / 2));
                    }
                    if ((DateTime.UtcNow - api.Hero.LastAttackTryTime).TotalSeconds < AttackTryCooldownSeconds)
                    {
                        return true;
                    }
                    api.Hero.LastAttackTryTime = DateTime.UtcNow;
                    targetNpc.tempAttackTries += 1;
                    api.WriteLog("Attacking " + targetNpc.Username);
                    api.GameMethods.AttackStartLaser(targetNpc.UserId, targetNpc.X, targetNpc.Y);
                    return true;
                }

                if (npcSetting.UseRockets && npcSetting.RocketId != null && api.Inventory.Get(npcSetting.RocketId) > 0)
                {
                    api.GameMethods.AttackStartRocket(targetNpc.UserId, targetNpc.X, targetNpc.Y);
                }
                if (npcSetting.UseRocketLauncher && npcSetting.RocketLauncherId != null && api.Inventory.Get(npcSetting.RocketLauncherId) > RocketLauncherMinCount)
                {
                    api.GameMethods.AttackStartRocketLauncher();
                }

                // Refactored: Get and switch to higher priority target if needed
                SwitchToHigherPriorityTarget(ref targetNpc, ref npcSetting, strictPriority: true);

                DarkOrbit.Objects.Boxes.Box closeBox = api.Logic.GetClosestBox();
                if (!string.IsNullOrEmpty(closeBox.Hash)
                    && HelpTools.CalculateDistance(closeBox.Position, targetNpc.GetMotherShip.Position) < targetNpc.GetRadius(npcSetting.Radius) + TargetMaxVariation
                    && HelpTools.CalculateDistance(closeBox.Position, targetNpc.GetMotherShip.Position) > (targetNpc.GetRadius(npcSetting.Radius) * TargetToCloseDiv))
                {
                    if (closeBox.CurrentDistance <= BoxDistanceThreshold)
                    {
                        api.GameMethods.BoxCollect(closeBox);
                        api.Collectibles.Remove(closeBox.Hash, out _);
                        Thread.Sleep(Logic.GetRandomSleepTime(BoxCollectSleepMin, BoxCollectSleepMax));
                        return true;
                    }
                    if (closeBox.TargetDistance > BoxDistanceThreshold)
                    {
                        api.WriteLog($"Going to Box in Range {closeBox.Type}.");
                        api.GameMethods.Move(closeBox);
                        return true;
                    }
                    if (closeBox.TargetDistance <= BoxDistanceThreshold)
                    {
                        return true;
                    }
                }

                var motherShip = targetNpc.GetMotherShip;
                var flag = (motherShip.IsMoving || !api.EntitiesUnderAttackBy.IsEmpty);
                if (npcSetting.UseCircle
                    && flag
                    && (!api.Hero.IsFlying
                        || api.Hero.FlyingTimeMs < FlyingTimeMsThreshold
                        || motherShip.TargetDistance < (npcSetting.Radius / 2)
                        || (DateTime.UtcNow - motherShip.tempLastCircleMove).TotalSeconds >= CircleMoveCooldownSeconds
                    ))
                {
                    return api.Logic.DoCircle(motherShip, npcSetting, api.EntitiesUnderAttackBy.Count);
                }
                else if (!npcSetting.UseCircle && (motherShip.CurrentDistance > targetNpc.GetRadius(npcSetting.Radius) || motherShip.CurrentDistance < (npcSetting.Radius * TargetToCloseDiv)))
                {
                    
                    api.GameMethods.Move(api.Logic.GetCirclePosition(motherShip.X, motherShip.Y, targetNpc.GetRadius(npcSetting.Radius), 0));
                    return true;
                }
            }
            return false;
        }

        private void HandleShieldPercentageFormationChange(ref Npc targetNpc, ref NpcSetting npcSetting)
        {
            if (api.Hero.DroneFormation.Equals(npcSetting.DroneFormation) && targetNpc.ShieldPercentage > 15)
            {
                return;
            }
            if (api.Hero.DroneFormation.Equals(npcSetting.ShieldBelowFifteenFormation) && targetNpc.ShieldPercentage <= 15)
            {
                return;
            }
            string nextFormation;
            if (targetNpc.ShieldPercentage <= 15 && npcSetting.UseShieldBelowFifteenFormation)
            {
                nextFormation = npcSetting.ShieldBelowFifteenFormation;
            }
            else
            {
                nextFormation = npcSetting.DroneFormation;
            }
            if (api.Hero.DroneFormation.Equals(nextFormation))
            {
                return;
            }
            api.GameMethods.DroneFormationChange(nextFormation);
            api.Logging.Log($"Changing formation to {nextFormation} for {targetNpc.Username}, shield: {targetNpc.ShieldPercentage}%.");
        }

        // Refactored: Switch to higher priority target logic
        private void SwitchToHigherPriorityTarget(ref Npc targetNpc, ref DarkOrbit.Settings.NpcSetting npcSetting, bool strictPriority = false)
        {
            Npc possibleTarget = api.Logic.GetClosestNpc();
            DarkOrbit.Settings.NpcSetting possibleTargetSettings = api.Logic.GetNpcSetting(possibleTarget);
            if (possibleTarget.UserId != targetNpc.UserId
                && (strictPriority ? possibleTargetSettings.Priority > npcSetting.Priority : possibleTargetSettings.Priority >= npcSetting.Priority)
                && targetNpc.CurrentDistance > possibleTarget.CurrentDistance)
            {
                api.Logic.Target = possibleTarget;
                targetNpc = possibleTarget;
                npcSetting = possibleTargetSettings;
                if (strictPriority)
                {
                    api.WriteLog("Found Higher Priority Npc " + possibleTarget.Username);
                    api.Logic.SetState(this, LogicState.GotoNpc);
                }
            }
        }

        // Refactored: Weapon switching logic
        private void HandleWeaponSwitching(DarkOrbit.Settings.NpcSetting npcSetting)
        {
            if (api.Hero.CurrentLaser.Id != npcSetting.LaserId
                || (npcSetting.UseSAB && api.Hero.CurrentLaser.Id != CommandConstants.AMMO_SAB_50)
                || (npcSetting.UseRSB && api.Hero.CurrentLaser.Id != CommandConstants.AMMO_RSB))
            {
                api.GameMethods.LaserChange(npcSetting.UseSAB ? CommandConstants.AMMO_SAB_50 : npcSetting.UseRSB ? CommandConstants.AMMO_RSB : npcSetting.LaserId!);
            }
            if (npcSetting.UseRockets && npcSetting.RocketId != null && api.Hero.CurrentRocket.Id != npcSetting.RocketId)
            {
                api.GameMethods.RocketChange(npcSetting.RocketId!);
            }
            if (npcSetting.UseRocketLauncher && npcSetting.RocketLauncherId != null && api.Hero.CurrentRocketLauncher.Id != npcSetting.RocketLauncherId)
            {
                api.GameMethods.RocketLauncherChange(npcSetting.RocketLauncherId!);
            }
        }
    }
}
