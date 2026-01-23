namespace DarksideApi.DarkOrbit.Commands.Map
{
    public class MapRemovePOICommand : Command   //1677
    {
        public new const short Id = 6676;
        public string zoneId;

        public MapRemovePOICommand()
        {
            this.zoneId = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            this.zoneId = reader.ReadUTF();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
