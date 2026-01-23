using System.Diagnostics;

namespace DarksideApi.DarkOrbit.Objects.Hero
{
    public class Inventory
    {
        private readonly Dictionary<string, long> _items;
        public int BootyKeysGreen { get; set; }
        public int BootyKeysRed { get; set; }
        public int BootyKeysBlue { get; set; }
        public int BootyKeysSilver { get; set; }
        public int BootyKeysBlackLightChifre { get; set; }
        public int BootyKeysMasque { get; set; }
        public int BootyKeysBlack { get; set; }
        public int BootyKeysAstral { get; set; }
        public int BootyKeysAstralPrime { get; set; }
        public int BootyKeysLucientEgg { get; set; }
        public int BootyKeysEmpyrian { get; set; }
        public int BootyKeysPrometheus { get; set; }
        public int BootyKeysLaurel { get; set; }
        public int BootyKeysProsperous { get; set; }
        public double ExtraEnergy { get; set; }
        public double JumpVouchers { get; set; }

        public Inventory()
        {
            this._items = [];
        }

        public void Add(string key, long value)
        {
            if (this._items.TryAdd(key, value))
            {
                return;
            }
            this._items[key] = value;
        }

        public long Get(string key)
        {
            return this._items.TryGetValue(key, out var value) ? value : 0;
        }

        public void AddUridium(long amount)
        {
            this.Add("currency_uridium", amount);
        }

        public void AddCredits(long amount)
        {
            this.Add("currency_credits", amount);
        }

        public long GetUridium => this.Get("currency_uridium");
        public long GetCredits => this.Get("currency_credits");
        public long GetQuickCoupons => this.Get("resource_collectable_quickcoupon");
        public long GetFrozenLabyrinthAccessKeys => this.Get("ACCESS-FLB");
        public long GetEscortAccessKeys => this.Get("ACCESS-ESCORT");
    }
}
