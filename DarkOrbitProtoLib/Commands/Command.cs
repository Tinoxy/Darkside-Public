namespace DarksideApi.DarkOrbit.Commands
{
    public abstract class Command   //14.31 -> ORDER, 71.181 -> ["VersionRequest","ObfuscationRequest","HandshakeRequest","LoginRequest","KeepAlive","TrackVariableRequest","UserKeyBindingsUpdate","AudioSettingsRequest"];
    {
        protected ByteArray @byte;

        public Command()
        {
            this.@byte = new ByteArray();
        }

        public const short Id = 0;
        public abstract void Read(ByteArray reader);
        public abstract byte[] Write();
        //public virtual string ToString() => JsonSerializer.Serialize(this);
    }
}
