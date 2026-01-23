namespace DarksideApi.DarkOrbit.Settings
{
    public class DispatcherSetting : Setting
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public double Duration { get; set; } = 0;
        public bool InstantFinish { get; set; } = false;
        public int InstantFinishAmount { get; set; } = 1;
        public bool OnlyOnDispatchDay { get; set; } = false;
        public string DurationInMin => (this.Duration / 60) + "Min.";
    }
}
