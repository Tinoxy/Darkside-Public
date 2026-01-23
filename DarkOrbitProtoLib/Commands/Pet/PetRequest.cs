namespace DarksideApi.DarkOrbit.Commands.Pet
{
    public class PetRequest : Command   //547
    {
        public new const short Id = 153;
        public short request;   //1215

        public PetRequest() { }

        public PetRequest(short request)
        {
            this.request = request;
        }

        public override void Read(ByteArray reader)
        {
            this.request = reader.ReadShort();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteShort(this.request);
            return this.@byte.ToArray();
        }
    }
}
