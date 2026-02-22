namespace DarksideApi.DarkOrbit.Commands.Booster
{
    public class BoosterDetailModule : Command  //BoosterDetailModule
    {
        public new const short Id = 9868;
        public string lootId;

        public BoosterDetailModule()
        {
            this.lootId = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            this.lootId = reader.ReadUTF();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "lootId: " + this.lootId;
        }
    }
}
