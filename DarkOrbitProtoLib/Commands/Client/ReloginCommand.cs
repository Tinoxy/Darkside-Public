namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class ReloginCommand : Command //class_1498, ReloginCommand
    {
        public new const short Id = -17430;
        public int mapId;
        public int delayInMs;

        public override void Read(ByteArray reader)
        {
            this.mapId = reader.ReadInt();
            this.mapId = this.mapId << 16 | this.mapId >>> 16;
            this.delayInMs = reader.ReadInt();
            this.delayInMs = this.delayInMs >>> 9 | this.delayInMs << 23;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
