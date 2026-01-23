namespace DarksideApi.DarkOrbit.Commands.EternalBlackLight
{
    public class EternalBlacklightBoosterInfoCommand : Command //BoosterInfoCommand
    {
        public new const short Id = 31280;
        public EternalBlacklightBoosterCommand boosterOption2;   //var_2906
        public EternalBlacklightBoosterCommand boosterOption1; //var_2742
        public List<EternalBlacklightBoosterCommand> currentBoosters; //var_4165
        public EternalBlacklightSkillPointInfoCommand skillPointInfo;  //var_2436
        public EternalBlacklightBoosterCommand boosterOption3; //var_3058

        public EternalBlacklightBoosterInfoCommand()
        {
            this.boosterOption2 = new();
            this.boosterOption1 = new();
            this.currentBoosters = [];
            this.skillPointInfo = new();
            this.boosterOption3 = new();
        }

        public override void Read(ByteArray reader)
        {
            var i = 0;
            var max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                var module = new EternalBlacklightBoosterCommand();
                module.Read(reader);
                this.currentBoosters.Add(module);
                i++;

            }
            if (reader.ReadShort() != 0)
            {
                this.boosterOption1.Read(reader);
            }
            if (reader.ReadShort() != 0)
            {
                this.boosterOption3.Read(reader);
            }
            if (reader.ReadShort() != 0)
            {
                this.boosterOption2.Read(reader);
            }
            if (reader.ReadShort() != 0)
            {
                this.skillPointInfo.Read(reader);
            }   
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
