using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Settings;
using DarksideApi.Util;
using DarksideApi.Util.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DarksideApi
{
    public class DarkOrbotSettings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string displayName = "New Account";

        [XmlIgnore, Newtonsoft.Json.JsonIgnore]
        private bool isRunning;
        [XmlIgnore, Newtonsoft.Json.JsonIgnore]
        private bool isLoggedInToHomepage;
        [XmlIgnore, Newtonsoft.Json.JsonIgnore]
        private bool isLoggingIn;
        [XmlIgnore, Newtonsoft.Json.JsonIgnore]
        private bool isInGame;

        private bool isMinimapShown = true;
        private bool isMinimapPlayerView;
        private bool logLogDamage;
        private bool createLog;

        private bool minimapShowBackground;
        private bool minimapShowBoxes = true;
        private bool minimapShowOres;
        private bool minimapShowNpcs = true;
        private bool minimapShowPlayers = true;
        private bool minimapShowGates = true;
        private bool minimapShowBuildings = true;
        private bool minimapShowPet = true;
        private bool minimapShowMines = true;
        private bool minimapShowZones = true;
        private bool minimapShowNpcNames;
        private bool minimapShowPlayerNames;
        private bool minimapShowCurrentInfo = true;
        private bool minimapShowShipInfo = true;
        private bool minimapShowWorkArea = true;
        private bool minimapShowBoosters = true;
        private bool minimapShowQuest = true;
        private bool minimapShowGroup = true;

        private string username = string.Empty;
        private string password = string.Empty;

        [XmlIgnore, Newtonsoft.Json.JsonIgnore]
        private string sessionId = string.Empty;

        private string twoCaptchaApiKey = string.Empty;
        private bool autoLogin;
        private bool autoStart;
        private bool isUnity;
        private bool useProxy;
        private string proxyAddress;
        private string proxyUser;
        private string proxyPassword;

        private bool generalPauseUsePause;
        private double generalPausePauseTime;
        private double generalPauseAfterPlayTime;
        private bool generalPauseAfterFastDeaths;
        private bool generalStopWhenDiminishingQuest;

        private bool generalCollectBoxes = true;
        private bool generalShootNpcs = true;
        private bool generalDoGalaxyGates;
        private bool generalPassiveShootNpcs;

        private string roamFormation = CommandConstants.DRONE_FORMATION_DEFAULT;
        private string repairFormation = CommandConstants.DRONE_FORMATION_DEFAULT;
        private bool repairUseInstantRepair;

        private int roamConfig = 1;
        private int repairConfig = 1;

        private int repairAt = 51;
        private int reviveOption;
        private int reviveSleep = 5;

        private bool autoCloak;
        private bool useFleeFormationForGotoRepair;
        private bool upgradeSkylab;
        private bool buySkylabRobots;
        private bool avoidMines = true;
        private bool avoidCbs = true;

        private bool petUse;
        private int petMode = 1;
        private bool petRepair = true;
        private bool petBuyFuel;
        private bool petUseRepairMode;
        private string petModeOptions = string.Empty;
        private bool petRepairItself = false;

        private bool useMap;
        private int mapSelect;
        private int maxEnemysOnMap;
        private bool useMapMaxDeaths;
        private int mapMaxDeaths;
        private bool mapAvoidEnemys;
        private bool mapUseAdvancedJumpCpu;
        private int mapUseAdvancedJumpCpuMinPathLength = 4;

        private bool onAttackedFlee = true;
        private int onAttackedFleeConfig = 1;
        private string onAttackedFleeFormation = CommandConstants.DRONE_FORMATION_DEFAULT;
        private bool onAttackedFleeUseMine;
        private string onAttackedFleeMine = CommandConstants.EQUIPMENT_MINE_SLM;
        private bool onAttackedFleeIgnorePortalsToPvP;

        private bool onAttackedShootback;
        private int onAttackedShootbackConfig = 1;
        private string onAttackedShootbackFormation = CommandConstants.DRONE_FORMATION_DEFAULT;
        private string onAttackedShootbackLaserType = CommandConstants.AMMO_LCB_10;
        private string onAttackedShootbackRocketType = CommandConstants.ROCKET_PLT_2026;
        private string onAttackedShootbackRocketLauncherType = CommandConstants.ROCKETLAUNCHER_ECO_10;
        private bool onAttackedShootbackUseRocket;
        private bool onAttackedShootbackUseRocketLauncher;
        private bool onAttackedShootbackUseSAB;
        private bool onAttackedShootbackUseRSB;
        private double onAttackedShootbackRadius = 450;
        private bool onAttackedShootbackAutoAttackClanless;
        private bool onAttackedShootbackAutoAttackEnemies;
        private int onAttackedShootbackAutoAttackMaxKills = 5;
        private bool onAttackedShootbackSupportAllies = true;
        private bool onAttackedShootbackUsePetMode;
        private int onAttackedShootbackPetMode = 1;
        private bool onAttackedShootbackUseEmp;
        private bool onAttackedShootbackUseIsh;
        private bool onAttackedShootbackPetKillers;

        private bool useDispatcher;

        private bool groupIsLeader;
        private bool groupAcceptAll;
        private bool groupInviteAll;
        private bool groupDeclineAll;

        private bool oreSellWithHanger;
        private long oreSellHangarId;
        private bool oreSellEverything;
        private bool stopCollectIfPlayersNearby;

        private bool useTaskSystem;

        public string ApiKey { get; private set; } = string.Empty;
        public ObservableKeyedCollection<string, NpcSetting> NpcSettings { get; private set; } = new ObservableKeyedCollection<string, NpcSetting>(x => x.Name, "Name");
        public ObservableKeyedCollection<string, BoxSetting> CollectBoxTypes { get; private set; } = new ObservableKeyedCollection<string, BoxSetting>(x => x.Type, "Type");
        public ObservableCollection<OreSetting> CollectOreTypes { get; private set; } = [];
        public ObservableCollection<CargoSettings> CargoUpgradeSettings { get; private set; } = [];
        public ObservableCollection<LabSetting> LabSettings { get; private set; } = [];
        public ObservableKeyedCollection<string, AuctionSetting> AuctionSettings { get; private set; } = new ObservableKeyedCollection<string, AuctionSetting>(x => x.ItemLoodId, "ItemLoodId");
        //public ObservableCollection<MapAreaSetting> MapAreaSettings { get; private set; } = [];   //ItemsSource="{Binding api.Settings.MapAreaSettings}"
        public ObservableCollection<AbilitySetting> AbilitySettings { get; private set; } = [];
        public ObservableKeyedCollection<int, GalaxyGateSetting> GalaxyGateSettings { get; private set; } = new ObservableKeyedCollection<int, GalaxyGateSetting>(x => x.GalaxyGateId, "GalaxyGateId");
        public ObservableCollection<GroupSetting> GroupSettings { get; private set; } = [];
        public ObservableKeyedCollection<int, DispatcherSetting> DispatcherSettings { get; private set; } = new ObservableKeyedCollection<int, DispatcherSetting>(x => x.Id, "Id");
        public ObservableCollection<TaskSetting> TaskSettings { get; private set; } = [];
        public ObservableKeyedCollection<int, WorkAreaSetting> WorkAreaSettings { get; private set; } = new ObservableKeyedCollection<int, WorkAreaSetting>(x => x.MapId, "MapId");
        public ObservableKeyedCollection<string, WorkMapSetting> WorkMapSettings { get; private set; } = new ObservableKeyedCollection<string, WorkMapSetting>(x => x.StartTimeUTCValue, "StartTimeUTCValue");

        public string DisplayName { get => this.displayName; set { this.displayName = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.DisplayName))); } }
        [XmlIgnore, Newtonsoft.Json.JsonIgnore]
        public bool IsRunning { get => this.isRunning; set { this.isRunning = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IsRunning))); } }
        [XmlIgnore, Newtonsoft.Json.JsonIgnore]
        public bool IsLoggedInToHomepage { get => this.isLoggedInToHomepage; set { this.isLoggedInToHomepage = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IsLoggedInToHomepage))); } }
        [XmlIgnore, Newtonsoft.Json.JsonIgnore]
        public bool IsLoggingIn { get => this.isLoggingIn; set { this.isLoggingIn = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IsLoggingIn))); } }
        [XmlIgnore, Newtonsoft.Json.JsonIgnore]
        public bool IsInGame { get => this.isInGame; set { this.isInGame = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IsInGame))); } }

        public bool IsMinimapShown { get => this.isMinimapShown; set { this.isMinimapShown = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IsMinimapShown))); } }
        public bool IsMinimapPlayerView { get => this.isMinimapPlayerView; set { this.isMinimapPlayerView = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IsMinimapPlayerView))); } }
        public bool LogLogDamage { get => this.logLogDamage; set { this.logLogDamage = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.LogLogDamage))); } }
        public bool CreateLog { get => this.createLog; set { this.createLog = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CreateLog))); } }

        public bool MinimapShowBackground { get => this.minimapShowBackground; set { this.minimapShowBackground = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MinimapShowBackground))); } }
        public bool MinimapShowBoxes { get => this.minimapShowBoxes; set { this.minimapShowBoxes = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MinimapShowBoxes))); } }
        public bool MinimapShowOres { get => this.minimapShowOres; set { this.minimapShowOres = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MinimapShowOres))); } }
        public bool MinimapShowNpcs { get => this.minimapShowNpcs; set { this.minimapShowNpcs = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MinimapShowNpcs))); } }
        public bool MinimapShowPlayers { get => this.minimapShowPlayers; set { this.minimapShowPlayers = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MinimapShowPlayers))); } }
        public bool MinimapShowGates { get => this.minimapShowGates; set { this.minimapShowGates = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MinimapShowGates))); } }
        public bool MinimapShowBuildings { get => this.minimapShowBuildings; set { this.minimapShowBuildings = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MinimapShowBuildings))); } }
        public bool MinimapShowPet { get => this.minimapShowPet; set { this.minimapShowPet = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MinimapShowPet))); } }
        public bool MinimapShowMines { get => this.minimapShowMines; set { this.minimapShowMines = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MinimapShowMines))); } }
        public bool MinimapShowZones { get => this.minimapShowZones; set { this.minimapShowZones = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MinimapShowZones))); } }
        public bool MinimapShowNpcNames { get => this.minimapShowNpcNames; set { this.minimapShowNpcNames = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MinimapShowNpcNames))); } }
        public bool MinimapShowPlayerNames { get => this.minimapShowPlayerNames; set { this.minimapShowPlayerNames = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MinimapShowPlayerNames))); } }
        public bool MinimapShowShipInfo { get => this.minimapShowShipInfo; set { this.minimapShowShipInfo = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MinimapShowShipInfo))); } }
        public bool MinimapShowCurrentInfo { get => this.minimapShowCurrentInfo; set { this.minimapShowCurrentInfo = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MinimapShowCurrentInfo))); } }
        public bool MinimapShowWorkArea { get => this.minimapShowWorkArea; set { this.minimapShowWorkArea = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MinimapShowWorkArea))); } }
        public bool MinimapShowBoosters { get => this.minimapShowBoosters; set { this.minimapShowBoosters = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MinimapShowBoosters))); } }
        public bool MinimapShowQuest { get => this.minimapShowQuest; set { this.minimapShowQuest = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MinimapShowQuest))); } }
        public bool MinimapShowGroup { get => this.minimapShowGroup; set { this.minimapShowGroup = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MinimapShowGroup))); } }

        public string Username { get => this.username; set { this.username = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Username))); } }
        public string Password { get => this.password; set { this.password = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Password))); } }
        public string SessionId { get => this.sessionId; set { this.sessionId = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.SessionId))); } }
        public string TwoCaptchaApiKey { get => this.twoCaptchaApiKey; set { this.twoCaptchaApiKey = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.TwoCaptchaApiKey))); } }
        public bool AutoLogin { get => this.autoLogin; set { this.autoLogin = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.AutoLogin))); } }
        public bool AutoStart { get => this.autoStart; set { this.autoStart = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.AutoStart))); } }
        public bool IsUnity { get => this.isUnity; set { this.isUnity = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IsUnity))); } }
        public bool UseProxy { get => this.useProxy; set { this.useProxy = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.UseProxy))); } }
        public string ProxyAddress { get => this.proxyAddress; set { this.proxyAddress = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.ProxyAddress))); } }
        public string ProxyUser { get => this.proxyUser; set { this.proxyUser = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.ProxyUser))); } }
        public string ProxyPassword { get => this.proxyPassword; set { this.proxyPassword = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.ProxyPassword))); } }

        public bool GeneralPauseUsePause { get => this.generalPauseUsePause; set { this.generalPauseUsePause = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.GeneralPauseUsePause))); } }
        public double GeneralPausePauseTime { get => this.generalPausePauseTime; set { this.generalPausePauseTime = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.GeneralPausePauseTime))); } }
        public double GeneralPauseAfterPlayTime { get => this.generalPauseAfterPlayTime; set { this.generalPauseAfterPlayTime = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.GeneralPauseAfterPlayTime))); } }
        public bool GeneralPauseAfterFastDeaths { get => this.generalPauseAfterFastDeaths; set { this.generalPauseAfterFastDeaths = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.GeneralPauseAfterFastDeaths))); } }
        public bool GeneralStopWhenDiminishingQuest { get => this.generalStopWhenDiminishingQuest; set { this.generalStopWhenDiminishingQuest = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.GeneralStopWhenDiminishingQuest))); } }

        public bool GeneralCollectBoxes { get => this.generalCollectBoxes; set { this.generalCollectBoxes = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.GeneralCollectBoxes))); } }
        public bool GeneralShootNpcs { get => this.generalShootNpcs; set { this.generalShootNpcs = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.GeneralShootNpcs))); } }
        public bool GeneralDoGalaxyGates { get => this.generalDoGalaxyGates; set { this.generalDoGalaxyGates = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.GeneralDoGalaxyGates))); } }
        public bool GeneralPassiveShootNpcs { get => this.generalPassiveShootNpcs; set { this.generalPassiveShootNpcs = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.GeneralPassiveShootNpcs))); } }

        public string RoamFormation { get => this.roamFormation; set { this.roamFormation = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.RoamFormation))); } }
        public string RepairFormation { get => this.repairFormation; set { this.repairFormation = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.RepairFormation))); } }
        public bool RepairUseInstantRepair { get => this.repairUseInstantRepair; set { this.repairUseInstantRepair = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.RepairUseInstantRepair))); } }

        public int RepairConfig { get => this.repairConfig; set { this.repairConfig = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.RepairConfig))); } }
        public int RoamConfig { get => this.roamConfig; set { this.roamConfig = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.RoamConfig))); } }

        public int RepairAt { get => this.repairAt; set { this.repairAt = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.RepairAt))); } }
        public int ReviveOption { get => this.reviveOption; set { this.reviveOption = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.ReviveOption))); } }
        public int ReviveSleep { get => this.reviveSleep; set { this.reviveSleep = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.ReviveSleep))); } }

        public bool GeneralAutoCloak { get => this.autoCloak; set { this.autoCloak = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.GeneralAutoCloak))); } }
        public bool UseFleeFormationForGotoRepair { get => this.useFleeFormationForGotoRepair; set { this.useFleeFormationForGotoRepair = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.UseFleeFormationForGotoRepair))); } }
        public bool GeneralUpgradeSkylab { get => this.upgradeSkylab; set { this.upgradeSkylab = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.GeneralUpgradeSkylab))); } }
        public bool GeneralBuySkylabRobots { get => this.buySkylabRobots; set { this.buySkylabRobots = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.GeneralBuySkylabRobots))); } }
        public bool GeneralAvoidMines { get => this.avoidMines; set { this.avoidMines = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.GeneralAvoidMines))); } }
        public bool GeneralAvoidCbs { get => this.avoidCbs; set { this.avoidCbs = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.GeneralAvoidCbs))); } }

        public bool PetUse { get => this.petUse; set { this.petUse = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.PetUse))); } }
        public int PetMode { get => this.petMode; set { this.petMode = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.PetMode))); } }
        public bool PetRepair { get => this.petRepair; set { this.petRepair = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.PetRepair))); } }
        public bool PetBuyFuel { get => this.petBuyFuel; set { this.petBuyFuel = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.PetBuyFuel))); } }
        public bool PetUseRepairMode { get => this.petUseRepairMode; set { this.petUseRepairMode = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.PetUseRepairMode))); } }
        public string PetModeOptions { get => this.petModeOptions; set { this.petModeOptions = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.PetModeOptions))); } }
        public bool PetRepairItself { get => this.petRepairItself; set { this.petRepairItself = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.PetRepairItself))); } }

        public bool UseMap { get => this.useMap; set { this.useMap = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.UseMap))); } }
        public int MapSelect { get => this.mapSelect; set { this.mapSelect = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MapSelect))); } }
        public int MaxEnemysOnMap { get => this.maxEnemysOnMap; set { this.maxEnemysOnMap = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MaxEnemysOnMap))); } }
        public bool UseMapMaxDeaths { get => this.useMapMaxDeaths; set { this.useMapMaxDeaths = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.UseMapMaxDeaths))); } }
        public int MapMaxDeaths { get => this.mapMaxDeaths; set { this.mapMaxDeaths = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MapMaxDeaths))); } }
        public bool MapAvoidEnemys { get => this.mapAvoidEnemys; set { this.mapAvoidEnemys = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MapAvoidEnemys))); } }
        public bool MapUseAdvancedJumpCpu { get => this.mapUseAdvancedJumpCpu; set { this.mapUseAdvancedJumpCpu = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MapUseAdvancedJumpCpu))); } }
        public int MapUseAdvancedJumpCpuMinPathLength { get => this.mapUseAdvancedJumpCpuMinPathLength; set { this.mapUseAdvancedJumpCpuMinPathLength = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MapUseAdvancedJumpCpuMinPathLength))); } }

        public bool OnAttackedFlee { get => this.onAttackedFlee; set { this.onAttackedFlee = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OnAttackedFlee))); } }
        public int OnAttackedFleeConfig { get => this.onAttackedFleeConfig; set { this.onAttackedFleeConfig = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OnAttackedFleeConfig))); } }
        public string OnAttackedFleeFormation { get => this.onAttackedFleeFormation; set { this.onAttackedFleeFormation = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OnAttackedFleeFormation))); } }
        public bool OnAttackedFleeUseMine { get => this.onAttackedFleeUseMine; set { this.onAttackedFleeUseMine = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OnAttackedFleeUseMine))); } }
        public string OnAttackedFleeMine { get => this.onAttackedFleeMine; set { this.onAttackedFleeMine = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OnAttackedFleeMine))); } }
        public bool OnAttackedIgnorePortalsToPvP { get => this.onAttackedFleeIgnorePortalsToPvP; set { this.onAttackedFleeIgnorePortalsToPvP = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OnAttackedIgnorePortalsToPvP))); } }

        public bool OnAttackedShootback { get => this.onAttackedShootback; set { this.onAttackedShootback = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OnAttackedShootback))); } }
        public int OnAttackedShootbackConfig { get => this.onAttackedShootbackConfig; set { this.onAttackedShootbackConfig = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OnAttackedShootbackConfig))); } }
        public string OnAttackedShootbackFormation { get => this.onAttackedShootbackFormation; set { this.onAttackedShootbackFormation = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OnAttackedShootbackFormation))); } }
        public string OnAttackedShootbackLaserType { get => this.onAttackedShootbackLaserType; set { this.onAttackedShootbackLaserType = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OnAttackedShootbackLaserType))); } }
        public string OnAttackedShootbackRocketType { get => this.onAttackedShootbackRocketType; set { this.onAttackedShootbackRocketType = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OnAttackedShootbackRocketType))); } }
        public string OnAttackedShootbackRocketLauncherType { get => this.onAttackedShootbackRocketLauncherType; set { this.onAttackedShootbackRocketLauncherType = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OnAttackedShootbackRocketLauncherType))); } }
        public bool OnAttackedShootbackUseRocket { get => this.onAttackedShootbackUseRocket; set { this.onAttackedShootbackUseRocket = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OnAttackedShootbackUseRocket))); } }
        public bool OnAttackedShootbackUseRocketLauncher { get => this.onAttackedShootbackUseRocketLauncher; set { this.onAttackedShootbackUseRocketLauncher = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OnAttackedShootbackUseRocketLauncher))); } }
        public bool OnAttackedShootbackUseSAB { get => this.onAttackedShootbackUseSAB; set { this.onAttackedShootbackUseSAB = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OnAttackedShootbackUseSAB))); } }
        public bool OnAttackedShootbackUseRSB { get => this.onAttackedShootbackUseRSB; set { this.onAttackedShootbackUseRSB = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OnAttackedShootbackUseRSB))); } }
        public double OnAttackedShootbackRadius { get => this.onAttackedShootbackRadius; set { this.onAttackedShootbackRadius = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OnAttackedShootbackRadius))); } }
        public bool OnAttackedShootbackAutoAttackClanless { get => this.onAttackedShootbackAutoAttackClanless; set { this.onAttackedShootbackAutoAttackClanless = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OnAttackedShootbackAutoAttackClanless))); } }
        public bool OnAttackedShootbackAutoAttackEnemies { get => this.onAttackedShootbackAutoAttackEnemies; set { this.onAttackedShootbackAutoAttackEnemies = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OnAttackedShootbackAutoAttackEnemies))); } }
        public int OnAttackedShootbackAutoAttackMaxKills { get => this.onAttackedShootbackAutoAttackMaxKills; set { this.onAttackedShootbackAutoAttackMaxKills = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OnAttackedShootbackAutoAttackMaxKills))); } }
        public bool OnAttackedShootbackSupportAllies { get => this.onAttackedShootbackSupportAllies; set { this.onAttackedShootbackSupportAllies = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OnAttackedShootbackSupportAllies))); } }
        public bool OnAttackedShootbackUsePetMode { get => this.onAttackedShootbackUsePetMode; set { this.onAttackedShootbackUsePetMode = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OnAttackedShootbackUsePetMode))); } }
        public int OnAttackedShootbackPetMode { get => this.onAttackedShootbackPetMode; set { this.onAttackedShootbackPetMode = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OnAttackedShootbackPetMode))); } }
        public bool OnAttackedShootbackUseEMP { get => this.onAttackedShootbackUseEmp; set { this.onAttackedShootbackUseEmp = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OnAttackedShootbackUseEMP))); } }
        public bool OnAttackedShootbackUseISH { get => this.onAttackedShootbackUseIsh; set { this.onAttackedShootbackUseIsh = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OnAttackedShootbackUseISH))); } }
        public bool OnAttackedShootPetKillers { get => this.onAttackedShootbackPetKillers; set { this.onAttackedShootbackPetKillers = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OnAttackedShootPetKillers))); } }

        public bool UseDispatcher { get => this.useDispatcher; set { this.useDispatcher = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.UseDispatcher))); } }

        public bool GroupIsLeader { get => this.groupIsLeader; set { this.groupIsLeader = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.GroupIsLeader))); } }
        public bool GroupAcceptAll { get => this.groupAcceptAll; set { this.groupAcceptAll = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.GroupAcceptAll))); } }
        public bool GroupInviteAll { get => this.groupInviteAll; set { this.groupInviteAll = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.GroupInviteAll))); } }
        public bool GroupDeclineAll { get => this.groupDeclineAll; set { this.groupDeclineAll = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.GroupDeclineAll))); } }

        public bool OreSellWithHanger { get => this.oreSellWithHanger; set { this.oreSellWithHanger = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OreSellWithHanger))); } }
        public long OreSellHangarId { get => this.oreSellHangarId; set { this.oreSellHangarId = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OreSellHangarId))); } }
        public bool OreSellEverything { get => this.oreSellEverything; set { this.oreSellEverything = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OreSellEverything))); } }
        public bool StopCollectIfPlayersNearby { get => this.stopCollectIfPlayersNearby; set { this.stopCollectIfPlayersNearby = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.StopCollectIfPlayersNearby))); } }

        public bool UseTaskSystem { get => this.useTaskSystem; set { this.useTaskSystem = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.UseTaskSystem))); } }

        public DarkOrbotSettings()
        {
            this.GenerateKey();
        }

        public void GenerateKey()
        {
            this.ApiKey = HelpTools.RandomString(16);
        }
    }
}
