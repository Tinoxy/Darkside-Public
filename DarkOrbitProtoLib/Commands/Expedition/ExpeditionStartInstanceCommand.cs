namespace DarksideApi.DarkOrbit.Commands.Expedition
{
    public class ExpeditionStartInstanceCommand : Command
    {
        public new const short Id = -5748;
        public int seconds;

        public override void Read(ByteArray reader)
        {
            this.seconds = reader.ReadInt();
            this.seconds = (this.seconds << 9 | this.seconds >>> 23);
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
