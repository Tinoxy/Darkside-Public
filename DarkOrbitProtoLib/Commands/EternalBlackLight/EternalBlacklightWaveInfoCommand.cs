namespace DarksideApi.DarkOrbit.Commands.EternalBlackLight
{
    public class EternalBlacklightWaveInfoCommand : Command    //class_911
    {
        public new const short Id = 5667;
        public int furthestWave;
        public int currentWave; //name_26

        public override void Read(ByteArray reader)
        { 
            this.furthestWave = reader.ReadInt();
            this.furthestWave = this.furthestWave << 2 | this.furthestWave >>> 30;
            this.currentWave = reader.ReadInt();
            this.currentWave = this.currentWave << 3 | this.currentWave >>> 29;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
