namespace DarksideApi.DarkOrbit.Commands.Attack
{
    public class AttackAbordRequest : Command   //AttackAbortLaserRequest
    {
        public new const short Id = 25;

        public override void Read(ByteArray reader)
        {
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            return this.@byte.ToArray();
        }
    }
}
