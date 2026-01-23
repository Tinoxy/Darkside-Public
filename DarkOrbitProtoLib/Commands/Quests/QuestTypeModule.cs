namespace DarksideApi.DarkOrbit.Commands.Quests
{
    public class QuestTypeModule : Command
    {
        public new const short Id = 4141;
        public QuestTypes type;

        public QuestTypeModule()
        {
            this.type = 0;
        }

        public override void Read(ByteArray reader)
        {
            this.type = (QuestTypes)reader.ReadShort();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public enum QuestTypes
        {
            UNDEFINED = 0,
            STARTER = 1,
            MISSION = 2,
            DAILY = 3,
            CHALLENGE = 4,
            EVENT = 5,
            WEEKLY = 6,
            BATTLEPASS = 7,
            DIMINISHING = 8,
            SEASON_PASS = 9,
            FREE_PEROID = 10,
        }

        public override string ToString()
        {
            return $"QuestTypeModule: {this.type}";
        }
    }
}
