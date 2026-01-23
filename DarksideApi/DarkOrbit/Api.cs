using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Commands.Client;
using DarksideApi.DarkOrbit.Constants;
using DarksideApi.DarkOrbit.Objects.Auction;
using DarksideApi.DarkOrbit.Objects.Boosters;
using DarksideApi.DarkOrbit.Objects.Boxes;
using DarksideApi.DarkOrbit.Objects.Client;
using DarksideApi.DarkOrbit.Objects.Group;
using DarksideApi.DarkOrbit.Objects.Hero;
using DarksideApi.DarkOrbit.Objects.Map;
using DarksideApi.DarkOrbit.Objects.Pets;
using DarksideApi.DarkOrbit.Objects.Quest;
using DarksideApi.DarkOrbit.Objects.Ship;
using DarksideApi.DarkOrbit.Objects.StarSystem;
using DarksideApi.Licensing;
using DarksideApi.Statistics.Objects;
using DarksideApi.Util.Collections;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Numerics;

namespace DarksideApi.DarkOrbit
{
    public class Api
    {
        public const string Version = "1.0.950";
        public const string BigpointClientVersion = "1.6.9";
        public static string TrackVersion { get; set; } = "10.0.90998538";
        public static readonly CommandConstants CommandConstants = new();

        public event EventHandler<string>? OnLogMessage;
        public event EventHandler<Statistics.StatisticsItem>? OnStatisticsItem;
        public event EventHandler<Vector2>? OnLastPosition;
        public event EventHandler<List<Hangar>>? OnAddHangars;
        public event EventHandler<List<MenuItem>>? OnAddMenuItems;
        public event EventHandler<List<Booster>>? OnAddBoosters;
        public event EventHandler<List<GroupMemberObj>>? OnAddGroupMembers;
        public event EventHandler<List<Quest>>? OnAddQuest;

        public static Dictionary<int, StarMap> StarSystem { get; set; } = [];
        public static Dictionary<int, Dictionary<string, int>> NpcIdsByMap { get; set; } = CommandConstants.NPCS_BY_MAP_IDS;
        public static Dictionary<int, Dictionary<string, int>> OreIdsByMap { get; set; } = [];

        public DarkOrbotSettings Settings { get; private set; }
        public Logic.Logic Logic { get; private set; }
        public GameClient GameClient { get; private set; }
        public BackpageApi BackpageApi { get; private set; }
        public GameMethods GameMethods { get; private set; }
        public Movement.Movement Movement { get; private set; }
        public Cooldown.Cooldown Cooldown { get; private set; }
        public GameObservers.GameObservers GameObservers { get; private set; }
        public Statistics.Statistics Statistics { get; private set; }
        public Hero Hero { get; private set; }
        public Map Map { get; set; }
        public Inventory Inventory { get; private set; }
        public Menu Menu { get; private set; }
        public TrackIntVariableModule VariableTrackModule { get; private set; }

        public ConcurrentDictionary<string, Collectible> Collectibles { get; private set; }
        public ConcurrentDictionary<int, Ship> Ships { get; private set; }
        public ConcurrentDictionary<int, Gate> Gates { get; private set; }
        public ConcurrentDictionary<int, MapAsset> MapAssets { get; private set; }
        public ConcurrentDictionary<int, DateTime> EntitiesUnderAttackBy { get; private set; }
        public ConcurrentDictionary<DateTime, string> LastDeaths { get; private set; }
        public ConcurrentDictionary<int, Pet> Pets { get; private set; }
        public ConcurrentDictionary<string, PoiZone> POIZones { get; private set; }
        public ConcurrentDictionary<string, Mine> Mines { get; private set; }
        public ConcurrentDictionary<int, GalaxyGateGenerator> GalaxyGateGenerators { get; private set; }
        public ObservableKeyedCollection<int, Quest> Quests { get; private set; }
        public ObservableCollection<Hangar> Hangars { get; private set; }
        public List<DispatcherObj> Dispatchers { get; private set; }
        public List<int> LockedGateIds { get; private set; }
        public Vector2 PointOfInterest { get; set; }
        public List<string> InvalidCollectibles { get; private set; }
        public Dictionary<int, int> AutoAttackKillAmounts { get; private set; }
        public Captcha Captcha { get; set; }
        public GroupObj Group { get; set; }
        public Quest DiminishingQuest { get; set; }
        public Auction Auction { get; private set; }

        public object ObjectLocker = new();
        public Logging.Logging Logging { get; private set; }

        public bool HasGroup => this.Group != null;

        public Api(DarkOrbotSettings settings, string injectionKey)
        {
            this.Settings = settings ?? new();

            this.Logging = new(this.Settings.DisplayName);

            this.Collectibles = [];
            this.Ships = [];
            this.Gates = [];
            this.MapAssets = [];
            this.EntitiesUnderAttackBy = [];
            this.LastDeaths = [];
            this.Pets = [];
            this.POIZones = [];
            this.Mines = [];
            this.GalaxyGateGenerators = [];
            this.Quests = new ObservableKeyedCollection<int, Quest>(x => x.Id, "Id");
            this.Hangars = [];
            this.Dispatchers = ConstantCollection.DISPATCHER_DEFAULT_LIST;
            this.LockedGateIds = [];
            this.InvalidCollectibles = [];
            this.PointOfInterest = new(-1, -1);
            this.AutoAttackKillAmounts = [];
            this.Captcha = null!;
            this.Group = null!;
            this.DiminishingQuest = null!;
            this.Auction = new(this);
            this.VariableTrackModule = new("map_clicks");

            if (InjectedKeyReviewer.Review(injectionKey))
            {
                this.Logic = new(this);
                this.BackpageApi = new(this);
                this.GameMethods = new(this);
                this.GameClient = new(this);
            }
            else
            {
                this.Logic = new();
                this.BackpageApi = new();
                this.GameMethods = new();
                this.GameClient = new();
                this.Logging.Log("key_invalid");
            }

            this.Hero = new(this);
            this.Map = new(0, 0, 0);
            this.Inventory = new();
            this.Menu = new();
            this.GameObservers = new(this);
            this.Movement = new(this);
            this.Cooldown = new(this);
            this.Statistics = new(this);
        }

        public void Clear()
        {
            this.Hero.Reset();
            this.Collectibles.Clear();
            this.Ships.Clear();
            this.Gates.Clear();
            this.MapAssets.Clear();
            this.POIZones.Clear();
            this.Pets.Clear();
            this.Mines.Clear();
            this.Hero.Cargo.Clear();
            this.EntitiesUnderAttackBy.Clear();
            this.InvalidCollectibles.Clear();
            this.GalaxyGateGenerators.Clear();
            //Quests.Clear();
            this.PointOfInterest = new(-1, -1);
            this.Quests.Remove(this.DiminishingQuest?.Id ?? 0);
            this.DiminishingQuest = null!;
            this.Captcha = null!;
            this.Logic.Target = null!;
        }

        public void LoadSettings(DarkOrbotSettings settings)
        {
            foreach (System.Reflection.PropertyInfo key in settings.GetType().GetProperties())
            {
                System.Reflection.PropertyInfo? current = this.Settings.GetType().GetProperty(key.Name);
                var newValue = key.GetValue(settings, null);

                current?.SetValue(this.Settings, newValue, null);
            }
        }

        public void WriteLog(string message)
        {
            OnLogMessage?.Invoke(this, message);
            this.Logging.Log(message);
        }

        public void AddLootResource(string name, long amount, long total = 0)
        {
            OnStatisticsItem?.Invoke(this, new ResourceStatisticsItem(name, amount, total == 0 ? this.Inventory.Get(name) : total));
        }

        public void AddLootNpc(string name, long amount, long total = 0)
        {
            OnStatisticsItem?.Invoke(this, new NpcStatisticsItem(name, amount, total));
        }

        public void AddLootBox(string name, long amount, long total = 0)
        {
            OnStatisticsItem?.Invoke(this, new BoxStatisticsItem(name, amount, total));
        }

        public void AddLootDeath(string name, long amount, long total = 0)
        {
            OnStatisticsItem?.Invoke(this, new DeathStatisticsItem(name, amount, total));
        }

        public void AddLastPosition(int x, int y)
        {
            OnLastPosition?.Invoke(this, new(x, y));
        }

        public void AddHangar(List<Hangar> hangars)
        {
            OnAddHangars?.Invoke(this, hangars);
        }

        public void AddMenuItems(List<MenuItem> items)
        {
            OnAddMenuItems?.Invoke(this, items);
        }

        public void AddBoosters(List<Booster> boosters)
        {
            OnAddBoosters?.Invoke(this, boosters);
        }

        public void AddGroupMembers(List<GroupMemberObj> members)
        {
            OnAddGroupMembers?.Invoke(this, members);
        }

        public void AddQuest(List<Quest> quests)
        {
            OnAddQuest?.Invoke(this, quests);
        }

        public void AddVariableTrack()
        {
            if (this.VariableTrackModule.value == 0)
            {
                Task.Delay(60000).ContinueWith(_ =>
                {
                    this.GameMethods.TrackVariable();
                });
            }
            if (this.VariableTrackModule.value >= 107374182)
            {
                this.VariableTrackModule.value = 1;
            }
            this.VariableTrackModule.value += 1;
        }
    }
}
