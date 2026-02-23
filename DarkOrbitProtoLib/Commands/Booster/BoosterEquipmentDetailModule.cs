namespace DarksideApi.DarkOrbit.Commands.Booster
{
    public class BoosterEquipmentDetailModule : BoosterDetailModule
    {
        public new const short Id = 31601;
        public int amount;

        public BoosterEquipmentDetailModule()
        {
            this.lootId = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            reader.ReadShort();
            base.Read(reader);

            this.amount = reader.ReadInt();
            this.amount = this.amount >>> 5 | this.amount << 27;
        }

        public override string ToString()
        {
            return base.ToString() + ", amount: " + this.amount;
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort((short)Id);
            this.@byte.WriteShort((short)0);
            this.@byte.WriteInt(this.amount >>> 5 | this.amount << 27);
            return this.@byte.ToArray();
        }
    }

}
