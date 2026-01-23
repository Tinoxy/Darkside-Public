namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class HealCommand : Command
    {
        public new const short Id = 31;
        public int healType;
        public int healerId;
        public int healedId;     //var_4809
        public int currentHitpoints;
        public int healAmount;

        public override void Read(ByteArray reader)
        {
            this.healType = reader.ReadShort();
            this.healerId = reader.ReadInt();
            this.healerId = (this.healerId << 19) | (this.healerId >> 13);
            this.healedId = reader.ReadInt();
            this.healedId = (this.healedId << 3) | (this.healedId >> 29);
            this.currentHitpoints = reader.ReadInt();
            this.currentHitpoints = (this.currentHitpoints << 4) | (this.currentHitpoints >> 28);
            this.healAmount = reader.ReadInt();
            this.healAmount = (this.healAmount >> 25) | (this.healAmount << 7);
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
