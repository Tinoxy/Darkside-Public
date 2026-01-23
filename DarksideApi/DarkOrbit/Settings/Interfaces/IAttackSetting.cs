namespace DarksideApi.DarkOrbit.Settings.Interfaces
{
    public interface IAttackSetting
    {
        public int Config { get; set; }
        public string DroneFormation { get; set; }
        public string LaserId { get; set; }
        public bool UseRockets { get; set; }
        public string RocketId { get; set; }
        public bool UseRocketLauncher { get; set; }
        public string RocketLauncherId { get; set; }
        public bool UseSAB { get; set; }
        public bool UseRSB { get; set; }
        public double Radius { get; set; }
        public bool UseCircle { get; set; }
        public bool UseAbility { get; set; }
        public bool OnlyFree { get; set; }
        public bool Finish { get; set; }
        public int Priority { get; set; }
    }
}
