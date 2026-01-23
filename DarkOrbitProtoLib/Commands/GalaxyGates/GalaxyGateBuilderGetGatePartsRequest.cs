namespace DarksideApi.DarkOrbit.Commands.GalaxyGates
{
    public class GalaxyGateBuilderGetGatePartsRequest : Command   //828
    {
        public new const short Id = -554;
        public int gateId;

        public const byte ALPHA = 1;
        public const byte BETA = 2;
        public const byte GAMMA = 3;
        public const byte DELTA = 4;
        public const byte EPSILON = 5;
        public const byte ZETA = 6;
        public const byte KAPPA = 7;
        public const byte LAMBDA = 8;
        public const byte KRONOS = 9;
        public const byte HADES = 10;
        public const byte STREUNER = 11;

        public GalaxyGateBuilderGetGatePartsRequest(int gateId)
        {
            this.gateId = gateId;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
