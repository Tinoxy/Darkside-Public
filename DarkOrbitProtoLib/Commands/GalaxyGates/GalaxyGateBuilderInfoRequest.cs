namespace DarksideApi.DarkOrbit.Commands.GalaxyGates
{
    public class GalaxyGateBuilderInfoRequest : Command   //GGBuilderInfoRequest
    {
        public new const short Id = 22388;

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            return this.@byte.ToArray();
        }
    }
}
