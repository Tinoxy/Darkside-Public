namespace DarksideApi.DarkOrbit.Commands.Hero
{
    public class SpeedUpdateCommand : Command  //1768
    {
        public new const short Id = 96;
        public int speed;   //var_4774
        public int speedDisplayed;   //var_2025

        public override void Read(ByteArray reader)
        {
            this.speed = reader.ReadInt();
            this.speed = this.speed << 12 % 32 | this.speed >>> 32 - 12 % 32;
            this.speedDisplayed = reader.ReadInt();
            this.speedDisplayed = this.speedDisplayed << 1 % 32 | this.speedDisplayed >>> 32 - 1 % 32;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}