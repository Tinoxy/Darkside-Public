namespace DarksideApi.DarkOrbit.Commands.Abilities
{
    public class AbilityEffectActivationCommand : Command
    {
        public new const short Id = 275;
        public int selectedAbilityId;
        public int activatorId;
        public bool isPresistent;
        public List<int> targetIds;

        public AbilityEffectActivationCommand()
        {
            targetIds = [];
        }

        public override void Read(ByteArray reader)
        {
            this.selectedAbilityId = reader.ReadInt();
            this.selectedAbilityId = this.selectedAbilityId >>> 29 | this.selectedAbilityId << 3;
            this.activatorId = reader.ReadInt();
            this.activatorId = this.activatorId << 21 | this.activatorId >>> 11;
            this.isPresistent = reader.ReadBool();
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
