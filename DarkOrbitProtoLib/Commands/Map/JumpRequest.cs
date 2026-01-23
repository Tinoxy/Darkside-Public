namespace DarksideApi.DarkOrbit.Commands.Map
{
    public class JumpRequest : Command    //class_1161
    {
        public new const short Id = 10;

        public override void Read(ByteArray reader)
        {
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            return this.@byte.ToArray();
        }
    }
}
