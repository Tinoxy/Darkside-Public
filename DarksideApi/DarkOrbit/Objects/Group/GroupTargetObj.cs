using DarksideApi.Util;
using System.Numerics;

namespace DarksideApi.DarkOrbit.Objects.Group
{
    public class GroupTargetObj : IGameObject
    {
        private readonly Api api;

        public long Hp { get; set; }
        public long MaxHp { get; set; }
        public int Shield { get; set; }
        public int MaxShield { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Vector2 Position => new(this.X, this.Y);
        public string Name { get; set; }

        public double CurrentDistance => this.HasApi ? HelpTools.CalculateDistance(this.api.Hero.X, this.api.Hero.Y, this.X, this.Y) : double.MaxValue;
        public double TargetDistance => this.HasApi ? HelpTools.CalculateDistance(this.api.Hero.TargetX, this.api.Hero.TargetY, this.X, this.Y) : double.MaxValue;
        public double PathDistance => this.HasApi ? HelpTools.CalculateDistance(this.api.Hero.LastPathElement, this.Position) : double.MaxValue;
        public double HitpointsPercentage => this.MaxHp > 0 ? this.Hp / (double)this.MaxHp * 100.0 : 0;
        public bool HasApi => this.api != null;

        public GroupTargetObj(Api api, string name, long hp, long maxHp, int shield, int maxShield, int x = -1, int y = -1)
        {
            this.api = api;
            this.Name = name;
            this.Hp = hp;
            this.MaxHp = maxHp;
            this.Shield = shield;
            this.MaxShield = maxShield;
            this.X = x;
            this.Y = y;
        }

        public override string ToString() => $"{this.Name} (HP: {this.Hp}/{this.MaxHp}, Shield: {this.Shield}/{this.MaxShield}), Position: {this.X}/{this.Y}";
    }
}
