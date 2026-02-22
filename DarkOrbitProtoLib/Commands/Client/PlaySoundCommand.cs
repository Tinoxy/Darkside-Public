namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class PlaySoundCommand : Command //class_1366
    {
        public new const short Id = -11763;
        public SoundType status;

        public override void Read(ByteArray reader)
        {
            this.status = (SoundType)reader.ReadShort();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public enum SoundType
        {
            HELLSTORM_EMPTY = 0,
            HELLSTORM_LOADING = 1,
            HELLSTORM_FULLY_LOADED = 2,
            NEW_SHIP_RECEIVED = 3,
            NPC_SLENDER_ATTACK = 4,
            NPC_SLENDER_MENACE = 5,
            NPC_SLENDER_REVEAL = 6,
            NPC_SLENDER_VANISH = 7,
        }
    }
}
