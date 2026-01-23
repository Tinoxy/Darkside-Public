namespace DarksideApi.DarkOrbit.Commands.Pet
{
    public class PetIsDestroyedCommand : Command
    {
        public new const short Id = 227;
        public DestroyedType destroyedType;

        public override void Read(ByteArray reader)
        {
            this.destroyedType = (DestroyedType)reader.ReadShort();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public enum DestroyedType
        {
            NONE,
            USED_KAMIKAZE
        }
    }
}
