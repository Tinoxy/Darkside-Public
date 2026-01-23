using DarksideApi.DarkOrbit;
using DarksideApi.DarkOrbit.Objects;
using DarksideApi.DarkOrbit.Objects.Boxes;
using DarksideApi.DarkOrbit.Objects.Map;
using DarksideApi.DarkOrbit.Objects.Ship;
using DarksideApi.Util;
using System.Numerics;

namespace DarksideApi.Logic.Modules
{
    internal class AvoidModule(Api api) : ILogicModule
    {
        private IGameObject toAvoid = Mine.Empty();

        private void AvoidChecker()
        {
            Mine mine = api.Logic.GetClosestMine();
            if (api.Settings.GeneralAvoidMines && !string.IsNullOrEmpty(mine.Hash) && mine.TempAvoidTries < 3 && mine != this.toAvoid)
            {
                api.Logic.SetBackupState(this, api.Logic.State);
                api.Logic.SetState(this, LogicState.AvoidMine);
                this.toAvoid = mine;
                return;
            }
            MapAsset clanBase = api.Logic.GetClosestEnemyClanBase(1000);
            if (api.Settings.GeneralAvoidCbs && clanBase.AssetId > 0 && clanBase.TempAvoidTries < 3 && clanBase != this.toAvoid)
            {
                api.Logic.SetBackupState(this, api.Logic.State);
                api.Logic.SetState(this, LogicState.AvoidClanBase);
                this.toAvoid = clanBase;
                return;
            }
        }

        private bool MineHandler()
        {
            if (api.Logic.State != LogicState.AvoidMine)
            {
                return false;
            }
            if (this.toAvoid is not Mine mine)
            {
                return false;
            }

            // Wenn Mine schon mehrfach versucht, abbrechen
            if (mine.TempAvoidTries >= 3)
            {
                //api.WriteLog($"Mine {mine.Hash} reached max avoid tries, skipping.");
                mine.TempAvoidTries = 0;
                this.toAvoid = Mine.Empty();
                api.Logic.SetState(this, api.Logic.BackupState);
                return false;
            }

            // Erstversuch oder weitere Versuche: finde eine sichere Position
            if (mine.TempAvoidTries < 1)
            {
                // besondere Behandlung für Frozen Labyrinth Mines: weiter weg setzen
                var baseDistance = mine.IsFrozenLabyrinthMine ? Random.Shared.Next(1500, 2500) : Random.Shared.Next(400, 1000);

                var avoidPos = FindAvoidPositionForMine(mine, baseDistance, attempts: 8, safeDistanceFromOtherMines: 600);

                // Falls keine geeignete Position gefunden wurde, fallback auf frühere Strategie (einfacher Versatz)
                if (avoidPos == null)
                {
                    avoidPos = api.Logic.GetAvoidPosition(mine.X, mine.Y, baseDistance);
                }

                var randomAdd = Random.Shared.Next(20, 100);

                // Clamp an Map-Grenzen
                if (avoidPos.Value.X < 0 || avoidPos.Value.X > (api.Map.UsableX + api.Map.UsableWidth))
                {
                    avoidPos = new Vector2(avoidPos.Value.X < 0 ? randomAdd : (float)((api.Map.UsableX + api.Map.UsableWidth) - randomAdd), avoidPos.Value.Y);
                }
                if (avoidPos.Value.Y < 0 || avoidPos.Value.Y > (api.Map.UsableY + api.Map.UsableHeight))
                {
                    avoidPos = new Vector2(avoidPos.Value.X, avoidPos.Value.Y < 0 ? randomAdd : (float)((api.Map.UsableY + api.Map.UsableHeight) - randomAdd));
                }

                mine.TempAvoidTries += 1;

                api.WriteLog($"Avoiding Mine[{mine.Type}] (try {mine.TempAvoidTries}).");
                api.Logging.Log($"Avoiding Mine {mine.X}|{mine.Y}, new Position: {avoidPos.Value.X}|{avoidPos.Value.Y}, try: {mine.TempAvoidTries}");

                this.CheckCurrentTarget();
                LinkedList<Vector2> currentPath = api.Hero.Path;

                // Bewegung ausführen und realistische Wartezeit (distanz / speed)
                api.GameMethods.Move(avoidPos.Value);
                var distance = HelpTools.CalculateDistance(api.Hero.X, api.Hero.Y, (int)avoidPos.Value.X, (int)avoidPos.Value.Y);
                var waitMs = (int)(distance / Math.Max(api.Hero.Speed, 1) * 1000d);
                waitMs = Math.Clamp(waitMs, 200, 8000);
                Thread.Sleep(waitMs);

                api.Hero.Path = currentPath;
            }

            // Solange noch im Flug und Mine nahe, weiter warten
            if (api.Hero.FlyingTimeMs > 0 && mine.CurrentDistance < 1200)
            {
                Thread.Sleep(100);
                return true;
            }

            // Erfolgreich ausgewichen
            mine.TempAvoidTries = 0;
            this.toAvoid = Mine.Empty();
            api.Logic.SetState(this, api.Logic.BackupState);
            return false;
        }

        /// <summary>
        /// Probiere mehrere zufällige Positionen um die Mine herum und gib die erste zurück,
        /// die innerhalb der Map liegt und nicht zu nah an einer anderen Mine ist.
        /// </summary>
        private Vector2? FindAvoidPositionForMine(Mine mine, int baseDistance, int attempts = 6, int safeDistanceFromOtherMines = 500)
        {
            for (int i = 0; i < attempts; i++)
            {
                var angleDeg = Random.Shared.NextDouble() * 360.0;
                var angleRad = angleDeg * Math.PI / 180.0;
                // Variiere die Distanz leicht, damit nicht immer dieselbe Position verwendet wird
                var distance = baseDistance + Random.Shared.Next(-150, 151);
                var offsetX = (float)(Math.Cos(angleRad) * distance);
                var offsetY = (float)(Math.Sin(angleRad) * distance);

                var candidate = new Vector2(mine.X + offsetX, mine.Y + offsetY);

                // Clamp to usable map bounds
                var minX = api.Map.UsableX;
                var minY = api.Map.UsableY;
                var maxX = api.Map.UsableX + api.Map.UsableWidth;
                var maxY = api.Map.UsableY + api.Map.UsableHeight;

                candidate.X = Math.Clamp(candidate.X, minX + 10, maxX - 10);
                candidate.Y = Math.Clamp(candidate.Y, minY + 10, maxY - 10);

                // Prüfe, ob andere Mine in der Nähe ist
                if (api.Logic.IsMineCloseTo(candidate, safeDistanceFromOtherMines))
                {
                    continue;
                }

                // Prüfe, ob Position zu nahe an Held ist (zu kurzer Move)
                var candDistToHero = HelpTools.CalculateDistance((int)candidate.X, (int)candidate.Y, api.Hero.X, api.Hero.Y);
                if (candDistToHero < 200)
                {
                    continue;
                }

                return candidate;
            }

            return null;
        }

        private bool ClanBaseHandler()
        {
            if (api.Logic.State != LogicState.AvoidClanBase)
            {
                return false;
            }
            if (this.toAvoid is not MapAsset clanBase)
            {
                return false;
            }
            if (!clanBase.IsEnemy)
            {
                return false;
            }

            // Wenn schon zu oft versucht, abbrechen und zurück zum Backup-State
            if (clanBase.TempAvoidTries >= 3)
            {
                //api.WriteLog($"Skipping avoidance for clan base {clanBase.ClanTag} ({clanBase.Name}) after {clanBase.TempAvoidTries} tries.");
                clanBase.TempAvoidTries = 0;
                this.toAvoid = Mine.Empty();
                api.Logic.SetState(this, api.Logic.BackupState);
                return false;
            }

            // Versuche, eine sinnvolle Ausweichposition zu finden
            if (clanBase.TempAvoidTries < 1)
            {
                // Basispunkt: gegenüberliegende Kartecke bzw. ein Viertel, abhängig von Position des Helden zur Basis
                Vector2 baseTarget;
                var heroX = api.Hero.X;
                var heroY = api.Hero.Y;

                baseTarget = heroX >= clanBase.X
                    ? heroY >= clanBase.Y ? api.Map.CornerBottomRight : api.Map.CornerTopRight
                    : heroY <= clanBase.Y ? api.Map.CornerTopLeft : api.Map.CornerBottomLeft;

                // Versuche mehrere Kandidaten um den ausgewählten Basispunkt (Random Offsets)
                Vector2? avoidPos = FindAvoidPositionForClanBase(clanBase, baseTarget, attempts: 8, safeDistanceFromAssets: 800);

                // Fallback: benutze vorherige Logik, falls kein Kandidat gefunden wird
                if (avoidPos == null)
                {
                    avoidPos = api.Logic.GetQuarter();
                    // Verschiebe away von der ClanBase
                    var dx = avoidPos.Value.X >= clanBase.X ? 300 : -300;
                    var dy = avoidPos.Value.Y >= clanBase.Y ? 300 : -300;
                    avoidPos = new Vector2(avoidPos.Value.X + Random.Shared.Next((int)(dx - 100), (int)(dx + 100)), avoidPos.Value.Y + Random.Shared.Next((int)(dy - 100), (int)(dy + 100)));
                }

                // Clamp innerhalb der Map
                var minX = api.Map.UsableX;
                var minY = api.Map.UsableY;
                var maxX = api.Map.UsableX + api.Map.UsableWidth;
                var maxY = api.Map.UsableY + api.Map.UsableHeight;

                avoidPos = new(Math.Clamp(avoidPos.Value.X, minX + 10, maxX - 10), Math.Clamp(avoidPos.Value.Y, minY + 10, maxY - 10));

                clanBase.TempAvoidTries += 1;
                api.WriteLog($"Avoiding {clanBase.ClanTag}({clanBase.Name}) (try {clanBase.TempAvoidTries}).");
                api.Logging.Log($"Avoiding Clan Base {clanBase.X}|{clanBase.Y}, new Position: {avoidPos.Value.X}|{avoidPos.Value.Y}, try: {clanBase.TempAvoidTries}");

                this.CheckCurrentTarget();
                LinkedList<Vector2> currentPath = api.Hero.Path;

                api.GameMethods.Move(avoidPos.Value);
                // realistische Wartezeit basierend auf Distanz und Geschwindigkeit
                var distance = HelpTools.CalculateDistance(api.Hero.X, api.Hero.Y, (int)avoidPos.Value.X, (int)avoidPos.Value.Y);
                var waitMs = (int)(distance / Math.Max(api.Hero.Speed, 1) * 1000d);
                waitMs = Math.Clamp(waitMs, 300, 5500) + Logic.GetRandomSleepTime(-500, 500);
                Thread.Sleep(waitMs);

                api.Hero.Path = currentPath;
            }

            // Solange noch im Flug und ClanBase noch zu nahe, weiter warten
            if (api.Hero.FlyingTimeMs > 0 && clanBase.CurrentDistance < 2000)
            {
                Thread.Sleep(100);
                return true;
            }

            // Erfolgreich ausgewichen
            this.toAvoid = Mine.Empty();
            clanBase.TempAvoidTries = 0;
            api.Logic.SetState(this, api.Logic.BackupState);
            return false;
        }

        /// <summary>
        /// Suche sichere Positionen um die gewählte Kartecke / Basis herum.
        /// Prüft, dass die Position nicht innerhalb einer ClanBase liegt und nicht zu nah an anderen MapAssets ist.
        /// </summary>
        private Vector2? FindAvoidPositionForClanBase(MapAsset clanBase, Vector2 baseTarget, int attempts = 6, int safeDistanceFromAssets = 800)
        {
            var minX = api.Map.UsableX;
            var minY = api.Map.UsableY;
            var maxX = api.Map.UsableX + api.Map.UsableWidth;
            var maxY = api.Map.UsableY + api.Map.UsableHeight;

            for (int i = 0; i < attempts; i++)
            {
                // Erzeuge Kandidaten um baseTarget mit wachsender Streuung
                var spread = 200 + i * 150;
                var candidate = new Vector2(
                    baseTarget.X + Random.Shared.Next(-spread, spread),
                    baseTarget.Y + Random.Shared.Next(-spread, spread)
                );

                // Clamp innerhalb der Map
                candidate.X = Math.Clamp(candidate.X, minX + 10, maxX - 10);
                candidate.Y = Math.Clamp(candidate.Y, minY + 10, maxY - 10);

                // Stelle sicher, dass Kandidat nicht in einer ClanBase ist (sichere Zone)
                if (api.Logic.IsInClanBase((int)candidate.X, (int)candidate.Y))
                {
                    continue;
                }

                // Vermeide Kandidaten, die zu nah an anderen MapAssets (z.B. Basen, Gebäude) sind
                var tooClose = api.MapAssets.Values.Any(a =>
                    a != null &&
                    a.AssetId > 0 &&
                    HelpTools.CalculateDistance((int)candidate.X, (int)candidate.Y, a.X, a.Y) < safeDistanceFromAssets
                );
                if (tooClose)
                {
                    continue;
                }

                // Vermeide Kandidaten, die zu nah an der ClanBase selbst sind (bleibe wirklich "ausweichend")
                var distToBase = HelpTools.CalculateDistance((int)candidate.X, (int)candidate.Y, clanBase.X, clanBase.Y);
                if (distToBase < 800)
                {
                    continue;
                }

                // Vermeide Kandidaten, die sehr nah am Helden liegen
                var distToHero = HelpTools.CalculateDistance((int)candidate.X, (int)candidate.Y, api.Hero.X, api.Hero.Y);
                if (distToHero < 300)
                {
                    continue;
                }

                return candidate;
            }

            return null;
        }

        private void CheckCurrentTarget()
        {
            if (api.Logic.Target is not Collectible c)
            {
                return;
            }
            if (c.tempMoveTries > 3)
            {
                api.InvalidCollectibles.Add(c.Hash);
                api.Logic.Target = null;
                return;
            }
            c.IncreaseMoveTries();
        }

        private void EnemyHandler()
        {
            if (api.Ships.IsEmpty
                || !api.Settings.MapAvoidEnemys
                || api.Logic.State == LogicState.GotoFleeSpotted
                || api.Logic.State == LogicState.Revive
                || api.Logic.State == LogicState.GotoRepair
                || api.Logic.State == LogicState.Repair
                || api.Logic.State == LogicState.RepairJump
                || api.Logic.State == LogicState.GotoFlee
                || api.Logic.State == LogicState.FleeJump
                || api.Logic.State == LogicState.GotoFleeSpotted
                || api.Logic.State == LogicState.GotoShootback
                || api.Logic.State == LogicState.Shootback
                || api.Map.Id == 94
                || api.Map.Id == 93
                || api.Map.Id == 92
                || api.Map.Id == 91)
            {
                return;
            }
            var players = api.Ships.Values.OfType<Player>().ToList();
            if (players.Count == 0)
            {
                return;
            }
            Player? player = players.Find(p => p.IsEnemy && HelpTools.IsMovingTowards(api.Hero.X, api.Hero.Y, p.tempLastPositions));
            if (player == null)
            {
                return;
            }
            api.Logic.SetState(this, LogicState.GotoFleeSpotted);
            api.Logic.Target = player;
            api.WriteLog("Enemy Faction Nearby, fleeing!");
        }

        [Obsolete("Pirate Trader Avoidance is deprecated and will be removed in future versions.")]
        private void PirateTraderHandler()
        {
            if (api.Map.Id != 92)
            {
                return;
            }
            if (api.Logic.Target is Gate
                || (api.Logic.Target != null && ((api.Logic.Target.X > 95 || api.Logic.Target.X < 115) && (api.Logic.Target.Y > 60 || api.Logic.Target.Y < 72)) && api.Logic.Target.CurrentDistance < 750))
            {
                return;
            }
            if (api.MapAssets.IsEmpty
                || api.Logic.State == LogicState.Repair
                || api.Logic.State == LogicState.SellOre
                || api.Logic.State == LogicState.GotoSellOre
                || api.Logic.State == LogicState.GotoRepair)
            {
                return;
            }
            //TOP LEFT: 98, 63
            //BOTTOM RIGHT: 111, 70
            var x = api.Hero.X / 100;
            var y = api.Hero.Y / 100;
            //api.GameMethods.Move(120 * 100, 67 * 100);
            if (x <= 95 || x >= 115)
            {
                return;
            }
            if (y <= 60 || y >= 72)
            {
                return;
            }

            LogicState originalState = api.Logic.State;
            api.Logic.SetState(this, LogicState.AvoidTrader);
            var avoidX = 116 * 100 + Random.Shared.Next(-100, 100);
            var avoidY = 64 * 100 + Random.Shared.Next(-100, 100);
            var distance = HelpTools.CalculateDistance(api.Hero.X, api.Hero.Y, avoidX, avoidY);

            api.GameMethods.Move(avoidX, avoidY);
            Thread.Sleep((int)(double)(distance / api.Hero.Speed * 1000d));
            api.Logic.SetState(this, originalState);
        }

        public bool Run()
        {
            if (api.Logic.State == LogicState.Revive)
            {
                return false;
            }

            this.AvoidChecker();

            if (this.MineHandler() || this.ClanBaseHandler())
            {
                return true;
            }

            this.EnemyHandler();
            //this.PirateTraderHandler();

            return false;
        }
    }
}
