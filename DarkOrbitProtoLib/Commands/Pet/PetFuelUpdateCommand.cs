namespace DarksideApi.DarkOrbit.Commands.Pet
{
    public class PetFuelUpdateCommand : Command //class_1372
    {
        public new const short Id = 162;
        public int currentFuel; //var_4734
        public int maxFuel;

        public PetFuelUpdateCommand()
        {
        }

        public override void Read(ByteArray reader)
        {
            this.currentFuel = reader.ReadInt();
            this.currentFuel = this.currentFuel >>> 1 % 32 | this.currentFuel << 32 - 1 % 32;
            this.maxFuel = reader.ReadInt();
            this.maxFuel = this.maxFuel << 3 % 32 | this.maxFuel >>> 32 - 3 % 32;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
