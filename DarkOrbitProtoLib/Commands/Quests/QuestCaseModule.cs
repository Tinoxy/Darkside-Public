namespace DarksideApi.DarkOrbit.Commands.Quests
{
    public class QuestCaseModule : Command    //689
    {
        public new const short Id = 2000;
        public int questId;
        public bool active;
        public bool mandatory;
        public bool ordered;
        public int mandatoryCount;
        public List<QuestElementModule> modifier;//:Vector.<net.bigpoint.darkorbit.com.module.class_691>;

        public QuestCaseModule()
        {
            this.modifier = [];
        }

        public override void Read(ByteArray reader)
        {
            this.questId = reader.ReadInt();
            this.questId = this.questId >>> 11 % 32 | this.questId << 32 - 11 % 32;
            this.active = reader.ReadBool();
            this.mandatory = reader.ReadBool();
            this.ordered = reader.ReadBool();
            this.mandatoryCount = reader.ReadInt();
            this.mandatoryCount = this.mandatoryCount << 1 % 32 | this.mandatoryCount >>> 32 - 1 % 32;
            var i = 0;
            var max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                QuestElementModule module = new();
                module.Read(reader);
                this.modifier.Add(module);
                i++;
            }
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"QuestCaseModule(questId={this.questId}, active={this.active}, mandatory={this.mandatory}, ordered={this.ordered}, mandatoryCount={this.mandatoryCount},\nmodifier={string.Join(";", this.modifier)})\n";
        }
    }
}
