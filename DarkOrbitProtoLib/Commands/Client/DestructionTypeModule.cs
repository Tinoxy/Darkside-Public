namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class DestructionTypeModule : Command //class_1773
    {
        public new const short Id = 259;
        public int type = 0;    //var_2295
        public override void Read(ByteArray reader)
        {
            this.type = reader.ReadShort();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
