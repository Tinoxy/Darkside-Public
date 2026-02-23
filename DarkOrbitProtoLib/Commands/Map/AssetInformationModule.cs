namespace DarksideApi.DarkOrbit.Commands.Map
{
    public class AssetInformationModule : Command
    {
        public new const short Id = 3430;

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
