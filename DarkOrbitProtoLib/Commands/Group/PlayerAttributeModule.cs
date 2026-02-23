namespace DarksideApi.DarkOrbit.Commands.Group
{
    public class PlayerAttributeModule : Command    //471
    {
        public new const short Id = 31689;

        public PlayerAttributeModule()
        {
        }

        public override void Read(ByteArray reader)
        {
        }

        public override byte[] Write()
        {
            @byte.WriteShort(Id);
            return this.@byte.ToArray();
        }
    }
}
