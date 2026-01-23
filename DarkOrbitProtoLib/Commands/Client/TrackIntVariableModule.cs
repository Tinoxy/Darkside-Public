namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class TrackIntVariableModule : TrackVariableModule  //513, TrackIntVar
    {
        public new const short Id = -13931;
        public int value;

        public TrackIntVariableModule(string key, int value = 0) : base(key)
        {
            this.value = value;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            base.Write();
            this.@byte.WriteInt(this.value >>> 5 | this.value << 27);
            return this.@byte.ToArray();
        }
    }
}
