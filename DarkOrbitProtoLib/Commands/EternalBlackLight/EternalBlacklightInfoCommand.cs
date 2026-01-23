namespace DarksideApi.DarkOrbit.Commands.EternalBlackLight
{
    public class EternalBlacklightInfoCommand : Command //EternalBlacklightInfoCommand
    {
        public new const short Id = 3806;
        public EternalBlacklightCpuInfoCommand cpuInfoCommand;  //var_2868
        public EternalBlacklightBoosterInfoCommand boosterInfoCommand;  //var_3930
        public EternalBlacklightWaveInfoCommand waveInfoCommand;    //var_5719

        public EternalBlacklightInfoCommand()
        {
            this.cpuInfoCommand = new();
            this.boosterInfoCommand = new();
            this.waveInfoCommand = new();
        }

        public override void Read(ByteArray reader)
        {
            if (reader.ReadShort() != 0)
            {
                this.waveInfoCommand.Read(reader);
            }
            if (reader.ReadShort() != 0)
            {
                this.cpuInfoCommand.Read(reader);
            }
            if (reader.ReadShort() != 0)
            {
                this.boosterInfoCommand.Read(reader);
            }
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
