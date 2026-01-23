namespace DarksideApi.DarkOrbit.Commands.Ship
{
    public class ShipRemoveCommand : Command    //1167
    {
        public new const short Id = 52;
        public int userId;  //userId

        public override void Read(ByteArray reader)
        {
            this.userId = reader.ReadInt();
            this.userId = this.userId >>> 5 | this.userId << 27;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
