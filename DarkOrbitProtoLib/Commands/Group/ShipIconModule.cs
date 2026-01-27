namespace DarksideApi.DarkOrbit.Commands.Group
{
    public class ShipIconModule : PlayerAttributeModule   //472
    {
        public new const short Id = 29221;
        public int shipIcon;

        public override void Read(ByteArray reader)
        {
            reader.ReadShort(); //471 -> Empty
            base.Read(reader);

            this.shipIcon = reader.ReadShort();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort((short)Id);
            this.@byte.WriteShort((short)0);
            // TODO: konnte nicht automatisch konvertieren: base.Read(reader);
            this.@byte.WriteShort((short)this.shipIcon);
            return this.@byte.ToArray();
        }
    }
}
