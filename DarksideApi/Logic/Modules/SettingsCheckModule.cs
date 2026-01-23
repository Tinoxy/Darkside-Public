using DarksideApi.DarkOrbit;
using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Objects.Boxes;
using DarksideApi.DarkOrbit.Objects.Group;
using DarksideApi.DarkOrbit.Objects.Map;
using DarksideApi.DarkOrbit.Objects.Ship;
using DarksideApi.Util;

namespace DarksideApi.Logic.Modules
{
    internal class SettingsCheckModule(Api api) : ILogicModule
    {
        private void SetPause(LogicState state, double duration)
        {
            api.WriteLog($"Will Pause for: {Math.Round(duration * 60, 0)}Min.");
            api.Logic.SetState(this, state);
            api.Logic.PauseStartTime = DateTime.UtcNow;
            api.GameMethods.GameLogout();
        }

        private void CheckRepairSettings()
        {
            if (api.Hero.HitpointsPercentage >= api.Logic.GetRepairAt()
                || api.Logic.State == LogicState.Revive
                || api.Logic.State == LogicState.GotoRepair
                || api.Logic.State == LogicState.Repair
                || api.Logic.State == LogicState.RepairJump
                || api.Logic.State == LogicState.GalaxyGateRepair
                || api.Logic.State == LogicState.GotoFlee
                || api.Logic.State == LogicState.FleeJump
                || api.Logic.State == LogicState.GotoFleeSpotted
                || api.Logic.State == LogicState.GotoShootback
                || api.Logic.State == LogicState.Shootback)
            {
                return;
            }
            api.WriteLog("Going to Repair...");

            api.Logic.SetState(this, api.Map.IsGalaxyGate ? LogicState.GalaxyGateRepair : LogicState.GotoRepair);
        }

        private void CheckTimeBasedMapSettings()
        {
            if (!api.Logic.IsAnyTimebasedWorkmapAvailable(DateTime.UtcNow))
            {
                return;
            }
            DarkOrbit.Settings.WorkMapSetting? nextWorkMapSettingByTime = api.Logic.GetWorkmapAfterTime(DateTime.UtcNow);
            if (nextWorkMapSettingByTime == null
                || nextWorkMapSettingByTime.MapId == api.Settings.MapSelect)
            {
                return;
            }
            if (CommandConstants.MAP_NAMES.TryGetValue(nextWorkMapSettingByTime.MapId, out var nextMapNameByTime))
            {
                api.WriteLog($"Switching Workmap to: {nextMapNameByTime}[{nextWorkMapSettingByTime.StartTimeUTC.ToShortTimeString()}]");
            }
            else
            {
                api.WriteLog($"Switching Workmap {nextWorkMapSettingByTime.StartTimeUTC.ToShortTimeString()}");
            }

            api.Settings.MapSelect = nextWorkMapSettingByTime.MapId;
        }

        private void CheckMapSettings()
        {
            if (!api.Settings.UseMap
                || api.Settings.GeneralDoGalaxyGates
                || IsCorrectMap()
                || api.Logic.State == LogicState.Revive
                || api.Logic.State == LogicState.GotoGateJump
                || api.Logic.State == LogicState.GateJump
                || api.Logic.State == LogicState.RepairJump
                || api.Logic.State == LogicState.GotoRepair
                || api.Logic.State == LogicState.Repair
                || api.Logic.State == LogicState.GotoSellOre
                || api.Logic.State == LogicState.SellOre
                || api.Logic.State == LogicState.SellOreDone
                || api.Logic.State == LogicState.GotoGateJumpToAccessGG
                || api.Hero.IsAttacking)
            {
                return;
            }
            api.WriteLog("Going to Gate...");
            api.Logic.SetState(this, LogicState.GotoGateJump);
        }

        private bool IsCorrectMap()
        {
            if (api.Settings.MapSelect == -2) 
            {
                return api.Map.IsFrozenLabyrinth;
            }
            if (api.Settings.MapSelect == -3)
            {
                if (api.Map.IsMimesisEscort && !api.Gates.IsEmpty)
                {
                    return false;
                }
                return api.Map.IsMimesisEscort;
            }
            return api.Map.Id == api.Settings.MapSelect;
        }

        private void CheckOreSellSettings()
        {
            if (!api.Settings.OreSellWithHanger
                || api.Hero.Cargo.FreeCargoSpace > 0 || (api.Hero.Cargo.Get(Ore.OreType.PALLADIUM) < 15 && api.Hero.Cargo.UsedCargoSpace != api.Hero.Cargo.MaxCargoSpace)
                || api.Map.Id != 93
                || api.Hero.IsAttacking
                || api.Logic.State == LogicState.GotoSellOre
                || api.Logic.State == LogicState.SellOre
                || api.Logic.State == LogicState.SellOreDone
                || api.Logic.State == LogicState.Revive
                || api.Logic.State == LogicState.GotoRepair
                || api.Logic.State == LogicState.Repair
                || api.Logic.State == LogicState.RepairJump
                || api.Logic.State == LogicState.GalaxyGateRepair
                || api.Logic.State == LogicState.GotoFlee
                || api.Logic.State == LogicState.FleeJump
                || api.Logic.State == LogicState.GotoFleeSpotted
                || api.Logic.State == LogicState.GotoShootback
                || api.Logic.State == LogicState.Shootback)
            {
                return;
            }
            api.Logic.SetState(this, LogicState.GotoSellOre);
            api.WriteLog("Going to Sell Ore...");
        }

        private void CheckGroupSettings()
        {
            if (!api.Settings.GroupIsLeader)
            {
                return;
            }
            api.Group ??= new GroupObj(api, api.Hero.Username!, api.Hero.UserId);

            foreach (DarkOrbit.Settings.GroupSetting groupSetting in api.Settings.GroupSettings)
            {
                if (!groupSetting.IsEnabled)
                {
                    continue;
                }
                if (api.Group.IsInvited(groupSetting.Username!))
                {
                    continue;
                }
                api.Group.Invite(groupSetting.Username!);
            }
        }

        private void CheckCloakSettings()
        {
            if (!api.Settings.GeneralAutoCloak
                || api.Hero.IsCloaked
                || !api.EntitiesUnderAttackBy.IsEmpty
                || api.Map.IsGalaxyGate
                || !api.Menu.IsCloakAvailable
                || api.Hero.IsAttacking
                || api.Logic.State == LogicState.Npc
                || api.Logic.State == LogicState.GotoNpc
                || api.Logic.State == LogicState.Shootback
                || api.Logic.State == LogicState.GotoShootback)
            {
                return;
            }
            api.GameMethods.Cloak();
            api.WriteLog("Cloaking...");
        }

        private void CheckPetSettings()
        {
            if (api.Settings.PetUse && !api.Hero.IsPetActive && api.Hero.PetStartTries < 3 && !api.Hero.IsPetOutOfFuel)
            {
                if (((api.Hero.PetStartTries >= 2 
                    && !api.Hero.IsPetOutOfFuel) || api.Hero.IsPetDestroyed)
                    && api.Settings.PetRepair)
                {
                        api.Logic.TryPetRevive();
                    Thread.Sleep(Logic.GetRandomSleepTime());
                }
                if (!api.Hero.IsPetDestroyed)
                    api.GameMethods.PetActivate();
            }
            if (!api.Settings.PetUse && api.Hero.IsPetActive)
            {
                api.GameMethods.PetDeactivate();
            }
            if (!api.Settings.PetUse || !api.Hero.IsPetActive)
            {
                return;
            }
            var gear = api.Hero.Pet.Gear;
            var options = 0;
            if (api.Hero.IsRepairing && api.Settings.PetUseRepairMode
                && api.Hero.Pet.Gear != CommandConstants.PET_GEAR_COMBO_SHIP_REPAIR
                && !api.Hero.Pet.IsAttacking
                && !api.Hero.IsAttacking)
            {
                gear = CommandConstants.PET_GEAR_COMBO_SHIP_REPAIR;
            }
            else
            if (api.Map.IsGalaxyGate && api.Logic.GetGalaxyGateSetting(api.Map.Name).UsePetMode)
            {
                gear = api.Logic.GetGalaxyGateSetting(api.Map.Name).PetMode;
            }
            else
            if (!api.Map.IsGalaxyGate && api.Logic.Target is Npc npc
                && api.Logic.GetNpcSetting(npc).UsePetMode)
            {
                gear = api.Logic.GetNpcSetting(npc).PetMode;
            }
            else
            if (api.Logic.Target is Player
                && api.Settings.OnAttackedShootbackUsePetMode)
            {
                gear = api.Settings.OnAttackedShootbackPetMode;
            }
            else
            if (api.Settings.PetRepairItself 
                && api.Hero.Pet.MaxHitPoints > 0
                && api.Hero.Pet.CurrentHitpointsPercentage < 99)
            {
                gear = CommandConstants.PET_GEAR_REPAIR_PET;
            }
            else
            if (api.Settings.PetMode > 0)
            {
                if (!string.IsNullOrEmpty(api.Settings.PetModeOptions)
                    && Api.NpcIdsByMap.TryGetValue(api.Map.Id, out Dictionary<string, int>? npcs)
                    && npcs.TryGetValue(api.Settings.PetModeOptions ?? string.Empty, out var npcId))
                {
                    options = (short)npcId;
                }
                gear = api.Settings.PetMode == CommandConstants.PET_GEAR_TRADE_POD
                    && (api.Hero.Cargo.FreeCargoSpace > 0 || api.Cooldown.IsCooldownActive(DarkOrbit.Cooldown.CooldownType.PET_TRADE_GEAR) || api.Hero.IsFlying)
                    ? CommandConstants.PET_GEAR_AUTO_LOOT
                    : api.Settings.PetMode;
            }
            if (api.Hero.Pet.Gear == gear && options == api.Hero.Pet.ModeOptions)
            {
                return;
            }
            api.GameMethods.PetModeSet(gear, options);
            if (gear != CommandConstants.PET_GEAR_TRADE_POD)
            {
                return;
            }
            Thread.Sleep(Logic.GetRandomSleepTime(1000, 1600) + (int)api.GameClient.PingTime);
        }

        public bool Run()
        {
            if ((api.Settings.GeneralPauseUsePause && !api.Map.IsGalaxyGate && (DateTime.UtcNow - api.Logic.StartTime).TotalMinutes > 60 * api.Settings.GeneralPauseAfterPlayTime)
                || (api.Settings.GeneralPauseAfterFastDeaths && HelpTools.IsInTimespan([.. api.LastDeaths.Keys], 5))
                || (api.DiminishingQuest != null && api.Settings.GeneralStopWhenDiminishingQuest))
            {
                Gate gate = api.Logic.GetClosestGate(1);

                if (gate.CurrentDistance > 35 && gate.TargetDistance > 35 && !gate.IsOnTarget(api.Hero.LastPathElement))
                {
                    api.GameMethods.Move(gate, 10);
                    Thread.Sleep(1000);
                    return true;
                }

                if (gate.CurrentDistance > 35)
                {
                    Thread.Sleep(500);
                    return true;
                }
                if (api.DiminishingQuest != null && api.Settings.GeneralStopWhenDiminishingQuest)
                {
                    api.WriteLog("Diminishing Quest appeared, logging out...");
                    api.Logic.SetState(this, LogicState.None);
                    return true;
                }
                if ((api.Settings.GeneralPauseAfterFastDeaths && HelpTools.IsInTimespan([.. api.LastDeaths.Keys], 5)))
                {
                    this.SetPause(LogicState.PausingDeaths, 10);
                    return true;
                }
                this.SetPause(LogicState.Pausing, api.Settings.GeneralPausePauseTime);
                return true;
            }

            if (api.Hero.CurrentHangar != null)
            {
                DarkOrbit.Objects.Client.Hangar? tradeHangar = api.Hangars.FirstOrDefault(h => h.HangarId == api.Settings.OreSellHangarId);

                if (tradeHangar != null && (api.Hero.DefaultHangarId <= 0
                    || (api.Hero.DefaultHangarId != api.Hero.CurrentHangar.HangarId && api.Hero.CurrentHangar.HangarId != tradeHangar.HangarId)))
                {
                    api.Hero.DefaultHangarId = api.Hero.CurrentHangar.HangarId;
                }

                if (tradeHangar != null && api.Hero.CurrentHangar.HangarId == tradeHangar.HangarId
                    && api.Logic.State != LogicState.SellOre
                    && api.Logic.State != LogicState.GotoSellOre
                    && api.Logic.State != LogicState.SellOreDone)
                {
                    api.Logic.SetState(this, LogicState.SellOre);
                    return false;
                }
                if ((api.Hero.CurrentHangar.HangarId != api.Hero.DefaultHangarId && api.Logic.State == LogicState.SellOreDone)
                    ||
                    (api.Hero.CurrentHangar.HangarId == api.Hero.DefaultHangarId && api.Logic.State == LogicState.GotoSellOre))
                {
                    if ((api.Logic.State == LogicState.GotoSellOre) && !api.EntitiesUnderAttackBy.IsEmpty)
                    {
                        Gate gate = api.Logic.GetClosestGate();
                        if ((gate.CurrentDistance > 20 
                            && gate.TargetDistance > 20 
                            && !gate.IsOnTarget(api.Hero.LastPathElement))
                            || !api.Hero.IsFlying)
                        {
                            api.GameMethods.Move(gate, 10);
                            return true;
                        }

                        if (gate.CurrentDistance > 20)
                        {
                            Thread.Sleep(1000);
                            return true;
                        }
                    }

                    if (api.EntitiesUnderAttackBy.IsEmpty)
                    {
                        api.GameMethods.Move(api.Hero, 20);
                        Thread.Sleep(Logic.GetRandomSleepTime());
                    }

                    if (!api.Hero.IsLoggingOut && !api.Hero.IsFlying)
                    {
                        api.GameMethods.GameLogout();
                        Thread.Sleep(1000);
                        return true;
                    }
                }
            }

            if (api.Settings.UseMapMaxDeaths && api.LastDeaths.Count > api.Settings.MapMaxDeaths
                && api.Logic.State != LogicState.Revive
                && api.Logic.State != LogicState.GotoRepair
                && api.Logic.State != LogicState.Repair
                && api.Logic.State != LogicState.RepairJump
                && api.Logic.State != LogicState.GalaxyGateRepair
                && api.Logic.State != LogicState.GotoFlee
                && api.Logic.State != LogicState.FleeJump
                && api.Logic.State != LogicState.GotoFleeSpotted
                && api.Logic.State != LogicState.GotoShootback
                && api.Logic.State != LogicState.Shootback)
            {
                api.WriteLog($"Too many Deaths({api.LastDeaths.Count}), stopping...");
                api.GameObservers.SessionObserver.StopObserver();
                api.GameObservers.GameTaskObserver.StopObserver();
                api.GameMethods.GameLogout();
                api.Logic.SetState(this, DarksideApi.Logic.LogicState.Pausing);
                api.GameClient.CanDisconnect = false;
                api.Logic.StopLogic();
                return true;
            }

            this.CheckRepairSettings();

            if (api.Logic.State == LogicState.GotoDiminishingQuest)
            {
                //
            }

            if (api.Settings.GeneralDoGalaxyGates && !api.Map.IsGalaxyGate && api.Settings.GalaxyGateSettings.Any(s => s.IsEnabled)
                && api.Logic.State != LogicState.GotoGateJumpToAccessGG
                && api.Logic.State != LogicState.Repair
                && api.Logic.State != LogicState.GotoRepair
                && !api.Gates.IsEmpty)
            {
                Gate gg = api.Logic.GetClosestGalaxyGate();

                if (gg.GateId <= 1)
                {
                    api.Logging.Log("GateId returned smaller Value than 2, checking access by...");
                    KeyValuePair<int, int> accessedBy = api.Logic.GetClosestGalaxyGateId();
                    if (accessedBy.Value <= 0)
                    {
                        api.Logging.Log("Accessed By returned -1");
                        api.Settings.GeneralDoGalaxyGates = false;
                        return true;
                    }
                    if (api.Logic.State == LogicState.GateJump)
                    {
                        api.Logging.Log("Already jumping to Galaxy Gate, waiting for it to finish...");
                        return false;
                    }
                    if (accessedBy.Value == api.Map.Id)
                    {
                        gg = api.Logic.GetClosestGate(accessedBy.Key);
                        if (gg.GateId <= 1)
                        {
                            api.Logging.Log("No Gate found to access Galaxy Gate, stopping Galaxy Gates...");
                            api.Settings.GeneralDoGalaxyGates = false;
                            return true;
                        }

                        goto IL_CORRECT_MAP;
                    }

                    if (!CommandConstants.GALAXY_GATES.TryGetValue(accessedBy.Key, out var ggName))
                    {
                        ggName = "GG";
                    }

                    api.WriteLog($"Going to Gate to access next {ggName}...");
                    api.Hero.GGAccessTargetMapId = accessedBy.Value;
                    api.Logic.SetState(this, LogicState.GotoGateJumpToAccessGG);

                    return false;
                }

            IL_CORRECT_MAP:
                if (!api.Hero.IsFlying && gg.CurrentDistance > 10 && gg.TargetDistance > 10 && !gg.IsOnTarget(api.Hero.LastPathElement))
                {
                    api.GameMethods.Move(gg, 10);
                    Thread.Sleep(1000);
                    return true;
                }

                api.Hero.GGAccessTargetMapId = 0;
                if (gg.CurrentDistance <= 20)
                {

                    var name = "next GG";
                    if (CommandConstants.GALAXY_GATES.TryGetValue(gg.GateDesignId, out var galaxyGateName))
                    {
                        name = galaxyGateName;
                    }

                    api.WriteLog("Jumping to " + name);
                    api.GameMethods.Jump();
                    api.Logic.SetState(this, LogicState.GalaxyGate);
                    Thread.Sleep(2000);
                }

                Thread.Sleep(1000);
                return true;
            }
            else if (api.Settings.GeneralDoGalaxyGates && api.Map.IsGalaxyGate && api.Logic.State != LogicState.GalaxyGateRepair && api.Logic.State != LogicState.GalaxyGate)
            {
                api.Logic.SetState(this, LogicState.GalaxyGate);
            }

            this.CheckTimeBasedMapSettings();

            this.CheckMapSettings();

            this.CheckOreSellSettings();

            this.CheckPetSettings();

            this.CheckCloakSettings();

            this.CheckGroupSettings();

            return false;
        }
    }
}
