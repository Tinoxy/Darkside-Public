namespace DarksideApi.DarkOrbit.Chat.Commands
{
    public abstract class Command
    {
        protected ByteArray @byte;

        public Command()
        {
            this.@byte = new ByteArray();
        }

        public const short Id = 0;
        public abstract void Read(ByteArray reader);
        public abstract byte[] Write();
    }
}
