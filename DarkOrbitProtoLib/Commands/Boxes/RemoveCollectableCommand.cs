namespace DarksideApi.DarkOrbit.Commands.Boxes
{
    public class RemoveCollectableCommand : Command //RemoveCollectableCommand
    {
        public new const short Id = -21530;
        public string hash;
        public bool collected;  //name_138

        public RemoveCollectableCommand()
        {
            this.hash = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            this.collected = reader.ReadBool();
            this.hash = reader.ReadUTF();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
