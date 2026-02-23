namespace DarksideApi.DarkOrbit.Commands.Expedition
{
    public class ExpeditionShowUiCommand : Command
    {
        public new const short Id = -22785;
        public string param0;
        public string param1;
        public UIType type;

        public ExpeditionShowUiCommand()
        {
            this.param0 = string.Empty;
            this.param1 = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            this.type = (UIType)reader.ReadShort();
            this.param1 = reader.ReadUTF();
            this.param0 = reader.ReadUTF();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public enum UIType
        {
            RESET = 2,
            CLEAR_SHIP = 3,
            CLEAR_ACTIVE_SHIP = 4,
            LEAVE = 5,
            TICKET = 1,
            SELECT_ERR = 6,
            NONE = 0,
        }

        public override string ToString()
        {
            return $"ExpeditionShowUiCommand: param0={this.param0}, param1={this.param1}, type={this.type}";
        }
    }
}
