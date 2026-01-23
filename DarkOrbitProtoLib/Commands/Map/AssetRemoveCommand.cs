using DarksideApi.DarkOrbit.Commands.Client;

namespace DarksideApi.DarkOrbit.Commands.Map
{
    public class AssetRemoveCommand : Command // class_1051
    {
        public new const short Id = 3245;
        public DestructionTypeModule type;
        public int uid;

        public AssetRemoveCommand()
        {
            this.type = new();
        }

        public override void Read(ByteArray reader)
        {
            if (reader.ReadShort() != 0)
            {
                this.type.Read(reader);
            }
            this.uid = reader.ReadInt();
            this.uid = this.uid >>> 16 % 32 | this.uid << 32 - 16 % 32;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
