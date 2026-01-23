using DarksideApi.DarkOrbit;
using DarksideApi.Statistics.Objects;
using System.Collections.ObjectModel;

namespace DarksideApi.Statistics
{
    public class Statistics
    {
        private readonly Api api;

        public ObservableCollection<NpcStatisticsItem> NpcStatistics { get; private set; } = [];
        public ObservableCollection<BoxStatisticsItem> BoxStatistics { get; private set; } = [];
        public ObservableCollection<ResourceStatisticsItem> ResourcesStatistics { get; private set; } = [];
        public ObservableCollection<DeathStatisticsItem> DeathsStatistics { get; private set; } = [];
        public string LoadedShip { get; set; } = string.Empty;

        public StatisticsItem CollectedCredits { get; set; } = new("Credits", 0);
        public StatisticsItem CollectedUridium { get; set; } = new("Uridium", 0);
        public StatisticsItem CollectedXP { get; set; } = new("XP", 0);
        public StatisticsItem CollectedHonor { get; set; } = new("Honor", 0);
        public StatisticsItem CollectedEE { get; set; } = new("EE", 0);
        public StatisticsItem AmountPlayersDestroyed { get; set; } = new("Players Destroyed", 0);
        public StatisticsItem AmountShotback { get; set; } = new("Shotback", 0);
        public StatisticsItem AmountFled { get; set; } = new("Fled", 0);
        public StatisticsItem AmountTrades { get; set; } = new("Trades", 0);
        public StatisticsItem AvgClicks { get; set; } = new("Clicks", 0);
        public StatisticsItem AvgDamage { get; set; } = new("Damage", 0);
        public StatisticsItem AvgSearchTime { get; set; } = new("Search Time", 0, 0, "Seconds", false);

        private readonly List<int> clicks = [];
        private readonly Dictionary<string, List<int>> damage = [];
        private readonly List<double> searchTimeSeconds = [];
        private readonly string[] lastReceived = new string[5];
        private int lastReceivedIndex = 0;

        private DateTime searchTimeStart;
        private bool didResetSerchTime;
        private object _lock = new();

        public Statistics(Api api)
        {
            this.api = api;
            this.didResetSerchTime = true;
        }

        public void Reset()
        {
            lock (this._lock)
            {
                this.NpcStatistics.Clear();
                this.BoxStatistics.Clear();
                this.ResourcesStatistics.Clear();
                this.DeathsStatistics.Clear();

                this.CollectedCredits.Amount = 0;
                this.CollectedUridium.Amount = 0;
                this.CollectedXP.Amount = 0;
                this.CollectedHonor.Amount = 0;
                this.CollectedEE.Amount = 0;
                this.AmountPlayersDestroyed.Amount = 0;
                this.AmountShotback.Amount = 0;
                this.AmountFled.Amount = 0;
                this.AmountTrades.Amount = 0;
                this.AvgClicks.Amount = 0;
                this.AvgDamage.Amount = 0;
                this.AvgDamage.DisplayInfo = string.Empty;
                this.AvgSearchTime.Amount = 0;
                this.didResetSerchTime = true;

                this.clicks.Clear();
                this.damage.Clear();
                this.searchTimeSeconds.Clear();
            }
        }

        public void AddLoot(StatisticsItem item)
        {
            lock (this._lock)
            {
                UpdatePerHour(api.Logic.TotalRunTime);
                if (item is NpcStatisticsItem npcItem)
                {
                    this.AddNpcItem(npcItem);
                    return;
                }
                if (item is BoxStatisticsItem boxItem)
                {
                    this.AddBoxItem(boxItem);
                    return;
                }
                if (item is DeathStatisticsItem deathItem)
                {
                    this.AddDeathItem(deathItem);
                    return;
                }
                if (item is ResourceStatisticsItem resourceItem)
                {
                    this.AddResourceItem(resourceItem);
                    return;
                }
            }
        }

        public void UpdatePerHour(DateTime runTime)
        {
            lock (this._lock)
            {
                for (var i = 0; i < this.NpcStatistics.Count; i++)
                {
                    this.NpcStatistics.ElementAt(i).UpdatePerHour(runTime);
                }
                for (var i = 0; i < this.BoxStatistics.Count; i++)
                {
                    this.BoxStatistics.ElementAt(i).UpdatePerHour(runTime);
                }
                for (var i = 0; i < this.ResourcesStatistics.Count; i++)
                {
                    this.ResourcesStatistics.ElementAt(i).UpdatePerHour(runTime);
                }
                for (var i = 0; i < this.DeathsStatistics.Count; i++)
                {
                    this.DeathsStatistics.ElementAt(i).UpdatePerHour(runTime);
                }

                this.CollectedEE.UpdatePerHour(runTime);
                this.CollectedCredits.UpdatePerHour(runTime);
                this.CollectedHonor.UpdatePerHour(runTime);
                this.CollectedUridium.UpdatePerHour(runTime);
                this.CollectedXP.UpdatePerHour(runTime);
                this.AmountFled.UpdatePerHour(runTime);
                this.AmountPlayersDestroyed.UpdatePerHour(runTime);
                this.AmountShotback.UpdatePerHour(runTime);
                this.AmountTrades.UpdatePerHour(runTime);
                this.AvgClicks.Amount = this.GetAvgClicksPerMin();
                this.UpdateAvgDamage();
                this.AvgSearchTime.Amount = this.GetAvgSearchTime();
            }
        }

        public void AddClick(int i = 1)
        {
            lock (this._lock)
            {
                if (this.clicks.Count > 300)
                {
                    this.clicks.RemoveRange(100, (this.clicks.Count - 100));
                }
                this.clicks.Add(i);
            }
        }

        public void AddDamage(string ammoType, int damageAmount)
        {
            lock (this._lock)
            {
                if (!this.damage.ContainsKey(ammoType))
                {
                    this.damage.Add(ammoType, []);
                }
                if (this.damage[ammoType].Count > 300)
                {
                    this.damage[ammoType].RemoveRange(100, (this.damage[ammoType].Count - 100));
                }
                this.damage[ammoType].Add(damageAmount);
            }
        }

        public void AddSearchTime()
        {
            TimeSpan timespan = DateTime.UtcNow - this.searchTimeStart;
            if (this.searchTimeSeconds.Count > 300)
            {
                this.searchTimeSeconds.RemoveRange(100, this.searchTimeSeconds.Count - 100);
            }
            this.searchTimeSeconds.Add(Math.Round(timespan.TotalSeconds, 2));
            this.didResetSerchTime = true;
        }

        public void StartSearchTime()
        {
            if (!this.didResetSerchTime)
                return;

            this.searchTimeStart = DateTime.UtcNow;
            this.didResetSerchTime = false;
        }

        public void ResetSearchTime()
        {
            this.didResetSerchTime = true;
        }

        public void UpdateCreditsTotal(long amount)
        {
            if (amount > 0)
                this.CollectedCredits.Total = amount;
            if (this.CollectedCredits.Total == 0)
                this.CollectedCredits.Total = this.CollectedCredits.Amount;
        }

        public void UpdateUridiumTotal(long amount)
        {
            if (amount > 0)
                this.CollectedUridium.Total = amount;
            if (this.CollectedUridium.Total == 0)
                this.CollectedUridium.Total = this.CollectedUridium.Amount;
        }

        private void UpdateAvgDamage()
        {
            this.AvgDamage.DisplayInfo = this.api.Hero.CurrentLaser.DisplayName;
            this.AvgDamage.Amount = this.GetAvgDamage(this.api.Hero.CurrentLaser.Id);
        }

        public string[] GetLastReceived => this.lastReceived;
        private long GetAvgClicksPerMin()
        {
            return this.clicks.Count > 0 ? (long)this.clicks.Average() : 0L;
        }

        private long GetAvgDamage(string ammoType)
        {
            return this.damage.TryGetValue(ammoType, out List<int>? damageList) && damageList.Count > 0 ? (long)damageList.Average() : 0L;
        }

        private long GetAvgSearchTime()
        {
            return (long)Math.Round(this.searchTimeSeconds.Count > 0 ? this.searchTimeSeconds.Average() : 0, 0);
        }

        private void AddLastReceived(string message)
        {
            lock (this._lock)
            {
                this.lastReceived[this.lastReceivedIndex] = message;
                this.lastReceivedIndex = (this.lastReceivedIndex + 1) % this.lastReceived.Length;
            }
        }

        private void AddDeathItem(DeathStatisticsItem item)
        {
            IEnumerable<DeathStatisticsItem> found = this.DeathsStatistics.Where(d => d.Name == item.Name);
            if (found.Any())
            {
                var objIndex = this.DeathsStatistics.IndexOf(found.First());
                this.DeathsStatistics[objIndex].Amount += item.Amount;
                if (item.Total > 0)
                    this.DeathsStatistics[objIndex].Total = item.Total;
                if (this.DeathsStatistics[objIndex].Total == 0)
                    this.DeathsStatistics[objIndex].Total = this.DeathsStatistics[objIndex].Amount;
                return;
            }
            this.DeathsStatistics.Add(item);
        }

        private void AddNpcItem(NpcStatisticsItem item)
        {
            IEnumerable<NpcStatisticsItem> found = this.NpcStatistics.Where(d => d.Name == item.Name);
            if (found.Any())
            {
                var objIndex = this.NpcStatistics.IndexOf(found.First());
                this.NpcStatistics[objIndex].Amount += item.Amount;
                if (item.Total > 0)
                    this.NpcStatistics[objIndex].Total = item.Total;
                if (this.NpcStatistics[objIndex].Total == 0)
                    this.NpcStatistics[objIndex].Total = this.NpcStatistics[objIndex].Amount;
                return;
            }
            this.NpcStatistics.Add(item);
        }

        private void AddBoxItem(BoxStatisticsItem item)
        {
            IEnumerable<BoxStatisticsItem> found = this.BoxStatistics.Where(d => d.Name == item.Name);
            if (found.Any())
            {
                var objIndex = this.BoxStatistics.IndexOf(found.First());
                this.BoxStatistics[objIndex].Amount += item.Amount;
                if (item.Total > 0)
                    this.BoxStatistics[objIndex].Total = item.Total;
                if (this.BoxStatistics[objIndex].Total == 0)
                    this.BoxStatistics[objIndex].Total = this.BoxStatistics[objIndex].Amount;
                return;
            }
            this.BoxStatistics.Add(item);
        }

        private void AddResourceItem(ResourceStatisticsItem item)
        {
            this.AddLastReceived($"{item.AmountString}x {item.Name}");

            IEnumerable<ResourceStatisticsItem> found = this.ResourcesStatistics.Where(d => d.Name == item.Name);
            if (found.Any())
            {
                var objIndex = this.ResourcesStatistics.IndexOf(found.First());
                this.ResourcesStatistics[objIndex].Amount += item.Amount;
                if (item.Total > 0)
                    this.ResourcesStatistics[objIndex].Total = item.Total;
                if (this.ResourcesStatistics[objIndex].Total == 0)
                    this.ResourcesStatistics[objIndex].Total = this.ResourcesStatistics[objIndex].Amount;
                return;
            }
            this.ResourcesStatistics.Add(item);
        }
    }
}
