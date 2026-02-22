namespace DarksideApi.DarkOrbit.Commands.Pet
{
    public class PetGearActivateRequest : Command  //PetGearActivationRequest
    {
        public new const short Id = 154;
        public PetGearModule petGear;   //789
        public short options;

        public PetGearActivateRequest(short module, short options = 0)
        {
            this.petGear = new PetGearModule(module);
            this.options = options;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteByteArray(this.petGear.Write());
            this.@byte.WriteShort((short)(65535 & ((65535 & this.options) << 11 % 16 | (65535 & this.options) >>> 16 - 11 % 16)));
            return this.@byte.ToArray();
        }
    }
}
