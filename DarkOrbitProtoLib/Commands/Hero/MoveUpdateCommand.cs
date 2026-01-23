namespace DarksideApi.DarkOrbit.Commands.Hero
{
    public class MoveUpdateCommand : Command    //1175
    {
        public new const short Id = 29;
        public int x;
        public int y;

        public MoveUpdateCommand()
        {
        }

        public override void Read(ByteArray reader)
        {
            this.x = reader.ReadInt();
            this.x = this.x >>> 9 % 32 | this.x << 32 - 9 % 32;
            this.y = reader.ReadInt();
            this.y = this.y << 7 % 32 | this.y >>> 32 - 7 % 32;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
