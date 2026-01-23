namespace DarksideApi.DarkOrbit.Commands.Captcha
{
    public class CaptchaTriggerCommand : Command //CaptchaTriggerCommand
    {
        public new const short Id = 29831;
        public int captchaParam;  //var_5736
        public int captchaTimer;    //var_4771
        public int x;       //var_2089
        public int blackBoxesCount; //var_261
        public int y;       //var_2738
        public int radius;  //var_5498
        public int redBoxesCount;   //var_4816
        public int type;

        public override void Read(ByteArray reader)
        {
            this.type = reader.ReadShort();
            this.blackBoxesCount = reader.ReadInt();
            this.blackBoxesCount = this.blackBoxesCount >>> 9 | this.blackBoxesCount << 23;
            this.captchaTimer = reader.ReadInt();
            this.captchaTimer = this.captchaTimer << 10 | this.captchaTimer >>> 22;
            this.captchaParam = reader.ReadInt();
            this.captchaParam = this.captchaParam << 1 | this.captchaParam >>> 31;
            this.redBoxesCount = reader.ReadInt();
            this.redBoxesCount = this.redBoxesCount << 11 | this.redBoxesCount >>> 21;
            this.y = reader.ReadInt();
            this.y = this.y >>> 12 | this.y << 20;
            this.radius = reader.ReadInt();
            this.radius = this.radius >>> 10 | this.radius << 22;
            this.x = reader.ReadInt();
            this.x = this.x >>> 8 | this.x << 24;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
