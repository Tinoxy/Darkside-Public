namespace DarksideApi.DarkOrbit.Objects.Boosters
{
    public class EternalBlacklightBooster : Booster
    {
        public int Amount { get; set; }
        public new string DisplayName => base.DisplayName + " " + this.DisplayAmount;
        public string DisplayAmount => $"x{this.Amount}";

        public EternalBlacklightBooster(string boosterId, int amount) : base(boosterId)
        {
            this.Amount = amount;
        }

        public override string ToString()
        {
            return $"{this.DisplayName} {this.DisplayAmount}";
        }
    }
}
