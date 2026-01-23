namespace DarksideApi.DarkOrbit.Commands.DailyLogin
{
    public class DailyLoginRewardDayCommand : Command
    {
        public new const int Id = 28670;
        public int nextMonthSeconds;
        public bool end;
        public int day;
        public bool claimed;
        public int month;

        public override void Read(ByteArray reader)
        {
            this.nextMonthSeconds = reader.ReadInt();
            this.nextMonthSeconds = this.nextMonthSeconds >>> 20 | this.nextMonthSeconds << 12;
            this.end = reader.ReadBool();
            this.day = reader.ReadInt();
            this.day = this.day << 15 | this.day >>> 17;   
            this.claimed = reader.ReadBool();
            this.month = reader.ReadInt();
            this.month = this.month >>> 3 | this.month << 29;                      
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
