namespace DarksideApi.DarkOrbit.Objects.Boosters
{
    public class PlayerBooster : Booster
    {
        public int Duration { get; set; }
        public new string DisplayName => base.DisplayName + " " + this.DisplayDuration;
        public string DisplayDuration => this.Duration switch
        {
            0 => "/",
            < 60 => $"{this.Duration}S",
            < 3600 => $"{this.Duration / 60}M",
            < 86400 => $"{this.Duration / 3600}H",
            _ => $"{this.Duration / 86400}D"
        };

        public PlayerBooster(string boosterId, int duration) : base(boosterId)
        {
            this.Duration = duration;
        }

        public override string ToString()
        {
            return $"{this.DisplayName} {this.DisplayDuration}";
        }
    }
}
