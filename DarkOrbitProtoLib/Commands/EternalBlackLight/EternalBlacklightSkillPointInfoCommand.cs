namespace DarksideApi.DarkOrbit.Commands.EternalBlackLight
{
    public class EternalBlacklightSkillPointInfoCommand : Command    //class_909
    {
        public new const short Id = 25778;
        public int points;  //var_5886

        public override void Read(ByteArray reader)
        {
            this.points = reader.ReadInt();
            this.points = this.points << 5 | this.points >>> 27;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
