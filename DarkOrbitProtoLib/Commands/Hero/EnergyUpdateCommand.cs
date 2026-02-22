namespace DarksideApi.DarkOrbit.Commands.Hero
{
    public class EnergyUpdateCommand : Command
    {
        public new const short Id = 30499;
        public int currentCaptainEnergy;
        public int captainEnergyDrain;
        public int maxCaptainEnergy;

        public override void Read(ByteArray reader)
        {
            this.captainEnergyDrain = reader.ReadInt();
            this.captainEnergyDrain = this.captainEnergyDrain >>> 2 | this.captainEnergyDrain << 30;
            this.currentCaptainEnergy = reader.ReadInt();
            this.currentCaptainEnergy = this.currentCaptainEnergy >>> 19 | this.currentCaptainEnergy << 13;
            this.maxCaptainEnergy = reader.ReadInt();
            this.maxCaptainEnergy = this.maxCaptainEnergy >>> 17 | this.maxCaptainEnergy << 15;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
