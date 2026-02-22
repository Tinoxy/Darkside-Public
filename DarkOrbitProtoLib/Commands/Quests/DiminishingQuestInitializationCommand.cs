namespace DarksideApi.DarkOrbit.Commands.Quests
{
    public class DiminishingQuestInitializationCommand : Command    //class_1477
    {
        public new const short Id = 602;
        public double timeToQuestEnd;
        public QuestAcceptabilityStatusModule acceptabilityStatus;
        public QuestDefinitionModule quest;

        public DiminishingQuestInitializationCommand()
        {
            this.timeToQuestEnd = 0;
            this.acceptabilityStatus = new();
            this.quest = new();
        }

        public override void Read(ByteArray reader)
        {
            this.timeToQuestEnd = reader.ReadDouble();
            if (reader.ReadShort() != 0)
            {
                this.acceptabilityStatus.Read(reader);
            }
            if (reader.ReadShort() != 0)
            {
                this.quest.Read(reader);
            }
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
