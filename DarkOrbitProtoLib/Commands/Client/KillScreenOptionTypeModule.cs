namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class KillScreenOptionTypeModule : Command //class_301;
    {
        public new const short Id = 254;
        public short selection; //var_446

        public KillScreenOptionTypeModule(short selection)
        {
            this.selection = selection;
        }

        public override void Read(ByteArray reader)
        {
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteShort(this.selection);
            return this.@byte.ToArray();
        }
    }
}
