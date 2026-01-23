namespace DarksideApi.DarkOrbit.Commands.Pet
{
    public class PetUIRepairButtonCommand : Command
    {
        public new const short Id = 244;
        public bool enabled;
        public int repairCost;

        public override void Read(ByteArray reader)
        {
            this.enabled = reader.ReadBool();
            this.repairCost = reader.ReadInt();
            this.repairCost = this.repairCost >>> 1 | this.repairCost << 31;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
