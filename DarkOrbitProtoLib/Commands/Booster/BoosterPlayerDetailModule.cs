namespace DarksideApi.DarkOrbit.Commands.Booster
{
    public class BoosterPlayerDetailModule : BoosterDetailModule
    {
        public new const short Id = 7686;
        public int secondsLeft;

        public BoosterPlayerDetailModule()
        {
            this.lootId = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            reader.ReadShort();
            base.Read(reader);

            this.secondsLeft = reader.ReadInt();
            this.secondsLeft = this.secondsLeft >>> 22 | this.secondsLeft << 10;
        }

        public override string ToString()
        {
            return base.ToString() + ", secondsLeft: " + this.secondsLeft;
        }
    }
}
