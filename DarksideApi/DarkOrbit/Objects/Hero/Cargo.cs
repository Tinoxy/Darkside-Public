using DarksideApi.DarkOrbit.Objects.Boxes;
using System.ComponentModel;

namespace DarksideApi.DarkOrbit.Objects.Hero
{
    public class Cargo : INotifyPropertyChanged
    {
        private readonly Api api;

        private readonly Dictionary<Ore.OreType, long> cargo;
        private long maxCargoSpace;
        private long cargoSpace;
        public long MaxCargoSpace { get => this.maxCargoSpace; set { this.maxCargoSpace = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MaxCargoSpace))); } }
        public long UsedCargoSpace { get => this.cargoSpace; set { this.cargoSpace = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.UsedCargoSpace))); } }
        public long FreeCargoSpace => this.MaxCargoSpace - this.UsedCargoSpace;
        private DateTime lastLabUpdate = DateTime.UtcNow;

        public event PropertyChangedEventHandler? PropertyChanged;

        public enum LabType
        {
            LASER = 0, ROCKETS = 1, DRIVING = 2, SHIELD = 3,
        }

        public Cargo(Api api)
        {
            this.api = api;
            this.cargo = [];
        }

        public void Set(Ore.OreType type, long amount)
        {
            if (this.cargo.TryAdd(type, amount))
            {
                return;
            }
            this.cargo[type] = amount;
        }

        public void Clear()
        {
            this.cargo.Clear();
        }

        public long Get(Ore.OreType ore)
        {
            return this.cargo.TryGetValue(ore, out var oreCount) ? oreCount : 0;
        }

        public Dictionary<Ore.OreType, long> GetTradeableOres => this.cargo.Where(o => IsTradableOre(o.Key)).ToDictionary();

        public static bool IsTradableOre(Ore.OreType ore)
        {
            return ore == Ore.OreType.TERBIUM
            || ore == Ore.OreType.PROMETID
            || ore == Ore.OreType.PROMETIUM
            || ore == Ore.OreType.PROMERIUM
            || ore == Ore.OreType.DURANIUM
            || ore == Ore.OreType.ENDURIUM;
        }

        public bool UpgradeLab(LabType type, Ore.OreType oreType, int sameOreTypeCount = 1)
        {
            if ((DateTime.UtcNow - this.lastLabUpdate).TotalSeconds < 3)
            {
                return false;
            }
            long amount;
            switch (type)
            {
                case LabType.LASER:
                case LabType.ROCKETS:
                    if (oreType != Ore.OreType.PROMETID && oreType != Ore.OreType.PROMERIUM && oreType != Ore.OreType.SEPROM)
                    {
                        return false;
                    }
                    if (!this.cargo.TryGetValue(oreType, out amount))
                    {
                        return false;
                    }
                    break;
                case LabType.DRIVING:
                    if (oreType != Ore.OreType.DURANIUM && oreType != Ore.OreType.PROMERIUM)
                    {
                        return false;
                    }
                    if (!this.cargo.TryGetValue(oreType, out amount))
                    {
                        return false;
                    }
                    break;
                case LabType.SHIELD:
                    if (oreType != Ore.OreType.DURANIUM && oreType != Ore.OreType.PROMERIUM && oreType != Ore.OreType.SEPROM)
                    {
                        return false;
                    }
                    if (!this.cargo.TryGetValue(oreType, out amount))
                    {
                        return false;
                    }
                    break;
                default:
                    this.api.Logging.Log($"Unknown Lab Upgrade Combination: {type}, {oreType}");
                    return false;
            }

            // Schutz gegen ungültigen sameOreTypeCount und sichere Berechnung des finalen Amounts
            if (sameOreTypeCount < 1)
            {
                sameOreTypeCount = 1;
            }

            long finalAmount = amount / sameOreTypeCount;

            if (finalAmount < 1)
            {
                // Zu wenig Material für ein Upgrade — keinen LabUpdate ausführen
                //this.api.Logging.Log($"Not enough ore for LabUpdate: Type={type}, Ore={oreType}, Available={amount}, RequiredPerUpgrade={sameOreTypeCount}");
                return false;
            }

            this.lastLabUpdate = DateTime.UtcNow;
            this.api.GameMethods.LabUpdate(type, oreType, finalAmount);
            this.api.Logging.Log($"Lab Update {type}, count: {finalAmount} using {oreType}");
            //UpdateCargo(oreType, (int)-amount);
            return true;
        }

        public bool UpgradeCargo(Ore.OreType targetType)
        {
            var upgradeAmount = targetType switch
            {
                Ore.OreType.PROMETID => (long)(Math.Min(this.Get(Ore.OreType.PROMETIUM) / 20L, this.Get(Ore.OreType.ENDURIUM) / 10L)),
                Ore.OreType.DURANIUM => (long)(Math.Min(this.Get(Ore.OreType.TERBIUM) / 20L, this.Get(Ore.OreType.ENDURIUM) / 10L)),
                Ore.OreType.PROMERIUM => (long)(Math.Min(Math.Min(this.Get(Ore.OreType.PROMETID) / 20L, this.Get(Ore.OreType.DURANIUM) / 10L), this.Get(Ore.OreType.XENOMIT))),
                _ => 0L,
            };
            if (upgradeAmount < 1)
            {
                return false;
            }
            this.api.Logging.Log($"Upgrading Ores to {targetType}, count: {upgradeAmount}");
            this.api.GameMethods.OreUpgrade(targetType, upgradeAmount);
            return true;
        }
    }
}
