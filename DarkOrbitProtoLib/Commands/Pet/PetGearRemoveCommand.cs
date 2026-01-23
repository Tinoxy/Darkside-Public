namespace DarksideApi.DarkOrbit.Commands.Pet
{
    public class PetGearRemoveCommand : Command //1795
    {
        public new const short Id = 233;
        public PetGearModule gearModule;
        public int level;
        public int amount;

        public PetGearRemoveCommand()
        {
            this.gearModule = new();
        }

        public override void Read(ByteArray reader)
        {
            if (reader.ReadShort() != 0)
            {
                this.gearModule.Read(reader);
            }
            this.level = reader.ReadInt();
            this.level = this.level << 11 % 32 | this.level >>> 32 - 11 % 32;
            this.amount = reader.ReadInt();
            this.amount = this.amount << 2 % 32 | this.amount >>> 32 - 2 % 32;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
