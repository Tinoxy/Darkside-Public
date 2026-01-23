using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Objects.Boosters;
using DarksideApi.DarkOrbit.Objects.Client;
using DarksideApi.DarkOrbit.Objects.Pets;
using DarksideApi.Util;
using DarksideApi.Util.Collections;
using DarksideApi.Util.Extensions;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Numerics;

namespace DarksideApi.DarkOrbit.Objects.Hero
{
    public class Hero : IGameObject, INotifyPropertyChanged
    {
        private readonly Api api;
        private int userId = 0;
        public string ServerTag { get; set; } = string.Empty;
        private string username = string.Empty;
        private string clanTag = string.Empty;
        private string sessionId = string.Empty;
        public int InstanceId { get; set; }
        private int factionId;
        private string ship = string.Empty;
        private int level;
        public int NextMapId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public double CurrentDistance => 0;
        public double TargetDistance => HelpTools.CalculateDistance(this.TargetX, this.TargetY, this.X, this.Y);
        public double PathDistance => HelpTools.CalculateDistance(this.LastPathElement, this.Position);
        public int TargetX { get; set; }
        public int TargetY { get; set; }
        public Vector2 Position => new(this.X, this.Y);
        public Vector2 TargetPosition => new(this.TargetX, this.TargetY);
        public double Angle { get; set; }
        public LinkedList<Vector2> Path { get; set; } = [];
        public Vector2 LastPathElement => this.Path.Count > 0 ? this.Path.Last() : this.api.Hero.Position;
        public Vector2 FirstPathElement => this.Path.Count > 0 ? this.Path.First() : this.api.Hero.Position;
        private int speed;
        private int config;
        private string droneFormation = string.Empty;
        public long Hitpoints { get; set; }
        public long MaxHitpoints { get; set; }
        public int Shield { get; set; }
        public int MaxShield { get; set; }
        public int NanoShield { get; set; }
        public int MaxNanoShield { get; set; }
        public double ExperiencePoints { get; set; }
        public double HonorPoints { get; set; }
        private double _flyingTimeMs;
        public double FlyingTimeMs
        {
            get => this._flyingTimeMs;
            set => this._flyingTimeMs = value;
        }
        public bool IsFlying => this.TargetDistance > 0 && this.FlyingTimeMs > 0;
        public bool IsCloaked { get; set; }
        public DateTime LastRepairTime { get; set; }
        public bool IsRepairing => (DateTime.UtcNow - this.LastRepairTime).TotalSeconds <= 3;
        private bool hasPremium { get; set; }
        public int RepairAmount { get; set; }
        public double HitpointsPercentage => this.MaxHitpoints > 0 ? this.Hitpoints / (double)this.MaxHitpoints * 100.0 : 0;
        public double ShieldPercentage => this.MaxShield > 0 ? this.Shield / (double)this.MaxShield * 100.0 : 0;
        public double NanoShieldPercentage => this.MaxNanoShield > 0 ? this.NanoShield / (double)this.MaxNanoShield * 100.0 : 0;
        public DateTime LastAttackTime { get; set; }
        public DateTime LastAttackTryTime { get; set; }
        public int CurrentTarget { get; set; }
        public bool IsAttacking => (DateTime.UtcNow - this.LastAttackTime).TotalSeconds < 5;
        public bool IsPetActive => this.api.Pets.Values.Any(p => p.OwnerId == this.UserId);
        public bool IsPetOutOfFuel { get; set; }
        public bool IsPetDestroyed { get; set; }
        public int PetStartTries { get; set; }
        public Pet Pet => this.api.Pets.Values.Where(p => p.OwnerId == this.UserId).FirstOrDefault() ?? Pet.Empty();
        private Cargo cargo;
        private MenuItem currentLaser;
        private MenuItem currentRocket;
        private MenuItem currentRocketLauncher;
        public ConcurrentDictionary<string, MenuItem> CurrentAbilities { get; set; }
        public ObservableKeyedCollection<string, Booster> ActiveBoosters { get; set; } = new ObservableKeyedCollection<string, Booster>(x => x.BoosterId, "BoosterId");
        public int RocketLauncherLoadingStatus { get; set; }
        public int DispatcherSlotsTotal { get; set; }
        public int DispatcherSlotsFree { get; set; }
        public int AdvancedJumpCpuDuration { get; set; } = 10;
        public long DefaultHangarId { get; set; }
        public int GGAccessTargetMapId { get; set; }
        public int CurrentCaptainEnergy { get; set; }
        public int MaxCaptainEnergy { get; set; }
        public double CaptainEnergyPercent => this.MaxCaptainEnergy > 0 ? this.CurrentCaptainEnergy / (double)this.MaxCaptainEnergy * 100.0 : 0;
        public byte GalaxyGeneratorSpinAmount { get; set; }
        public Hangar? CurrentHangar => this.api.Hangars.FirstOrDefault(h => h.IsActive);
        public bool IsJumping { get; set; }
        public DateTime LastJump { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsFrozen { get; set; }
        public bool IsSaboteurDebuffed { get; set; }
        public bool HasSpawnedDestroyed { get; set; }
        public DateTime LogoutStartTime { get; set; }
        public bool IsLoggingOut => this.HasPremium ? (DateTime.UtcNow - this.LogoutStartTime).TotalSeconds < 6.5 : (DateTime.UtcNow - this.LogoutStartTime).TotalSeconds < 21.5;
        public DateTime CollectStartTime { get; set; }
        public bool IsCollecting => (DateTime.UtcNow - this.CollectStartTime).TotalMilliseconds < 1750;
        public bool IsCurrentlyBuying { get; set; } = false;
        public bool IsCurrentlyUpgradingSkylab { get; set; } = false;
        public bool IsCurrentlyBidding { get; set; } = false;
        public bool DispatchDay { get; set; } = false;
        public ConcurrentDictionary<int, DispatcherObj> ActiveDispatchers { get; private set; } = [];
        public ConcurrentDictionary<int, int> DispatcherInstantFinishActivationAmounts { get; private set; } = [];
        public bool HasRequestedShop = false;

        public bool HasApi => this.api != null;

        public Hero(Api api)
        {
            this.api = api;
            this.Cargo = new(api);
            this.CurrentLaser = MenuItem.Empty();
            this.CurrentRocket = MenuItem.Empty();
            this.CurrentRocketLauncher = MenuItem.Empty();
            this.CurrentAbilities = [];
        }

        public void Reset()
        {
            this.Title = string.Empty;
            this.IsJumping = false;
            this.CollectStartTime = DateTime.MinValue;
            this.LastAttackTime = DateTime.MinValue;
            this.LogoutStartTime = DateTime.MinValue;
            this.IsSaboteurDebuffed = false;
            this.RepairAmount = 0;
            this.PetStartTries = 0;
            this.IsCloaked = false;
            this.Ship = string.Empty;
            this.IsCurrentlyBuying = false;
            this.CurrentLaser = MenuItem.Empty();
            this.CurrentRocket = MenuItem.Empty();
            this.CurrentRocketLauncher = MenuItem.Empty();
            this.CurrentAbilities = [];
            this.api.AddBoosters([]);
        }

        public int GetHomeMapId => this.FactionId == 1 ? 1 : this.FactionId == 2 ? 5 : this.FactionId == 3 ? 9 : -1;
        public event PropertyChangedEventHandler? PropertyChanged;

        public string Username { get => this.username; set { this.username = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Username))); } }
        public int UserId { get => this.userId; set { this.userId = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.UserId))); } }
        public string ClanTag { get => this.clanTag; set { this.clanTag = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.ClanTag))); } }
        public string SessionId { get => this.sessionId; set { this.sessionId = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.SessionId))); } }
        public int FactionId { get => this.factionId; set { this.factionId = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.FactionId))); } }
        public string Ship { get => this.ship; set { this.ship = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Ship))); } }
        public int Level { get => this.level; set { this.level = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Level))); } }
        public bool HasPremium { get => this.hasPremium; set { this.hasPremium = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.HasPremium))); } }
        public int Speed { get => this.speed; set { this.speed = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Speed))); } }
        public int Config { get => this.config; set { this.config = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Config))); } }
        public string DroneFormation { get => this.droneFormation; set { this.droneFormation = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.DroneFormation))); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.DroneFormationReadable))); } }
        public string DroneFormationReadable => CommandConstants.DRONE_FORMATIONS.TryGetKey(this.api.Hero.DroneFormation, out var formationStr) ? formationStr : this.droneFormation;
        public Cargo Cargo { get => this.cargo; private set { this.cargo = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Cargo))); } }
        public MenuItem CurrentLaser { get => this.currentLaser; set { this.currentLaser = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CurrentLaser))); } }
        public MenuItem CurrentRocket { get => this.currentRocket; set { this.currentRocket = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CurrentRocket))); } }
        public MenuItem CurrentRocketLauncher { get => this.currentRocketLauncher; set { this.currentRocketLauncher = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CurrentRocketLauncher))); } }
    }
}
