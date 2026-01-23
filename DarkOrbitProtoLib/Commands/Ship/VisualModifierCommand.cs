namespace DarksideApi.DarkOrbit.Commands.Ship
{
    public class VisualModifierCommand : Command  //class_325
    {
        public new const short Id = 280;
        public int userId;
        public int modifier;
        public int attribute;
        public string additionalAttribute;
        public int count;
        public bool activated;

        public VisualModifierCommand()
        {
            this.additionalAttribute = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            this.userId = reader.ReadInt();
            this.userId = this.userId >>> 12 | this.userId << 20;
            this.modifier = reader.ReadShort();
            this.attribute = reader.ReadInt();
            this.attribute = this.attribute << 7 | this.attribute >>> 25;
            this.additionalAttribute = reader.ReadUTF();
            this.count = reader.ReadInt();
            this.count = this.count >>> 14 | this.count << 18;
            this.activated = reader.ReadBool();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
