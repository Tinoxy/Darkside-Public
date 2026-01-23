namespace DarksideApi.DarkOrbit.Commands.Quests
{
    public class QuestConditionUpdateCommand : Command
    {
        public new const short Id = 2014;
        public int questConditionId;
        public QuestConditionStateModule questCondition;

        public QuestConditionUpdateCommand()
        {
            this.questCondition = new();
        }

        public override void Read(ByteArray reader)
        {
            this.questConditionId = reader.ReadInt();
            this.questConditionId = this.questConditionId << 2 | this.questConditionId >>> 30;
            if (reader.ReadShort() != 0)
            {
                this.questCondition.Read(reader);
            }
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
