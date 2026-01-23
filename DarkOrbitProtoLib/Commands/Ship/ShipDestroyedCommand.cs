namespace DarksideApi.DarkOrbit.Commands.Ship
{
    public class ShipDestroyedCommand : Command   //1172
    {
        public new const short Id = 30;
        public int userId;  //var_4149
        public int explosionTypeId;

        public override void Read(ByteArray reader)
        {
            this.userId = reader.ReadInt();
            this.userId = this.userId << 5 | this.userId >>> 27;
            this.explosionTypeId = reader.ReadInt();
            this.explosionTypeId = this.explosionTypeId >>> 7 | this.explosionTypeId << 25;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
