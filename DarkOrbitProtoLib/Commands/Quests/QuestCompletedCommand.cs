namespace DarksideApi.DarkOrbit.Commands.Quests
{
    public class QuestCompletedCommand : Command
    {
        public new const short Id = 2004;
        public int questId;
        public int rating;

        public override void Read(ByteArray reader)
        {
            this.questId = reader.ReadInt();
            this.questId = this.questId >> 2 | this.questId << 30;
            this.rating = reader.ReadInt();
            this.rating = this.rating >> 13 | this.rating << 19;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
