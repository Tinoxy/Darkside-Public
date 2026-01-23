namespace DarksideApi.DarkOrbit.Commands.Pet
{
    public class PetHitpointsUpdateCommand : Command
    {
        public new const short Id = 158;
        public int hitpointsNow;
        public int hitpointsMax;
        public bool useRepairGear;

        public override void Read(ByteArray reader)
        {
            this.hitpointsNow = reader.ReadInt();
            this.hitpointsNow = this.hitpointsNow >>> 19 | this.hitpointsNow << 13;
            this.hitpointsMax = reader.ReadInt();
            this.hitpointsMax = this.hitpointsMax << 8 | this.hitpointsMax >>> 24;
            this.useRepairGear = reader.ReadBool();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
