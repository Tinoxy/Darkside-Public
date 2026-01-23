namespace DarksideApi.DarkOrbit.Commands.Group
{
    public class TargetModule : PlayerAttributeModule
    {
        public new const short Id = -31362;
        public string name;
        public HealthModule health;
        public ShipIconModule shipType;

        public TargetModule()
        {
            this.name = string.Empty;
            this.health = new();
            this.shipType = new();
        }

        public override void Read(ByteArray reader)
        {
            reader.ReadShort();
            base.Read(reader);
 
            if (reader.ReadShort() != 0)
                this.shipType.Read(reader);
            if (reader.ReadShort() != 0)
                this.health.Read(reader);
            this.name = reader.ReadUTF();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
