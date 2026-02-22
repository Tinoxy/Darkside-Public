namespace DarksideApi.DarkOrbit.Commands.EternalBlackLight
{
    public class EternalBlacklightChooseOptionRequest : Command   //EBGChooseOptionUnityRequest
    {
        public new const short Id = 3331;
        public int selectedBooster; //4037

        public EternalBlacklightChooseOptionRequest(int selectedBooster)
        {
            this.selectedBooster = selectedBooster;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteInt(this.selectedBooster << 6 % 32 | this.selectedBooster >>> 32 - 6 % 32);
            return this.@byte.ToArray();
        }
    }
}
