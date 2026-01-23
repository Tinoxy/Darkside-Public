namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class AudioSettingsRequest : Command
    {
        public new const short Id = 267;
        public int sound;
        public int music;
        public int voice;
        public bool playCombatMusic;

        public AudioSettingsRequest(int sound, int music, int voice, bool playCombatMusic)
        {
            this.sound = sound;
            this.music = music;
            this.voice = voice;
            this.playCombatMusic = playCombatMusic;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteInt(this.sound << 2 | this.sound >>> 30);
            this.@byte.WriteInt(this.music << 28 | this.sound >>> 4);
            this.@byte.WriteInt(this.voice << 25 | this.voice >>> 7);
            this.@byte.WriteBool(this.playCombatMusic);
            return this.@byte.ToArray();
        }
    }
}
