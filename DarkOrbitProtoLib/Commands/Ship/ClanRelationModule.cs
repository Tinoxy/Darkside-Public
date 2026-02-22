namespace DarksideApi.DarkOrbit.Commands.Ship
{
    public class ClanRelationModule : Command   //570
    {
        public new const short Id = 12443;
        public int type;

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
