namespace DarksideApi.DarkOrbit.Objects.Boosters
{
    public class Booster
    {
        public string BoosterId { get; set; }
        public string DisplayName => this.BoosterId.Split('_').Last().ToUpper();

        public Booster(string boosterId)
        {
            this.BoosterId = boosterId;
        }

        public override string ToString()
        {
            return this.DisplayName;
        }
    }
}
