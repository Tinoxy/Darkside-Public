namespace DarksideApi.DarkOrbit.Commands.EternalBlackLight
{
    public class EternalBlacklightCpuInfoCommand : Command //EBGCpuInfoCommand
    {
        public new const short Id = -3953;
        public int cpuCount;

        public override void Read(ByteArray reader)
        {
            this.cpuCount = reader.ReadInt();
            this.cpuCount = this.cpuCount >>> 11 | this.cpuCount << 21;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
