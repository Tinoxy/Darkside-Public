namespace DarksideApi.DarkOrbit.Commands.Attack
{
    public class AttackAbortLaserCommand : Command   //1166
    {
        public new const short Id = -17421;
        public int userId;  //uid

        public override void Read(ByteArray reader)
        {
            this.userId = reader.ReadInt();
            this.userId = this.userId >>> 2 | this.userId << 30;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
