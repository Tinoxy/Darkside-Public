namespace DarksideApi.DarkOrbit.Commands.Boxes
{
    public class HarvestRequest : Command    //class_509
    {
        public new const short Id = 94;
        public string hash; //var_4860

        public HarvestRequest(string hash)
        {
            this.hash = hash;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteUTF(this.hash);
            return this.@byte.ToArray();
        }
    }
}
