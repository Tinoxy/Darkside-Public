using DarksideApi.DarkOrbit.Commands;

namespace DarksideApi.DarkOrbit.Settings
{
    public class CargoSettings : Setting
    {
        public int Type { get; set; }
        public string TypeName => CommandConstants.ORE_LOOKUP_REVERSE[this.Type];
    }
}
