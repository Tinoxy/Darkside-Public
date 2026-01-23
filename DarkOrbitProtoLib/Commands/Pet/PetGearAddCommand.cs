namespace DarksideApi.DarkOrbit.Commands.Pet
{
    public class PetGearAddCommand : Command  //1782
    {
        public new const short Id = 234;
        public PetGearModule gearModule;
        public int level;
        public int amount;
        public bool enabled;

        public PetGearAddCommand()
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
            this.level = this.level >>> 8 % 32 | this.level << 32 - 8 % 32;
            this.amount = reader.ReadInt();
            this.amount = this.amount >>> 12 % 32 | this.amount << 32 - 12 % 32;
            this.enabled = reader.ReadBool();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
