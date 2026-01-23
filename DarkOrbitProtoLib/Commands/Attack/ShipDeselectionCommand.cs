namespace DarksideApi.DarkOrbit.Commands.Attack
{
    public class ShipDeselectionCommand : Command    //1168
    {
        public new const short Id = 17;

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
