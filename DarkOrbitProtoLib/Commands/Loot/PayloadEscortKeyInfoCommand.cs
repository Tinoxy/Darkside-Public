namespace DarksideApi.DarkOrbit.Commands.Loot
{
    public class PayloadEscortKeyInfoCommand : Command
    {
        public new const short Id = 13385;
        public int keyCount;

        public override void Read(ByteArray reader)
        {
            this.keyCount = reader.ReadInt();
            this.keyCount = this.keyCount << 10 % 32 | this.keyCount >>> 32 - 10 % 32;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
