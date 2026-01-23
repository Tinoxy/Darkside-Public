using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Objects.Boxes;
using DarksideApi.DarkOrbit.Objects.Hero;
using DarksideApi.DarkOrbit.Objects.Ship;
using DarksideApi.DarkOrbit.Settings;
using DarksideApi.Util.Extensions;

namespace DarksideApi.DarkOrbit.GameObservers
{
    public class GameTaskObserver : IGameObserver
    {
        private readonly Api api;
        private bool _running;
        private Thread? thread;

        private DateTime lastDispatcherServerUpdate;
        private DateTime lastDispatcherLocalUpdate;
        private DateTime lastGalaxyGateGeneratorServerUpdate;

        // Magic numbers as constants
        private const int ObserverSleepMs = 1000;
        private const int UnderAttackTimeoutMs = 4000;
        private const int UnderAttackMinDistance = 1524;
        private const int BiddingMinute = 49;
        private const int BiddingStartSecond = 44;
        private const int BiddingEndSecond = 59;
        private const int DispatcherQuickFinishThreshold = 10;
        private const int DispatcherSleepMs = 100;
        private const int DispatcherUpdateSleepMs = 200;
        private const int LabSleepMs = 500;
        private const int CargoSleepMs = 500;
        private const int GalaxyGateGeneratorUpdateMinutes = 5;
        private const int GalaxyGateGeneratorSpinWindowSeconds = 90;
        private const int GalaxyGateSpinSleepMs = 100;
        private const int RebuyLaserThreshold = 1000;
        private const int RebuyRocketThreshold = 100;
        private const int RebuyRocketLauncherThreshold = 100;
        private const int PetFuelThreshold = 1;

        public GameTaskObserver(Api api)
        {
            this.api = api;
            this.lastDispatcherServerUpdate = DateTime.MinValue;
            this.lastDispatcherLocalUpdate = DateTime.MinValue;
            this.lastGalaxyGateGeneratorServerUpdate = DateTime.UtcNow.AddMinutes(-3);
        }

        public void InitTaskObserver()
        {
            this.StopObserver();
            this.StartObserver();
        }

        public void StartObserver()
        {
            this._running = true;
            this.thread = new(this.Observe)
            {
                Priority = ThreadPriority.AboveNormal,
                IsBackground = true,
            };
            this.thread.Start();
        }

        public void StopObserver()
        {
            try
            {
                this._running = false;
                this.thread?.Join();
            }
            catch (Exception) { }
        }

        public void ForceLastGalaxyGateSpinnerUpdate()
        {
            this.lastGalaxyGateGeneratorServerUpdate = DateTime.MinValue;
        }

        public void Observe()
        {
            this.api.Logging.Log("Starting Task Observer");

            var biddingCompleted = false;
            while (this._running)
            {
                try
                {
                    Thread.Sleep(ObserverSleepMs);

                    //STOP HERE IF BOT IS NOT CONNECTED
                    if (this.api.GameClient.GameState != GameState.Ready)
                    {
                        continue;
                    }

                    //BIDDING CHECKER
                    if (!biddingCompleted && this.api.Settings.IsRunning && DateTime.UtcNow.TimeOfDay >= new TimeSpan(DateTime.UtcNow.Hour, BiddingMinute, BiddingStartSecond) && DateTime.UtcNow.TimeOfDay < new TimeSpan(DateTime.UtcNow.Hour, BiddingMinute, BiddingEndSecond))
                    {
                        biddingCompleted = true;
                        this.api.Auction.ClearBids();
                        this.api.Auction.InitializeAuction();
                    }
                    else if (DateTime.UtcNow.TimeOfDay >= new TimeSpan(DateTime.UtcNow.Hour, BiddingMinute, BiddingEndSecond))
                    {
                        biddingCompleted = false;
                    }

                    //UNDER ATTACK UPDATER
                    for (var i = 0; i < this.api.EntitiesUnderAttackBy.Count; i++)
                    {
                        KeyValuePair<int, DateTime> attacker = this.api.EntitiesUnderAttackBy.ElementAt(i);
                        if ((DateTime.UtcNow - attacker.Value).TotalMilliseconds < UnderAttackTimeoutMs && this.api.Ships.TryGetValue(attacker.Key, out Ship? ship) && ship.CurrentDistance >= UnderAttackMinDistance)
                        {
                            continue;
                        }
                        this.api.EntitiesUnderAttackBy.Remove(attacker.Key, out _);
                    }

                    //ABILITY CHECKER
                    for (var i = 0; i < this.api.Settings.AbilitySettings.Count; i++)
                    {
                        if (!this.api.Settings.IsRunning)
                        {
                            break;
                        }
                        AbilitySetting setting = this.api.Settings.AbilitySettings.ElementAt(i);
                        if (!setting.Activatable(this.api))
                        {
                            continue;
                        }
                        if (!this.api.Hero.CurrentAbilities.TryGetValue(setting.AbilityType ?? string.Empty, out MenuItem? abilityMenuItem))
                        {
                            continue;
                        }
                        if (!abilityMenuItem.IsAvailable || !abilityMenuItem.IsActivatable)
                        {
                            continue;
                        }
                        this.api.GameMethods.AbilityActivate(abilityMenuItem.Id);
                        setting.lastUsed = DateTime.UtcNow;
                        Thread.Sleep(Logic.Logic.GetRandomSleepTime());
                    }

                    //CARGO & LAB CHECKER
                    for (var i = 0; i < this.api.Settings.LabSettings.Count; i++)
                    {
                        LabSetting labSetting = this.api.Settings.LabSettings[i];
                        if (!labSetting.IsEnabled)
                        {
                            continue;
                        }
                        if (!this.api.Hero.Cargo.UpgradeLab((Cargo.LabType)labSetting.Type, (Ore.OreType)labSetting.UsingOreType, this.api.Settings.LabSettings.Count(l => l.UsingOreType == labSetting.UsingOreType)))
                            continue;

                        Thread.Sleep(LabSleepMs);
                    }
                    for (var i = 0; i < this.api.Settings.CargoUpgradeSettings.Count; i++)
                    {
                        CargoSettings cargoSetting = this.api.Settings.CargoUpgradeSettings[i];
                        if (!cargoSetting.IsEnabled)
                        {
                            continue;
                        }
                        if (!this.api.Hero.Cargo.UpgradeCargo((Ore.OreType)cargoSetting.Type))
                            continue;

                        Thread.Sleep(CargoSleepMs);
                    }

                    //DISPATCHER CHECKER
                    if (this.api.Settings.UseDispatcher)
                    {
                        if (!this.api.Hero.ActiveDispatchers.IsEmpty)
                        {
                            for (var i = 0; i < this.api.Hero.ActiveDispatchers.Count; i++)
                            {
                                Objects.Client.DispatcherObj dispatcher = this.api.Hero.ActiveDispatchers.ElementAt(i).Value;
                                DispatcherSetting setting = this.api.Settings.DispatcherSettings.First(ds => ds.Id == dispatcher.Id) ?? new DispatcherSetting();

                                if (dispatcher.Duration > 0
                                    && (!setting.InstantFinish || this.api.Hero.DispatcherInstantFinishActivationAmounts.TryGetValue(dispatcher.Id, out var activationTimes) && activationTimes >= setting.InstantFinishAmount))
                                {
                                    continue;
                                }
                                if (dispatcher.DurationPercentage > DispatcherQuickFinishThreshold
                                    && setting.InstantFinish
                                    && this.api.Inventory.GetQuickCoupons > 0
                                    && (!this.api.Hero.DispatcherInstantFinishActivationAmounts.TryGetValue(dispatcher.Id, out var dispatchActivationTimes) || dispatchActivationTimes < setting.InstantFinishAmount))
                                {
                                    this.api.GameMethods.DispatcherQuickFinish(dispatcher);
                                    if (!this.api.Hero.DispatcherInstantFinishActivationAmounts.TryAdd(dispatcher.Id, 1))
                                    {
                                        this.api.Hero.DispatcherInstantFinishActivationAmounts[dispatcher.Id] += 1;
                                    }
                                    Thread.Sleep(DispatcherSleepMs);
                                    continue;
                                }
                                this.api.GameMethods.DispatcherRetrieve(dispatcher);
                                Thread.Sleep(DispatcherSleepMs);
                            }
                        }

                        if (this.api.Hero.DispatcherSlotsFree > 0)
                        {
                            foreach (DispatcherSetting? dispatcherSetting in this.api.Settings.DispatcherSettings.OrderByDescending(d => d.InstantFinishAmount))
                            {
                                if (!dispatcherSetting.IsEnabled)
                                    continue;

                                Objects.Client.DispatcherObj? dispatcher = this.api.Dispatchers.FirstOrDefault(d => d.Id == dispatcherSetting.Id);
                                if (dispatcher == null)
                                    continue;

                                if (dispatcherSetting.OnlyOnDispatchDay && !this.api.Hero.DispatchDay)
                                    continue;

                                if (dispatcherSetting.InstantFinishAmount >= 0 && this.api.Hero.DispatcherInstantFinishActivationAmounts.TryGetValue(dispatcher.Id, out var activationTimes) && activationTimes >= dispatcherSetting.InstantFinishAmount)
                                    continue;

                                this.api.GameMethods.DispatcherSend(dispatcher);
                                Thread.Sleep(Logic.Logic.GetRandomSleepTime());
                                this.api.GameMethods.DispatcherUpdate();
                                Thread.Sleep(DispatcherUpdateSleepMs);
                            }
                        }

                        if ((DateTime.UtcNow - this.lastDispatcherServerUpdate).TotalMinutes > GalaxyGateGeneratorUpdateMinutes)
                        {
                            this.api.GameMethods.DispatcherUpdate();
                            this.lastDispatcherServerUpdate = DateTime.UtcNow;
                            this.lastDispatcherLocalUpdate = DateTime.UtcNow;
                        }
                    }

                    //GALAXY GATE GENERATOR CHECKER
                    if ((DateTime.UtcNow - this.lastGalaxyGateGeneratorServerUpdate).TotalMinutes > GalaxyGateGeneratorUpdateMinutes && !this.api.Hero.IsLoggingOut)
                    {
                        this.api.GameMethods.GalaxyGateGeneratorOpen();
                        this.lastGalaxyGateGeneratorServerUpdate = DateTime.UtcNow;
                    }
                    if ((DateTime.UtcNow - this.lastGalaxyGateGeneratorServerUpdate).TotalSeconds < GalaxyGateGeneratorSpinWindowSeconds && this.api.Inventory.ExtraEnergy >= 1 && !this.api.Hero.IsLoggingOut)
                    {
                        List<GalaxyGateSetting> ggSpinnerSettings = this.api.Logic.GetGGSpinnerSettings();

                        foreach (GalaxyGateSetting setting in ggSpinnerSettings)
                        {
                            if (!this.api.GalaxyGateGenerators.TryGetValue(setting.SpinGateId, out Objects.Client.GalaxyGateGenerator? gate))
                            {
                                this.api.Logging.Log($"Did not find Gate {setting.SpinGateId} in Spinner Gates");
                                if (this.api.Map.IsGalaxyGate
                                    || this.api.Map.IsBlackLightMap && this.api.Gates.Where(g => CommandConstants.GALAXY_GATES.ContainsKey(g.Value.GateDesignId)).Any())
                                {
                                    continue;
                                }
                                if (setting.GalaxyGateId == -1 && !this.api.Map.IsEternalBlackLightGatePlaced && setting.SpinPlaceGate)
                                {
                                    this.api.GameMethods.EBGPlace();
                                    this.api.Logging.Log("Placing EBG...");
                                    continue;
                                }
                                if (setting.GalaxyGateId == 84 && this.api.Menu.Get("").IsActivatable && setting.SpinPlaceGate)
                                {
                                    this.api.GameMethods.QZActivate();
                                    this.api.Logging.Log("Using QZ CPU...");
                                    continue;
                                }
                                continue;
                            }
                            if (gate.CurrentParts >= gate.MaxParts)
                            {
                                this.api.GameMethods.GalaxyGateGeneratorPlace(setting.SpinGateId);
                                this.lastGalaxyGateGeneratorServerUpdate = DateTime.MinValue;
                                break;
                            }
                            if (gate.CurrentParts >= gate.MaxParts)
                            {
                                continue;
                            }
                            if (setting.SpinAmount <= 0 
                                || (setting.SpinAmount > this.api.Inventory.ExtraEnergy || this.api.Inventory.ExtraEnergy <= 0)
                                && !setting.SpinGateWithUri)
                            {
                                continue;
                            }
                            if (CommandConstants.GG_SPINNER_NAME_ID.TryGetKey(setting.SpinGateId, out var gateName))
                            {
                                this.api.WriteLog("Spinning 10 " + gateName);
                            }
                            if (this.api.Hero.GalaxyGeneratorSpinAmount != setting.SpinAmount)
                            {
                                this.api.GameMethods.GalaxyGateGeneratorSelectSpinAmount(setting.SpinAmount);
                                Thread.Sleep(Logic.Logic.GetRandomSleepTime());
                            }
                            this.api.GameMethods.GalaxyGateGeneratorSpin(setting.SpinGateId, setting.SpinAmount, false);
                            Thread.Sleep(GalaxyGateSpinSleepMs);
                        }
                    }

                    //PET FUEL REBUY CHECKER
                    if (this.api.Settings.PetUse
                        && this.api.Settings.PetBuyFuel
                        && (this.api.Hero.IsPetOutOfFuel || this.api.Hero.IsPetActive && this.api.Hero.Pet.FuelPercentage < PetFuelThreshold))
                    {
                        this.api.Logic.RebuyPetFuel();
                        this.api.Hero.IsPetOutOfFuel = false;
                    }

                    //TASK SYSTEM CHECKER
                    this.api.GameObservers.TaskManager.CheckTasks();

                    //AMMO REBUY CHECKER
                    if (this.api.Logic.Target is Npc npc)
                    {
                        if (this.api.Map.IsGalaxyGate)
                        {
                            GalaxyGateSetting setting = this.api.Logic.GetGalaxyGateSetting(this.api.Map.Name);
                            if (setting.UseRebuyLaser && this.api.Inventory.Get(setting.LaserId) < RebuyLaserThreshold)
                            {
                                this.api.Logic.RebuyAmmo(setting.LaserId, false);
                            }
                            if (setting.UseRebuyRocket && setting.UseRockets && this.api.Inventory.Get(setting.RocketId) < RebuyRocketThreshold)
                            {
                                this.api.Logic.RebuyAmmo(setting.RocketId, true);
                            }
                            if (setting.UseRebuyRocketLauncher && setting.UseRocketLauncher && this.api.Inventory.Get(setting.RocketLauncherId) < RebuyRocketLauncherThreshold)
                            {
                                this.api.Logic.RebuyAmmo(setting.RocketLauncherId, true);
                            }
                            continue;
                        }
                        NpcSetting npcSetting = this.api.Logic.GetNpcSetting(npc);
                        if (npcSetting.UseRebuyLaser && this.api.Inventory.Get(npcSetting.LaserId) < RebuyLaserThreshold)
                        {
                            this.api.Logic.RebuyAmmo(npcSetting.LaserId, false);
                        }
                        if (npcSetting.UseRockets && npcSetting.UseRebuyRocket && this.api.Inventory.Get(npcSetting.RocketId) < RebuyRocketThreshold)
                        {
                            this.api.Logic.RebuyAmmo(npcSetting.RocketId, true);
                        }
                        if (npcSetting.UseRocketLauncher && npcSetting.UseRebuyRocketLauncher && this.api.Inventory.Get(npcSetting.RocketLauncherId) < RebuyRocketLauncherThreshold)
                        {
                            this.api.Logic.RebuyAmmo(npcSetting.RocketLauncherId, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.api.Logging.Log($"Exception in GameTaskObserver: {ex}");
                }
            }
        }
    }
}
