namespace DarksideApi.DarkOrbit.Commands.Pet
{
    public class PetGearLocatorUpdateCommand : Command  //PetGearLocatorUpdate
    {
        public new const short Id = 237;
        public int x;
        public int y;

        public PetGearLocatorUpdateCommand()
        {
        }

        public override void Read(ByteArray reader)
        {
            this.x = reader.ReadInt();
            this.x = this.x >>> 9 | this.x << 23;
            this.y = reader.ReadInt();
            this.y = this.y >>> 4 | this.y << 28;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
