namespace DarksideApi.DarkOrbit.Objects.Client
{
    public class GalaxyGateGenerator
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int Status { get; set; }
        public int CurrentParts { get; set; }
        public int MaxParts { get; private set; }
        public int LifesLeft { get; set; }
        public int LifePurchaseCosts { get; set; }
        public int CurrentWave { get; set; }

        public GalaxyGateGenerator(string name, int id, int status, int currentParts, int maxParts, int lifesLeft, int lifePurchaseCosts, int currentWave)
        {
            this.Name = name;
            this.Id = id;
            this.Status = status;
            this.CurrentParts = currentParts;
            this.MaxParts = maxParts;
            this.LifesLeft = lifesLeft;
            this.LifePurchaseCosts = lifePurchaseCosts;
            this.CurrentWave = currentWave;
        }
    }
}
