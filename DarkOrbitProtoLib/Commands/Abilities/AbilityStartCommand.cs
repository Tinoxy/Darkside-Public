namespace DarksideApi.DarkOrbit.Commands.Abilities
{
    public class AbilityStartCommand : Command    //class_1639
    {
        public new const short Id = 279;
        public int selectedAbilityId;   //5297
        public int activatorId; //4852
        public bool noStopCommand;

        public AbilityStartCommand()
        {
        }

        public override void Read(ByteArray reader)
        {
            this.selectedAbilityId = reader.ReadInt();
            this.selectedAbilityId = this.selectedAbilityId >>> 12 % 32 | this.selectedAbilityId << 32 - 12 % 32;
            this.activatorId = reader.ReadInt();
            this.activatorId = this.activatorId << 4 % 32 | this.activatorId >>> 32 - 4 % 32;
            this.noStopCommand = reader.ReadBool();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
