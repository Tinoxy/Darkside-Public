using DarksideApi.DarkOrbit.Commands.Hero;

namespace DarksideApi.DarkOrbit.Commands.Action
{
    public class SellOreRequest : Command   //409
    {
        public new const short Id = 131;
        public OreCountModule cargoUpdate;   //var_5602

        public SellOreRequest(short oreType, long amount)
        {
            this.cargoUpdate = new OreCountModule(oreType, amount);
        }

        public override void Read(ByteArray reader)
        {
            reader.ReadShort();
            this.cargoUpdate.Read(reader);
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteByteArray(this.cargoUpdate.Write());
            return this.@byte.ToArray();
        }
    }
}
