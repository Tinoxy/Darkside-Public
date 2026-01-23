namespace DarksideApi.DarkOrbit.Commands.Ship
{
    public class MinimapStrategyModule : Command
    {
        public new const short Id = 31697;
        public int minimapStrategy;

        public override void Read(ByteArray reader)
        {
            this.minimapStrategy = reader.ReadShort();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
