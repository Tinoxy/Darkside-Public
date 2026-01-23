namespace DarksideApi.DarkOrbit.Commands.Quests
{
    public class QuestFailedCommand : Command
    {
        public new const short Id = 2005;
        public int questId;

        public override void Read(ByteArray reader)
        {
            this.questId = reader.ReadInt();
            this.questId = this.questId >> 5 | this.questId << 27;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
