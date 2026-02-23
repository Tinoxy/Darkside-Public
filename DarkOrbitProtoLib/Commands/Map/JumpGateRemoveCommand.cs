namespace DarksideApi.DarkOrbit.Commands.Map
{
    public class JumpGateRemoveCommand : Command    //JumpGateRemove
    {
        public new const short Id = 32164;
        public int gateId;

        public override void Read(ByteArray reader)
        {
            this.gateId = reader.ReadInt();
            this.gateId = this.gateId >>> 12 | this.gateId << 20;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
