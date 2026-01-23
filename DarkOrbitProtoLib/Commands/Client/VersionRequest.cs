namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class VersionRequest : Command
    {
        public new const short Id = 666;
        public string version;

        public VersionRequest(string version)
        {
            this.version = version;
        }

        public override void Read(ByteArray reader)
        {
            this.version = reader.ReadUTF();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteUTF(this.version!);
            return this.@byte.ToArray();
        }
    }
}
