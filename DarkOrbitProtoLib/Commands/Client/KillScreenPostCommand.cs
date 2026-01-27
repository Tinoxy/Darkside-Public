namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class KillScreenPostCommand : Command    //1468
    {
        public new const short Id = 252;
        public string killerName;  //var_5763
        public string killerEpppLink;
        public string playerShipAlias;     //var_4809
        public DestructionTypeModule cause;
        public List<KillscreenOptionModule> options;

        public KillScreenPostCommand()
        {
            this.killerName = string.Empty;
            this.killerEpppLink = string.Empty;
            this.playerShipAlias = string.Empty;
            this.cause = new();
            this.options = [];
        }

        public override void Read(ByteArray reader)
        {
            this.killerName = reader.ReadUTF();
            this.killerEpppLink = reader.ReadUTF();
            this.playerShipAlias = reader.ReadUTF();
            if (reader.ReadShort() != 0) //225
            {
                this.cause.Read(reader);
            }
            int max = reader.ReadByte();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
