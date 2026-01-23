namespace DarksideApi.DarkOrbit.Commands.Booster
{
    public class BoosterFoundOnMapCommand : Command
    {
        public new const short Id = 187;
        public string lootId;
        public int hours;

        public BoosterFoundOnMapCommand()
        {
            this.lootId = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            this.lootId = reader.ReadUTF();
            this.hours = reader.ReadInt();
            this.hours = this.hours >>> 22 | this.hours << 10;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
