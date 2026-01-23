namespace DarksideApi.DarkOrbit.Commands.Booster
{
    public class AttributeBoosterUpdateCommand : Command    //AttributeBoosterUpdateCommand
    {
        public new const short Id = 101;
        public List<BoosterUpdateModule> boostedAttributes;
        public List<BoosterDetailModule> boostedDetailsUpdate;
        public List<BoosterDetailModule> boostedDetailsRemove;

        public AttributeBoosterUpdateCommand()
        {
            this.boostedAttributes = [];
            this.boostedDetailsUpdate = [];
            this.boostedDetailsRemove = [];
        }

        public override void Read(ByteArray reader)
        {
            var i = 0;
            var max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                BoosterUpdateModule boostedAttribute = new();
                boostedAttribute.Read(reader);
                this.boostedAttributes.Add(boostedAttribute);
                i++;
            }
            i = 0;
            max = reader.ReadByte();
            while (i < max)
            {
                BoosterDetailModule detailUpdate = this.GetBoosterDetailModule(reader.ReadShort());
                detailUpdate.Read(reader);
                this.boostedDetailsUpdate.Add(detailUpdate);
                i++;
            }
            i = 0;
            max = reader.ReadByte();
            while (i < max)
            {
                BoosterDetailModule detailRemove = this.GetBoosterDetailModule(reader.ReadShort());
                detailRemove.Read(reader);
                this.boostedDetailsRemove.Add(detailRemove);
                i++;
            }
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public BoosterDetailModule GetBoosterDetailModule(short id)
        {
            return id switch
            {
                BoosterBattlestationDetailModule.Id => new BoosterBattlestationDetailModule(),
                BoosterEquipmentDetailModule.Id => new BoosterEquipmentDetailModule(),
                BoosterPlayerDetailModule.Id => new BoosterPlayerDetailModule(),
                BoosterSharedDetailModule.Id => new BoosterSharedDetailModule(),
                _ => new BoosterDetailModule(),
            };
        }

        public override string ToString()
        {
            string a = "", b = "", c = "";
            this.boostedAttributes.ForEach(x => a += x.ToString() + ", ");
            this.boostedDetailsUpdate.ForEach(x => b += x.ToString() + ", ");
            this.boostedDetailsRemove.ForEach(x => c += x.ToString() + ", ");

            return "boostedAttributes: " + a + ", boostedDetailsUpdate: " + b + ", boostedDetailsRemove: " + c;
        }
    }
}
