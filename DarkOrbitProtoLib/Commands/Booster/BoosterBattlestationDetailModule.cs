namespace DarksideApi.DarkOrbit.Commands.Booster
{
    public class BoosterBattlestationDetailModule : BoosterDetailModule
    {
        public new const short Id = -28412;
        public float percentage;

        public BoosterBattlestationDetailModule()
        {
            this.lootId = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            reader.ReadShort();
            base.Read(reader);

            this.percentage = reader.ReadFloat();
        }

        public override string ToString()
        {
            return base.ToString() + ", percentage: " + this.percentage;
        }
    }
}
