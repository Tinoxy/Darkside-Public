namespace DarksideApi.DarkOrbit.Commands.Skylab
{
    public class SkylabPurchaseRobotRequest : Command
    {
        public new const short Id = -25632;
        public int robotType;
        public string construction;

        public SkylabPurchaseRobotRequest(int robotType, string construction)
        {
            this.robotType = robotType;
            this.construction = construction;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteInt(this.robotType >>> 12 | this.robotType << 20);
            this.@byte.WriteUTF(this.construction);
            return this.@byte.ToArray();
        }
    }
}
