namespace DarksideApi.DarkOrbit.Commands.Action
{
    public class LabItemModule : Command  //1152
    {
        public new const short Id = 134;
        public short itemType;

        public const int LASER = 0;
        public const int ROCKETS = 1;
        public const int DRIVING = 2;
        public const int SHIELD = 3;

        public LabItemModule(short itemType)
        {
            this.itemType = itemType;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteShort(this.itemType);
            return this.@byte.ToArray();
        }
    }
}
