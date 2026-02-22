namespace DarksideApi.DarkOrbit.Commands.Expedition
{
    public class ExpeditionShipListRequest : Command
    {
        public new const short Id = -30877;
        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            return this.@byte.ToArray();
        }
    }
}
