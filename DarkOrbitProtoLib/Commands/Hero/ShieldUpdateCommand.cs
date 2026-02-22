namespace DarksideApi.DarkOrbit.Commands.Hero
{
    public class ShieldUpdateCommand : Command  //AttributeShieldUpdateCommand
    {
        public new const short Id = 248;
        public int shield;  //var_4411
        public int maxShield;   //name_93

        public override void Read(ByteArray reader)
        {
            this.shield = reader.ReadInt();
            this.shield = this.shield >>> 16 | this.shield << 16;
            this.maxShield = reader.ReadInt();
            this.maxShield = this.maxShield >>> 12 | this.maxShield << 20;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
