using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Settings.Interfaces;

namespace DarksideApi.DarkOrbit.Settings
{
    public class GalaxyGateSetting : Setting, IAttackSetting, IRebuySetting, IPetSetting
    {
        public int GalaxyGateId { get; set; }
        public string GalaxyGateName { get; set; } = "GG";
        public double Radius { get; set; } = 600;
        public string LaserId { get; set; } = CommandConstants.AMMO_LCB_10;
        public bool UseRebuyLaser { get; set; }
        public bool UseRockets { get; set; }
        public string RocketId { get; set; } = CommandConstants.ROCKET_PLT_2021;
        public bool UseRebuyRocket { get; set; }
        public bool UseRocketLauncher { get; set; }
        public string RocketLauncherId { get; set; } = CommandConstants.ROCKETLAUNCHER_ECO_10;
        public bool UseRebuyRocketLauncher { get; set; }
        public bool UseSAB { get; set; }
        public bool UseRSB { get; set; }
        public int Config { get; set; } = 1;
        public int RepairConfig { get; set; } = 1;
        public string DroneFormation { get; set; } = CommandConstants.DRONE_FORMATION_DEFAULT;
        public string RepairFormation { get; set; } = CommandConstants.DRONE_FORMATION_DEFAULT;
        public string CornerFormation { get; set; } = CommandConstants.DRONE_FORMATION_DEFAULT;
        public bool UseCircle { get; set; } = true;
        public bool UseAbility { get; set; } = true;
        public bool OnlyFree { get; set; } = false;
        public bool Finish { get; set; } = true;
        public bool CollectBoxes { get; set; }
        public int MaxDeaths { get; set; } = 4;
        public bool SpinGenerator { get; set; }
        public byte SpinAmount { get; set; } = 1;
        public byte SpinGateId { get; set; }
        public bool SpinGateWithUri { get; set; }
        public bool SpinPlaceGate { get; set; }
        public bool CircleWhileRepairing { get; set; }
        public string FinishFormation { get; set; } = CommandConstants.DRONE_FORMATION_DEFAULT;
        public int StopAtWave { get; set; } = -1;
        public bool UsePetMode { get; set; } = false;
        public int PetMode { get; set; } = 1;
        public int Priority { get; set; } = 0;
    }
}
