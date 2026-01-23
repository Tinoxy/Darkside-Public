namespace DarksideApi.DarkOrbit.Commands.Dispatch
{
    public class DispatchActiveEventCommand : Command
    {
        public new const short Id = -22936;
        public EventType eventType;

        public override void Read(ByteArray reader)
        {
            this.eventType = (EventType)reader.ReadShort();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public enum EventType
        {
            NONE,
            DISPATCH_DAY,
            GALAXY_GATE_DAY,
        }
    }
}
