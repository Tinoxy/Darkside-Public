namespace DarksideApi.DarkOrbit.Commands.Captcha
{
    public class CaptchaCollectRequest : Command    //CaptchaCollectRequest
    {
        public new const short Id = 6087;
        public int index;

        public CaptchaCollectRequest(int index)
        {
            this.index = index;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteInt(this.index << 4 % 32 | this.index >>> 28 % 32);
            return this.@byte.ToArray();
        }
    }
}
