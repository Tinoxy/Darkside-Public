namespace DarksideApi.DarkOrbit.Commands.Attack
{
    public class AttackRangeExceededCommand : Command
    {
        public new const short Id = 15320;
        public int maxRange;
        public AttackType type;
        public int userId;

        public override void Read(ByteArray reader)
        {
            this.maxRange = reader.ReadInt();
            this.maxRange = this.maxRange >>> 17 | this.maxRange << 15;
            this.type = (AttackType)reader.ReadShort();
            this.userId = reader.ReadInt();
            this.userId = this.userId >>> 24 | this.userId << 8;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public enum AttackType
        {
            LASER,
            ROCKET,
            HELLSTORM,
            MISC
        }
    }
}
