using DarksideApi.DarkOrbit.Commands.Loot;

namespace DarksideApi.DarkOrbit.Commands.Quests
{
    public class QuestDefinitionModule : Command //class_229
    {
        public new const short Id = 2099;
        public int questId;
        public List<QuestTypeModule> types;
        public QuestCaseModule rootCase;   //var_4897
        public List<LootModule> rewards;
        public List<QuestTypeModule> icons;
        public string textKeyFoundation;
        public string portraiId;

        public QuestDefinitionModule()
        {
            this.types = [];
            this.rootCase = new();
            this.rewards = [];
            this.icons = [];
            this.textKeyFoundation = string.Empty;
            this.portraiId = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            this.questId = reader.ReadInt();
            this.questId = this.questId << 15 % 32 | this.questId >>> 32 - 15 % 32;
            var i = 0;
            var max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                var t = new QuestTypeModule();
                t.Read(reader);
                this.types.Add(t);
                i++;
            }
            if (reader.ReadShort() != 0)
            {
                this.rootCase.Read(reader);
            }
            i = 0;
            max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                var r = new LootModule();
                r.Read(reader);
                this.rewards.Add(r);
                i++;
            }
            i = 0;
            max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                var c = new QuestTypeModule();
                c.Read(reader);
                this.icons.Add(c);
                i++;
            }
            this.textKeyFoundation = reader.ReadUTF();
            this.portraiId = reader.ReadUTF();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"QuestDefinitionModule: {this.questId}, RootCase:\n{this.rootCase}\nTypes: {string.Join(";", this.types)},\nRewards: {string.Join(";", this.rewards)},\nIcons: {string.Join(";", this.icons)},\nTextKeyFoundation: {this.textKeyFoundation}, PortraitId: {this.portraiId}";
        }
    }
}
