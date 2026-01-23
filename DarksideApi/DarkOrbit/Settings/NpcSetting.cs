using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Settings.Interfaces;
using DarksideApi.Util;

namespace DarksideApi.DarkOrbit.Settings
{
    public class NpcSetting : Setting, IAttackSetting, IRebuySetting, IPetSetting
    {
        public string? Name { get; set; } = string.Empty;
        public int Config { get; set; } = 1;
        public string DroneFormation { get; set; } = CommandConstants.DRONE_FORMATION_DEFAULT;
        public string LaserId { get; set; } = CommandConstants.AMMO_LCB_10;
        public bool UseRockets { get; set; }
        public string RocketId { get; set; } = CommandConstants.ROCKET_PLT_2026;
        public bool UseRocketLauncher { get; set; }
        public string RocketLauncherId { get; set; } = CommandConstants.ROCKETLAUNCHER_ECO_10;
        public bool UseSAB { get; set; }
        public bool UseRSB { get; set; }
        public double Radius { get; set; } = 550;
        public int Priority { get; set; }
        public int RepairAt { get; set; } = 50;
        public bool UseRebuyLaser { get; set; }
        public bool UseRebuyRocket { get; set; }
        public bool UseRebuyRocketLauncher { get; set; }
        public bool UseCircle { get; set; } = true;
        public bool UseAbility { get; set; } = true;
        public bool OnlyFree { get; set; } = true;
        public bool Finish { get; set; } = false;
        public bool UsePetMode { get; set; } = false;
        public int PetMode { get; set; } = 1;
        public bool UseShieldBelowFifteenFormation { get; set; } = false;
        public string ShieldBelowFifteenFormation { get; set; } = CommandConstants.DRONE_FORMATION_DEFAULT;

        public string DisplayDroneFormation => CommandConstants.DRONE_FORMATIONS.Where(d => d.Value == this.DroneFormation).FirstOrDefault().Key;
        public string DisplayLaser => CommandConstants.LASER_TYPES.Where(l => l.Value == this.LaserId).FirstOrDefault().Key ?? this.LaserId.Split('_').Last() ?? this.LaserId;
        public string DisplayFoundOnMapId
        {
            get
            {
                if (string.IsNullOrEmpty(this.Name))
                {
                    return "Other";
                }
                var mapId = 0;
                foreach (KeyValuePair<int, Dictionary<string, int>> map in Api.NpcIdsByMap)
                {
                    if (!map.Value.ContainsKey(this.Name))
                    {
                        continue;
                    }
                    mapId = map.Key;
                    break;
                }

                return HelpTools.GetMapName(mapId);
            }
        }
        public int FoundOnMapId
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Name))
                {
                    foreach (KeyValuePair<int, Dictionary<string, int>> map in Api.NpcIdsByMap)
                    {
                        if (!map.Value.ContainsKey(this.Name))
                        {
                            continue;
                        }
                        return map.Key;
                    }
                }
                return 999;
            }
        }
        public int FoundOnMaps
        {
            get
            {
                if (HelpTools.IsUpperMap(FoundOnMapId))
                {
                    return 1;
                }
                if (FoundOnMapId <= 16)
                {
                    return 0;
                }
                return 2;
            }
        }
    }
}
