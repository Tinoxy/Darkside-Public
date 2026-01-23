namespace DarksideApi.DarkOrbit.Commands.Quests
{
    public class QuestInitializationCommand : Command
    {
        public new const short Id = 2017;
        public QuestDefinitionModule quest;

        public QuestInitializationCommand()
        {
            this.quest = new();
        }

        public override void Read(ByteArray reader)
        {
            if (reader.ReadShort() != 0)
            {
                this.quest.Read(reader);
            }
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"QuestInitializationCommand: {this.quest}";
        }
    }
}
