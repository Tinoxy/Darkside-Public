using DarksideApi.Util;
using System.Numerics;

namespace DarksideApi.DarkOrbit.Objects.Pets
{
    public class Pet(Api api, int userId, int ownerId, int level, int gear, int x, int y, string name, string clanTag, bool visible) : IGameObject
    {
        private readonly Api api = api;

        public int UserId { get; set; } = userId;
        public int OwnerId { get; set; } = ownerId;
        public string Name { get; set; } = name;
        public string ClanTag { get; set; } = clanTag;
        public int Level { get; set; } = level;
        public int Gear { get; set; } = gear;
        public int ModeOptions { get; set; } = -1;
        public int X { get; set; } = x;
        public int Y { get; set; } = y;
        public Vector2 Position => new(this.X, this.Y);
        public int TargetX { get; set; } = x;
        public int TargetY { get; set; } = y;
        public bool IsVisible { get; set; } = visible;
        public int Speed { get; set; }
        public int Fuel { get; set; }
        public int MaxFuel { get; set; }
        public int HitPoints { get; set; }
        public int MaxHitPoints { get; set; }
        public double CurrentHitpointsPercentage => this.MaxHitPoints > 0 ? (double)(this.HitPoints / (double)this.MaxHitPoints * 100d) : 0d;
        public double FuelPercentage => this.Fuel > 0 && this.MaxFuel > 0 ? (double)(this.Fuel / (double)this.MaxFuel * 100d) : 1d;
        public double CurrentDistance => this.HasApi ? HelpTools.CalculateDistance(this.api.Hero.X, this.api.Hero.Y, this.X, this.Y) : double.MaxValue;
        public double TargetDistance => this.HasApi ? HelpTools.CalculateDistance(this.api.Hero.TargetX, this.api.Hero.TargetY, this.X, this.Y) : double.MaxValue;
        public double PathDistance => this.HasApi ? HelpTools.CalculateDistance(this.api.Hero.LastPathElement, this.Position) : double.MaxValue;
        public bool HasApi => this.api != null;
        public DateTime LastAttackTime { get; set; }
        public bool IsAttacking => (DateTime.UtcNow - this.LastAttackTime).TotalSeconds < 5;
        public Dictionary<int, PetGear> Gears { get; set; } = [];

        public static Pet Empty()
        {
            return new Pet(null, 0, 0, 0, 0, 0, 0, "", "", false);
        }
    }
}
