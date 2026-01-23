using DarksideApi.DarkOrbit;
using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Commands.EternalBlackLight;
using DarksideApi.DarkOrbit.Handlers.Hero;
using DarksideApi.DarkOrbit.Objects;
using DarksideApi.DarkOrbit.Objects.Boxes;
using DarksideApi.DarkOrbit.Objects.Map;
using DarksideApi.DarkOrbit.Objects.Ship;
using DarksideApi.DarkOrbit.Objects.StarSystem;
using DarksideApi.DarkOrbit.Settings;
using DarksideApi.DarkOrbit.Settings.Interfaces;
using DarksideApi.Logic.Modules;
using DarksideApi.Util;
using DarksideApi.Util.Extensions;
using System;
using System.Diagnostics;
using System.Numerics;

namespace DarksideApi.Logic
{
    public class Logic
    {
        private readonly Api api;
        private readonly Random random;
        private readonly List<ILogicModule> modules;
        private Thread? thread;
        public IGameObject? Target { get; set; }
        public LogicState State { get; private set; }
        public LogicState BackupState { get; private set; }
        public DateTime StartTime { get; set; }
        public DateTime TotalRunTime { get; set; }
        public DateTime PauseStartTime { get; set; }
        private object _stateLocker;
        private int quarterMoveCounter;

        public Logic(Api api)
        {
            this.api = api;
            this.Target = null;
            this.TotalRunTime = DateTime.MinValue;
            this._stateLocker = new();
            this.modules = LogicModules.GetModules(api);
            this.random = Random.Shared;
            this.State = LogicState.None;
            this.quarterMoveCounter = 0;
        }

        public Logic()
        {
            this.api = null!;
            this.random = null!;
            this.modules = [];
            this._stateLocker = null!;
        }

        static internal int GetRandomSleepTime(int min = 200, int max = 400)
        {
            return Random.Shared.Next(min, max);
        }

        public void SetState(object sender, LogicState nextState)
        {
            if (this.State == LogicState.Revive
                && (sender is not ReviveModule)
                && (sender is not ShipInitializationHandler)
                && nextState != LogicState.Revive)
            {
                return;
            }
            lock (this._stateLocker)
            {
                this.State = nextState;
            }
            Debug.WriteLine("State set to: " + nextState + ", by: " + sender);
        }

        public void SetBackupState(object sender, LogicState currentState)
        {
            if (currentState == LogicState.AvoidMine
                || currentState == LogicState.AvoidClanBase
                || currentState == LogicState.AvoidTrader)
            {
                return;
            }
            lock (this._stateLocker)
            {
                this.BackupState = currentState;
            }
            Debug.WriteLine("Backup-State set to: " + currentState + ", by: " + sender);
        }

        internal int GetQuarterMoveCounter()
        {
            return this.quarterMoveCounter;
        }

        internal void SetQuarterMoveCounter(int value)
        {
            this.quarterMoveCounter = value;
        }

        internal int GetRepairAt()
        {
            if (this.Target is Npc npc)
            {
                NpcSetting setting = this.GetNpcSetting(npc);
                if (!string.IsNullOrEmpty(setting.Name))
                {
                    return setting.RepairAt;
                }
            }
            return this.api.Settings.RepairAt;
        }

        internal bool IsInvalidCollectible(Collectible collectible)
        {
            return this.api.InvalidCollectibles.Contains(collectible.Hash);
        }

        internal Box GetClosestBox()
        {
            if (this.api.Settings.StopCollectIfPlayersNearby)
            {
                Player player = this.GetClosestPlayer();
                if (player.UserId > 0 && player.CurrentDistance < 1500)
                    return Box.Empty();
            }
            IEnumerable<Box> result = (from b in this.api.Collectibles.Values.OfType<Box>()
                                       where b.IsValidBox(this.api.Settings.CollectBoxTypes)
                                       && !this.IsInvalidCollectible(b)
                                       && this.api.Map.IsInMap(b.X, b.Y)
                                       && this.IsInWorkArea(b.X, b.Y)
                                       && !this.IsInClanBase(b.X, b.Y)
                                       && !this.PetWithTargetExists(b.X, b.Y)
                                       && !this.PlayerWithTargetExists(b.X, b.Y)
                                       && !this.IsInNoAccessZone(b.X, b.Y)
                                       select b);
            return result.OrderByDescending(b => this.GetBoxSetting(b).Priority).ThenBy(b => b.CurrentDistance).FirstOrDefault() ?? Box.Empty();
        }

        internal Ore GetClosestOre()
        {
            return (from o in this.api.Collectibles.Values.OfType<Ore>()
                    where o.IsValidOre(this.api.Settings.CollectOreTypes)
                    && !this.IsInvalidCollectible(o)
                    && this.api.Map.IsInMap(o.X, o.Y)
                    && this.IsInWorkArea(o.X, o.Y)
                    && !this.PlayerWithTargetExists(o.X, o.Y)
                    && !this.PetWithTargetExists(o.X, o.Y)
                    orderby o.CurrentDistance
                    select o).FirstOrDefault() ?? Ore.Empty();
        }

        internal Collectible GetClosestCollectible()
        {
            Box box = this.GetClosestBox();
            Ore ore = this.GetClosestOre();
            return (box.CurrentDistance < ore.CurrentDistance) ? box : ore;
        }

        internal Box GetClosestBoxGG()
        {
            this.Target = (from b in this.api.Collectibles.Values.OfType<Box>() where (!b.IsShipCargo || this.api.Hero.Cargo.FreeCargoSpace > 0) && !this.api.POIZones.Values.Any(p => p.IsInZone(b.X, b.Y)) orderby b.CurrentDistance select b).FirstOrDefault() ?? Box.Empty();
            return (Box)this.Target;
        }

        internal Box GetClosestBoxWithoutTargetGG()
        {
            return (from b in this.api.Collectibles.Values.OfType<Box>() where (!b.IsShipCargo) && !this.api.POIZones.Values.Any(p => p.IsInZone(b.X, b.Y)) orderby b.CurrentDistance select b).FirstOrDefault() ?? Box.Empty();
        }

        /*
internal Box GetClosestBoxWithoutTarget() => (from b in api.Collectibles.Values.OfType<Box>()
                                             where b.IsValidBox(api.Settings.CollectBoxTypes)
                                             && !IsInvalidCollectible(b)
                                             && api.Map.IsInMap(b.X, b.Y)
                                             && IsInWorkArea(b.X, b.Y)
                                             && !IsInClanBase(b.X, b.Y)
                                             && !PetWithTargetExists(b.X, b.Y)
                                             && !PlayerWithTargetExists(b.X, b.Y)
                                             && !IsInZone(b.X, b.Y)
                                             orderby b.CurrentDistance
                                             select b).FirstOrDefault() ?? Box.Empty();*/

        internal Box? GetClosestCaptchaBox()
        {
            return this.api.Captcha == null
                ? null
                : (from cb in this.api.Captcha.CaptchaBoxes.Values where cb.Type == this.api.Captcha.BoxToCollect orderby cb.CurrentDistance select cb).FirstOrDefault() ?? null;
        }

        internal BoxSetting GetBoxSetting(Box box)
        {
            return this.api.Settings.CollectBoxTypes.First(b => b.Type == box.Type) ?? new BoxSetting();
        }

        public NpcSetting GetNpcSetting(Npc npc)
        {
            return !this.api.Settings.NpcSettings.TryGetValue(npc.Username, out NpcSetting? setting) ? new NpcSetting() : setting;
        }

        internal Npc GetClosestNpc(bool ignoreProtection = false)
        {
            lock (this.api.ObjectLocker)
            {
                IOrderedEnumerable<Npc> result = this.api.Ships.Values.OfType<Npc>()
                    .Where(n => n.IsValid(this.GetNpcSetting(n)) && this.api.Map.IsInMap(n.Position) && this.IsInWorkArea(n.Position))
                    .OrderByDescending(n => this.GetNpcSetting(n).Priority)
                    .ThenBy(n => n.CurrentDistance);

                Npc nextTarget = result.FirstOrDefault() ?? Npc.Empty();

                if (nextTarget.IsProtected && !ignoreProtection)
                {
                    var interceptors = this.api.Logic.GetNpcsByNameInRadius(CommandConstants.NPC_INTERCEPTOR, nextTarget.X, nextTarget.Y, 620, false).OrderBy(s => s.CurrentDistance).ToList();
                    if (interceptors.Count > 2 && nextTarget.HitpointsPercentage > 75)
                    {
                        nextTarget.MinioinNpcs.Clear();
                        interceptors.ForEach(i => nextTarget.MinioinNpcs.Add(i.UserId));

                        nextTarget.tempAttackTries = 0;
                        return interceptors.First();
                    }
                    var hitacMinions = this.api.Logic.GetNpcsByNameInRadius(CommandConstants.NPC_HITAC_MINION, nextTarget.X, nextTarget.Y, 620, false).OrderBy(s => s.CurrentDistance).ToList();
                    if (hitacMinions.Count > 0)
                    {
                        nextTarget.MinioinNpcs.Clear();
                        hitacMinions.ForEach(h => nextTarget.MinioinNpcs.Add(h.UserId));

                        nextTarget.tempAttackTries = 0;
                        return hitacMinions.First();
                    }
                }

                return nextTarget;
            }
        }

        internal Npc GetClosestGGNpc()
        {
            lock (this.api.ObjectLocker)
            {
                var npcs = this.api.Ships.Values.OfType<Npc>().Where(n => n.IsSelectable).ToList();
                Npc[] npcsInCorner = [.. npcs.Where(n => n.IsInCorner)];
                Npc[] npcsFleeingToCorner = [.. npcs.Where(n => n.IsFleeingToCorner)];
                if (npcs.Exists(n => n.IsWarhead))
                {
                    return npcs.Where(n => n.IsWarhead).OrderBy(n => n.CurrentDistance).First();
                }
                if (npcs.Exists(n => n.IsTurrent))
                {
                    return npcs.Where(n => n.IsTurrent).OrderBy(n => n.CurrentDistance).First();
                }
                if (npcs.Exists(n => n.Username.Contains("=^(Plutus)^=") && !n.IsInCorner))
                {
                    return npcs.Where(n => n.Username.Contains("=^(Plutus)^=")).OrderBy(n => n.CurrentDistance).First();
                }
                if (npcs.Exists(n => n.Username.Contains("Specialist")))
                {
                    Npc specialist = npcs.Where(n => n.Username.Contains("Specialist")).OrderBy(n => n.CurrentDistance).First();
                    if (!specialist.IsInCorner && !specialist.IsFleeingToCorner)
                    {
                        return specialist;
                    }
                }
                if (npcs.Exists(n => n.Username.Contains("Rocketeer")))
                {
                    Npc rocketeer = npcs.Where(n => n.Username.Contains("Rocketeer")).OrderBy(n => n.CurrentDistance).First();
                    if (!rocketeer.IsInCorner && !rocketeer.IsFleeingToCorner)
                    {
                        return rocketeer;
                    }
                }
                if (npcs.Exists(n => n.Username.Contains("Missle Storm")))  //DSE
                {
                    Npc storm = npcs.Where(n => n.Username.Contains("Missle Storm")).OrderBy(n => n.CurrentDistance).First();
                    if (!storm.IsInCorner && !storm.IsFleeingToCorner)
                    {
                        return storm;
                    }
                }
                return (from n in npcs
                        where ((n.IsProtected && !npcs.Any(n => !n.IsProtected)) || !n.IsProtected)
                        && (!n.IsDefendable)
                        && ((npcs.Count - npcsInCorner.Length) <= 0 || !n.IsInCorner)
                        && ((npcs.Count - npcsFleeingToCorner.Length) <= 0 || !n.IsFleeingToCorner)
                        orderby n.CurrentDistance
                        select n).FirstOrDefault() ?? Npc.Empty();
            }
        }

        internal Npc GetClosestGGNpcNoLimitation()
        {
            lock (this.api.ObjectLocker)
            {
                var npcs = this.api.Ships.Values.OfType<Npc>().Where(n => n.IsSelectable).ToList();
                return (from n in npcs where !n.IsInCorner orderby n.CurrentDistance select n).FirstOrDefault() ?? Npc.Empty();
            }
        }

        internal Npc GetClosestGGProtectedNpc()
        {
            lock (this.api.ObjectLocker)
            {
                var npcs = this.api.Ships.Values.OfType<Npc>().Where(n => n.IsSelectable).ToList();
                return (from n in npcs where n.IsProtected orderby n.CurrentDistance select n).FirstOrDefault() ?? Npc.Empty();
            }
        }

        internal int GetActiveGGNpcsCount()
        {
            lock (this.api.ObjectLocker)
            {
                var npcs = this.api.Ships.Values.OfType<Npc>().Where(n => n.IsSelectable).ToList();
                return this.api.Ships.Count - npcs.Count(n => n.IsFleeing || n.IsInCorner);
            }
        }

        internal NpcSetting? GetNpcSettingByName(string npcName, bool exactMatch = true)
        {
            return exactMatch ? api.Settings.NpcSettings.TryGetValue(npcName, out var setting) ? setting : null :
                api.Settings.NpcSettings.FirstOrDefault(s => s.Name != null && s.Name.Contains(npcName.Split(" ").ElementAt(1) ?? npcName, StringComparison.CurrentCultureIgnoreCase)) ?? null;
        }

        internal List<Npc> GetNpcsByNameInRadius(string name, int centerX, int centerY, double radius, bool onlyFullHp = false)
        {
            lock (this.api.ObjectLocker)
            {
                var npcs = this.api.Ships.Values.OfType<Npc>().ToList();
                return [.. (from n in npcs
                        where (n.Username.Contains(name)
                        && HelpTools.CalculateDistance(centerX, centerY, n.X, n.Y) <= radius
                        && (!onlyFullHp || n.HitpointsPercentage >= 99))
                        select n)];
            }
        }

        internal Npc GetEMFreighterNpc()
        {
            lock (this.api.ObjectLocker)
            {
                var npcs = this.api.Ships.Values.OfType<Npc>().ToList();
                return (from n in npcs where n.IsEmFreighter select n).FirstOrDefault() ?? Npc.Empty();
            }
        }   

        public GalaxyGateSetting GetGalaxyGateSetting(string ggName)
        {
            for (var i = 0; i < this.api.Settings.GalaxyGateSettings.Count; i++)
            {
                GalaxyGateSetting setting = this.api.Settings.GalaxyGateSettings.ElementAt(i);
                if (!setting.GalaxyGateName.Equals(ggName, StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }
                return setting;
            }
            this.api.Logging.Log("Did not find " + ggName + " in GG Settings");
            return new();
        }

        public List<GalaxyGateSetting> GetGGSpinnerSettings()
        {
            return [.. this.api.Settings.GalaxyGateSettings.Where(gg => gg.SpinGenerator)];
        }

        internal Player GetClosestPlayer()
        {
            lock (this.api.ObjectLocker)
            {
                var list = this.api.Ships.Values.OfType<Player>().ToList();
                return list.Count <= 0 ? Player.Empty() : (from p in list orderby p.CurrentDistance select p).First() ?? Player.Empty();
            }
        }
        internal bool PlayerWithTargetExists(int x, int y)
        {
            return (from p in this.api.Ships.Values.OfType<Player>() where p.TargetX == x && p.TargetY == y select p).Any();
        }

        internal bool PetWithTargetExists(int x, int y)
        {
            return (from p in this.api.Pets.Values where p.TargetX == x && p.TargetY == y select p).Any();
        }

        internal Mine GetClosestMine()
        {
            return (from m in this.api.Mines.Values where m.CurrentDistance < (m.IsFrozenLabyrinthMine ? 555 : 360) orderby m.CurrentDistance select m).FirstOrDefault() ?? Mine.Empty();
        }

        internal bool IsMineCloseTo(Vector2 position, double radius = 360)
        {
            return HelpTools.CalculateDistance(this.GetClosestMine().Position, position) < radius;
        }

        internal MapAsset GetClosestEnemyClanBase(double maxDistance = 600)
        {
            return (from c in this.api.MapAssets.Values where c.IsEnemy && c.IsClanBase && c.CurrentDistance <= maxDistance orderby c.CurrentDistance select c).FirstOrDefault() ?? MapAsset.Empty();
        }

        internal MapAsset GetClosestBaseBuilding(bool toRepair = false)
        {
            return (from a in this.api.MapAssets.Values where a.IsBaseBuilding && (!toRepair || !this.api.Map.IsBlackLightMap) orderby a.CurrentDistance select a).FirstOrDefault() ?? MapAsset.Empty();
        }

        internal MapAsset GetClosestTrader()
        {
            return (from a in this.api.MapAssets.Values where a.IsTrader orderby a.CurrentDistance select a).FirstOrDefault() ?? MapAsset.Empty();
        }

        internal MapAsset GetClosestRepairDock()
        {
            return (from a in this.api.MapAssets.Values where a.IsRepairDock orderby a.CurrentDistance select a).FirstOrDefault() ?? MapAsset.Empty();
        }

        internal MapAsset GetClosestGOPPositivAsset()
        {
            return (from a in this.api.MapAssets.Values where a.IsGOPGeneratorPositiv && (a.X > 100 && a.Y > 100) orderby a.CurrentDistance select a).FirstOrDefault() ?? MapAsset.Empty();
        }

        internal PoiZone GetClosestEquipmentZone()
        {
            return (from z in this.api.POIZones.Values where z.Type == 16 orderby z.CurrentDistance select z).FirstOrDefault() ?? PoiZone.Empty();
        }

        internal PoiZone GetClosestZone()
        {
            return (from z in this.api.POIZones.Values orderby z.CurrentDistance select z).FirstOrDefault() ?? PoiZone.Empty();
        }

        internal Gate GetClosestGate()
        {
            bool anyGateInWorkArea = IsAnyGateInWorkArea();
            return (from g in this.api.Gates.Values
                    where
                    this.api.Map.IsInMap(g.X, g.Y)
                    && (this.IsInWorkArea(g.X, g.Y) || !anyGateInWorkArea)
                    && (this.api.EntitiesUnderAttackBy.IsEmpty || !this.api.LockedGateIds.Contains(g.GateId))
                    orderby g.CurrentDistance
                    select g).FirstOrDefault() ?? Gate.Empty();
        }

        internal Gate GetClosestGateWithoutArea()
        {
            return (from g in this.api.Gates.Values orderby g.CurrentDistance select g).FirstOrDefault() ?? Gate.Empty();
        }

        internal Gate GetClosestGate(int designId, bool ignorePvP = false)
        {
            bool anyGateInWorkArea = IsAnyGateInWorkArea();
            return (from g in this.api.Gates.Values
                    where
                    (this.api.Map.IsPirateMap || g.GateDesignId == designId)
                    && (!ignorePvP || !g.LeadsToPvPMap())
                    && this.api.Map.IsInMap(g.X, g.Y)
                    && (this.IsInWorkArea(g.X, g.Y) || !anyGateInWorkArea)
                    && (this.api.Map.IsPirateMap || this.api.Map.IsFrozenLabyrinth || g.IsUsable())
                    && (this.api.EntitiesUnderAttackBy.IsEmpty || !this.api.LockedGateIds.Contains(g.GateId))
                    orderby g.CurrentDistance
                    select g).FirstOrDefault() ?? Gate.Empty();
        }

        internal Gate GetClosestGateToPosition(int x, int y, bool visibleOnMap = false)
        {
            return (from g in this.api.Gates.Values where !visibleOnMap || g.IsVisible orderby HelpTools.CalculateDistance(g.X, g.Y, x, y) select g).FirstOrDefault() ?? Gate.Empty();
        }

        internal Gate GetNextGate(int maxRandom = 3)
        {
            IOrderedEnumerable<Gate> gates = (from g in this.api.Gates.Values where g.CurrentDistance > 1000 && this.api.Map.IsInMap(g.X, g.Y) orderby g.CurrentDistance select g);
            return gates.Any() ? gates.ElementAt(this.random.Next(0, (maxRandom > (gates.Count() - 1) ? gates.Count() - 1 : maxRandom))) : Gate.Empty();
        }

        internal StarPortal GetGateById(int gateId)
        {
            return (from g in Api.StarSystem[this.api.Map.Id].Portals where g.GateId == gateId select g).FirstOrDefault() ?? StarPortal.Empty();
        }

        internal Gate GetGateByDistanceTo(int x, int y)
        {
            return (from g in this.api.Gates.Values orderby HelpTools.CalculateDistance(g.X, g.Y, x, y) select g).FirstOrDefault() ?? Gate.Empty();
        }

        internal List<Gate> GetGatesByDistanceTo(int x, int y)
        {
            return (from g in this.api.Gates.Values orderby HelpTools.CalculateDistance(g.X, g.Y, x, y) select g).ToList() ?? [];
        }

        internal Gate GetClosestGalaxyGate()
        {
            List<Gate> gateList = [];
            foreach (GalaxyGateSetting gs in this.api.Settings.GalaxyGateSettings)
            {
                if (!gs.IsEnabled)
                {
                    Debug.WriteLine($"Skipping Galaxy Gate {gs.GalaxyGateName} because it is disabled.");
                    continue;
                }
                if (this.api.GalaxyGateGenerators.TryGetValue(gs.SpinGateId, out DarkOrbit.Objects.Client.GalaxyGateGenerator? ggs) && ggs.LifesLeft > 0 && ggs.LifesLeft < (5 - (gs.MaxDeaths)))
                {
                    Debug.WriteLine($"Skipping Galaxy Gate {gs.GalaxyGateName} because it has not enough lives left({ggs.LifesLeft}, max Deaths: {gs.MaxDeaths}).");
                    continue;
                }
                IEnumerable<Gate> gates = this.api.Gates.Values
                    .Where(g => (g.GateDesignId == (this.api.Map.IsBlackLightMap ? 54 : gs.GalaxyGateId))
                        && !this.api.LockedGateIds.Contains(g.GateId)
                        && (!g.IsQuarantineZoneGate || (this.api.Inventory.Get("items_ammunition_ggportal_quarantine-zone-cpu") > 0 && this.api.Group != null))
                        && (!g.IsLowGate || (this.api.Group != null))
                    //&& (!g.IsDeepSpaceEchoesGate || (api.Inventory.Get("items_resource_key_access-dse") > 0)))
                    );
                if (!gates.Any())
                {
                    Debug.WriteLine($"Skipping Galaxy Gate {gs.GalaxyGateName} because no gates found with design id: {gs.GalaxyGateId}.");
                    continue;
                }
                gateList.AddRange(gates);
            }
            return gateList.OrderBy(g => g.CurrentDistance).FirstOrDefault() ?? Gate.Empty();
        }

        internal KeyValuePair<int, int> GetClosestGalaxyGateId()
        {
            Dictionary<int, int> gateList = [];
            foreach (GalaxyGateSetting gs in this.api.Settings.GalaxyGateSettings)
            {
                if (!gs.IsEnabled)
                {
                    this.api.Logging.Log($"Skipping Galaxy Gate {gs.GalaxyGateName} because it is disabled.");
                    continue;
                }
                if (this.api.GalaxyGateGenerators.TryGetValue(gs.SpinGateId, out DarkOrbit.Objects.Client.GalaxyGateGenerator? ggs) && ggs.LifesLeft > 0 && ggs.LifesLeft < (5 - (gs.MaxDeaths)))
                {
                    this.api.Logging.Log($"Skipping Galaxy Gate {gs.GalaxyGateName} because it has not enough lives left({ggs.LifesLeft}, max Deaths: {gs.MaxDeaths}).");
                    continue;
                }
                if (ggs != null && ggs.CurrentWave < gs.StopAtWave && gs.StopAtWave > 0)
                {
                    this.api.Logging.Log("Ignoring GG, Stop At Wave is reached!");
                    continue;
                }
                var accessedBy = this.GetGalaxyGateAccessMapId(gs.GalaxyGateId);
                if (accessedBy < 0)
                {
                    this.api.Logging.Log($"Skipping Galaxy Gate {gs.GalaxyGateName} because it is not accessible in the current map.");
                    continue;
                }
                gateList.Add(gs.GalaxyGateId, accessedBy);
            }
            if (gateList.Count <= 0)
            {
                this.api.Logging.Log("No access Maps returned");
                return new KeyValuePair<int, int>(-1, -1);
            }
            return gateList.Aggregate((x, y) => Math.Abs(x.Value - this.api.Map.Id) < Math.Abs(y.Value - this.api.Map.Id) ? x : y);
        }

        internal Gate GetClosestGateIgnoringDesignId(int designIdToIgnore = 0)
        {
            return (from g in this.api.Gates.Values where g.GateDesignId != designIdToIgnore orderby g.CurrentDistance select g).FirstOrDefault() ?? this.api.Gates.Values.FirstOrDefault() ?? Gate.Empty();
        }

        internal List<Gate> GetClosestGatesIgnoringDesignId(int designIdToIgnore = 0)
        {
            return [.. (from g in this.api.Gates.Values where g.GateDesignId != designIdToIgnore orderby g.CurrentDistance select g)];
        }

        internal PoiZone GetClosestPoiZone(double distance = 100)
        {
            return (from z in this.api.POIZones.Values where z.IsNoAccess && z.CurrentDistance <= distance orderby z.CurrentDistance select z).FirstOrDefault() ?? PoiZone.Empty();
        }

        internal bool IsAnyTimebasedWorkmapAvailable(DateTime dateTime)
        {
            var nowUtc = dateTime.ToUniversalTime();
            return this.api.Settings.WorkMapSettings
                .Where(w => w.IsEnabled)
                .Any(w => GetNextOccurrenceUtc(w, nowUtc) > nowUtc);
        }

        internal WorkMapSetting? GetWorkmapAfterTime(DateTime dateTime)
        {
            var nowUtc = dateTime.ToUniversalTime();

            // Erzeuge geordneten Zeitplan (nächste Vorkommnisse) und logge ihn zur Fehlersuche.
            var schedule = this.api.Settings.WorkMapSettings
                .Where(w => w.IsEnabled)
                .Select(w => (setting: w, nextStart: GetNextOccurrenceUtc(w, nowUtc)))
                .OrderBy(x => x.nextStart)
                .ToList();

            // Debug-Logging der kommenden Termine (UTC) — kompakt, eine Zeile pro Eintrag.
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"WorkMap schedule (nowUtc={nowUtc:O}):");
            foreach (var (setting, nextStart) in schedule)
            {
                var mapName = CommandConstants.MAP_NAMES.TryGetValue(setting.MapId, out var n) ? n : setting.MapId.ToString();
                var minutesUntil = (nextStart - nowUtc).TotalMinutes;
                sb.AppendLine($" - Map {setting.MapId} ({mapName}) at {setting.StartTimeUTC:HH:mm} UTC -> nextUtc={nextStart:O} (in {minutesUntil:F1} min)");
            }
            this.api.Logging.Log(sb.ToString());

            // Wähle das tatsächlich nächstgelegene Kommende (> now)
            return schedule.FirstOrDefault(x => x.nextStart > nowUtc && GetWorkmapDayEnabled(x.setting)).setting;
        }

        // Liefert die nächste UTC-Instanz der im Setting hinterlegten Startzeit (heute oder morgen).
        private static DateTime GetNextOccurrenceUtc(DarksideApi.DarkOrbit.Settings.WorkMapSetting w, DateTime nowUtc)
        {
            // Nutze nur TimeOfDay aus dem gespeicherten StartTimeUTC (Datum ist Platzhalter)           
            var st = w.StartTimeUTC;
            var candidate = new DateTime(nowUtc.Year, nowUtc.Month, nowUtc.Day, st.Hour, st.Minute, st.Second, DateTimeKind.Utc);

            if (candidate <= nowUtc)
                candidate = candidate.AddDays(1);

            return candidate;
        }

        private static bool GetWorkmapDayEnabled(DarksideApi.DarkOrbit.Settings.WorkMapSetting w)
        {
            var day = DateTime.UtcNow.DayOfWeek;
            switch (day)
            {
                case DayOfWeek.Monday:
                    return w.Monday;
                case DayOfWeek.Tuesday:
                    return w.Tuesday;
                case DayOfWeek.Wednesday:
                    return w.Wednesday;
                case DayOfWeek.Thursday:
                    return w.Thursday;
                case DayOfWeek.Friday:
                    return w.Friday;
                case DayOfWeek.Saturday:
                    return w.Saturday;
                case DayOfWeek.Sunday:
                    return w.Sunday;
            }
            return true;
        }

        internal int GetQuarterIndex(Vector2 quarter)
        {
            // Find the closest corner to the given quarter vector
            var corners = new[]
            {
                this.api.Map.CornerTopLeft,
                this.api.Map.CornerTopRight,
                this.api.Map.CornerBottomRight,
                this.api.Map.CornerBottomLeft
            };
            int closestIndex = 0;
            double minDistance = double.MaxValue;
            for (int i = 0; i < corners.Length; i++)
            {
                double dist = HelpTools.CalculateDistance(quarter, corners[i]);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closestIndex = i;
                }
            }
            Debug.WriteLine($"GetQuarterIndex return: {closestIndex}");
            return closestIndex;
        }

        internal Vector2 GetNextQuarter(int index = 0)
        {
            Vector2 quarter = this.GetQuarter(index);
            if (quarter == this.api.Map.CornerTopLeft)
            {
                Debug.WriteLine("GetNextQuarter return: CornerTopRight");
                return this.api.Map.CornerTopRight;
            }
            if (quarter == this.api.Map.CornerTopRight)
            {
                Debug.WriteLine("GetNextQuarter return: CornerBottomRight");
                return this.api.Map.CornerBottomRight;
            }
            if (quarter == this.api.Map.CornerBottomLeft)
            {
                Debug.WriteLine("GetNextQuarter return: CornerTopLeft");
                return this.api.Map.CornerTopLeft;
            }
            if (quarter == this.api.Map.CornerBottomRight)
            {
                Debug.WriteLine("GetNextQuarter return: CornerBottomLeft");
                return this.api.Map.CornerBottomLeft;
            }
            Debug.WriteLine("GetNextQuarter return DEFAULT: CornerTopRight");
            return this.api.Map.CornerTopRight;
        }

        internal bool IsOnQuarterBorder(Vector2 quarter, int x, int y)
        {
            if (quarter == this.api.Map.CornerTopRight)
            {
                return (x > this.api.Map.CornerTopRight.X || y < this.api.Map.CornerTopRight.Y);
            }
            if (quarter == this.api.Map.CornerBottomRight)
            {
                return (x > this.api.Map.CornerBottomRight.X || y > this.api.Map.CornerBottomRight.Y);
            }
            return quarter == this.api.Map.CornerBottomLeft
                ? x < 2000 || y > this.api.Map.CornerBottomLeft.Y
                : quarter == this.api.Map.CornerTopLeft && (x < 2000 || y < 2000);
        }

        internal Vector2 GetQuarter(int index = 0)
        {
            return this.api.Map.Corners.OrderBy(corner => HelpTools.CalculateDistance(this.api.Hero.Position, corner)).ElementAt(index);
        }

        internal bool IsGGNpcsBunchedUp()
        {
            Npc[] npcs = this.api.Ships.Values.OfType<Npc>().ToArray();
            if (npcs.Length == 1)
            {
                return true;
            }
            var activeNpcs = npcs.Where(n => !n.IsInCorner).OrderByDescending(n => n.CurrentDistance).ToList();

            return activeNpcs.Count > 0 && (activeNpcs.First().CurrentDistance <= 1000 || activeNpcs.ElementAt((int)(activeNpcs.Count * 0.3)).CurrentDistance <= 1000);
        }

        internal Vector2 GetDragPosition(IGameObject gameObject, double minRange = 550.0)
        {
            var distance = gameObject.CurrentDistance;
            Vector2 heroPos = new(this.api.Hero.X, this.api.Hero.Y);
            if (distance >= minRange)
            {
                return heroPos;
            }
            Vector2 quarter = this.GetQuarter();
            var quarterDistance = HelpTools.CalculateDistance(heroPos.X, heroPos.Y, quarter.X, quarter.Y);

            if (quarterDistance < 50
                || this.api.Map.TargetQuarter.X <= 0
                || this.IsOnQuarterBorder(quarter, (int)heroPos.X, (int)heroPos.Y))
            {
                this.api.Map.TargetQuarter = this.GetNextQuarter();
            }
            var distanceToMove = (int)Math.Round(minRange - distance, 0);
            var heroPosXRnd = heroPos.X + this.random.Next(-62, 58);
            var heroPosYRnd = heroPos.Y + this.random.Next(-58, 62);

            if (this.api.Map.TargetQuarter == this.api.Map.CornerTopRight)
            {
                return new(heroPos.X + distanceToMove, heroPosYRnd);
            }
            if (this.api.Map.TargetQuarter == this.api.Map.CornerBottomRight)
            {
                return new(heroPosXRnd, heroPos.Y + distanceToMove);
            }
            return this.api.Map.TargetQuarter == this.api.Map.CornerBottomLeft
                ? new(heroPos.X - distanceToMove, heroPosYRnd)
                : this.api.Map.TargetQuarter == this.api.Map.CornerTopLeft
                ? new(heroPosXRnd, heroPos.Y - distanceToMove)
                : this.GetCirclePosition(gameObject.X, gameObject.Y, minRange, 0.0);
        }

        internal Vector2 GetDragPosition(Vector2 currentPosition, double minRangeBeforeSwitch = 1000)
        {
            if (HelpTools.CalculateDistance(this.api.Map.CornerTopLeft, currentPosition) < minRangeBeforeSwitch
                || HelpTools.CalculateDistance(this.api.Map.CornerTopRight, currentPosition) < minRangeBeforeSwitch
                || HelpTools.CalculateDistance(this.api.Map.CornerBottomLeft, currentPosition) < minRangeBeforeSwitch
                || HelpTools.CalculateDistance(this.api.Map.CornerBottomRight, currentPosition) < minRangeBeforeSwitch)
            {
                return this.GetNextQuarter();
            }
            return currentPosition; // No need to change position if not close to a corner
        }

        internal double GetDegreeFromCornerIndex(int index)
        {
            return index switch
            {
                0 => 45,// Top Left
                1 => 135,// Top Right
                2 => 225,// Bottom Right
                3 => 315,// Bottom Left
                _ => (double)0,// Default case, should not happen
            };
        }

        internal Vector2 GetCirclePosition(int x, int y, double distance, double addAngle = 0.5)
        {
            var f = (Math.Atan2(this.api.Hero.X - x, this.api.Hero.Y - y) + addAngle) + (Math.PI / 180);
            var newX = x + distance * Math.Sin(f);
            var newY = y + distance * Math.Cos(f);
            return new((int)newX, (int)newY);
        }

        internal Vector2 GetCirclePosition(Vector2 vector, double distance, double addAngle = 0.5) => GetCirclePosition((int)vector.X, (int)vector.Y, distance, addAngle);   

        internal Vector2 GetAvoidPosition(int x, int y, double distance = 1500, double adjustBy = 20)
        {
            var heroPos = new Vector2(this.api.Hero.X, this.api.Hero.Y);
            var awayFromPos = new Vector2(x, y);

            var angle = awayFromPos.Angle(heroPos);
            double moveDistance = this.api.Hero.Speed;
            var distanceToPos = distance + 100;

            Vector2 target = new()
            {
                X = (float)(awayFromPos.X - Math.Cos(angle) * distanceToPos),
                Y = (float)(awayFromPos.Y - Math.Sin(angle) * distanceToPos),
            };

            moveDistance -= HelpTools.CalculateDistance(target, heroPos);

            if (moveDistance > 0)
            {

                angle += moveDistance / 3000;

                target.X = (float)(awayFromPos.X - Math.Cos(angle) * distanceToPos);
                target.Y = (float)(awayFromPos.Y - Math.Sin(angle) * distanceToPos);
            }

            return target;
        }

        internal Vector2 GetSafeCirclePosition(IAttackSetting setting)
        {
            if (this.Target is not Npc target)
            {
                return this.api.Hero.Position;
            }
            Vector2 direction = this.api.Hero.IsFlying ? this.api.Hero.TargetPosition : this.api.Hero.Position;
            Vector2 heroLoc = api.Hero.Position;
            Vector2 targetLoc = target.GetPositionInTime(400);

            var distance = target.CurrentDistance;
            var angle = targetLoc.Angle(heroLoc);
            var radius = target.GetRadius(setting.Radius);
            var noCircle = !setting.UseCircle;

            if (radius > 750) noCircle = false;

            double angleDiff;
            if (noCircle)
            {
                var dist = targetLoc.DistanceTo(direction);
                var minRad = Math.Max(0, Math.Min(radius - 200, radius * 0.5));
                if (dist <= radius && dist >= minRad)
                {
                    return this.api.Hero.Position;
                }
                distance = minRad + this.random.NextDouble() * (radius - minRad - 10);
                angleDiff = (this.random.NextDouble() * 0.1) - 0.05;
            }
            else
            {
                double maxRadFix = setting.Radius / 2,
                        radiusFix = (int)Math.Max(Math.Min(radius - distance, maxRadFix), -maxRadFix);
                distance = (radius += radiusFix);
                // Moved distance + speed - distance to chosen radius same angle, divided by radius
                angleDiff = Math.Max((this.api.Hero.Speed * 0.625) + (Math.Min(200, target.Speed) * 0.625)
                        - heroLoc.DistanceTo(targetLoc.Of(angle, radius)), 0) / radius;
            }
            direction = this.GetBestCircleDirection(target, targetLoc, angle, angleDiff, distance);
            var tempCircleClockwise = target.tempCircleClockwise;

            while ((!this.IsInMapCircle(direction) || this.IsInNoAccessZone(direction) || (!this.IsInWorkArea(direction) && !target.IsFleeing) || this.IsInClanBase(direction) || this.IsMineCloseTo(direction))
                && distance < 10000)
            {
                direction = targetLoc.ToAngle(angle += (!target.tempCircleClockwise ? -0.3 : 0.3), distance += 2);
                if ((!this.IsInMapCircle(direction) || this.IsInNoAccessZone(direction) || (!this.IsInWorkArea(direction) && !target.IsFleeing) || this.IsInClanBase(direction) || this.IsMineCloseTo(direction))
                    && tempCircleClockwise == target.tempCircleClockwise)
                {
                    target.tempCircleClockwise = !target.tempCircleClockwise;
                }
            }

            if (distance >= 10000)
                direction = targetLoc.ToAngle(angle, 500);

            return direction;
        }

        private Vector2 GetBestCircleDirection(Npc target, Vector2 targetLoc, double angle, double angleDiff, double distance)
        {
            var iteration = 1;
            double forwardScore = 0, backScore = 0;
            do
            {
                forwardScore += this.GetCircleScore(targetLoc.Of(angle + (angleDiff * iteration), distance));
                backScore += this.GetCircleScore(targetLoc.Of(angle - (angleDiff * iteration), distance));
                // Toggle direction if either one of the directions is perfect, or one is 300 better.
                if (forwardScore < 0 != backScore < 0 || Math.Abs(forwardScore - backScore) > 300)
                    break;
            } while (iteration++ < 180);

            if (iteration <= 180)
                target.tempCircleClockwise = !(backScore > forwardScore);

            return targetLoc.Of(angle + angleDiff * (!target.tempCircleClockwise ? -1 : 1), distance);
        }

        private double GetCircleScore(Vector2 loc)
        {
            return (this.IsInMapCircle(loc) ? 0 : -1000) - this.api.Ships.Values.OfType<Npc>() // Consider barrier as bad as 1000 radius units.
                    .Where(n => n != this.Target)
                    .Sum(n => Math.Max(0, 600 - n.Position.DistanceTo(loc)));
        }

        private bool IsInMapCircle(Vector2 position)
        {
            return (this.api.Map.IsMimesisEscort && !this.api.Map.IsFrozenLabyrinthSeason) ? (HelpTools.CalculateDistance(GetEMFreighterNpc().Position, position) < 1000) : this.api.Map.IsInMap(position);
        }

        internal bool DoCircle(Ship targetShip, IAttackSetting setting, int entitiesUnderAttackWithoutTargetCount = 0, bool useCircle = true)
        {
            var radius = targetShip.GetRadius(!this.api.EntitiesUnderAttackBy.IsEmpty ? (setting.Radius / 1.1) : (setting.Radius / 1.3));
            var circleAdd = ((targetShip.HitpointsPercentage < 25 && entitiesUnderAttackWithoutTargetCount < 1) || !useCircle) ? (targetShip.tempCircleClockwise ? 0.0 : 45.0) : (targetShip.tempCircleClockwise ? 0.5 : -0.5);
            Vector2 newPosition = this.api.Logic.GetSafeCirclePosition(setting);

            if (newPosition == this.api.Hero.Position)
                newPosition = this.api.Logic.GetCirclePosition(targetShip.X, targetShip.Y, radius, circleAdd);
            /*
            if (!this.api.Map.IsInMap((int)newPosition.X, (int)newPosition.Y))
            {
                targetShip.tempCircleClockwise = !targetShip.tempCircleClockwise;
                return true;
            }
            PoiZone closestZone = this.api.Logic.GetClosestPoiZone(radius);
            if (!string.IsNullOrEmpty(closestZone.ZoneId) && closestZone.IsInZone((int)newPosition.X, (int)newPosition.Y))//closestZone.IsInZone(newPosition[0], newPosition[1]))
            {
                targetShip.tempCircleClockwise = !targetShip.tempCircleClockwise;
                return true;
            }
            Npc closestNpc = this.api.Logic.GetClosestNpc(true);
            if (closestNpc.UserId != targetShip.UserId
                && closestNpc.IsHeavyNpc && (closestNpc.LastAttackerId <= 0 || closestNpc.LastAttackerId == this.api.Hero.UserId)
                && HelpTools.CalculateDistance(closestNpc.X, closestNpc.Y, newPosition.X, newPosition.Y) < 360)
            {
                targetShip.tempCircleClockwise = !targetShip.tempCircleClockwise;
                return true;
            }*/

            this.api.GameMethods.Move(newPosition);
            targetShip.tempLastCircleMove = DateTime.UtcNow;
            return true;
        }

        internal bool IsInClanBase(Vector2 position)
        {
            return this.IsInClanBase((int)position.X, (int)position.Y);
        }

        internal bool IsInClanBase(int x, int y)
        {
            return !this.api.MapAssets.IsEmpty && this.api.MapAssets.Values.Any(a => a.IsClanBase && a.IsEnemy && HelpTools.CalculateDistance(a.X, a.Y, x, y) < 1000);
        }

        internal bool IsInNoAccessZone(Vector2 position)
        {
            return this.api.POIZones.Values.Any(p => p.IsNoAccess && p.IsInZone(position));
        }

        internal bool GetIsInNoAccessZone(Vector2 positionInZone, out PoiZone? zone, Vector2? closestPosition = null)
        {
            var toPosition = closestPosition ?? api.Hero.Position;
            zone = this.api.POIZones.Values.Where(p => p.IsNoAccess && p.IsInZone(positionInZone)).OrderBy(p => HelpTools.CalculateDistance(p.Position, toPosition)).FirstOrDefault();
            return zone != null;
        }

        internal bool IsInNoAccessZone(int x, int y)
        {
            return this.api.POIZones.Values.Any(p => p.IsNoAccess && p.IsInZone(x, y));
        }

        internal bool IsInWorkArea(Vector2 position)
        {
            return this.IsInWorkArea((int)position.X, (int)position.Y);
        }

        internal bool IsInWorkArea(int x, int y)
        {
            return !this.api.Settings.WorkAreaSettings.TryGetValue(this.api.Map.Id, out WorkAreaSetting? workAreaSetting) || (x >= workAreaSetting.CornerTopLeft.X && x <= workAreaSetting.CornerBottomRight.X)
                    && (y >= workAreaSetting.CornerTopLeft.Y && y <= workAreaSetting.CornerBottomRight.Y);
        }

        internal bool IsAnyGateInWorkArea() => this.api.Gates.Values.Any(g => this.IsInWorkArea(g.X, g.Y) && g.IsUsable());

        internal Vector2 GetRandomPosition()
        {
            // 1. If hero has a path, use the first path element
            if (this.api.Hero.Path.Count > 0)
            {
                return this.api.Hero.FirstPathElement;
            }

            // 2. If there is a valid PointOfInterest, use it if far enough
            if (this.api.PointOfInterest.X > 0 && this.api.PointOfInterest.Y > 0)
            {
                Vector2 closestPoint = this.api.PointOfInterest;
                if (HelpTools.CalculateDistance(this.api.Hero.Position, closestPoint) < 100)
                {
                    this.api.PointOfInterest = new(-1, -1);
                }
                else if (HelpTools.CalculateDistance(this.api.Hero.LastPathElement, closestPoint) > 100
                    && this.IsInWorkArea(closestPoint))
                {
                    return new(closestPoint.X + this.random.Next(-15, 15), closestPoint.Y + this.random.Next(-15, 15));
                }
            }

            // 3. If work area is defined, use a random point in it
            if (this.api.Settings.WorkAreaSettings.TryGetValue(this.api.Map.Id, out WorkAreaSetting? workAreaSetting))
            {
                return new(workAreaSetting.GetRandomX(), workAreaSetting.GetRandomY());
            }

            // 4. Special map logic
            if (this.api.Map.IsPiratePalladiumMap)
            {
                if (this.api.Hero.X > (320 * 100) && this.api.Hero.Y > 0 && this.api.Hero.X < (420 * 100) && this.api.Hero.Y < (142 * 100))
                {
                    if (HelpTools.CalculateDistance(this.api.Hero.X, this.api.Hero.Y, 333 * 100, 138 * 100) > 600)
                    {
                        return new((333 * 100) + this.random.Next(-200, 200), (138 * 100) + this.random.Next(-200, 200));
                    }
                }
                return new(this.random.Next(12000, 32000), this.random.Next(18000, 25500));
            }
            if (this.api.Map.IsPirateTraderMap)
            {
                /*MapAsset trader = this.GetClosestBaseBuilding();
                if (trader.CurrentDistance < 1000 && this.api.Hero.X > trader.X && (this.api.Hero.Y > (trader.Y - 500) || this.api.Hero.Y < (trader.Y + 500)))
                {
                    return new(120 * 100, 67 * 100);
                }*/
            }
            if (this.api.Map.IsMimesisEscort && !this.api.Map.IsFrozenLabyrinthSeason)
            {
                Npc escort = this.api.Logic.GetEMFreighterNpc();
                if (escort.UserId > 0)
                {
                    return api.Ships.Values.OfType<Npc>().Count() > 1 ? new(escort.X + this.random.Next(-1000, 1000), escort.Y + this.random.Next(-1000, 1000)) : api.Hero.Position;
                }
            }

            // 5. Try to use map corners and randomize
            int nextCornerIndex = (this.quarterMoveCounter + 1 >= 4) ? this.random.Next(0, this.api.Map.Corners.Count) : this.quarterMoveCounter + 1;
            Vector2 nextPosition = this.api.Map.Corners.ElementAt(nextCornerIndex);
            if (nextPosition.X == 0 || HelpTools.GetChance(50))
            {
                nextPosition = new(
                    this.random.Next(this.api.Map.UsableX, this.api.Map.UsableX + this.api.Map.UsableWidth),
                    this.random.Next(this.api.Map.UsableY, this.api.Map.UsableY + this.api.Map.UsableHeight)
                );
                this.api.Logging.Log("Move Random: " + nextPosition);
            }
            else if (HelpTools.GetChance(42))
            {
                this.quarterMoveCounter = 5;
                nextPosition = new(
                    this.api.Map.UsableX + (this.api.Map.UsableWidth / 2) + this.random.Next(-2000, 2000),
                    this.api.Map.UsableY + (this.api.Map.UsableHeight / 2) + this.random.Next(-2000, 2000)
                );
                this.api.Logging.Log("Move Center: " + nextPosition);
            }
            else
            {
                nextPosition = new(
                    nextPosition.X + this.random.Next(-2000, 2000),
                    nextPosition.Y + this.random.Next(-2000, 2000)
                );
                this.api.Logging.Log("Move Quarter: " + nextPosition);
            }

            // 6. Validate position is in map
            if (!this.api.Map.IsInMap(nextPosition))
            {
                nextPosition = new(
                    this.random.Next(this.api.Map.UsableX, this.api.Map.UsableX + this.api.Map.UsableWidth),
                    this.random.Next(this.api.Map.UsableY, this.api.Map.UsableY + this.api.Map.UsableHeight)
                );
            }

            // 7. Avoid POI zones
            PoiZone possibleZone = this.api.POIZones.Values.FirstOrDefault(p => p.IsInZone((int)nextPosition.X, (int)nextPosition.Y)) ?? PoiZone.Empty();
            if (!string.IsNullOrEmpty(possibleZone.ZoneId))
            {
                nextPosition = new(
                    this.random.Next(this.api.Map.UsableX, this.api.Map.UsableX + this.api.Map.UsableWidth),
                    this.random.Next(this.api.Map.UsableY, this.api.Map.UsableY + this.api.Map.UsableHeight)
                );
            }

            return nextPosition;
        }

        internal void IncreaseMoveQuarter()
        {
            this.quarterMoveCounter += 1;
        }

        internal int GetGalaxyGateAccessMapId(int gg)
        {
            return !CommandConstants.GALAXY_GATE_ACCESSED_BY_MAPID(this.api.Hero.FactionId).TryGetValue(gg, out var mapId) ? -1 : mapId;
        }

        internal bool MoveToPortalToJumpToMap(int targetMapid)
        {
            if (this.api.Map.Id == targetMapid)
                return false;

            if (targetMapid == -2 && api.Inventory.GetFrozenLabyrinthAccessKeys <= -1 && api.Map.IsFrozenLabyrinthSeason)   //0 -> 17.12.2025 DO removed min key amount 
            {
                api.Logging.Log("No Frozen Access Keys found");
                this.api.Settings.MapSelect += 4;
                return false;
            }
            if (targetMapid == -3 && this.api.Inventory.GetEscortAccessKeys <= 0)
            {
                this.api.Logging.Log("No Mimesis Escort Access Keys found");
                //this.api.Settings.MapSelect += 5;
                //return false;
            }

            var gateId = -1;
            List<StarPortal> gates = Api.StarSystem[this.api.Map.Id].Portals;
            var pathLength = -1;

            //if (gates.Count == 1)
            //{
            //    gateId = gates.First().GateId;
            //}
            //else
            //{
                IEnumerable<StarPortal> possibleGates = gates.Where(g => g.LeadsToMapId == targetMapid);
                if (possibleGates.Any())
                {
                    gateId = possibleGates
                        //.OrderBy(g => this.api.Movement.GeneratePath(g.X, g.Y).Count)
                        .OrderBy(g => HelpTools.CalculateDistance(this.api.Hero.Position, g.Position))
                        .First().GateId;

                Debug.WriteLine($"Move to Portal [{targetMapid}] Possible Gates: " + string.Join(",", possibleGates));
            }
                else
                {
                    List<int> mapsToGoTo = HelpTools.GetPath(this.api.Map.Id, targetMapid, this.api.Hero.FactionId);
                    var nextMapId = mapsToGoTo.First();
                    IEnumerable<StarPortal> starGates = Api.StarSystem[this.api.Map.Id].Portals.Where(p => p.LeadsToMapId == nextMapId);

                    if (!starGates.Any())
                    {
                        api.Logging.Log("Did not find matching stargate for next map id " + nextMapId);
                        return false;
                    }

                    gateId = starGates
                        //.OrderBy(g => this.api.Movement.GeneratePath(g.X, g.Y).Count)
                        .OrderBy(g => HelpTools.CalculateDistance(this.api.Hero.Position, g.Position))
                        .First().GateId;
                    pathLength = mapsToGoTo.Count;

                Debug.WriteLine($"Move to Portal [{targetMapid}] Path: " + string.Join(",", starGates));
                }
            //} 

            if (this.api.Settings.MapUseAdvancedJumpCpu && pathLength > this.api.Settings.MapUseAdvancedJumpCpuMinPathLength
                && targetMapid != 91 && targetMapid != 92 && targetMapid != 93 && targetMapid != 94
                && targetMapid != 306 && targetMapid != 307 && targetMapid != 308
                && !this.api.Map.IsPirateMap && !this.api.Map.IsBlackLightMap && !this.api.Map.IsMimesisEscort && !this.api.Map.IsFrozenLabyrinth
                && this.api.EntitiesUnderAttackBy.IsEmpty && !api.Hero.IsAttacking)
            {              
                if (targetMapid < 0)
                {
                    targetMapid = CommandConstants.GET_HOMEMAP_ID(this.api.Hero.FactionId);
                }
                api.Logging.Log("Using Advanced Jump CPU to reach target Map: " + targetMapid);
                this.api.GameMethods.AdvancedJumpCPUSelect(targetMapid);
                Thread.Sleep(GetRandomSleepTime());
                this.api.GameMethods.AdvancedJumpCPUJump();
                this.api.Hero.IsJumping = true;
                return true;
            }

            if (gateId <= 0)
            {
                this.api.Logging.Log("GateId is invalid");
                return false;
            }

            StarPortal g = this.GetGateById(gateId);
            this.Target = this.GetClosestGateToPosition(g.X, g.Y, false);

            if (this.api.Map.Id == 92 && this.GetClosestGateWithoutArea().CurrentDistance <= 50 && this.api.Hero.X > 15000)
            {
                var adjustY = this.random.Next(500, 975);
                var moveToY = (this.api.Hero.Y >= 6500 ? this.api.Hero.Y + adjustY : this.api.Hero.Y - adjustY);
                this.api.GameMethods.Move(this.api.Hero.X + this.random.Next(0, 72), moveToY);
                Thread.Sleep((int)this.api.Hero.FlyingTimeMs);
            }
            if ((this.Target.PathDistance > 20 && this.Target.TargetDistance > 20) && this.Target.CurrentDistance > 20)
            {
                this.api.GameMethods.Move(this.Target, 15);
                Thread.Sleep(GetRandomSleepTime());
            }

            return true;
        }

        internal void MoveToClosestGateToStop(Gate? gate = null)
        {
            if (this.api.GameClient.GameState != GameState.Ready)
            {
                return;
            }
            this.api.Hero.Path.Clear();

            Gate closestGate = gate ?? this.GetClosestGate(1);
            MapAsset closestBuilding = this.GetClosestBaseBuilding();

            IGameObject safePlace = (closestGate.CurrentDistance < closestBuilding.CurrentDistance) ? closestGate : closestBuilding;

            if (safePlace.X <= 0 || safePlace.Y <= 0)
                return;

            this.api.GameMethods.Move(safePlace, 50);
        }

        internal void RebuyAmmo(string type, bool isRocket)
        {
            if (this.api.Hero.IsCurrentlyBuying)
            {
                return;
            }
            this.api.Hero.IsCurrentlyBuying = true;
            Task.Factory.StartNew(() =>
            {
                if (isRocket)
                {
                    if (!this.api.Settings.IsUnity)
                        this.api.BackpageApi.ShopBuyRocket(type);
                    else
                        this.api.GameMethods.ShopBuyRocket(type, 100);
                }
                else
                {
                    if (!this.api.Settings.IsUnity)
                        this.api.BackpageApi.ShopBuyLaser(type);
                    else
                        this.api.GameMethods.ShopBuyLaser(type, 10000);
                }
                this.api.Hero.IsCurrentlyBuying = false;
            });
        }

        internal void RebuyPetFuel()
        {
            if (this.api.Hero.IsCurrentlyBuying)
            {
                return;
            }
            this.api.Hero.IsCurrentlyBuying = true;
            Task.Factory.StartNew(() =>
            {
                if (!this.api.Settings.IsUnity)
                    this.api.BackpageApi.ShopBuyPetFuel();
                else
                    this.api.GameMethods.ShopBuyPetFuel(1000);

                this.api.Hero.IsCurrentlyBuying = false;
            });
        }

        internal void DeactivateCurrentHangar()
        {
            if (this.api.Hero.CurrentHangar != null)
            {
                this.api.Hero.CurrentHangar.IsActive = false;
                return;
            }

            foreach (DarkOrbit.Objects.Client.Hangar hangar in this.api.Hangars)
            {
                hangar.IsActive = false;
            }
        }

        internal void EBGSelectBooster(List<EternalBlacklightBoosterCommand> boosterOptions)
        {
            var availableString = string.Empty;
            boosterOptions.ForEach(b => availableString += ", " + b.boosterType);
            this.api.Logging.Log("AVAILABLE BOOSTERS: " + availableString);

            EternalBlacklightBoosterCommand highestPrioirity = boosterOptions.OrderByDescending(booster => CommandConstants.EBG_BOOSTER_TYPES_PRIORITY[booster.boosterType]).First();
            var index = 0;
            if (highestPrioirity != null)
            {
                index = boosterOptions.IndexOf(highestPrioirity);
            }

            this.api.GameMethods.EBGBoosterSelect(index);
            this.api.WriteLog($"Selecting {boosterOptions.ElementAt(index).value}% {boosterOptions.ElementAt(index).boosterType} Booster...");
        }

        internal void TryPetRevive()
        {
            if (!this.api.Settings.PetUse || !this.api.Settings.PetRepair)
            {
                return;
            }
            if (this.api.MapAssets.Values.Any(a => a.IsEnemy && a.IsClanBase && a.CurrentDistance < 1500))
            {
                return;
            }
            this.api.GameMethods.PetRevive();  
            this.api.WriteLog("Reviving Pet...");
        }

        private void BotLogic()
        {
            this.api.WriteLog("Bot starting...");
            while (this.api.Settings.IsRunning)
            {
                try
                {
                    Thread.Sleep(100);

                    if (this.State == LogicState.Pausing)
                    {
                        if ((DateTime.UtcNow - this.PauseStartTime).TotalMinutes > 60 * this.api.Settings.GeneralPausePauseTime + 0.15)
                        {
                            this.api.Logging.Log("Paused for: " + this.api.Settings.GeneralPausePauseTime);
                            this.State = LogicState.None;
                            this.StartTime = DateTime.UtcNow;
                            this.api.GameClient.GameState = GameState.Ready;
                            this.api.GameClient.GameState = GameState.Disconnected;
                            this.api.AddLootResource($"Pause {Math.Round(this.api.Settings.GeneralPausePauseTime, 2)}Hours", 1);
                        }
                        Thread.Sleep(5000);
                        continue;
                    }
                    if (this.State == LogicState.PausingDeaths)
                    {
                        if ((DateTime.UtcNow - this.PauseStartTime).TotalMinutes > 10)
                        {
                            this.api.Logging.Log("Paused for: 10min");
                            this.State = LogicState.None;
                            this.StartTime = DateTime.UtcNow;
                            this.api.LastDeaths.Clear();
                            this.api.GameClient.GameState = GameState.Ready;
                            this.api.GameClient.GameState = GameState.Disconnected;
                            this.api.AddLootResource($"Pause 10Min", 1);
                        }
                        Thread.Sleep(5000);
                        continue;
                    }

                    if ((this.State == LogicState.SellOreDone || this.State == LogicState.GotoSellOre)
                        && this.api.GameClient.GameState != GameState.Ready)
                    {
                        this.api.Logging.Log("Trying to change Hangar, state: " + this.State + ", Game State: " + this.api.GameClient.GameState);
                        DarkOrbit.Objects.Client.Hangar? tradeHangar = this.api.Hangars.Where(h => h.HangarId == this.api.Settings.OreSellHangarId).FirstOrDefault();
                        this.api.GameClient.ChangeHangar(this.State == LogicState.SellOreDone ? this.api.Hero.DefaultHangarId : tradeHangar.HangarId);
                        this.api.Logic.SetState(this, this.State == LogicState.SellOreDone ? LogicState.None : LogicState.SellOre);
                        this.api.GameClient.GameState = GameState.Reconnect;
                    }

                    if ((this.api.GameClient.GameState != GameState.Ready && this.State != LogicState.Revive)
                        || this.api.Hero.IsJumping
                        || this.State == LogicState.CaptchaPending
                        || this.api.Hero.IsLoggingOut)
                    {
                        Thread.Sleep(2000);
                        continue;
                    }

                    for (var i = 0; i < this.modules.Count; i++)
                    {
                        if (!this.modules[i].Run() && this.api.Settings.IsRunning)
                        {
                            continue;
                        }
                        break;
                    }
                }
                catch (Exception ex)
                {
                    this.api.Logging.Log(ex);
                }
            }
        }

        public void StartLogic()
        {
            try
            {
                this.StopLogic(true);

                if (this.TotalRunTime <= DateTime.MinValue)
                {
                    this.TotalRunTime = DateTime.UtcNow;
                }
                this.StartTime = DateTime.UtcNow;
                this.api.Settings.IsRunning = true;
                this.api.Statistics.StartSearchTime();
                this.thread?.Start();
            }
            catch (Exception ex)
            {
                this.api.Logging.Log(ex);
            }
        }

        public void StopLogic(bool reset = false)
        {
            try
            {
                this.api.WriteLog("Bot stoppped.");

                this.api.Settings.IsRunning = false;
                this.api.Statistics.ResetSearchTime();

                if (reset)
                {
                    this.thread?.Join();
                    this.thread = new Thread(this.BotLogic);
                    return;
                }
                this.State = LogicState.None;
                this.MoveToClosestGateToStop();
                this.api.GameObservers.TaskManager.StopTasks();

                this.TotalRunTime = DateTime.MinValue;
            }
            catch (Exception ex)
            {
                this.api.Logging.Log(ex);
            }
        }
    }
}
