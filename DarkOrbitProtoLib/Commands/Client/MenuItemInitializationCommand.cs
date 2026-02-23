namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class MenuItemInitializationCommand : Command  //2253
    {
        public new const short Id = 9903;
        public MenuItemTimerCommand timer;
        public LoginResponseCommand cooldownGroup;
        public MenuItemStatusCommand status;
        public int actionStyle;
        public bool showTooltipCooldownTimer;
        public int defaultIndex;
        public int counterType;

        public MenuItemInitializationCommand()
        {
            this.timer = new();
            this.cooldownGroup = new();
            this.status = new();
        }

        public override void Read(ByteArray reader)
        {
            if (reader.ReadShort() != 0)
            {
                this.cooldownGroup.Read(reader);
            }
            if (reader.ReadShort() != 0)
            {
                this.status.Read(reader);
            }
            this.actionStyle = reader.ReadShort();
            if (reader.ReadShort() != 0)
            {
                this.timer.Read(reader);
            }
            this.defaultIndex = reader.ReadInt();
            this.defaultIndex = this.defaultIndex >>> 12 | this.defaultIndex << 20;
            this.counterType = reader.ReadShort();
            this.showTooltipCooldownTimer = reader.ReadBool();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
