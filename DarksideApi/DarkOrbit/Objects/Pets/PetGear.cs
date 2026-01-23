namespace DarksideApi.DarkOrbit.Objects.Pets
{
    public class PetGear
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public int Amount { get; set; }
        public bool IsEnabled { get; set; }

        public PetGear(int id, int level, int amount, bool isEnabled)
        {
            this.Id = id;
            this.Level = level;
            this.Amount = amount;
            this.IsEnabled = isEnabled;
        }
    }
}
