using DarksideApi.DarkOrbit.Commands.Hero;

namespace DarksideApi.DarkOrbit.Commands.Action
{
    public class UpgradeOreRequest : Command    //313
    {
        public new const short Id = 132;    //var_847
        public OreCountModule cargoUpdate;

        public UpgradeOreRequest(short type, long amount)
        {
            this.cargoUpdate = new(type, amount);
        }

        public override void Read(ByteArray reader)
        {
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteByteArray(this.cargoUpdate.Write());
            return this.@byte.ToArray();
        }
    }
}
