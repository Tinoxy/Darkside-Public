namespace DarksideApi.DarkOrbit.Commands.Map
{
    public class JumpgateCreateCommand : Command  //1322
    {
        public new const short Id = 3389;
        public int gateId;
        public int factionId;
        public int designId;    //name_184
        public int X;
        public int Y;
        public bool isVisibleOnMinimap;
        public bool showBubble;
        public List<int> decoratorAssetTypes;

        public JumpgateCreateCommand()
        {
            this.decoratorAssetTypes = [];
        }

        public override void Read(ByteArray reader)
        {
            this.gateId = reader.ReadInt();
            this.gateId = this.gateId >>> 10 % 32 | this.gateId << 32 - 10 % 32;
            this.factionId = reader.ReadInt();
            this.factionId = this.factionId << 2 % 32 | this.factionId >>> 32 - 2 % 32;
            this.designId = reader.ReadInt();
            this.designId = this.designId << 15 % 32 | this.designId >>> 32 - 15 % 32;
            this.X = reader.ReadInt();
            this.X = this.X >>> 11 % 32 | this.X << 32 - 11 % 32;
            this.Y = reader.ReadInt();
            this.Y = this.Y >>> 9 % 32 | this.Y << 32 - 9 % 32;
            this.isVisibleOnMinimap = reader.ReadBool();
            this.showBubble = reader.ReadBool();
            var i = 0;
            int max = reader.ReadByte();
            while (i < max)
            {
                var mod = reader.ReadInt();
                mod = mod << 2 % 32 | mod >>> 32 - 2 % 32;
                this.decoratorAssetTypes.Add(mod);
                i++;
            }
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
