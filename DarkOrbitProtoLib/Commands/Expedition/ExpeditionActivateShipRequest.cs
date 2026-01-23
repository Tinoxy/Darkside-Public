namespace DarksideApi.DarkOrbit.Commands.Expedition
{
    public class ExpeditionActivateShipRequest : Command
    {
        public new const short Id = 18594;
        public long hangarId;

        public ExpeditionActivateShipRequest(long hangarId)
        {
            this.hangarId = hangarId;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteInt64(this.hangarId);
            return this.@byte.ToArray();
        }
    }
}
