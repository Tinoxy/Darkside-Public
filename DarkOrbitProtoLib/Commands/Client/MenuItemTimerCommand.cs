namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class MenuItemTimerCommand : Command
    {
        public new const short Id = 8670;
        public double maxTime;
        public double time;
        public LoginResponseCommand timerState;
        public bool activatable;
        public string menuItemId;

        public MenuItemTimerCommand()
        {
            this.timerState = new();
            this.menuItemId = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            this.menuItemId = reader.ReadUTF();
            this.activatable = reader.ReadBool();
            this.maxTime = reader.ReadDouble();
            this.time = reader.ReadDouble();
            if (reader.ReadShort() != 0)
            {
                this.timerState.Read(reader);
            }
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
