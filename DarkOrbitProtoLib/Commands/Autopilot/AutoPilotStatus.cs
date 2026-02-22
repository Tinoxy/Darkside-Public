namespace DarksideApi.DarkOrbit.Commands.Autopilot
{
    public class AutoPilotStatus : Command
    {
        public new const short Id = -31192
            ;
        public Auto_Pilot_Status status;

        public AutoPilotStatus(Auto_Pilot_Status status)
        {
            this.status = status;
        }

        public AutoPilotStatus()
        {
        }

        public override void Read(ByteArray reader)
        {
            this.status = (Auto_Pilot_Status)reader.ReadShort();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteShort((short)status);
            return this.@byte.ToArray();
        }

        public enum Auto_Pilot_Status
        {
            START = 0,
            STOP = 1
        }
    }
}
