namespace DarksideApi.DarkOrbit.Commands.Hero
{
    public class SpeedUpdateCommand : Command  //AttributeShipSpeedUpdateCommand
    {
        public new const short Id = 96;
        public int speed;   //var_4774
        public int speedDisplayed;   //var_2025

        public override void Read(ByteArray reader)
        {
            this.speed = reader.ReadInt();
            this.speed = this.speed << 12 | this.speed >>> 20;
            this.speedDisplayed = reader.ReadInt();
            this.speedDisplayed = this.speedDisplayed << 1 | this.speedDisplayed >>> 1;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}