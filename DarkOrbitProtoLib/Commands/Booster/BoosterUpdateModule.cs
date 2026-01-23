namespace DarksideApi.DarkOrbit.Commands.Booster
{
    public class BoosterUpdateModule : Command  //BoosterUpdateModule
    {
        public new const short Id = 100;
        public string attributeType;
        public float value;

        public BoosterUpdateModule()
        {
            this.attributeType = string.Empty;
            this.value = 0;
        }

        public override void Read(ByteArray reader)
        {
            this.attributeType = reader.ReadUTF();
            this.value = reader.ReadFloat();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "attributeType: " + this.attributeType + ", value: " + this.value;
        }
    }
}
