namespace DarksideApi.DarkOrbit.Commands.Quests
{
    public class QuestAcceptabilityStatusModule : Command //class_676
    {
        public new const short Id = 10101;
        public short type;

        public QuestAcceptabilityStatusModule(short type)
        {
            this.type = type;
        }

        public QuestAcceptabilityStatusModule()
        {
            this.type = 0;
        }

        public override void Read(ByteArray reader)
        {
            this.type = reader.ReadShort();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteShort(this.type);
            return this.@byte.ToArray();
        }
    }
}
