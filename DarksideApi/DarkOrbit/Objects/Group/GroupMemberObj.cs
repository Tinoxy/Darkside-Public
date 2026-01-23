using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Objects.Ship;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;

namespace DarksideApi.DarkOrbit.Objects.Group
{
    public class GroupMemberObj : INotifyPropertyChanged
    {
        protected readonly Api api;

        public int Id => this.Player.UserId;
        public string DisplayName => this.Player.Username;
        public string GetStatusAsString
        {
            get
            {
                if (this.IsInRaidGate)
                {
                    return "Gate-Stage: " + this.RaidGateStage;
                }
                if (this.Target != null && this.Target.Name != string.Empty && this.Target.Hp > 0)
                {
                    return $"{this.Target.Name} ({(this.MapId == this.api.Map.Id ? ((int)this.Target.CurrentDistance+"d") : ((int)this.Target.HitpointsPercentage + "%"))})";
                }
                return "Roaming";
            }
        }
        public string DisplayHitpoints => $"{(int)this.HitpointsPercentage}%";
        public string DisplayMap => CommandConstants.MAP_NAMES.TryGetValue(this.MapId, out var mapName) ? mapName : "Unknown";
        public string DisplayPosition => $"{(int)this.Position.X / 100}/{(int)this.Position.Y / 100}";

        public int MapId { get; set; }
        public GroupTargetObj Target { get; set; }
        public Player Player { get; set; }

        public bool IsInRaidGate { get; set; }
        public int RaidGateStage { get; set; }

        public Vector2 Position => this.Player.Position;

        public double CurrentDistance => this.Player.CurrentDistance;
        public double TargetDistance => this.Player.TargetDistance;

        public double HitpointsPercentage => this.Player.MaxHitpoints > 0 ? this.Player.Hitpoints / (double)this.Player.MaxHitpoints * 100.0 : 0;
        public double ShieldPercentage => this.Player.MaxShield > 0 ? this.Player.Shield / (double)this.Player.MaxShield * 100.0 : 0;
        public double NanoShieldPercentage => this.Player.MaxNanoShield > 0 ? this.Player.NanoShield / (double)this.Player.MaxNanoShield * 100.0 : 0;

        public readonly byte R;
        public readonly byte G;
        public readonly byte B;
        public string ColorCode => $"#{this.R:X2}{this.G:X2}{this.B:X2}";

        public event PropertyChangedEventHandler? PropertyChanged;

        public GroupMemberObj(Api api, Player player, GroupTargetObj target, int mapId)
        {
            this.api = api;
            this.Player = player;
            this.Target = target;
            this.MapId = mapId;

            this.R = (byte)Random.Shared.Next(0, 255);
            this.G = (byte)Random.Shared.Next(0, 255);
            this.B = (byte)Random.Shared.Next(0, 255);

            Debug.WriteLine("New Group member: " + player.Username);
        }

        public void Updated()
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.GetStatusAsString)));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.DisplayHitpoints)));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MapId)));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Player)));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Target)));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.DisplayMap)));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.DisplayPosition)));

            Debug.WriteLine("Group Member Updated: " + this.Player.ToString() + " -> " + this.Target.ToString());
        }
    }
}
