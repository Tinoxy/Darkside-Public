namespace DarksideApi.DarkOrbit.Commands.Expedition
{
    public class ExpeditionSelectShipRequest : Command
    {
        public new const short Id = 17836;
        public List<long> hangarId;

        public ExpeditionSelectShipRequest()
        {
            this.hangarId = [];
        }

        public ExpeditionSelectShipRequest(List<long> hangarIds)
        {
            this.hangarId = hangarIds;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteByte((byte)this.hangarId.Count);
            foreach (var id in this.hangarId)
            {
                this.@byte.WriteInt64(id);
            }
            return this.@byte.ToArray();
        }
    }
}
