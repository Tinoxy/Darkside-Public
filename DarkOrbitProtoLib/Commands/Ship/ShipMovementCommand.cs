namespace DarksideApi.DarkOrbit.Commands.Ship
{
    public class ShipMovementCommand : Command  //MoveCommand
    {
        public new const short Id = 90;
        public int userId;
        public int x;
        public int y;
        public int timeToTarget;    //var_765

        public override void Read(ByteArray reader)
        {
            this.userId = reader.ReadInt();
            this.userId = this.userId >>> 16 | this.userId << 16;
            this.x = reader.ReadInt();
            this.x = this.x >>> 13 | this.x << 19;
            this.y = reader.ReadInt();
            this.y = this.y << 16 | this.y >>> 16;
            this.timeToTarget = reader.ReadInt();
            this.timeToTarget = this.timeToTarget << 14 | this.timeToTarget >>> 18;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
