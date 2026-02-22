namespace DarksideApi.DarkOrbit.Commands.Quests
{
    public class QuestAcceptRequest : Command   //class_1034
    {
        public new const short Id = 2035;
        public QuestAcceptabilityStatusModule acceptabilityModule;
        public int questId;

        public QuestAcceptRequest(int questId, short acceptability)
        {
            this.questId = questId;
            this.acceptabilityModule = new(acceptability);
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteByteArray(this.acceptabilityModule.Write());
            this.@byte.WriteInt(this.questId >>> 1 % 32 | this.questId << 32 - 1 % 32);
            return this.@byte.ToArray();
        }
    }
}
