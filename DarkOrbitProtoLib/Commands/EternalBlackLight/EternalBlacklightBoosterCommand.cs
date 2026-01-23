namespace DarksideApi.DarkOrbit.Commands.EternalBlackLight
{
    public class EternalBlacklightBoosterCommand : Command    //class_1871
    {
        public new const short Id = 19480;
        public BoosterType boosterType; //var_4663
        public int value;    //value

        public EternalBlacklightBoosterCommand()
        {
        }

        public EternalBlacklightBoosterCommand(BoosterType type, int value)
        {
            this.boosterType = type;
            this.value = value;
        }

        public override void Read(ByteArray reader)
        {
            this.value = reader.ReadInt();
            this.value = this.value >>> 16 % 32 | this.value << 32 - 16 % 32;
            this.boosterType = (BoosterType)reader.ReadShort();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public enum BoosterType
        {
            COOLDOWN,
            DMG,
            DMG_L,
            DMG_R,
            HITCHANCE,
            HP,
            SHD,
            SPEED
        }
    }
}
