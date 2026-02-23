namespace DarksideApi.DarkOrbit.Commands.Mines
{
    public class RemoveMineCommand : Command    //RemoveMineCommand
    {
        public new const short Id = -932;
        public string hash;

        public RemoveMineCommand()
        {
            this.hash = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            this.hash = reader.ReadUTF();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
