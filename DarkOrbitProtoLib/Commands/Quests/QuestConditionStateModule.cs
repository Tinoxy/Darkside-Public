namespace DarksideApi.DarkOrbit.Commands.Quests
{
    public class QuestConditionStateModule : Command //1884 -> 1906
    {
        public new const short Id = 2037;
        public double currentValue;
        public bool active;
        public bool completed;

        public QuestConditionStateModule()
        {

        }

        public override void Read(ByteArray reader)
        {
            this.currentValue = reader.ReadDouble();
            this.active = reader.ReadBool();
            this.completed = reader.ReadBool();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"QuestConditionStateModule(currentValue={this.currentValue}, active={this.active}, completed={this.completed})";
        }
    }
}
