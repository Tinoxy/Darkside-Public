namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class TrackVariableRequest : Command
    {
        public new const short Id = 10253;
        public List<TrackVariableModule> data;

        public TrackVariableRequest(List<TrackVariableModule> data)
        {
            this.data = data;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteByte((byte)this.data.Count);
            foreach (TrackVariableModule track in this.data)
            {
                this.@byte.WriteByteArray(track.Write());
            }
            return this.@byte.ToArray();
        }
    }
}
