using DarksideApi.DarkOrbit;
using DarksideApi.DarkOrbit.Objects;
using DarksideApi.DarkOrbit.Objects.Boxes;
using DarksideApi.DarkOrbit.Objects.Map;
using DarksideApi.DarkOrbit.Objects.Ship;
using DarksideApi.DarkOrbit.Settings;
using DarksideApi.DarkOrbit.Settings.Interfaces;
using DarksideApi.Util;
using System.Diagnostics;

namespace DarksideApi.Logic.Modules
{
    internal class RepairModule(Api api) : ILogicModule
    {
        public bool Run()
        {
            if (api.Logic.State == LogicState.GotoRepair)
            {
                if (api.Map.IsGalaxyGate)
                {
                    api.Logic.SetState(this, LogicState.GalaxyGateRepair);
                    return false;
                }
                var gateDesign = api.Map.IsFrozenLabyrinth ? 83 : 1;

                IGameObject closestObj = api.Logic.GetClosestGate(gateDesign);
                Gate closestGateNoArea = api.Logic.GetClosestGateWithoutArea();
                MapAsset closestBase = api.Logic.GetClosestBaseBuilding(true);
                Npc closestEmFreighter = api.Logic.GetEMFreighterNpc();
                if ((closestBase.AssetId > 0 && closestBase.CurrentDistance < closestObj.CurrentDistance))
                {
                    closestObj = closestBase;
                }
                PoiZone closestZone = api.Logic.GetClosestEquipmentZone();
                if (closestZone.Type > 0 && (closestZone.CurrentDistance < closestObj.CurrentDistance || closestZone.IsInZone(api.Hero.X, api.Hero.Y)))
                {
                    closestObj = closestZone;
                }
                if (closestGateNoArea.CurrentDistance < 50)
                {
                    closestObj = closestGateNoArea;
                }
                if (api.Map.IsMimesisEscort && !api.Map.IsFrozenLabyrinthSeason)
                {
                    closestObj = closestEmFreighter;
                }

                if (api.Hero.HitpointsPercentage >= 99)
                {
                    api.Logic.SetState(this, LogicState.None);
                    return true;
                }

                if (api.Map.IsMimesisEscort && !api.Map.IsFrozenLabyrinthSeason)
                {
                    api.Logic.SetState(this, LogicState.Repair);
                    Thread.Sleep(500);
                    return true;
                }

                if (closestObj.X <= 0 && closestObj.Y <= 0)
                {
                    if (api.Hero.IsRepairing)
                    {
                        api.Logic.SetState(this, LogicState.Repair);
                    }
                    Thread.Sleep(500);
                    return true;
                }

                if ((!api.Hero.IsFlying && closestObj.CurrentDistance > 100)
                    || (closestObj.TargetDistance > 100
                    && closestObj.PathDistance > 100))
                {
                    api.GameMethods.Move(closestObj, 100);
                    Thread.Sleep(300);
                    return true;
                }
                if (api.Settings.UseFleeFormationForGotoRepair
                    && api.Hero.DroneFormation != api.Settings.OnAttackedFleeFormation
                    && closestObj.CurrentDistance > 200)
                {
                    api.GameMethods.DroneFormationChange(api.Settings.OnAttackedFleeFormation);
                }
                if (!api.Hero.IsRepairing && api.EntitiesUnderAttackBy.IsEmpty)
                {
                    api.GameMethods.Repair();
                    Thread.Sleep(500);
                }
                if (closestObj.CurrentDistance <= 200 || api.Hero.IsRepairing)
                {
                    api.Logic.SetState(this, LogicState.Repair);
                }
                Thread.Sleep(1000);
                return true;
            }
            if (api.Logic.State == LogicState.Repair)
            {
                if (api.Hero.Config != api.Settings.RepairConfig)
                {
                    api.GameMethods.ConfigurationChange(api.Settings.RepairConfig);
                }
                if (api.Hero.DroneFormation != api.Settings.RepairFormation)
                {
                    api.GameMethods.DroneFormationChange(api.Settings.RepairFormation);
                }

                if (api.Hero.HitpointsPercentage >= 99)
                {
                    api.Logic.SetState(this, LogicState.None);
                    api.WriteLog("Finished Repairing...");
                    Thread.Sleep(2000);
                    return true;
                }

                var gateDesign = api.Map.IsFrozenLabyrinth ? 83 : 1;
                IGameObject closestObj = api.Logic.Target is Gate ? api.Logic.Target : api.Logic.GetClosestGate(gateDesign);
                Gate closestGateNoArea = api.Logic.GetClosestGateWithoutArea();
                MapAsset closestBase = api.Logic.GetClosestBaseBuilding(true);
                Npc closestEmFreighter = api.Logic.GetEMFreighterNpc();
                if ((closestBase.AssetId > 0 && closestBase.CurrentDistance < closestObj.CurrentDistance))
                {
                    closestObj = closestBase;
                }
                PoiZone closestZone = api.Logic.GetClosestEquipmentZone();
                if (closestZone.Type > 0 && (closestZone.CurrentDistance < closestObj.CurrentDistance || closestZone.IsInZone(api.Hero.X, api.Hero.Y)))
                {
                    closestObj = closestZone;
                }
                if (closestGateNoArea.CurrentDistance < 50)
                {
                    closestObj = closestGateNoArea;
                }
                if (api.Map.IsMimesisEscort && !api.Map.IsFrozenLabyrinthSeason)
                {
                    closestObj = closestEmFreighter;
                }

                if (api.Hero.IsAttacking && HelpTools.GetChance(32))
                {
                    api.GameMethods.AttackStop();
                }

                if (api.Map.IsMimesisEscort && !api.Map.IsFrozenLabyrinthSeason)
                {
                    api.GameMethods.Repair();

                    var nextPos = api.Logic.GetCirclePosition(closestObj.Position, 1200, 0.7);

                    api.GameMethods.Move(nextPos);

                    Debug.WriteLine("Mimesis Escort - Repairing and moving around EmFreighter");

                    Thread.Sleep(Logic.GetRandomSleepTime(900, 1200));
                    return true;
                }

                if (!api.Hero.IsRepairing)
                {
                    api.GameMethods.Repair();
                    if (!api.EntitiesUnderAttackBy.IsEmpty)
                    {
                        if (closestObj.CurrentDistance < 100)
                        {
                            api.Logic.Target = api.Logic.GetClosestGate(gateDesign);

                            if (!api.Map.IsPirateMap && (!api.Map.IsFrozenLabyrinth || (api.Inventory.GetFrozenLabyrinthAccessKeys > 5 && (DateTime.UtcNow - api.Hero.LastJump).TotalSeconds > 60)))
                            {
                                api.Logic.SetState(this, LogicState.RepairJump);
                                api.Logging.Log("Repair under Attack, jumping...");
                            }
                            else if (HelpTools.CalculateDistance(api.Hero.LastPathElement, api.Logic.Target.Position) < 100)
                            {
                                Gate possibleNextGate = api.Logic.GetNextGate(3);
                                api.Logic.Target = possibleNextGate;
                                api.GameMethods.Move(api.Logic.Target, 55);
                                api.Logging.Log("Repair under Attack, going to next Gate...");
                            }
                            Thread.Sleep(2000);
                            return true;
                        }
                        else if (closestObj.TargetDistance > 100
                            && closestObj.CurrentDistance > 100
                            && HelpTools.CalculateDistance(api.Hero.LastPathElement, closestObj.Position) > 100)
                        {
                            api.GameMethods.Move(closestObj, 45);
                            Thread.Sleep(Logic.GetRandomSleepTime());
                        }
                    }
                }

                if (api.Settings.RepairUseInstantRepair && api.Hero.HitpointsPercentage < 70)
                {
                    MapAsset repairDock = api.Logic.GetClosestRepairDock();
                    if (repairDock.AssetId > 0 && repairDock.CurrentDistance <= 2000 && repairDock.IsInstantRepairAvialable)
                    {
                        if (repairDock.CurrentDistance > 150 && repairDock.TargetDistance > 150)
                        {
                            api.GameMethods.Move(repairDock, 30);
                            Thread.Sleep(Logic.GetRandomSleepTime());
                        }
                        if (repairDock.CurrentDistance <= 200)
                        {
                            api.GameMethods.RepairInstant(repairDock.AssetId);
                            Thread.Sleep(Logic.GetRandomSleepTime());
                        }
                        Thread.Sleep(250);
                        return true;
                    }
                }
                if (api.Hero.IsRepairing && api.Settings.OreSellEverything)
                {
                    MapAsset trader = api.Logic.GetClosestTrader();
                    if (trader.AssetId > 0 && trader.CurrentDistance < 1000 && api.Hero.Cargo.FreeCargoSpace < api.Hero.Cargo.MaxCargoSpace)
                    {
                        if (trader.CurrentDistance > 100 && trader.TargetDistance > 100)
                        {
                            api.GameMethods.Move(trader, 30);
                            Thread.Sleep(Logic.GetRandomSleepTime());
                        }
                        if (trader.CurrentDistance < 100)
                        {
                            var palladium = api.Hero.Cargo.Get(Ore.OreType.PALLADIUM);
                            if (palladium >= 15 && api.Map.Id == 52)
                            {
                                api.GameMethods.OreTrade(palladium);
                                Thread.Sleep(Logic.GetRandomSleepTime());
                            }
                            foreach (KeyValuePair<Ore.OreType, long> ore in api.Hero.Cargo.GetTradeableOres)
                            {
                                api.GameMethods.OreSell(ore.Key, ore.Value);
                                Thread.Sleep(Logic.GetRandomSleepTime());
                            }
                        }
                    }
                }

                Thread.Sleep(2500);
                return true;
            }
            return false;
        }
    }
}
