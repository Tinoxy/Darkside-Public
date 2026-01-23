using DarksideApi.DarkOrbit.Commands;

namespace DarksideApi.DarkOrbit.Settings
{
    public class LabSetting : Setting
    {
        public int Type { get; set; }
        public int UsingOreType { get; set; }
        public string OreName => CommandConstants.ORE_LOOKUP_REVERSE[this.UsingOreType];
        public string TypeName => CommandConstants.LAB_TYPE_LOOKUP[this.Type];
    }
}
