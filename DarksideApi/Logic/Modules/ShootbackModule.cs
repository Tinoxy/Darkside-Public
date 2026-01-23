using DarksideApi.DarkOrbit;
using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Objects.Ship;
using DarksideApi.DarkOrbit.Settings;
using DarksideApi.Util;

namespace DarksideApi.Logic.Modules
{
    internal class ShootbackModule(Api api) : ILogicModule
    {
        public bool Run()
        {
            if (api.Logic.State == LogicState.GotoShootback)
            {
                api.Logging.Log("Goto Shootback Target: " + api.Logic.Target);
                if (api.Logic.Target is not Player)
                {
                    Player[] possibleTargets = api.Ships.Values.OfType<Player>().Where(e => api.EntitiesUnderAttackBy.Any(p => p.Key == e.UserId)).ToArray();
                    if (possibleTargets.Length <= 0)
                    {
                        api.Logging.Log("Goto Shootback invalid Target: " + api.Logic.Target);
                        api.Logic.SetState(this, LogicState.None);
                        return false;
                    }
                    api.Logic.Target = possibleTargets.FirstOrDefault();
                    api.Logging.Log("Goto Shootback set Target to: " + api.Logic.Target);
                }

                var targetPlayer = (api.Logic.Target as Player);

                if (!api.Ships.ContainsKey(targetPlayer.UserId))
                {
                    api.Logging.Log("Goto Shootback ships does not contain Target: " + targetPlayer.UserId);
                    api.Hero.LastAttackTime = DateTime.MinValue;
                    api.Logic.SetState(this, LogicState.None);
                    return true;
                }

                if (targetPlayer.CurrentDistance > 600 && targetPlayer.TargetDistance > api.Settings.OnAttackedShootbackRadius)
                {
                    api.GameMethods.Move(targetPlayer, (int)api.Settings.OnAttackedShootbackRadius);
                }

                if (api.Hero.DroneFormation != api.Settings.OnAttackedShootbackFormation)
                {
                    api.GameMethods.DroneFormationChange(api.Settings.OnAttackedShootbackFormation);
                }
                if (api.Hero.Config != api.Settings.OnAttackedShootbackConfig)
                {
                    api.GameMethods.ConfigurationChange(api.Settings.OnAttackedShootbackConfig);
                }

                if (api.Hero.CurrentLaser.Id != api.Settings.OnAttackedShootbackLaserType
                || api.Settings.OnAttackedShootbackUseSAB && api.Hero.CurrentLaser.Id != CommandConstants.AMMO_SAB_50
                    || api.Settings.OnAttackedShootbackUseRSB && api.Hero.CurrentLaser.Id != CommandConstants.AMMO_RSB)
                {
                    api.GameMethods.LaserChange(api.Settings.OnAttackedShootbackUseSAB ? CommandConstants.AMMO_SAB_50 : api.Settings.OnAttackedShootbackUseRSB ? CommandConstants.AMMO_RSB : api.Settings.OnAttackedShootbackLaserType);
                }
                if (api.Settings.OnAttackedShootbackUseRocket && api.Hero.CurrentRocket.Id != api.Settings.OnAttackedShootbackRocketType)
                {
                    api.GameMethods.RocketChange(api.Settings.OnAttackedShootbackRocketType!);
                }
                if (api.Settings.OnAttackedShootbackUseRocketLauncher && api.Hero.CurrentRocketLauncher.Id != api.Settings.OnAttackedShootbackRocketLauncherType)
                {
                    api.GameMethods.RocketLauncherChange(api.Settings.OnAttackedShootbackRocketLauncherType);
                }
                if (targetPlayer.CurrentDistance >= 600)
                {
                    api.Logging.Log("Goto Shootback distance to big: " + targetPlayer.CurrentDistance);
                    Thread.Sleep(500);
                    return true;
                }

                targetPlayer.tempCircleClockwise = HelpTools.GetChance();
                api.Logic.SetState(this, LogicState.Shootback);
            }

            if (api.Logic.State == LogicState.Shootback)
            {
                if (api.Logic.Target is not Player)
                {
                    api.Logging.Log("Shootback invalid Target: " + api.Logic.Target);
                    api.Logic.SetState(this, LogicState.None);
                    return true;
                }

                var targetPlayer = (api.Logic.Target as Player);

                if (!api.EntitiesUnderAttackBy.ContainsKey(targetPlayer.UserId) && !api.Hero.IsAttacking)
                {
                    DarkOrbit.Objects.Map.Gate closestGate = api.Logic.GetClosestGate();
                    if (HelpTools.CalculateDistance(closestGate.X, closestGate.Y, targetPlayer.X, targetPlayer.Y) < 100)
                    {
                        api.Logic.Target = null;
                        api.Logic.SetState(this, LogicState.None);
                        return true;
                    }
                }
                if (!targetPlayer.IsSelected)
                {
                    api.WriteLog("Locking " + targetPlayer.Username);
                    api.GameMethods.TargetSelect(targetPlayer);
                }

                if (!api.Ships.ContainsKey(targetPlayer.UserId))
                {
                    api.Logging.Log("Shootback ships does not contain Target: " + api.Logic.Target);
                    api.Hero.LastAttackTime = DateTime.MinValue;
                    api.Logic.SetState(this, LogicState.None);
                    return true;
                }

                if (api.Hero.DroneFormation != api.Settings.OnAttackedShootbackFormation)
                {
                    api.GameMethods.DroneFormationChange(api.Settings.OnAttackedShootbackFormation);
                }
                if (api.Hero.Config != api.Settings.OnAttackedShootbackConfig)
                {
                    api.GameMethods.ConfigurationChange(api.Settings.OnAttackedShootbackConfig);
                }

                if (targetPlayer.ShieldPercentage <= 0 && api.Hero.CurrentLaser.Id == CommandConstants.AMMO_SAB_50 && api.Settings.OnAttackedShootbackLaserType != CommandConstants.AMMO_SAB_50)
                {
                    api.GameMethods.LaserChange(api.Settings.OnAttackedShootbackLaserType);
                }

                if (api.Settings.OnAttackedShootbackUseRSB && (!api.Settings.OnAttackedShootbackUseSAB || targetPlayer.ShieldPercentage <= 0) && (DateTime.UtcNow - targetPlayer.tempRsbLastUsed).TotalMilliseconds > 3300)
                {
                    targetPlayer.tempRsbLastUsed = DateTime.UtcNow;
                    api.GameMethods.LaserChange(CommandConstants.AMMO_RSB);
                    Thread.Sleep(Logic.GetRandomSleepTime(50, 100));
                }
                else if (api.Hero.CurrentLaser.Id == CommandConstants.AMMO_RSB && (DateTime.UtcNow - targetPlayer.tempRsbLastUsed).TotalMilliseconds <= 3300)
                {
                    api.GameMethods.LaserChange(api.Settings.OnAttackedShootbackUseSAB && targetPlayer.ShieldPercentage > 0 ? CommandConstants.AMMO_SAB_50 : api.Settings.OnAttackedShootbackLaserType);
                }

                if (!api.Hero.IsAttacking && targetPlayer.CurrentDistance < 600)
                {
                    if (targetPlayer.tempAttackTries >= 2)
                    {
                        api.GameMethods.Move(targetPlayer, (int)(api.Settings.OnAttackedShootbackRadius / 2));
                        Thread.Sleep((int)(1000 / api.Hero.Speed) * 500);
                    }
                    if (targetPlayer.tempAttackTries > 10 && targetPlayer.CurrentDistance < 650)
                    {
                        api.Logic.Target = null;
                        api.Logic.SetState(this, LogicState.None);
                        return true;
                    }
                    targetPlayer.tempAttackTries += 1;
                    api.WriteLog("Attacking " + targetPlayer.Username);
                    api.GameMethods.AttackStartLaser(targetPlayer.UserId, targetPlayer.X, targetPlayer.Y);
                    Thread.Sleep(Logic.GetRandomSleepTime());
                }

                if (api.Settings.OnAttackedShootbackUseRocket)
                {
                    api.GameMethods.AttackStartRocket(targetPlayer.UserId, targetPlayer.X, targetPlayer.Y);
                }
                if (api.Settings.OnAttackedShootbackUseRocketLauncher)
                {
                    api.GameMethods.AttackStartRocketLauncher();
                }
                if (api.EntitiesUnderAttackBy.ContainsKey(targetPlayer.UserId))
                {
                    var itemUsed = false;
                    if (api.Settings.OnAttackedShootbackUseEMP && !itemUsed && !api.Cooldown.IsCooldownActive(DarkOrbit.Cooldown.CooldownType.ELECTRO_MAGNETIC_PULSE))
                    {
                        api.GameMethods.MenuItemUse(CommandConstants.EQUIPMENT_EMP);
                        api.WriteLog("Using EMP!");
                        itemUsed = true;
                        api.Cooldown.AddCooldown(DarkOrbit.Cooldown.CooldownType.ELECTRO_MAGNETIC_PULSE, 30);
                        Thread.Sleep(Logic.GetRandomSleepTime());
                    }
                    if (api.Settings.OnAttackedShootbackUseISH && !itemUsed && !api.Cooldown.IsCooldownActive(DarkOrbit.Cooldown.CooldownType.INSTANT_SHIELD))
                    {
                        api.WriteLog("Using Instashield!");
                        api.GameMethods.MenuItemUse(CommandConstants.EQUIPMENT_ISH);
                        itemUsed = true;
                        api.Cooldown.AddCooldown(DarkOrbit.Cooldown.CooldownType.INSTANT_SHIELD, 35);
                        Thread.Sleep(Logic.GetRandomSleepTime());
                    }
                    if (itemUsed)
                    {
                        api.EntitiesUnderAttackBy.Remove(targetPlayer.UserId, out _);
                        return true;
                    }
                }

                var flag = (targetPlayer.IsMoving || !api.EntitiesUnderAttackBy.IsEmpty);// || (targetNpc.CurrentDistance > npcSetting.Radius));
                if (flag
                    && (!api.Hero.IsFlying
                        || api.Hero.FlyingTimeMs < 300
                        || targetPlayer.TargetDistance < (api.Settings.OnAttackedShootbackRadius / 2)
                        || (DateTime.UtcNow - targetPlayer.tempLastCircleMove).TotalSeconds >= 2
                    ))
                {
                    var setting = new NpcSetting()
                    {
                        Radius = api.Settings.OnAttackedShootbackRadius,
                    };
                    return api.Logic.DoCircle(targetPlayer, setting, api.EntitiesUnderAttackBy.Count);
                }
                else if (targetPlayer.CurrentDistance > targetPlayer.GetRadius(api.Settings.OnAttackedShootbackRadius))
                {
                    api.GameMethods.Move(api.Logic.GetCirclePosition(targetPlayer.X, targetPlayer.Y, targetPlayer.GetRadius(api.Settings.OnAttackedShootbackRadius), 0));
                    return true;
                }
            }
            return false;
        }
    }
}
