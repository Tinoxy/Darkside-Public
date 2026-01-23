using DarksideApi.DarkOrbit;
using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Objects.Boxes;
using DarksideApi.DarkOrbit.Objects.Map;
using DarksideApi.DarkOrbit.Objects.Ship;
using DarksideApi.DarkOrbit.Settings;
using DarksideApi.Util;
using System.Diagnostics;
using System.Numerics;

namespace DarksideApi.Logic.Modules
{
    internal class GalaxyGateModule(Api api) : ILogicModule
    {
        private const int targetToCloseSub = 90;
        private const int targetInRange = 1062;
        private const int targetMaxVariation = 50;
        private int degreeRemoveIndex = 0;

        private const int HitpointsPercentageFull = 99;
        private const int RepairNpcDistanceThreshold = 1200;
        private const int RepairNpcPathDistanceThreshold = 3000;
        private const int RepairNpcCurrentDistanceThreshold = 3000;
        private const int RepairFlyingTimeMsThreshold = 300;
        private const int RepairNpcCurrentDistanceCircleThreshold = 750;
        private const int CircleRadiusBase = 1399;
        private const int CircleRadiusRandomMin = 50;
        private const int CircleRadiusRandomMax = 250;
        private const int MoveSleepShort = 100;
        private const int MoveSleepMedium = 500;
        private const int MoveSleepLong = 1000;
        private const int MoveSleepVeryLong = 2000;
        private const int BoxTargetDistanceThreshold = 1;
        private const int NpcDefendableDistance = 300;
        private const int CircleMoveCooldownMs = 200;
        private const double FleeMoveFactor = 0.03;

        public bool Run()
        {
            if (api.Logic.State == LogicState.GalaxyGateRepair)
            {
                api.Logic.Target = null;
                if (api.Hero.HitpointsPercentage >= HitpointsPercentageFull || !api.Map.IsGalaxyGate)
                {
                    api.WriteLog("Finished Repairing...");
                    api.Logic.SetState(this, LogicState.GalaxyGate);
                    return true;
                }
                Npc closestNpc = api.Logic.GetClosestGGNpcNoLimitation();
                GalaxyGateSetting setting = api.Logic.GetGalaxyGateSetting(api.Map.Name);

                if (api.Hero.Config != setting.RepairConfig)
                {
                    api.GameMethods.ConfigurationChange(setting.RepairConfig);
                }
                if (api.Hero.DroneFormation != setting.RepairFormation)
                {
                    api.GameMethods.DroneFormationChange(setting.RepairFormation);
                }

                if (!api.Hero.IsRepairing)
                {
                    api.GameMethods.Repair();
                }

                bool doNextMove = false;
                if (closestNpc.CurrentDistance > RepairNpcDistanceThreshold && !api.EntitiesUnderAttackBy.IsEmpty)
                {
                    api.Logging.Log("SERVER LAG! MOVING AWAY!");
                    doNextMove = true;
                }
                else if (!api.EntitiesUnderAttackBy.IsEmpty ||
                    (!setting.CircleWhileRepairing && closestNpc.CurrentDistance < RepairNpcCurrentDistanceThreshold && (closestNpc.PathDistance < RepairNpcPathDistanceThreshold || !api.Hero.IsFlying)))
                {
                    api.Logging.Log($"NEXT MOVE: CIRCLE: {setting.CircleWhileRepairing} & Current Distance: {closestNpc.CurrentDistance} & Path Distance: {closestNpc.PathDistance} || Under Attack: {!api.EntitiesUnderAttackBy.IsEmpty}");
                    doNextMove = true;
                }
                else if (!api.Map.IsInMap(api.Hero.Position))
                {
                    api.Logging.Log($"NEXT MOVE: HERO NOT IN MAP: {api.Map.IsInMap(api.Hero.Position)} ");
                    doNextMove = true;
                }

                if (!doNextMove)
                {
                    api.Logging.Log($"NO NEXT MOVE: CIRCLE: {setting.CircleWhileRepairing} & Current Distance: {closestNpc.CurrentDistance} & Path Distance: {closestNpc.PathDistance} || Under Attack: {api.EntitiesUnderAttackBy.IsEmpty}");
                    Thread.Sleep(MoveSleepMedium);
                    return true;
                }

                // --- Begin IL_NEXT_MOVE logic ---
                Vector2 nextQuarter = api.Logic.GetNextQuarter(0);

                if (nextQuarter == api.Logic.GetQuarter())
                {
                    var adaptedQuarter = api.Logic.GetQuarter((api.Logic.GetQuarterIndex(api.Hero.Position) + 1) % 4);
                    api.Logging.Log($"NEXT QUARTER = CURRENT QUARTER! Adapted: {adaptedQuarter} Original: {nextQuarter}");
                    nextQuarter = adaptedQuarter;
                }

                if (api.Logic.GetActiveGGNpcsCount() <= 0 && api.EntitiesUnderAttackBy.IsEmpty)
                {
                    api.Logging.Log($"ACTIVE COUNT: {api.Logic.GetActiveGGNpcsCount()}, NEXT QUARTER = CURRENT QUARTER! Original: {nextQuarter}");
                    nextQuarter = api.Logic.GetQuarter();
                }

                if (setting.CircleWhileRepairing && api.Logic.GetActiveGGNpcsCount() > 0)
                {
                    if (api.Hero.FlyingTimeMs >= RepairFlyingTimeMsThreshold && closestNpc.CurrentDistance > RepairNpcCurrentDistanceCircleThreshold)
                    {
                        return true;
                    }

                    var rad = (CircleRadiusBase + Random.Shared.Next(CircleRadiusRandomMin, CircleRadiusRandomMax));
                    if (!api.Logic.DoCircle(closestNpc, new GalaxyGateSetting() { Radius = rad, UseCircle = true, }))
                    {
                        Vector2 newPosition = api.Logic.GetCirclePosition(closestNpc.X, closestNpc.Y, rad, closestNpc.tempCircleClockwise ? 0.5 : -0.5);
                        if (!api.Map.IsInMap(newPosition))
                        {
                            closestNpc.tempCircleClockwise = !closestNpc.tempCircleClockwise;
                            newPosition = api.Logic.GetCirclePosition(closestNpc.X, closestNpc.Y, rad, closestNpc.tempCircleClockwise ? 0.5 : -0.5);
                        }

                        PoiZone closestZone = api.Logic.GetClosestPoiZone(rad);
                        if (!string.IsNullOrEmpty(closestZone.ZoneId)
                            && closestZone.IsInZone((int)newPosition.X, (int)newPosition.Y))
                        {
                            closestNpc.tempCircleClockwise = !closestNpc.tempCircleClockwise;
                            newPosition = api.Logic.GetCirclePosition(closestNpc.X, closestNpc.Y, rad, closestNpc.tempCircleClockwise ? 0.5 : -0.5);
                        }

                        api.GameMethods.Move(newPosition);
                    }

                    Thread.Sleep(MoveSleepShort);

                    return true;
                }

                if (api.Hero.IsFlying)
                {
                    api.Logging.Log("NEXT MOVE ALREADY FLYING");
                    Thread.Sleep(MoveSleepLong);
                    return true;
                }

                if (!setting.CircleWhileRepairing)
                {
                    api.Logging.Log($"NO CIRCLE NEXT MOVE: FLYING: {api.Hero.IsFlying} & PATH != QUARTER: {api.Hero.LastPathElement != nextQuarter} NEXT-QUARTER: {nextQuarter}");
                    api.GameMethods.Move(nextQuarter);
                    Thread.Sleep(Logic.GetRandomSleepTime());
                    return true;
                }

                Thread.Sleep(MoveSleepVeryLong);
                return true;
            }

            if (api.Logic.State == LogicState.GalaxyGate)
            {
                GalaxyGateSetting setting = api.Logic.GetGalaxyGateSetting(api.Map.Name);
                var config = setting.Config;
                var formation = setting.DroneFormation;
                var laser = setting.LaserId;
                var rocket = setting.RocketId;
                var launcher = setting.RocketLauncherId;
                var radius = setting.Radius;

                if (api.Ships.IsEmpty && api.Gates.IsEmpty && api.Collectibles.IsEmpty)
                {
                    api.Logic.Target = null;
                    Thread.Sleep(1000);
                    return true;
                }

                if (api.Ships.IsEmpty && api.Map.CurrentGGWave > 0 && api.Hero.DroneFormation != setting.FinishFormation)
                {
                    api.GameMethods.DroneFormationChange(setting.FinishFormation);
                    Thread.Sleep(200);
                }

                bool beaconFlag = false;
                if (api.Map.IsPlutus && !api.MapAssets.IsEmpty)
                {
                    MapAsset asset = api.Logic.GetClosestGOPPositivAsset();

                    if (asset.AssetId > 0)
                    {
                        if (asset.CurrentDistance > 10 && asset.TargetDistance > 10 && api.Hero.LastPathElement != asset.Position && (!api.Hero.IsFlying || api.POIZones.IsEmpty))
                        {
                            api.WriteLog("Going to next Beacon");
                            api.GameMethods.Move(asset);
                        }

                        if (asset.CurrentDistance > 10)
                        {
                            Thread.Sleep(200);
                            return true;
                        }

                        if (asset.CurrentDistance <= 10)
                        {
                            beaconFlag = true;
                        }
                    }
                }

                if (setting.CollectBoxes && !api.Collectibles.IsEmpty && api.Ships.IsEmpty)
                {
                    Box box = api.Logic.GetClosestBoxGG();

                    if (!string.IsNullOrEmpty(box.Hash))
                    {
                        api.Logic.Target = box;
                        if ((box.TargetDistance > 10
                            && box.PathDistance > 10) 
                            || (!api.Hero.IsFlying && api.Logic.Target.CurrentDistance > 10))
                        {
                            api.WriteLog("Going to next " + box.Type);
                            api.GameMethods.Move(box);
                        }

                        if (box.CurrentDistance > 10)
                        {
                            Thread.Sleep(100);
                            return true;
                        }

                        api.GameMethods.BoxCollect(box);

                        while (api.Hero.IsCollecting)
                        {
                            Thread.Sleep(10);
                            continue;
                        }

                        //api.AddLootBox(box.Type, 1);
                        api.Collectibles.Remove(box.Hash, out _);
                        return true;
                    }
                }

                if (!api.Gates.IsEmpty)
                {
                    if (api.Logic.Target is not Gate)
                    {
                        //api.Logic.Target = api.Logic.GetGateByDistanceTo(2000, 2000);
                        var gateToIgnore = (((api.Map.CurrentGGWave >= setting.StopAtWave || (api.Map.CurrentGGWave + 1) >= setting.StopAtWave) && setting.StopAtWave > 0) ? setting.GalaxyGateId : 1);
                        if (api.Gates.Count > 2)
                        {
                            List<Gate> gates = api.Logic.GetClosestGatesIgnoringDesignId(gateToIgnore);
                            api.Logic.Target = gates.ElementAt(Random.Shared.Next(0, gates.Count - 1));
                        }
                        else
                        {
                            api.Logic.Target = api.Logic.GetClosestGateIgnoringDesignId(gateToIgnore);
                        }
                    }

                    if ((api.Logic.Target.CurrentDistance > targetMaxVariation
                        && api.Logic.Target.TargetDistance > targetMaxVariation
                        && api.Logic.Target.PathDistance > targetMaxVariation)
                        || (!api.Hero.IsFlying && api.Logic.Target.CurrentDistance > 10))
                    {
                        api.GameMethods.Move(api.Logic.Target);
                        Thread.Sleep(Logic.GetRandomSleepTime());
                        return true;
                    }

                    if (api.Logic.Target.CurrentDistance <= 10)
                    {
                        var name = "GG";
                        if (CommandConstants.GALAXY_GATES.TryGetValue(((Gate)api.Logic.Target).GateDesignId, out var ggName))
                        {
                            name = ggName;
                        }
                        Thread.Sleep(2000);
                        api.WriteLog($"Jumping to next {name} Wave");
                        api.GameMethods.Jump();
                    }

                    Thread.Sleep(1000);
                    return true;
                }

                if (!api.Ships.IsEmpty)
                {
                    Npc targetNpc = api.Logic.GetClosestGGNpc();
                    var possibleSettings = api.Logic.GetNpcSettingByName(targetNpc.Username, true);

                    if (possibleSettings != null)
                    {
                        config = possibleSettings.Config;
                        formation = possibleSettings.DroneFormation;
                        laser = possibleSettings.LaserId;
                        rocket = possibleSettings.RocketId;
                        launcher = possibleSettings.RocketLauncherId;
                        radius = possibleSettings.Radius;
                        Debug.WriteLine("Changed Attack Settings to NPC-Specific");
                    }

                    if (api.Hero.Config != config)
                    {
                        api.GameMethods.ConfigurationChange(config);
                    }

                    if (api.Hero.CurrentLaser.Id != laser)
                    {
                        api.GameMethods.LaserChange(laser);
                    }
                    if (setting.UseRockets && api.Hero.CurrentRocket.Id != rocket)
                    {
                        api.GameMethods.RocketChange(rocket);
                    }
                    if (setting.UseRocketLauncher && api.Hero.CurrentRocketLauncher.Id != launcher)
                    {
                        api.GameMethods.RocketLauncherChange(launcher);
                    }

                    if (api.Logic.Target is Npc currentNpc
                        && api.Ships.ContainsKey(currentNpc.UserId)
                        && !currentNpc.IsProtected
                        && !currentNpc.IsDefendable
                        && (api.Logic.GetActiveGGNpcsCount() <= 0 || !currentNpc.IsFleeingToCorner)
                        && (currentNpc.CurrentDistance - targetNpc.CurrentDistance) < (radius / 2)
                        && !targetNpc.IsWarhead)
                    {
                        targetNpc = currentNpc;
                    }
                    api.Logic.Target = targetNpc;

                    if (!api.Ships.ContainsKey(targetNpc.UserId))
                    {
                        api.Logic.Target = null;
                        return true;
                    }

                    targetNpc ??= api.Logic.GetClosestGGNpc();
                    var isTargetDistanceToClose = (targetNpc.TargetDistance < (radius - targetToCloseSub) && targetNpc.CurrentDistance <= targetInRange);

                    if ((targetNpc.CurrentDistance > radius + targetMaxVariation)
                        && (targetNpc.TargetDistance > radius || isTargetDistanceToClose)
                        && targetNpc.PathDistance >= radius - targetToCloseSub
                        && ((!api.Hero.IsFlying || api.POIZones.Count < 2) || isTargetDistanceToClose)
                        && !beaconFlag)
                    {
                        Vector2 newPosition = api.Hero.Path.Count > 1 ? api.Hero.FirstPathElement : api.Logic.GetCirclePosition(targetNpc.X, targetNpc.Y, (targetNpc.IsFleeingToCorner ? (radius * 0.9) : radius), 0);
                        if (targetNpc.IsInCorner && api.Logic.GetActiveGGNpcsCount() < 1)
                        {

                            /*if (HelpTools.CalculateDistance(0, 0, targetNpc.X, targetNpc.Y) < HelpTools.CalculateDistance(20495, 13363, targetNpc.X, targetNpc.Y))
                                newPosition = new(450, 302);
                            else
                                newPosition = new(20495, 13363);*/
                            newPosition = api.Logic.GetCirclePosition(targetNpc.X, targetNpc.Y, (radius * 0.95), 0.0);
                        }
                        if (HelpTools.CalculateDistance(api.Hero.TargetX, api.Hero.TargetY, newPosition.X, newPosition.Y) < 10)
                        {
                            Debug.WriteLine("Move to ignore: target distance to new Position < 10");
                            return true;
                        }
                        api.GameMethods.Move(newPosition);
                        Thread.Sleep(Logic.GetRandomSleepTime());
                        Debug.WriteLine("Move to: " + newPosition);
                        return true;
                    }

                    if (targetNpc.IsProtected && api.Ships.Values.OfType<Npc>().Any(s => !s.IsProtected))
                    {
                        Debug.WriteLine($"Target NPC {targetNpc.Username} is Protected!");
                        return true;
                    }
                    if (targetNpc.IsDefendable && targetNpc.CurrentDistance > NpcDefendableDistance)
                    {
                        api.GameMethods.Move(targetNpc, 100);
                        Thread.Sleep(Logic.GetRandomSleepTime());
                        return true;
                    }

                    if (targetNpc.IsInCorner && api.Hero.DroneFormation != setting.CornerFormation)
                        api.GameMethods.DroneFormationChange(setting.CornerFormation);
                    else if (!targetNpc.IsInCorner && api.Hero.DroneFormation != formation)
                        api.GameMethods.DroneFormationChange(formation);

                    if (targetNpc.CurrentDistance > targetInRange)
                    {
                        return true;
                    }

                    if (!targetNpc.IsSelected)
                    {
                        Debug.WriteLine("Selecting: " + targetNpc.Username);
                        api.GameMethods.TargetSelect(targetNpc);
                        Thread.Sleep(Logic.GetRandomSleepTime());
                    }

                    if (targetNpc.CurrentDistance > radius && targetNpc.TargetDistance <= radius)
                    {
                        Debug.WriteLine("CURRENT DISTANCE > RADIUS & TARGET DISTANCE <= RADIUS");
                        return true;
                    }
                    if (!api.Hero.IsAttacking || api.Hero.CurrentTarget != targetNpc.UserId)
                    {
                        if (targetNpc.tempAttackTries > 3)
                        {
                            targetNpc.tempAttackTries = 0;
                            api.Logic.Target = null;
                            api.GameMethods.Move(new Vector2(api.Hero.X + (int)Math.Round((targetNpc.X - api.Hero.X) * FleeMoveFactor, 0), api.Hero.Y + (int)Math.Round((targetNpc.Y - api.Hero.Y) * FleeMoveFactor, 0)));
                            return true;
                        }
                        targetNpc.tempAttackTries += 1;
                        api.GameMethods.AttackStartLaser(targetNpc.UserId, targetNpc.X, targetNpc.Y);
                        Thread.Sleep(Logic.GetRandomSleepTime());
                    }

                    if (setting.UseRockets)
                    {
                        api.GameMethods.AttackStartRocket(targetNpc.UserId, targetNpc.X, targetNpc.Y);
                    }
                    if (setting.UseRocketLauncher)
                    {
                        api.GameMethods.AttackStartRocketLauncher();
                    }

                    if (!api.POIZones.IsEmpty
                        && targetNpc.PathDistance <= radius
                        && api.Hero.IsFlying
                        && !api.Hero.IsAttacking)
                    {
                        Debug.WriteLine($"ZONES NOT EMPTY & PathDistance: {targetNpc.PathDistance} & FLYING: {api.Hero.IsFlying} & ATTACKING: {api.Hero.IsAttacking}");
                        return true;
                    }

                    if (targetNpc.IsInCorner && setting.CornerFormation != api.Hero.DroneFormation)
                    {
                        //api.GameMethods.DroneSetFormation(setting.CornerFormation);
                    }

                    Box closeBox = api.Logic.GetClosestBoxWithoutTargetGG();
                    if (!string.IsNullOrEmpty(closeBox.Hash)
                        && HelpTools.CalculateDistance(closeBox.Position, targetNpc.Position) < radius + targetMaxVariation
                        && HelpTools.CalculateDistance(closeBox.Position, targetNpc.Position) > radius * 0.75
                        && !targetNpc.IsInCorner)
                    {
                        if (closeBox.CurrentDistance <= BoxTargetDistanceThreshold)
                        {
                            api.GameMethods.BoxCollect(closeBox);
                            api.Collectibles.Remove(closeBox.Hash, out _);
                            return true;
                        }
                        if (closeBox.TargetDistance > BoxTargetDistanceThreshold)
                        {
                            api.WriteLog($"Going to Box in Range {closeBox.Type}.");
                            api.GameMethods.Move(closeBox);
                            return true;
                        }
                        if (closeBox.TargetDistance <= BoxTargetDistanceThreshold)
                        {
                            return true;
                        }
                    }

                    if (!targetNpc.IsInCorner
                        && ((api.Logic.IsGGNpcsBunchedUp() && setting.UseCircle) || targetNpc.IsTurrent)
                        && (DateTime.UtcNow - targetNpc.tempLastCircleMove).TotalMilliseconds >= CircleMoveCooldownMs)
                    {
                        Npc circleNpc = targetNpc;
                        /*var protectedNpc = api.Logic.GetClosestGGProtectedNpc();
                        if (protectedNpc.UserId > 1000 && protectedNpc != targetNpc)
                        {
                            circleNpc = protectedNpc;
                        }*/
                        Debug.WriteLine("CIRCLE");
                        return api.Logic.DoCircle(circleNpc, setting);
                    }
                    Debug.WriteLine($"NO CIRCLE! CORNER: {targetNpc.IsInCorner} BUNCHED {api.Logic.IsGGNpcsBunchedUp()} & USE: {setting.UseCircle} || TURRENT: {targetNpc.IsTurrent} & TIME: {(DateTime.UtcNow - targetNpc.tempLastCircleMove).TotalMilliseconds}");
                    if (!targetNpc.IsInCorner
                        && api.Hero.FlyingTimeMs < 200)
                    {
                        Vector2 nextCorner = api.Logic.GetDragPosition(api.Hero.Position, radius * 2);
                        var index = api.Map.GetCornerIndex(nextCorner);
                        if (index != api.Logic.GetQuarterMoveCounter())
                        {
                            this.degreeRemoveIndex = 0;
                        }
                        api.Logic.SetQuarterMoveCounter(index);
                        var degree = api.Logic.GetDegreeFromCornerIndex(index);
                        Vector2 newPosition = api.Logic.GetCirclePosition(targetNpc.X, targetNpc.Y, radius, degree - (this.degreeRemoveIndex < degree ? this.degreeRemoveIndex : 0));
                        this.degreeRemoveIndex += 1;

                        if (!api.Map.IsInMap(newPosition))
                        {
                            this.degreeRemoveIndex -= 1;
                            targetNpc.tempCircleClockwise = !targetNpc.tempCircleClockwise;
                            return true;
                        }

                        PoiZone closestZone = api.Logic.GetClosestPoiZone(radius);
                        if (!string.IsNullOrEmpty(closestZone.ZoneId) && closestZone.IsInZone((int)newPosition.X, (int)newPosition.Y))
                        {
                            this.degreeRemoveIndex -= 1;
                            targetNpc.tempCircleClockwise = !targetNpc.tempCircleClockwise;
                            return true;
                        }

                        api.GameMethods.Move(newPosition);
                        return true;
                    }
                }

                return true;
            }
            return false;
        }
    }
}
