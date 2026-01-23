namespace DarksideApi.DarkOrbit.Commands.Abilities
{
    public class AbilityEffectDeActivationCommand : Command
    {
        public new const short Id = 282;
        public int selectedAbilityId;
        public int activatorId;
        public List<int> targetIds;

        public AbilityEffectDeActivationCommand()
        {
            targetIds = [];
        }

        public override void Read(ByteArray reader)
        {
            this.selectedAbilityId = reader.ReadInt();
            this.selectedAbilityId = this.selectedAbilityId << 7 | this.selectedAbilityId >>> 25;
            this.activatorId = reader.ReadInt();
            this.activatorId = this.activatorId >>> 15 | this.activatorId << 17;
            var i = 0;
            var max = reader.ReadByte();
            while (i < max)
            {
                this.targetIds.Add(reader.ReadInt());
                i++;
            }
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
