namespace DarksideApi.DarkOrbit.Chat.Commands.Login
{
    internal class InitConnectionCommand : Command
    {
        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteByte(42);
            this.@byte.WriteByte(1);
            return this.@byte.ToArray();
        }
    }
}
