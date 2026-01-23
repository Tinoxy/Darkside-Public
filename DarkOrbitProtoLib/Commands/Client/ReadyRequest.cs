namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class ReadyRequest : Command  //191
    {
        public new const short Id = 14;
        public static bool IsTwoDimensional { get; set; } = true;
        public short status;
        public string message;

        public ReadyRequest(short status, bool isUnity)
        {
            this.status = status;
            this.message = (isUnity ? "3D 1366x659" : "3D 1366x659 .root1.instance226.MainClientApplication0.ApplicationSkin2.Group3.Group4._-m5_5.instance24734 root1 false -1");
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteUTF(this.message);
            this.@byte.WriteShort(this.status);
            return this.@byte.ToArray();
        }
    }
}
