namespace DarksideApi.DarkOrbit.Commands.Attack
{
    public class AttackRangeExceededCommand : Command
    {
        public new const short Id = -25523;
        public int maxRange;
        public AttackType type;
        public int userId;

        public override void Read(ByteArray reader)
        {
            this.type = (AttackType)reader.ReadShort();
            this.userId = reader.ReadInt();
            this.userId = this.userId >>> 24 | this.userId << 8;
            this.maxRange = reader.ReadInt();
            this.maxRange = this.maxRange >>> 17 | this.maxRange << 15;
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
