using DarksideApi.DarkOrbit.Commands.Hero;

namespace DarksideApi.DarkOrbit.Commands.Action
{
    public class LabUpdateItemRequest : Command   //771
    {
        public new const short Id = 133;
        public LabItemModule itemToUpdate;      //var_946
        public OreCountModule updateWith;    //var_3971

        public LabUpdateItemRequest(LabItemModule itemToUpdate, OreCountModule updateWith)
        {
            this.itemToUpdate = itemToUpdate;
            this.updateWith = updateWith;
        }

        public override void Read(ByteArray reader)
        {
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteByteArray(this.itemToUpdate.Write());
            this.@byte.WriteByteArray(this.updateWith.Write());
            return this.@byte.ToArray();
        }
    }
}
