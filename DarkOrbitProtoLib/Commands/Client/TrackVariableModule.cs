namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class TrackVariableModule : Command   //497, TrackVariableModule
    {
        public new const short Id = -4926;
        public string key;

        public TrackVariableModule(string key)
        {
            this.key = key;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteUTF(this.key);
            return this.@byte.ToArray();
        }
    }
}
