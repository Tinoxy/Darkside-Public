namespace DarksideApi.DarkOrbit.Commands.Skylab
{
    public class SkylabActionRequest : Command
    {
        public new const short Id = -20176;
        public int countXenomit;
        public string subaction;
        public int countSeprom;
        public int countTerbium;
        public int countPrometid;
        public int countPromerium;
        public int countDuranium;
        public string transportMode;
        public string construction;
        public int countPrometium;
        public int countEndurium;

        public SkylabActionRequest(string transportMode, string construction, string subaction)
        {
            this.construction = construction;
            this.transportMode = transportMode;
            this.subaction = subaction;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteInt(this.countEndurium >>> 9 | this.countEndurium << 23);
            this.@byte.WriteInt(this.countPromerium >>> 25 | this.countPromerium << 7);
            this.@byte.WriteUTF(this.transportMode);
            this.@byte.WriteInt(this.countXenomit >>> 26 | this.countXenomit << 6); 
            this.@byte.WriteUTF(this.subaction);
            this.@byte.WriteInt(this.countPrometium >>> 13 | this.countPrometium << 19);
            this.@byte.WriteInt(this.countDuranium >>> 18 | this.countDuranium << 14);
            this.@byte.WriteUTF(this.construction);
            this.@byte.WriteInt(this.countPrometid >>> 2 | this.countPrometid << 30);
            this.@byte.WriteInt(this.countSeprom >>> 5 | this.countSeprom << 27);
            this.@byte.WriteInt(this.countTerbium >>> 21 | this.countTerbium << 11);
            return this.@byte.ToArray();
        }
    }
}
