namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class ServerVersionCheck : Command   //class_1619
    {
        public new const short Id = 667;
        public string version;
        public bool compatible;

        public ServerVersionCheck()
        {
            this.version = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            this.version = reader.ReadUTF();
            this.compatible = reader.ReadBool();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
