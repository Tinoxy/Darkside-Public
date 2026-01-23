namespace DarksideApi.DarkOrbit.Commands.Action
{
    public class EggTriggerRequest : Command
    {
        public new const short Id = -29989;
        public EggTriggerType eggTriggerType;

        public EggTriggerRequest(EggTriggerType eggTriggerType)
        {
            this.eggTriggerType = eggTriggerType;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteShort((short)this.eggTriggerType);
            return this.@byte.ToArray();
        }

        public enum EggTriggerType
        {
            MINIMAP_PING = 0,
            GOING_ROUND_THE_MAP = 1,
            GALAXY_GATE_FIREWORK = 2,
            RAINBOW_LASER = 3
        }
    }
}
