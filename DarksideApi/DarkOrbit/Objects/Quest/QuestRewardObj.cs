namespace DarksideApi.DarkOrbit.Objects.Quest
{
    public class QuestRewardObj
    {
        public string? Reward { get; private set; }
        public int Amount { get; private set; }

        public QuestRewardObj(string reward, int amount)
        {
            this.Reward = reward;
            this.Amount = amount;
        }

        public override string ToString()
        {
            return $"Reward: {this.Reward}, Amount: {this.Amount}";
        }
    }
}
