namespace DarksideApi.DarkOrbit.Commands.Pet
{
    public class PetInitializationCommand : Command
    {
        public new const short Id = 229;
        public bool hasPet;
        public bool hasFuel;
        public bool petIsAlive;

        public override void Read(ByteArray reader)
        {
            this.hasPet = reader.ReadBool();
            this.hasFuel = reader.ReadBool();
            this.petIsAlive = reader.ReadBool();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
