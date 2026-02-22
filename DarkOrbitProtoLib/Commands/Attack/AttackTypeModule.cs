namespace DarksideApi.DarkOrbit.Commands.Attack
{
    public class AttackTypeModule : Command
    {
        public new const short Id = 27;
        public AttackType attackType;

        public override void Read(ByteArray reader)
        {
            this.attackType = (AttackType)reader.ReadShort();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public enum AttackType
        {
            Rocket,
            Laser,
            Mine,
            Radiation,
            Plasma,
            ECI,
            SL,
            CID,
            Singularity,
            Kamikaze,
            Repair,
            Deceleraion,
            SHIELD_ABSORBER_ROCKET_CREDITS,
            SHIELD_ABSORBER_ROCKET_URIDIUM,
            STICKY_BOMB,
            Polarity,
            Incinerate,
            Spread,
            SLOW_AND_BLIND_ATTACK,
            HEAT_RELEASE
        }
    }
}
