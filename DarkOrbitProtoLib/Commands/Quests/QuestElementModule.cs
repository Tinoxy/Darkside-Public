namespace DarksideApi.DarkOrbit.Commands.Quests
{
    public class QuestElementModule : Command    //694
    {
        public new const short Id = 2001;
        public QuestCaseModule questCase;
        public QuestConditionModule condition;

        public QuestElementModule()
        {
            this.questCase = new();
            this.condition = new();
        }

        public override void Read(ByteArray reader)
        {
            if (reader.ReadShort() != 0)
            {
                this.questCase.Read(reader);
            }
            if (reader.ReadShort() != 0)
            {
                this.condition.Read(reader);
            }
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"QuestElementModule(questCase={this.questCase},\ncondition={this.condition})\n";
        }
    }
}
