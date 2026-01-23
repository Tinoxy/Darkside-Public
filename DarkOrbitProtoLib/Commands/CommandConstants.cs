using DarksideApi.DarkOrbit.Commands.GalaxyGates;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using static DarksideApi.DarkOrbit.Commands.EternalBlackLight.EternalBlacklightBoosterCommand;

namespace DarksideApi.DarkOrbit.Commands
{
    public class CommandConstants : INotifyPropertyChanged
    {
        public const int MAP_WIDTH_NORMAL = 21000;
        public const int MAP_HEIGHT_NORMAL = 13100;
        public static readonly int MAP_WIDTH_LARGE = MAP_WIDTH_NORMAL * 2;
        public static readonly int MAP_HEIGHT_LARGE = MAP_HEIGHT_NORMAL * 2;
        public static readonly int MAP_WIDTH_SMALL = MAP_WIDTH_NORMAL / 2;
        public static readonly int MAP_HEIGHT_SMALL = MAP_HEIGHT_NORMAL / 2;

        public const int STATUS_DEFAULT = 0;
        public const int STATUS_ERROR = 1;
        public const int STATUS_SHUTDOWN = 2;
        public const int STATUS_INVALID_DATA = 3;
        public const int STATUS_WRONG_INSTANCEID = 4;
        public const int STATUS_WRONGSERVER = 5;
        public const int STATUS_INVALID_SID = 6;
        public const int STATUS_SHIP_DESTROYED = 7;
        public const int STATUS_LOGGED_OUT = 8;
        public const int STATUS_IP_LIMIT = 9;
        public const int STATUS_MAX = 10;

        public const int PET_LAUNCH = 0;
        public const int PET_DEACTIVATE = 1;
        public const int PET_TOGGLE_ACTIVATION = 2;
        public const int PET_HOTKEY_GUARD = 3;
        public const int PET_DESTROYED_REPAIR = 4;
        public const int PET_HOTKEY_REPAIR = 5;

        public const int PET_GEAR_BEHAVIOR = 0;
        public const int PET_GEAR_PASSIVE = 1;
        public const int PET_GEAR_GUARD = 2;
        public const int PET_GEAR_GEAR = 3;
        public const int PET_GEAR_AUTO_LOOT = 4;
        public const int PET_GEAR_AUTO_RESOURCE_COLLECTION = 5;
        public const int PET_GEAR_ENEMY_LOCATOR = 6;
        public const int PET_GEAR_RESOURCE_LOCATOR = 7;
        public const int PET_GEAR_TRADE_POD = 8;
        public const int PET_GEAR_REPAIR_PET = 9;
        public const int PET_GEAR_KAMIKAZE = 10;
        public const int PET_GEAR_COMBO_SHIP_REPAIR = 11;
        public const int PET_GEAR_COMBO_GUARD = 12;
        public const int PET_GEAR_ADMIN = 13;
        public const int PET_GEAR_OPEN_FLAME = 14;
        public const int PET_GEAR_HEAT_RELEASE = 15;

        public const int PET_STATUS_DEAD = 0;
        public const int PET_STATUS_KAMIKAZE_USED = 1;
        public const int PET_STATUS_MAX = 2;

        public const int CLAN_RELATION_NONE = 0;
        public const int CLAN_RELATION_ALLIED = 1;
        public const int CLAN_RELATION_NON_AGGRESSION_PACT = 2;
        public const int CLAN_RELATION_AT_WAR = 3;

        public const string DRONE_FORMATION_DEFAULT = "drone_formation_default";
        public const string DRONE_FORMATION_3DX = "drone_formation_f-3d-x";
        public const string DRONE_FORMATION_ARROW = "drone_formation_f-02-ar";
        public const string DRONE_FORMATION_BARRAGE = "drone_formation_f-12-ba";
        public const string DRONE_FORMATION_BAT = "drone_formation_f-13-bt";
        public const string DRONE_FORMATION_CHEVRON = "drone_formation_f-08-ch";
        public const string DRONE_FORMATION_DOUBLEARROW = "drone_formation_f-06-da";
        public const string DRONE_FORMATION_DIAMOND = "drone_formation_f-07-di";
        public const string DRONE_FORMATION_HEART = "drone_formation_f-11-he";
        public const string DRONE_FORMATION_LANCE = "drone_formation_f-03-la";
        public const string DRONE_FORMATION_MOTH = "drone_formation_f-09-mo";
        public const string DRONE_FORMATION_PINCER = "drone_formation_f-05-pi";
        public const string DRONE_FORMATION_STAR = "drone_formation_f-04-st";
        public const string DRONE_FORMATION_TURTLE = "drone_formation_f-01-tu";
        public const string DRONE_FORMATION_CRAB = "drone_formation_f-10-cr";
        public const string DRONE_FORMATION_RING = "drone_formation_f-3d-rg";//"drone_formation_f-14-rg";
        public const string DRONE_FORMATION_DRILL = "drone_formation_f-3d-dr";//"drone_formation_f-15-dl";
        public const string DRONE_FORMATION_DOME = "drone_formation_f-3d-dm";//"drone_formation_f-17-dm";
        public const string DRONE_FORMATION_VETERAN = "drone_formation_f-3d-vt";//"drone_formation_f-16-vt";
        public const string DRONE_FORMATION_DOUBLEWHEEL = "drone_formation_f-3d-wl";//"drone_formation_f-18-dw";
        public const string DRONE_FORMATION_WAVE = "drone_formation_f-3d-wv";//"drone_formation_f-20-wv";

        public const string EQUIPMENT_REPAIR = "equipment_extra_repbot_rep";
        public const string EQUIPMENT_CLOAK = "equipment_extra_cpu_cl04k";
        public const string EQUIPMENT_AIM = "equipment_extra_cpu_aim";
        public const string EQUIPMENT_ROCKETLAUNCHER = "equipment_weapon_rocketlauncher_hst";
        public const string EQUIPMENT_AUTO_ROCKET = "equipment_extra_cpu_arol";
        public const string EQUIPMENT_AUTO_LAUNCHER = "equipment_extra_cpu_rllb";
        public const string EQUIPMENT_EBG_CPU = "ammunition_ggportal_eternal-blacklight-cpu";
        public const string EQUIPMENT_QZ_CPU = "ammunition_ggportal_quarantine-zone-cpu";
        public const string EQUIPMENT_MINE_ACM = "ammunition_mine_acm-01";
        public const string EQUIPMENT_MINE_DDM = "ammunition_mine_dd-m01";
        public const string EQUIPMENT_MINE_EMP = "ammunition_mine_emp-m01";
        public const string EQUIPMENT_MINE_SAB = "ammunition_mine_sab-m01";
        public const string EQUIPMENT_MINE_SLM = "ammunition_mine_sl-m01";
        public const string EQUIPMENT_MINE_SMB = "ammunition_mine_smb-01";
        public const string EQUIPMENT_MINE_IMB = "ammunition_mine_im-01";
        public const string EQUIPMENT_EMP = "ammunition_specialammo_emp-01";
        public const string EQUIPMENT_ISH = "equipment_extra_cpu_ish-01";

        public const string ROCKETLAUNCHER_HSTRM_01 = "ammunition_rocketlauncher_hstrm-01";
        public const string ROCKETLAUNCHER_UBR_100 = "ammunition_rocketlauncher_ubr-100";
        public const string ROCKETLAUNCHER_ECO_10 = "ammunition_rocketlauncher_eco-10";
        public const string ROCKETLAUNCHER_SAR_01 = "ammunition_rocketlauncher_sar-01";
        public const string ROCKETLAUNCHER_SAR_02 = "ammunition_rocketlauncher_sar-02";
        public const string ROCKETLAUNCHER_CBR = "ammunition_rocketlauncher_cbr";
        public const string ROCKETLAUNCHER_PIR_100 = "ammunition_rocketlauncher_pir-100";

        public const string AMMO_LCB_10 = "ammunition_laser_lcb-10";
        public const string AMMO_MCB_25 = "ammunition_laser_mcb-25";
        public const string AMMO_MCB_50 = "ammunition_laser_mcb-50";
        public const string AMMO_UCB_100 = "ammunition_laser_ucb-100";
        public const string AMMO_SAB_50 = "ammunition_laser_sab-50";
        public const string AMMO_RSB = "ammunition_laser_rsb-75";
        public const string AMMO_CC_C = "ammunition_laser_cc-c";
        public const string AMMO_A_BL = "ammunition_laser_a-bl";

        public const string ROCKET_PLT_2026 = "ammunition_rocket_plt-2026";
        public const string ROCKET_R_310 = "ammunition_rocket_r-310";
        public const string ROCKET_PLT_2021 = "ammunition_rocket_plt-2021";
        public const string ROCKET_PLT_3030 = "ammunition_rocket_plt-3030";
        public const string ROCKET_PLD_8 = "ammunition_rocket_pld-8";
        public const string ROCKET_DCR_250 = "ammunition_rocket_dcr-250";
        public const string ROCKET_AGT_500 = "ammunition_rocket_agt-500";
        public const string ROCKET_WIZ_X = "ammunition_rocket_wiz-x";

        public const string AMMO_SPECIAL_WIZ_X = "ammunition_specialammo_wiz-x";

        public static string BUY_AMMO_LCB_10 = "buy_" + AMMO_LCB_10;
        public static string BUY_AMMO_MCB_25 = "buy_" + AMMO_MCB_25;
        public static string BUY_AMMO_MCB_50 = "buy_" + AMMO_MCB_50;
        public static string BUY_AMMO_UCB_100 = "buy_" + AMMO_UCB_100;
        public static string BUY_AMMO_SAB_50 = "buy_" + AMMO_SAB_50;
        public static string BUY_ROCKET_R_310 = "buy_" + ROCKET_R_310;
        public static string BUY_ROCKET_PLT_2026 = "buy_" + ROCKET_PLT_2026;
        public static string BUY_ROCKET_PLT_2021 = "buy_" + ROCKET_PLT_2021;
        public static string BUY_ROCKET_PLT_3030 = "buy_" + ROCKET_PLT_3030;
        public const string BUY_PET_FUEL = "buy_resource_pet-fuel";

        public const string BOX_ASTRAL = "ASTRAL_BOOTY_BOX";
        public const string BOX_ASTRAL_PRIME = "ASTRAL_PRIME_BOOTY_BOX";
        public const string BOX_BONUS = "BONUS_BOX";
        public const string BOX_SOLAR = "SOLAR_CLASH";
        public const string BOX_BATTLEPASS = "BATTLEPASS";
        public const string BOX_SHIP = "FROM_SHIP";
        public const string BOX_MUCOSUM = "MUCOSUM";
        public const string BOX_SCRAP = "SCRAPIUM";
        public const string BOX_PRISMATIUM = "PRISMAIUM";
        public const string BOX_SHIP_BLOCKED = "FROM_SHIP_BLOCKED";
        public const string BOX_PIRATE_RED = "PIRATE_BOOTY_RED";
        public const string BOX_PIRATE_BLUE = "PIRATE_BOOTY_BLUE";
        public const string BOX_PIRATE_DEFAULT = "PIRATE_BOOTY";
        public const string BOX_PIRATE_GOLD = "PIRATE_BOOTY_GOLD";
        public const string BOX_PIRATE_SILVER = "PIRATE_BOOTY_SILVER";
        public const string BOX_CAPTACHA_BLACK = "POISON_PUSAT_BOX_BLACK";
        public const string BOX_CAPTACHA_RED = "BONUS_BOX_RED";
        public const string BOX_PIRATE_BLACK = "BLACK_BOOTY_BOX";
        public const string BOX_ALIEN_EGG = "ALIEN_EGG";
        public const string BOX_PIRATE_EGG = "LUCENT_ALIEN_EGG_BOOTY_BOX";
        public const string BOX_CHAROPOS = "CHAROPOS_BOX";
        public const string BOX_AGATUS = "AGATUS_BOX";
        public const string BOX_STYXUS = "STYXUS_BOX";
        public const string BOX_EMPYRIAN = "EMPYRIAN_BOOTY_BOX";
        public const string BOX_PROMETHEUS = "PROMETHEUS_BOOTY_BOX";
        public const string BOX_LAUREL = "LAUREL_BOOTY_BOX";
        public const string BOX_PROSPEROUS = "PROSPEROUS_BOOTY_BOX";
        public const string CANDY_CARGO = "CANDY_CARGO";

        public const string ORE_PROMETIUM = "ore_prometium";
        public const string ORE_ENDURIUM = "ore_endurium";
        public const string ORE_TERBIUM = "ore_terbium";
        public const string ORE_XENOMIT = "ore_xenomit";
        public const string ORE_PROMETID = "ore_prometid";
        public const string ORE_DURANIUM = "ore_duranium";
        public const string ORE_PROMERIUM = "ore_promerium";
        public const string ORE_SEPROM = "ore_seprom";
        public const string ORE_PALLADIUM = "ore_palladium";
        public const string ORE_OSMIUM = "ore_osmium";

        public const string CAPTACH_TYPE_0 = "captcha_choose_some_black";
        public const string CAPTACH_TYPE_1 = "captcha_choose_all_black";
        public const string CAPTACH_TYPE_2 = "captcha_choose_some_red";
        public const string CAPTACH_TYPE_3 = "captcha_choose_all_red";

        public const int POI_TYPE_GENERIC = 0;
        public const int POI_TYPE_FACTORIZED = 1;
        public const int POI_TYPE_TRIGGER = 2;
        public const int POI_TYPE_DAMAGE = 3;
        public const int POI_TYPE_HEALING = 4;
        public const int POI_TYPE_NO_ACCESS = 5;
        public const int POI_TYPE_FACTION_NO_ACCESS = 6;
        public const int POI_TYPE_ENTER_LEAVE = 7;
        public const int POI_TYPE_RADIATION = 8;
        public const int POI_TYPE_CAGE = 9;
        public const int POI_TYPE_MINE_FIELD = 10;
        public const int POI_TYPE_BUFF_ZONE = 11;
        public const int POI_TYPE_SECTOR_CONTROL_HOME_ZONE = 12;
        public const int POI_TYPE_SECTOR_CONTROL_SECTOR_ZONE = 13;
        public const int POI_TYPE_NO_ACCESS_CUSTOM = -1;

        public const int POI_DESIGN_NONE = 0;
        public const int POI_DESIGN_ASTEROIDS = 1;
        public const int POI_DESIGN_ASTEROIDS_BLUE = 2;
        public const int POI_DESIGN_ASTEROIDS_MIXED_WITH_SCRAP = 3;
        public const int POI_DESIGN_SCRAP = 4;
        public const int POI_DESIGN_NEBULA = 5;
        public const int POI_DESIGN_SIMPLE = 6;
        public const int POI_DESIGN_SECTOR_CONTROL_HOME_ZONE = 7;
        public const int POI_DESIGN_SECTOR_CONTROL_SECTOR_ZONE = 8;

        public const int POI_SHAPE_CIRCLE = 0;
        public const int POI_SHAPE_POLYGON = 1;
        public const int POI_SHAPE_RECTANGLE = 2;

        public const int DISPATCH_UPDATE_IN_PROGRESS = 1;
        public const int DISPATCH_UPDATE_AVAILABLE = 0;
        public const int DISPATCH_UPDATE = 2;

        public const int GALAXY_GATE_EBG_DESING_ID = 54;
        public const int GALAXY_GATE_VOT_DESIGN_ID = 81;

        public static readonly Vector2 LOW__GATE_STARTING_POSITION = new(13 * 100, 116 * 100);
        public static readonly Vector2 LOW_GATE_SECOND_BUOY_POSITION = new(200 * 100, 119 * 100);
        public static readonly Vector2 LOW_GATE_THIRD_BUOY_POSITION = new(49 * 100, 114 * 100);

        public const int LOW_GATE_MIN_VAGRANT_COUNT = 3;

        //51,52,53,54,55,56,57,58,70,71,72,73,74,75,76,77,78,223,300
        public static List<int> GET_GALAXY_GATE_MAP_IDS()
        {
            List<int> ggMapIds = [
            50, 51, 52, 53, 54, 55, 56, 57, 58,
            61, 62, 63, 64, 65, 66, 67, 68, 69,
            70, 71, 72, 73, 74, 75, 76, 77, 78,
            200, 201, 202, 203,
            223,225, 226, 227, 228, 229,
            230, 231, 232, 233, 234, 235, 236, 237, 238, 239,
            300, 301, 302, 303, 304, 305,
            330, 331, 332, 334, 335, 336,
            410, 411, 412, 413, 414, 415,

            450, 451, 452, 453, 454, 455,
            460, 461, 462, 463, 464, 465, 466, 467, 468, 469,
            466, 467, 468,
            470, 471, 472, 473, 474, 475, 476, 477, 478, 479,
            480, 481,
        ];
            if (DateTime.UtcNow.Month != 12)
            {
                //ggMapIds.AddRange([430, 431, 432, 433, 434, 435, 436, 437, 438,]);
                ggMapIds.AddRange([439, 440, 441, 442, 443, 444, 445,]);
            }
            return ggMapIds;
        }

        public static Dictionary<int, string> PET_MODES { get; private set; } = new()
        {
            { PET_GEAR_PASSIVE, "None"},
            { PET_GEAR_GUARD, "Guard Gear"},
            { PET_GEAR_AUTO_LOOT, "Loot Gear" },
            { PET_GEAR_AUTO_RESOURCE_COLLECTION, "Resource Gear"},
            { PET_GEAR_ENEMY_LOCATOR, "Enemy Locator Gear" },
            { PET_GEAR_RESOURCE_LOCATOR, "Resource Locator Gear" },
            { PET_GEAR_TRADE_POD, "Trade Gear" },
            { PET_GEAR_REPAIR_PET, "Repair Gear" },
            { PET_GEAR_KAMIKAZE, "Kamikaze Gear" },
            { PET_GEAR_COMBO_SHIP_REPAIR, "Combo Repair Gear"},
            { PET_GEAR_COMBO_GUARD, "Combo Guard Gear" },
            { PET_GEAR_OPEN_FLAME, "Open Flame Gear" },
            { PET_GEAR_HEAT_RELEASE, "H.E.A.T Release Gear" },
        };

        public static Dictionary<int, string> PET_MODES_SHORT { get; private set; } = new()
        {
            { PET_GEAR_PASSIVE, "Passiv"},
            { PET_GEAR_GUARD, "G-GM"},
            { PET_GEAR_AUTO_LOOT, "G-AL" },
            { PET_GEAR_AUTO_RESOURCE_COLLECTION, "G-AR"},
            { PET_GEAR_ENEMY_LOCATOR, "G-EL" },
            { PET_GEAR_RESOURCE_LOCATOR, "G-RL" },
            { PET_GEAR_TRADE_POD, "G-TRA" },
            { PET_GEAR_REPAIR_PET, "G-REP" },
            { PET_GEAR_KAMIKAZE, "G-KK" },
            { PET_GEAR_COMBO_SHIP_REPAIR, "C-REP"},
            { PET_GEAR_COMBO_GUARD, "C-GM" },
            { PET_GEAR_OPEN_FLAME, "G-SF" },
            { PET_GEAR_HEAT_RELEASE, "G-HR" },
        };

        public static Dictionary<string, short> AMMUNITION_TYPES = new()
        {
            { "ROCKET", 0 },
            { "TORPEDO", 1},
            { "WIZARD", 2},
            { "PRANK", 3},
            { "BATTERY", 4},
            { "FIREWORK", 5},
            { "FIREWORK_1", 6},
            { "FIREWORK_2", 7},
            { "FIREWORK_3", 8},
            { "MINE", 9},
            { "MINE_EMP", 10},
            { "MINE_SAB", 11},
            { "MINE_DD", 12},
            { "MINE_SL", 13},
            { "SMARTBOMB", 14},
            { "INSTANT_SHIELD", 15},
            { "PLASMA", 16},
            { "EMP", 17},
            { "LASER_AMMO", 18},
            { "ROCKET_AMMO", 19},
            { "ammunition_laser_rsb-75", 20},
            { "ammunition_rocketlauncher_hstrm-01", 21},
            { "ammunition_rocketlauncher_ubr-100", 22},
            { "ammunition_rocketlauncher_eco-10", 23},
            { "ammunition_rocketlauncher_sar-01", 24},
            { "ammunition_rocketlauncher_sar-02", 25},
            { "ammunition_laser_lcb-10", 26},
            { "ammunition_laser_mcb-25", 27},
            { "ammunition_laser_mcb-50", 28},
            { "ammunition_laser_ucb-100", 29},
            { "ammunition_laser_sab-50", 30},
            { "ammunition_laser_cbo-100", 31},
            { "ammunition_rocket_r-310", 32},
            { "ammunition_rocket_plt-2026", 33 },
            { "ammunition_rocket_plt-2021", 34},
            { "ammunition_rocket_plt-3030", 35},
            { "BDR-1211", 36 },
            { "ammunition_laser_rb-214", 37},
            { "ammunition_rocket_dcr-250", 38},
            { "ammunition_rocketlauncher_cbr", 39},
            { "HITAC_LASER", 40},
            { "ammunition_laser_job-100", 41},
            { "BDR-1212", 42},
            { "FIREWORK_COM", 43},
            { "FIREWORK_RZ", 44},
            { "RIC-3", 45},
            { "ammunition_rocketlauncher_pir-100", 46},
            { "K300M", 47},
        };

        public static Dictionary<string, string> LASER_TYPES { get; private set; } = new()
        {
            { "LCB-10", "ammunition_laser_lcb-10" },
            { "MCB-25",  "ammunition_laser_mcb-25" },
            { "MCB-50",  "ammunition_laser_mcb-50" },
            { "UCB-100",  "ammunition_laser_ucb-100" },
            { "SAB-50",  "ammunition_laser_sab-50" },
            { "RSB-75",  "ammunition_laser_rsb-75" },
            { "CBO-100",  "ammunition_laser_cbo-100" },
            { "JOB-100",  "ammunition_laser_job-100" },
            { "RB-214",  "ammunition_laser_rb-214" },
            { "CC-C",  "ammunition_laser_cc-c" },
            { "PIB-100",  "ammunition_laser_pib-100" },
            { "EMAA-20",  "ammunition_laser_emaa-20" },
            { "VB-142", "ammunition_laser_vb-142" },
            { "A-BL", AMMO_A_BL },
        };

        public static Dictionary<string, string> ROCKET_TYPES { get; private set; } = new()
        {
            { "PLT-2026", "ammunition_rocket_plt-2026" },
            { "R-310",  "ammunition_rocket_r-310" },
            { "PLT-2021",  "ammunition_rocket_plt-2021" },
            { "PLT-3030",  "ammunition_rocket_plt-3030" },
            { "PLD-8",  "ammunition_rocket_pld-8" },
            { "DCR-250",  "ammunition_rocket_dcr-250" },
            { "AGT-500",  "ammunition_rocket_agt-500" },
            { "WIZ-X",  AMMO_SPECIAL_WIZ_X },
        };

        public static Dictionary<string, string> ROCKETLAUNCHER_TYPES { get; private set; } = new()
        {
            { "ECO-10",  "ammunition_rocketlauncher_eco-10" },
            { "HSTRM-01", "ammunition_rocketlauncher_hstrm-01" },
            { "UBR-100",  "ammunition_rocketlauncher_ubr-100" },
            { "SAR-01",  "ammunition_rocketlauncher_sar-01" },
            { "SAR-02",  "ammunition_rocketlauncher_sar-02" },
            { "CBR",  "ammunition_rocketlauncher_cbr" },
            { "PIR-100",  "ammunition_rocketlauncher_pir-100" },
        };

        public static Dictionary<string, int> ROCKETLAUNCHER_IDS { get; private set; } = new()
        {
            { "ammunition_rocketlauncher_hstrm-01", 21 },
            { "ammunition_rocketlauncher_ubr-100", 22 },
            { "ammunition_rocketlauncher_eco-10", 23 },
            { "ammunition_rocketlauncher_sar-01", 24 },
            { "ammunition_rocketlauncher_sar-02", 25 },
            { "ammunition_rocketlauncher_cbr", 38 },
            { "ammunition_rocketlauncher_pir-100", 41 },
        };

        public static Dictionary<string, string> DRONE_FORMATIONS { get; private set; } = new Dictionary<string, string>() {
            { "Default", DRONE_FORMATION_DEFAULT },
            { "X", DRONE_FORMATION_3DX},
            { "Arrow", DRONE_FORMATION_ARROW},
            { "Barrage", DRONE_FORMATION_BARRAGE},
            { "Bat", DRONE_FORMATION_BAT},
            { "Chevron", DRONE_FORMATION_CHEVRON},
            { "Double Arrow", DRONE_FORMATION_DOUBLEARROW},
            { "Diamond", DRONE_FORMATION_DIAMOND},
            { "Heart", DRONE_FORMATION_HEART},
            { "Lance", DRONE_FORMATION_LANCE},
            { "Moth", DRONE_FORMATION_MOTH},
            { "Pincer", DRONE_FORMATION_PINCER},
            { "Star", DRONE_FORMATION_STAR},
            { "Turtle", DRONE_FORMATION_TURTLE},
            { "Crab",  DRONE_FORMATION_CRAB},
            { "Ring", DRONE_FORMATION_RING},
            { "Drill", DRONE_FORMATION_DRILL},
            { "Dome", DRONE_FORMATION_DOME},
            { "Veteran", DRONE_FORMATION_VETERAN},
            { "Double Wheel", DRONE_FORMATION_DOUBLEWHEEL},
            { "Wave", DRONE_FORMATION_WAVE},
        };

        public static Dictionary<int, string> DRONE_FORMATION_IDS { get; private set; } = new Dictionary<int, string>() {
            { 0, DRONE_FORMATION_DEFAULT },
            { 19, DRONE_FORMATION_3DX},
            { 2, DRONE_FORMATION_ARROW},
            { 12, DRONE_FORMATION_BARRAGE},
            { 13, DRONE_FORMATION_BAT},
            { 8, DRONE_FORMATION_CHEVRON},
            { 6, DRONE_FORMATION_DOUBLEARROW},
            { 7, DRONE_FORMATION_DIAMOND},
            { 11, DRONE_FORMATION_HEART},
            { 3, DRONE_FORMATION_LANCE},
            { 9, DRONE_FORMATION_MOTH},
            { 5, DRONE_FORMATION_PINCER},
            { 4, DRONE_FORMATION_STAR},
            { 1, DRONE_FORMATION_TURTLE},
            { 10,  DRONE_FORMATION_CRAB},
            { 14, DRONE_FORMATION_RING},
            { 15, DRONE_FORMATION_DRILL},
            { 17, DRONE_FORMATION_DOME},
            { 16, DRONE_FORMATION_VETERAN},
            { 18, DRONE_FORMATION_DOUBLEWHEEL},
            { 20, DRONE_FORMATION_WAVE},
        };

        public static Dictionary<string, string> SHIP_ABILITIES { get; private set; } = new Dictionary<string, string>()
        {
            { "Goliath X Frozen Claw", "ability_goliath-x_frozen-claw" },
            { "Spearhead Ultimate Cloak", "ability_spearhead_ultimate-cloak" },
            { "Spearhead JAM-X", "ability_spearhead_jam-x" },
            { "Spearhead Target Marker", "ability_spearhead_target-marker" },
            { "Spearhead Double Minimap", "ability_spearhead_double-minimap" },
            { "Aegis HP Repair", "ability_aegis_hp-repair" },
            { "Aegis Shield Repair", "ability_aegis_shield-repair" },
            { "Aegis Repair Pod", "ability_aegis_repair-pod" },
            { "Citadel Draw Fire", "ability_citadel_draw-fire" },
            { "Citadel Foritfy", "ability_citadel_fortify" },
            { "Citadel Protection", "ability_citadel_protection" },
            { "Citadel Travel", "ability_citadel_travel" },
            { "Solace", "ability_solace" },
            { "Spectrum", "ability_spectrum" },
            { "Venom", "ability_venom" },
            { "Sentinel", "ability_sentinel" },
            { "Diminisher", "ability_diminisher" },
            { "Lightning", "ability_lightning" },
            { "Cyborg", "ability_venom" },
            { "Tartarus Rapid Fire", "ability_tartarus_rapid-fire" },
            { "Tartaru Speed Boost", "ability_tartarus_speed-boost" },
            { "Mimesis Hologram", "ability_mimesis_hologram" },
            { "Mimesis Scramble", "ability_mimesis_scramble" },
            { "Mimesis Phase Out", "ability_mimesis_phase_out" },
            { "Disruptor Redirect", "ability_disruptor_redirect" },
            { "Disruptor Shield Disarray", "ability_disruptor_shield-disarray" },
            { "Disruptor DDOL", "ability_disruptor_ddol" },
            { "Disruptor Incinerate", "ability_solaris_incinerate" },
            { "Keres Spread", "ability_keres_spread" },
            { "Keres Sleight", "ability_keres_sleight" },
            { "Solaris Incinerate", "ability_solaris_inc" },
            { "Holo Self Reversal", "ability_holo_self-reversal" },
            { "Holo Enemy Reversal", "ability_holo_enemy-reversal" },
            { "Berserker Shield Link", "ability_berserker_shield-link" },
            { "Berserker Berserker", "ability_berserker_berserk" },
            { "Berserker Revenge", "ability_berserker_revenge" },
            { "Zephyr Momentum", "ability_zephyr_momentum" },
            { "Zephyr Barrage", "ability_zephyr_triple-barrage" },
            { "Retiarus Charge Shot", "ability_retiarus_charge-shot" },
            { "Retiarus Supercharge", "ability_retiarus_supercharge" },
            { "Orcus Assimilate", "ability_orcus_assimilate" },
            { "Tempest Voltage Link", "ability_tempest_voltage-link" },
            { "Tempest Volt Discharge", "ability_tempest_volt-discharge" },
            { "Tempest Volt Backup", "ability_tempest_volt-backup" },
            { "Spectrum Plus Reflecting", "ability_spectrum-plus_prismatic-reflecting" },
            { "Basilisk Noxious Nebula", "ability_basilisk_noxious-nebula" },
            { "Basilisk Heightend Valour", "ability_basilisk_heightened-valour" },
            { "Liberator Plus Self Repair", "ability_liberator-plus_self-repair" },
            { "Citadel Plus Prismatic Endurance", "ability_citadel-plus_prismatic-endurance" },
            { "Solace Plus", "ability_solace-plus_nano-cluster-repairer-plus" },
            { "Solaris Plus", "ability_solaris-plus_inc-plus" },
            { "Pusat Plus Speed Sap", "ability_pusat-plus_speed-sap" },
            { "Pusat Plus Stim Barrier", "ability_pusat-plus_stim-barrier" },
            { "Hectate Plus", "ability_hecate-plus_particle-beam-plus" },
            { "Hectate Plus Stockpile", "ability_hecate-plus_stockpile" },
            { "Hammerclaw Plus Reallocate", "ability_hammerclaw-plus_reallocate" },
            { "Spearhead Plus Neutralizing Marker", "ability_spearhead-plus_neutralizing-marker" },
            { "Spearhead Plus JAMX-Creed", "ability_spearhead-plus_jamx-creed" },
            { "Tartarus Plus Speed Boost", "ability_tartarus-plus_speed-boost-plus" },
            { "Tartatus Plus Rapid Fire", "ability_tartarus-plus_rapid-fire-plus" },
        };

        public static List<string> BOX_TYPES { get; private set; } = [
            "ASTRAL_BOOTY_BOX",
            "BONUS_BOX",
            "SOLAR_CLASH",
            "BATTLEPASS",
            "FROM_SHIP",
            "MUCOSUM",
            "SCRAPIUM",
            "PRISMATIUM",
            "AURUS_BOX",
            "FROM_SHIP_BLOCKED",
            "PIRATE_BOOTY_RED",
            "PIRATE_BOOTY_BLUE",
            "PIRATE_BOOTY",
            "PIRATE_BOOTY_GOLD",
            "PIRATE_BOOTY_SILVER",
            "BLACK_BOOTY_BOX",
            "CHAROPOS_BOX",
            "AGATUS_BOX",
            "STYXUS_BOX",
            "ASTRAL_PRIME_BOOTY_BOX",
            "ALIEN_EGG",
            "LUCENT_ALIEN_EGG_BOOTY_BOX",
            "BIFENON",
            "BIFENON_2",
            "MASQUE_BOOTY_BOX",
            "BIRTHDAY",
            "LUMINIUM",
            "LUMINIUM_2",
            "LUMINIUM_3",
            "LUMINIUM_4",
            "LUMINIUM_5",
            "ICY_BOX",
        ];

        public static void AddBoxType(string boxType)
        {
            Instance?.AddBox(boxType);
        }

        public void AddBox(string boxType)
        {
            BOX_TYPES.Add(boxType);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BOX_TYPES)));
        }

        public static ObservableCollection<string> ORE_TYPES { get; private set; } = [
            "PROMETIUM",
            "ENDURIUM",
            "TERBIUM",
            "XENOMIT",
            "PROMETID",
            "DURANIUM",
            "PROMERIUM",
            "SEPROM",
            "PALLADIUM",
            ];


        public static Dictionary<string, int> ORE_LOOKUP { get; private set; } = new()
        {
            { "PROMETIUM", 0},
            { "ENDURIUM", 1},
            { "TERBIUM", 2},
            { "XENOMIT", 3},
            { "PROMETID", 4},
            { "DURANIUM", 5},
            { "PROMERIUM", 6},
            { "SEPROM", 7},
            { "PALLADIUM", 8},
        };

        public static Dictionary<int, string> ORE_LOOKUP_REVERSE { get; private set; } = new()
        {
            { 0, "PROMETIUM"},
            { 1, "ENDURIUM"},
            { 2, "TERBIUM"},
            { 3, "XENOMIT"},
            { 4, "PROMETID"},
            { 5, "DURANIUM"},
            { 6, "PROMERIUM"},
            { 7, "SEPROM"},
            { 8, "PALLADIUM"},
        };

        public static Dictionary<int, string> LAB_TYPE_LOOKUP { get; private set; } = new()
        {
            { 0, "Lasers"},
            { 1, "Rockets"},
            { 2, "Speed Generators"},
            { 3, "Shield Generators"},
        };

        public const string NPC_INTERCEPTOR = "Interceptor";
        public const string NPC_HITAC_MINION = "Hitac-Minion";

        public static Dictionary<int, Dictionary<string, int>> NPCS_BY_MAP_IDS { get; private set; } = new()
    {
        {
            1,
            new()
            {
                { "-=[ Streuner ]=-", 1 },
                { "-=[ Recruit Streuner ]=-", 152 },
                { "-=[ Aider Streuner ]=-", 153 },
            }
        },
        {
            2,
            new()
            {
                { "-=[ Streuner ]=-", 1 },
                { "-=[ Lordakia ]=-", 2 },
                { "..::{ Boss Streuner }::..", 14 },
                { "..::{ Boss Lordakia }::..", 15 },
                { "-=[ Recruit Streuner ]=-", 152 },
                { "-=[ Aider Streuner ]=-", 153 },
            }
        },
        {
            3,
            new()
            {
                { "-=[ Lordakia ]=-", 2 },
                { "-=[ Devolarium ]=-", 3 },
                { "-=[ Mordon ]=-", 4 },
                { "-=[ Saimon ]=-", 6 },
                { "..::{ Boss Mordon }::..", 16 },
                { "..::{ Boss Saimon }::..", 17 },
                { "..::{ Boss Devolarium }::..", 18 },
            }
        },
        {
            4,
            new()
            {
                { "-=[ Lordakia ]=-", 2 },
                { "-=[ Mordon ]=-", 4 },
                { "-=[ Sibelon ]=-", 5 },
                { "-=[ Saimon ]=-", 6 },
                { "..::{ Boss Saimon }::..", 17 },
                { "..::{ Boss Sibelon }::..", 20 },
            }
        },
        {
            5,
            new()
            {
                { "-=[ Streuner ]=-", 1 },
                { "-=[ Recruit Streuner ]=-", 152 },
                { "-=[ Aider Streuner ]=-", 153 },
            }
        },
        {
            6,
            new()
            {
                { "-=[ Streuner ]=-", 1 },
                { "-=[ Lordakia ]=-", 2 },
                { "..::{ Boss Streuner }::..", 14 },
                { "..::{ Boss Lordakia }::..", 15 },
                { "-=[ Recruit Streuner ]=-", 152 },
                { "-=[ Aider Streuner ]=-", 153 },
            }
        },
        {
            7,
            new()
            {
                { "-=[ Lordakia ]=-", 2 },
                { "-=[ Devolarium ]=-", 3 },
                { "-=[ Mordon ]=-", 4 },
                { "-=[ Saimon ]=-", 6 },
                { "..::{ Boss Mordon }::..", 16 },
                { "..::{ Boss Saimon }::..", 17 },
                { "..::{ Boss Devolarium }::..", 18 },
            }
        },
        {
            8,
            new()
            {
                { "-=[ Lordakia ]=-", 2 },
                { "-=[ Mordon ]=-", 4 },
                { "-=[ Sibelon ]=-", 5 },
                { "-=[ Saimon ]=-", 6 },
                { "..::{ Boss Saimon }::..", 17 },
                { "..::{ Boss Sibelon }::..", 20 },
            }
        },
        {
            9,
            new()
            {
                { "-=[ Streuner ]=-", 1 },
                { "-=[ Recruit Streuner ]=-", 152 },
                { "-=[ Aider Streuner ]=-", 153 },
            }
        },
        {
            10,
            new()
            {
                { "-=[ Streuner ]=-", 1 },
                { "-=[ Lordakia ]=-", 2 },
                { "..::{ Boss Streuner }::..", 14 },
                { "..::{ Boss Lordakia }::..", 15 },
                { "-=[ Recruit Streuner ]=-", 152 },
                { "-=[ Aider Streuner ]=-", 153 },
            }
        },
        {
            11,
            new()
            {
                { "-=[ Lordakia ]=-", 2 },
                { "-=[ Devolarium ]=-", 3 },
                { "-=[ Mordon ]=-", 4 },
                { "-=[ Saimon ]=-", 6 },
                { "..::{ Boss Mordon }::..", 16 },
                { "..::{ Boss Saimon }::..", 17 },
                { "..::{ Boss Devolarium }::..", 18 },
            }
        },
        {
            12,
            new()
            {
                { "-=[ Lordakia ]=-", 2 },
                { "-=[ Mordon ]=-", 4 },
                { "-=[ Sibelon ]=-", 5 },
                { "-=[ Saimon ]=-", 6 },
                { "..::{ Boss Saimon }::..", 17 },
                { "..::{ Boss Sibelon }::..", 20 },
            }
        },
        { 13, new() { { "-=[ Deadly Battleray ]=-", 121 }, } },
        { 14, new() { { "-=[ Deadly Battleray ]=-", 121 }, } },
        { 15, new() { { "-=[ Deadly Battleray ]=-", 121 }, } },
        { 16, new() { { "-=[ Deadly Battleray ]=-", 121 }, } },
        {
            17,
            new()
            {
                { "-=[ Lordakia ]=-", 2 },
                { "-=[ Sibelonit ]=-", 7 },
                { "-=[ Lordakium ]=-", 8 },
                { "..::{ Boss Sibelonit }::..", 19 },
                { "..::{ Boss Lordakium }::...", 21 },
            }
        },
        {
            18,
            new()
            {
                { "-=[ Kristallin ]=-", 9 },
                { "-=[ Kristallon ]=-", 10 },
                { "-=[ Protegit ]=-", 12 },
                { "-=[ Cubikon ]=-", 13 },
                { "..::{ Boss Kristallin }::..", 22 },
                { "-=[ Plagued Kristallin ]=-", 161 },
            }
        },
        {
            19,
            new()
            {
                { "-=[ Kristallin ]=-", 9 },
                { "-=[ Kristallon ]=-", 10 },
                { "..::{ Boss Kristallin }::..", 22 },
                { "..::{ Boss Kristallon }::..", 23 },
                { "-=[ Blighted Kristallon ]=-", 162 },
                { "<=< Blighted Gygerthrall >=>", 163 },
            }
        },
        {
            20,
            new()
            {
                { "-=[ StreuneR ]=-", 11 },
                { "..::{ Boss StreuneR }::..", 24 },
                { @"\\ Purpose XXI //", 174 },
            }
        },
        {
            21,
            new()
            {
                { "-=[ Lordakia ]=-", 2 },
                { "-=[ Sibelonit ]=-", 7 },
                { "-=[ Lordakium ]=-", 8 },
                { "..::{ Boss Sibelonit }::..", 19 },
                { "..::{ Boss Lordakium }::...", 21 },
            }
        },
        {
            22,
            new()
            {
                { "-=[ Kristallin ]=-", 9 },
                { "-=[ Kristallon ]=-", 10 },
                { "-=[ Protegit ]=-", 12 },
                { "-=[ Cubikon ]=-", 13 },
                { "..::{ Boss Kristallin }::..", 22 },
                { "-=[ Plagued Kristallin ]=-", 161 },
            }
        },
        {
            23,
            new()
            {
                { "-=[ Kristallin ]=-", 9 },
                { "-=[ Kristallon ]=-", 10 },
                { "..::{ Boss Kristallin }::..", 22 },
                { "..::{ Boss Kristallon }::..", 23 },
            }
        },
        {
            24,
            new()
            {
                { "-=[ StreuneR ]=-", 11 },
                { "..::{ Boss StreuneR }::..", 24 },
                { @"\\ Purpose XXI //", 174 },
            }
        },
        {
            25,
            new()
            {
                { "-=[ Lordakia ]=-", 2 },
                { "-=[ Sibelonit ]=-", 7 },
                { "-=[ Lordakium ]=-", 8 },
                { "..::{ Boss Sibelonit }::..", 19 },
                { "..::{ Boss Lordakium }::...", 21 },
            }
        },
        {
            26,
            new()
            {
                { "-=[ Kristallin ]=-", 9 },
                { "-=[ Kristallon ]=-", 10 },
                { "-=[ Protegit ]=-", 12 },
                { "-=[ Cubikon ]=-", 13 },
                { "..::{ Boss Kristallin }::..", 22 },
                { "-=[ Plagued Kristallin ]=-", 161 },
            }
        },
        {
            27,
            new()
            {
                { "-=[ Kristallin ]=-", 9 },
                { "-=[ Kristallon ]=-", 10 },
                { "..::{ Boss Kristallin }::..", 22 },
                { "..::{ Boss Kristallon }::..", 23 },
                { "-=[ Blighted Kristallon ]=-", 162 },
                { "<=< Blighted Gygerthrall >=>", 163 },
            }
        },
        {
            28,
            new()
            {
                { "-=[ StreuneR ]=-", 11 },
                { "..::{ Boss StreuneR }::..", 24 },
                { @"\\ Purpose XXI //", 174 },
            }
        },
        {
            29,
            new()
            {
                { "..::{ Boss Streuner }::..", 14 },
                { "..::{ Boss Lordakia }::..", 15 },
                { "..::{ Boss Mordon }::..", 16 },
                { "..::{ Boss Saimon }::..", 17 },
                { "..::{ Boss Devolarium }::..", 18 },
                { "..::{ Boss Sibelonit }::..", 19 },
                { "..::{ Boss Sibelon }::..", 20 },
                { "..::{ Boss Lordakium }::...", 21 },
                { "..::{ Boss Kristallin }::..", 22 },
                { "..::{ Boss Kristallon }::..", 23 },
                { "..::{ Boss StreuneR }::..", 24 },
                { "( UberStreuner )", 28 },
                { "( UberLordakia )", 29 },
                { "( UberMordon )", 30 },
                { "( UberSaimon )", 31 },
                { "( UberDevolarium )", 32 },
                { "( UberSibelonit )", 33 },
                { "( UberSibelon )", 34 },
                { "( UberLordakium )", 35 },
                { "( UberKristallin )", 36 },
                { "( UberKristallon )", 37 },
                { "( UberStreuneR )", 38 },
                { "-=[ Deadly Battleray ]=-", 121 },
                { "( Uber Interceptor )", 163 },
                { "( Uber Barracuda )", 164 },
                { "( Uber Saboteur )", 165 },
                { "( Uber Annihilator )", 166 },
                { "( Uber Battleray )", 167 },
            }
        },
        {
            91,
            new()
            {
                { "-=[ Interceptor ]=-", 58 },
                { "-=[ Barracuda ]=-", 59 },
                { "-=[ Saboteur ]=-", 60 },
                { "-=[ Annihilator ]=-", 61 },
            }
        },
        {
            92,
            new()
            {
                { "-=[ Interceptor ]=-", 58 },
                { "-=[ Barracuda ]=-", 59 },
                { "-=[ Saboteur ]=-", 60 },
                { "-=[ Annihilator ]=-", 61 },
            }
        },
        {
            93,
            new()
            {
                { "-=[ Interceptor ]=-", 58 },
                { "-=[ Barracuda ]=-", 59 },
                { "-=[ Saboteur ]=-", 60 },
                { "-=[ Annihilator ]=-", 61 },
                { "-=[ Battleray ]=-", 62 },
            }
        },
        {
            94,
            new()
            {
                { "-=[: Pike :]=-", 167 },
                { "-=[: Barb :]=-", 168 },
                { "-=[: Lanternfish :]=-", 169 },
                { "-=[: Spookfish :]=-", 170 },
                { "-=[: The Stinger :]=-", 171 },
            }
         },
        {
            306,
            new()
            {
                { @"\\ Impulse II //", 175 },
                { @"\\ Attend IX //", 176 },
                { @"\\ Invoke XVI //", 177 },
                { @"\\ Mindfire Behemoth //", 178 },
                { @"\\ Strokelight Barrage //", 179 },
            }
        },
        {
            307,
            new()
            {
                { @"\\ Impulse II //", 175 },
                { @"\\ Attend IX //", 176 },
                { @"\\ Invoke XVI //", 177 },
                { @"\\ Mindfire Behemoth //", 178 },
                { @"\\ Strokelight Barrage //", 179 },
            }
        },
        {
            308,
            new()
            {
                { @"\\ Impulse II //", 175 },
                { @"\\ Attend IX //", 176 },
                { @"\\ Invoke XVI //", 177 },
                { @"\\ Mindfire Behemoth //", 178 },
                { @"\\ Strokelight Barrage //", 179 },
            }
        }
    };

        public static List<string> NPC_NAMES { get; private set; } = [
            "-=[ Streuner ]=-",
            "-=[ Recruit Streuner ]=-",
            "-=[ Aider Streuner ]=-",
            "-=[ Lordakia ]=-",
            "..::{ Boss Streuner }::..",
            "..::{ Boss Lordakia }::..",
            "-=[ Devolarium ]=-",
            "-=[ Mordon ]=-",
            "-=[ Saimon ]=-",
            "..::{ Boss Mordon }::..",
            "..::{ Boss Saimon }::..",
            "..::{ Boss Devolarium }::..",
            "-=[ Sibelon ]=-",
            "..::{ Boss Sibelon }::..",
            "-=[ Sibelonit ]=-",
            "-=[ Lordakium ]=-",
            "..::{ Boss Sibelonit }::..",
            "..::{ Boss Lordakium }::...",
            "* Lordakium Spore *",
            "-=[ Kristallin ]=-",
            "-=[ Kristallon ]=-",
            "-=[ Cubikon ]=-",
            "..::{ Boss Kristallin }::..",
            "-=[ Plagued Kristallin ]=-",
            "-=[ Protegit ]=-",
            "-=[ Blighted Gygerthal ]=-",
            "..::{ Boss Kristallon }::..",
            "-=[ Blighted Kristallon ]=-",
            "<=< Blighted Gygerthrall >=>",
             "-=[ StreuneR ]=-",
            "..::{ Boss StreuneR }::..",
            @"\\ Urge IV //",
            @"\\ Attitude XIII //",
            @"\\ Capital I //",
            @"\\ Regard CXI //",
            @"\\ Observe X //",
            @"\\ Find VII //",
            "( UberMordon )",
            "( UberStreuner )",
            "( UberStreuneR )",
            "( UberLordakia )",
            "( UberSaimon )",
            "( UberSibelon )",
            "( UberSibelonit )",
            "( UberKristallin )",
            "( UberKristallon )",
            "( UberDevolarium )",
            "( UberLordakium )",
            "( Uber Battleray )",
            "( Uber Interceptor )",
            "( Uber Annihilator )",
            "( Uber Barracuda )",
            "( Uber Saboteur )",
            "-=[ Terror Mime5is ]=-",
            "-=[ Raging Mimes1s ]=-",
            "-=[ Kamikaze Mime5is ]=-",
            "-=[ Cloning Mim3sis ]=-",
            "-=[ Hexor M1mesis ]=-",
            "-=[ Reflector Mimesi5 ]=-",
            "-=[ Cloned Mim3sis ]=-",
            "-=[ 1100101 ]=-",
            "<=< Ice Meteoroid >=>",
            "<=< Icy >=>",
            "<=< Super Ice Meteoroid >=>",
            "-=[ Sunburst Lordakium ]=-",
            "-=[ Hitac ]=-",
            "-= [Boss Curcubitor] =-",
            "-= [Curcubitor] =-",
            "-={ demaNeR Freighter }=-",
            "-={ demaNeR Escort }=-",
            "-=[ Vagrant ]=-",
            "-=[ Marauder ]=-",
            "-=[ Outcast ]=-",
            "-=[ Corsair ]=-",
            "-=[ Hooligan ]=-",
            "-=[ Ravager ]=-",
            "-=[ Convict ]=-",
            "-=[ Century Falcon ]=-",
            "-=[ Interceptor ]=-",
            "-=[ Annihilator ]=-",
            "-=[ Barracuda ]=-",
            "-=[ Saboteur ]=-",
            "-=[ Battleray ]=-",
            "-=[ Deadly Battleray ]=-",
            "-=[: Pike :]=-",
            "-=[: Barb :]=-",
            "-=[: Lanternfish :]=-",
            "-=[: Spookfish :]=-",
            "-=[: The Stinger :]=-",
            @"\\ Purpose XXI //",
            @"\\ Impulse II //",
            @"\\ Attend IX //",
            @"\\ Invoke XVI //",
            @"\\ Mindfire Behemoth //",
            @"\\ Strokelight Barrage //",
            ">={ Lanatum }=<",
            ">={ Styxus }=<",
            ">={ Charopos }=<",
            "-=[ Spinel ]=-",
            "-=[ Agatus ]=-",
            "-=[ Spinelus ]=-",
            "-x-[ Frozen Icy ]-x-",
            "-x-[ Frozen Kristallin ]-x-",
            "-x-[ Frozen Kristallon ]-x-",
            "-x-[ Frozen Boss Kristallin ]-x-",
            "-x-[ Frozen Boss Kristallon ]-x-",
            "-x-[ Frozen Meteoroid ]-x-",
            "-x-[ Frozen Super Meteoroid ]-x-",
            "-=[ Skoll ]=-",
            @"<=< Skoll\'s Icy >=>",
            "-x-[ Synk ]-x-",
            "-=[ Hitac 2.0 ]=-",
            "-=[ Hitac-Minion 2.0 ]=-",
            "-=[ Hitac 2.5 ]=-",
            "-=[ Hitac-Minion 2.5 ]=-",

        ];

        public static Dictionary<string, int> NPC_DEFAULT_RANGES = new()
        {
           { "-=[ Streuner ]=-", 450 },
           { "-=[ Recruit Streuner ]=-", 500 },
           { "-=[ Aider Streuner ]=-", 450 },
           { "-=[ Lordakia ]=-", 450 },
           { "..::{ Boss Streuner }::..", 450 },
           { "..::{ Boss Lordakia }::..", 450 },
           { "-=[ Devolarium ]=-", 536 },
           { "-=[ Mordon ]=-", 500 },
           { "-=[ Saimon ]=-", 500 },
           { "..::{ Boss Mordon }::..", 520 },
           { "..::{ Boss Saimon }::..", 500 },
           { "..::{ Boss Devolarium }::..", 575 },
           { "-=[ Sibelon ]=-", 530 },
           { "..::{ Boss Sibelon }::..", 570 },
           { "-=[ Sibelonit ]=-", 575 },
           { "-=[ Lordakium ]=-", 610 },
           { "..::{ Boss Sibelonit }::..", 575 },
           { "..::{ Boss Lordakium }::...", 625 },
           { "* Lordakium Spore *", 450 },
           { "-=[ Kristallin ]=-", 575 },
           { "-=[ Kristallon ]=-", 600 },
           { "-=[ Cubikon ]=-", 525 },
           { "..::{ Boss Kristallin }::..", 575 },
           { "-=[ Plagued Kristallin ]=-", 575 },
           { "-=[ Protegit ]=-", 525 },
           { "-=[ Blighted Gygerthal ]=-", 600 },
           { "..::{ Boss Kristallon }::..", 615 },
           { "-=[ Blighted Kristallon ]=-", 600 },
           { "<=< Blighted Gygerthrall >=>", 600 },
            { "-=[ StreuneR ]=-", 600 },
           { "..::{ Boss StreuneR }::..", 600 },
           { "( UberMordon )", 580 },
           { "( UberStreuner )", 500 },
           { "( UberStreuneR )", 580 },
           { "( UberLordakia )", 500 },
           { "( UberSaimon )", 500 },
           { "( UberSibelon )", 600 },
           { "( UberSibelonit )", 575 },
           { "( UberKristallin )", 580 },
           { "( UberLordakium )", 605 },
           { "( UberDevolarium )", 500 },
           { "-=[ Interceptor ]=-", 500 },
           { "-=[ Annihilator ]=-", 575 },
           { "-=[ Barracuda ]=-", 560 },
           { "-=[ Saboteur ]=-", 580 },
           { "-=[ Battleray ]=-", 590 },
           { "-=[: Pike :]=-", 500 },
           { "-=[: Barb :]=-", 575 },
           { "-=[: Lanternfish :]=-", 560 },
           { "-=[: Spookfish :]=-", 580 },
           { "-=[: The Stinger :]=-", 590 },
           { @"\\ Purpose XXI //", 600 },
           { @"\\ Impulse II //", 610 },
           { @"\\ Attend IX //", 670 },
           { @"\\ Invoke XVI //", 600 },
           { @"\\ Mindfire Behemoth //", 610 },
           { @"\\ Strokelight Barrage //", 610 },
           { "-=[ Terror Mime5is ]=-", 610 },
           { "-=[ Raging Mimes1s ]=-", 590 },
           { "-=[ Kamikaze Mime5is ]=-", 580 },
           { "-=[ Cloning Mim3sis ]=-", 590 },
           { "-=[ Hexor M1mesis ]=-", 590 },
           { "-=[ Reflector Mimesi5 ]=-", 670 },
           { "-=[ Cloned Mim3sis ]=-", 590 },
           { "-x-[ Frozen Icy ]-x-", 550 },
           { "-x-[ Frozen Kristallin ]-x-", 570 },
           { "-x-[ Frozen Kristallon ]-x-", 600 },
           { "-x-[ Frozen Boss Kristallin ]-x-", 600 },
           { "-x-[ Frozen Boss Kristallon ]-x-", 600 },
            { "-x-[ Frozen Meteoroid ]-x-", 600 },
            { "-x-[ Frozen Super Meteoroid ]-x-", 610 },
            { "-=[ Skoll ]=-", 610 },
            { @"<=< Skoll\'s Icy >=>", 550 },
            { "-x-[ Synk ]-x-", 600 },
        };

        public static Dictionary<string, string> MINE_TYPES = new()
        {
            { "ACM-1", EQUIPMENT_MINE_ACM },
            { "DD-M01", EQUIPMENT_MINE_DDM },
            { "EMP-M01", EQUIPMENT_MINE_EMP },
            { "SAB-M01", EQUIPMENT_MINE_SAB },
            { "SL-M01", EQUIPMENT_MINE_SLM },
            { "SMB", EQUIPMENT_MINE_SMB },
            { "IM-01", EQUIPMENT_MINE_SMB },
        };

        public static void AddNpcName(string npcName)
        {
            Instance?.AddNpc(npcName);
        }

        public void AddNpc(string npcName)
        {
            NPC_NAMES.Add(npcName);
            PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(NPC_NAMES)));
        }

        public static Dictionary<int, string> WORKABLE_MAPS => MAP_NAMES.Take(DateTime.UtcNow.Month == 12 ? 38 : 37).ToDictionary();

        public static List<string> TIME_TABLE => new()
        {
            { "0:00" },
            { "1:00" },
            { "2:00" },
            { "3:00" },
            { "4:00" },
            { "5:00" },
            { "6:00" },
            { "7:00" },
            { "8:00" },
            { "9:00" },
            { "10:00" },
            { "11:00" },
            { "12:00" },
            { "13:00" },
            { "14:00" },
            { "15:00" },
            { "16:00" },
            { "17:00" },
            { "18:00" },
            { "19:00" },
            { "20:00" },
            { "21:00" },
            { "22:00" },
            { "23:00" },
        };
        //https://darkorbit-22.bpsecure.com/spacemap/graphics/maps-config.xml
        public static Dictionary<int, string> MAP_NAMES { get; private set; } = new()
        {
            { 1, "1-1" },
            { 2, "1-2" },
            { 3, "1-3" },
            { 4, "1-4" },
            { 5, "2-1" },
            { 6, "2-2" },
            { 7, "2-3" },
            { 8, "2-4" },
            { 9, "3-1" },
            { 10, "3-2" },
            { 11, "3-3" },
            { 12, "3-4" },
            { 13, "4-1" },
            { 14, "4-2" },
            { 15, "4-3" },
            { 16, "4-4" },
            { 17, "1-5" },
            { 18, "1-6" },
            { 19, "1-7" },
            { 20, "1-8" },
            { 21, "2-5" },
            { 22, "2-6" },
            { 23, "2-7" },
            { 24, "2-8" },
            { 25, "3-5" },
            { 26, "3-6" },
            { 27, "3-7" },
            { 28, "3-8" },
            { 29, "4-5" },

            { 306, "1-BL" },
            { 307, "2-BL" },
            { 308, "3-BL" },

            { 91, "5-1" },
            { 92, "5-2" },
            { 93, "5-3" },
            { 94, "5-4" }, //count36

            { -3, "Mimesis Escort " },
            { -2, "Frozen Labyrinth" },

            { 50, "GG" },
            { 51, "GG α" },
            { 52, "GG β" },
            { 53, "GG γ" },
            { 54, "GG NC" },
            { 55, "GG δ" },
            { 56, "GG Orb" },
            { 57, "GG Y6" },
            { 58, "GG HSG" },
            { 70, "GG ε" },
            { 71, "GG ζ" }, //1
            { 72, "GG ζ" }, //2
            { 73, "GG ζ" }, //3
            { 74, "GG ϰ" },
            { 75, "GG λ" },
            { 76, "GG Kronos" },
            { 200, "LoW" },
            { 227, "GG VoT" },
            { 228, "Permafrost Fissure" },
            { 229, "Quarantine Zone" },
            { 230, "GG VoT" },
            { 231, "GG VoT" },
            { 232, "GG VoT" },
            { 233, "GG VoT" },
            { 234, "GG VoT" },
            { 235, "GG VoT" },
            { 236, "GG VoT" },
            { 300, "GG ς" },
            { 301, "GG ς" },
            { 302, "GG ς" },
            { 303, "GG ς" },
            { 304, "GG ς" },
            { 305, "GG ς" },
            { 330, "GG VoT" },  //৩
            { 331, "GG VoT" },
            { 332, "GG VoT" },
            { 333, "GG VoT" },
            { 334, "GG VoT" },
            { 335, "GG VoT" },
            { 336, "GG VoT" },
            { 410, "Gauntlet of Plutus Normal" },
            { 411, "Gauntlet of Plutus Normal" },
            { 412, "Gauntlet of Plutus Normal" },
            { 413, "Gauntlet of Plutus Normal" },
            { 414, "Gauntlet of Plutus Normal" },
            { 415, "Gauntlet of Plutus Normal" },
            { 430, DateTime.UtcNow.Month == 12 ? "ATLAS A" : "Escort VRU 1"},
            { 431, DateTime.UtcNow.Month == 12 ? "ATLAS B" : "Escort VRU 2" },
            { 432, DateTime.UtcNow.Month == 12 ? "ATLAS C" : "Escort VRU 3" },
            { 433, DateTime.UtcNow.Month == 12 ? "Cygni" : "Escort MMO 1" },
            { 434, DateTime.UtcNow.Month == 12 ? "Helvetios" : "Escort MMO 2" },
            { 435, DateTime.UtcNow.Month == 12 ? "Eridani" : "Escort MMO 3" },
            { 436, DateTime.UtcNow.Month == 12 ? "Sirius" : "Escort EIC 1" },
            { 437, DateTime.UtcNow.Month == 12 ? "Sadatoni" : "Escort EIC 2" },
            { 438, DateTime.UtcNow.Month == 12 ? "Persei" : "Escort EIC 3" },
            { 439, DateTime.UtcNow.Month == 12 ? "Volantis" : "GG ∞" },
            { 440, DateTime.UtcNow.Month == 12 ? "Alcyone" : "GG ∞" },
            { 441, DateTime.UtcNow.Month == 12 ? "Auriga" : "GG ∞" },
            { 442, DateTime.UtcNow.Month == 12 ? "Bootes" : "GG ∞" },
            { 443, DateTime.UtcNow.Month == 12 ? "Aquila" : "GG ∞" },
            { 444, DateTime.UtcNow.Month == 12 ? "Orion" :"GG ∞" },
            { 445, DateTime.UtcNow.Month == 12 ? "Maia" : "GG ∞" },
            { 450, "Gauntlet of Plutus Easy" },
            { 451, "Gauntlet of Plutus Easy" },
            { 452, "Gauntlet of Plutus Easy" },
            { 453, "Gauntlet of Plutus Easy" },
            { 454, "Gauntlet of Plutus Easy" },
            { 455, "Gauntlet of Plutus Easy" },
            { 460, "GG ∞" },
            { 461, "GG ∞" },
            { 462, "GG ∞" },
            { 463, "GG ∞" },
            { 464, "GG ∞" },
            { 465, "GG ∞" },
            { 466, "GG Astral" },
            { 467, "GG Astral" },
            { 468, "GG Astral" },
            { 469, "Gauntlet of Plutus Normal" },//"Plutus' Trove of Riches Normal" },
            { 470, "Gauntlet of Plutus Easy" },//"Plutus' Trove of Riches" },
            { 471, "Treacherous Domain" },  //easy
            { 472, "Treacherous Domain" },  //normal
            { 473, "GG Deep Space Echoes" },    //Expedition
            { 474, "GG Deep Space Echoes" },    //Expedition
            { 475, "GG Deep Space Echoes" },    //Expedition
            { 476, "GG Deep Space Echoes" },    //Expedition
            { 477, "GG Deep Space Echoes" },    //Expedition
            { 478, "GG Deep Space Echoes" },    //Expedition
            { 479, "GG Deep Space Echoes" },    //Expedition
            { 480, "GG Deep Space Echoes" },    //Expedition
            { 481, "GG Deep Space Echoes" },    //Expedition
        };

        public static Dictionary<int, string> GALAXY_GATES { get; private set; } = new()
        {
            { 2, "GG α" },
            { 3, "GG β" },
            { 4, "GG γ" },
            { 5, "GG δ" },
            //{ 34, "GG Devolarium Attack" },   //Same ID as LoW
            { 53, "GG ε" },
            { 54, "GG ζ" },
            { 70, "GG ϰ" },
            { 71, "GG λ" },
            { 72, "GG Kronos" },
            { 74, "GG Hades" },
            { 82, "GG ς" },
            { 84, "QZ" },    //Quarantine Zone
            { 89, "GG Astral" }, //Ascension GateId
            { 34, "LoW" },
            { -1, "GG ∞" }, //54
            { 235, "Gauntlet of Plutus Easy" },
            { 24, "Gauntlet of Plutus Normal" },
            { 238, "Treacherous Domain" },
            { 81, "GG VoT" },   //GG ৩
            { 301, "GG Deep Space Echoes" },  //
        };

        public static Dictionary<int, int> GALAXY_GATE_ACCESSED_BY_MAPID(int factionId)
        {
            return new()
            {
                { 2, GET_HOMEMAP_ID(factionId) },    //"GG α"
                { 3, GET_HOMEMAP_ID(factionId) },//"GG β"
                { 4, GET_HOMEMAP_ID(factionId) },//"GG γ"
                { 5, GET_HOMEMAP_ID(factionId) },//"GG δ"
                //{ 34, GET_HOMEMAP_ID(factionId) },//"GG Devolarium Attack" SAME ID AS LOW
                { 53, GET_HOMEMAP_ID(factionId) },//"GG ε"
                { 54, GET_HOMEMAP_ID(factionId) },//"GG ζ"
                { 70, GET_HOMEMAP_ID(factionId) },//"GG ϰ"
                { 71, GET_HOMEMAP_ID(factionId) },//"GG λ"
                { 72, GET_HOMEMAP_ID(factionId) },//"GG Kronos"
                { 74, GET_HOMEMAP_ID(factionId) },//"GG Hades"
                { 82, GET_HOMEMAP_ID(factionId) },//"GG ς"
                { 84, GET_OUTPOSTMAP_ID(factionId) - 1 },    //Quarantine Zone
                { 89, GET_HOMEMAP_ID(factionId) },//Ascension GateId
                { 34, GET_HOMEMAP_ID(factionId) + 2 },//"LoW"
                { -1, GET_BLACKLIGHTMAP_ID(factionId) },//"GG ∞"
                { 235, GET_HOMEMAP_ID(factionId) },//"Gauntlet of Plutus Easy"
                { 24, GET_OUTPOSTMAP_ID(factionId) },//"Gauntlet of Plutus Normal"
                { 238, GET_HOMEMAP_ID(factionId) },//"Treacherous Domain"
                { 81, GET_HOMEMAP_ID(factionId) + 3 },//"GG VoT"
                { 83, GET_HOMEMAP_ID(factionId) },//Frozen Labyrinth
                { 301, GET_HOMEMAP_ID(factionId) }, //DSE
            };
        }

        public static int GET_HOMEMAP_ID(int factionId)
        {
            return factionId == 1 ? 1 : factionId == 2 ? 5 : factionId == 3 ? 9 : 1;
        }

        public static int GET_OUTPOSTMAP_ID(int factionId)
        {
            return factionId == 1 ? 20 : factionId == 2 ? 24 : factionId == 3 ? 28 : 20;
        }

        public static int GET_BLACKLIGHTMAP_ID(int factionId)
        {
            return factionId == 1 ? 306 : factionId == 2 ? 307 : factionId == 3 ? 308 : 306;
        }

        public static Dictionary<string, string> AUCTION_NAMES = new()
        {
            { "MCB-25", "ammunition_laser_mcb-25" },
            { "MCB-50", "ammunition_laser_mcb-50" },
            { "SAB-50", "ammunition_laser_sab-50" },
            { "PLT-3030", "ammunition_rocket_plt-3030" },
            { "PLT-2021", "ammunition_rocket_plt-2021" },
            { "ACM-1", "ammunition_mine_acm-01" },
            { "G3N-7900", "equipment_generator_speed_g3n-7900" },
            { "SG3N-B02", "equipment_generator_shield_sg3n-b02" },
            { "ClO4k A", "equipment_extra_cpu_cl04k-xs" },
            { "CLO4K CPU", "equipment_extra_cpu_cl04k-m" },
            { "CLO4K CPU XL", "equipment_extra_cpu_cl04k-xl" },
            { "SLOT CPU 2", "equipment_extra_cpu_sle-02" },
            { "SLOT CPU 3", "equipment_extra_cpu_sle-03" },
            { "SLOT CPU 4", "equipment_extra_cpu_sle-04" },
            { "Rocket Auto CPU", "equipment_extra_cpu_arol-x" },
            { "Mine Turbo CPU", "equipment_extra_cpu_min-t02" },
            { "Jump CPU 2", "equipment_extra_cpu_jp-02" },
            { "Radar CPU", "equipment_extra_cpu_rd-x" },
            { "Drone Repair CPU", "equipment_extra_cpu_dr-02" },
            { "Ammo CPU", "equipment_extra_cpu_alb-x" },
            { "Smartbomb CPU", "equipment_extra_cpu_smb-01" },
            { "Instashield CPU", "equipment_extra_cpu_ish-01" },
            { "Generator Boost CPU", "equipment_extra_cpu_nc-agb-x" },
            { "Repair Robot 3", "equipment_extra_repbot_rep-3" },
            { "Repair Robot 4", "equipment_extra_repbot_rep-4" },
            { "Advanced Jump CPU", "equipment_extra_cpu_ajp-01" },
            { "Rocket CPU", "equipment_extra_cpu_rb-x" },
            { "Rocketlauncher CPU", "equipment_extra_cpu_rllb-x" },
            { "AIM CPU", "equipment_extra_cpu_aim-02" },
            { "Rocket Turbo CPU", "equipment_extra_cpu_rok-t01" },
            { "Repair Auto CPU", "equipment_extra_cpu_nc-rrb-x" },
            { "Lab CPU", "equipment_extra_cpu_nc-awb-x" },
            { "Cargo CPU", "equipment_extra_cpu_g3x-crgo-x" },
            { "LF-3", "equipment_weapon_laser_lf-3" },
            { "Hellstrom Launcher 2", "equipment_weapon_rocketlauncher_hst-2" },
            { "Xenomit", "resource_ore_xenomit" },
            { "Logfile", "resource_logfile" },
            { "Iris Drone", "drone_iris" },
            { "XP Booster", "booster_ep-b01" },
            { "Shield Booster", "booster_shd-b01" },
            { "Damage Booster", "booster_dmg-b01" },
            { "Repair Booster", "booster_rep-b01" },
            { "Honor Booster", "booster_hon-b01" },
            { "Shield Gen Booster", "booster_sreg-b01" },
            { "Resource Booster", "booster_res-b01" },
            { "HP Booster", "booster_hp-b01" },
        };

        public static readonly Dictionary<BoosterType, int> EBG_BOOSTER_TYPES_PRIORITY = new()
        {
            { BoosterType.COOLDOWN, 3 },
            { BoosterType.DMG, 4 },
            { BoosterType.DMG_L, 3 },
            { BoosterType.DMG_R, 2 },
            { BoosterType.HITCHANCE, 2 },
            { BoosterType.HP, 3 },
            { BoosterType.SHD, 0 },
            { BoosterType.SPEED, 1 }
        };

        public static List<int> GG_SPINNER_AMOUNT = [
            1,
            5,
            10,
            100,
            ];

        public static Dictionary<string, byte> GG_SPINNER_NAME_ID = new() {
            {"GG α", GalaxyGateBuilderGetGatePartsRequest.ALPHA },
            {"GG β", GalaxyGateBuilderGetGatePartsRequest.BETA },
            {"GG γ", GalaxyGateBuilderGetGatePartsRequest.GAMMA },
            {"GG δ", GalaxyGateBuilderGetGatePartsRequest.DELTA },
            {"GG ε", GalaxyGateBuilderGetGatePartsRequest.EPSILON },
            {"GG ζ", GalaxyGateBuilderGetGatePartsRequest.ZETA },
            {"GG ϰ", GalaxyGateBuilderGetGatePartsRequest.KAPPA },
            {"GG λ", GalaxyGateBuilderGetGatePartsRequest.LAMBDA },
            {"GG ς", GalaxyGateBuilderGetGatePartsRequest.STREUNER },
            };

        public static Dictionary<byte, int> GG_SPINNER_ID_MAX_PARTS = new()
        {
            {GalaxyGateBuilderGetGatePartsRequest.ALPHA, 34 },
            {GalaxyGateBuilderGetGatePartsRequest.BETA, 48 },
            {GalaxyGateBuilderGetGatePartsRequest.GAMMA, 82 },
            {GalaxyGateBuilderGetGatePartsRequest.DELTA, 128 },
            {GalaxyGateBuilderGetGatePartsRequest.EPSILON, 99 },
            {GalaxyGateBuilderGetGatePartsRequest.ZETA, 111 },
            {GalaxyGateBuilderGetGatePartsRequest.KAPPA, 120 },
            {GalaxyGateBuilderGetGatePartsRequest.LAMBDA, 45 },
            {GalaxyGateBuilderGetGatePartsRequest.STREUNER, 100 },
        };

        private static CommandConstants? Instance;
        public event PropertyChangedEventHandler? PropertyChanged;

        public CommandConstants()
        {
            Instance = this;
        }
    }
}
