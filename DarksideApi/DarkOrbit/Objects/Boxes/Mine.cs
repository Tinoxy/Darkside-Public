using DarksideApi.Util;
using System.Numerics;

namespace DarksideApi.DarkOrbit.Objects.Boxes
{
    public class Mine : IGameObject
    {
        private readonly Api api;

        public string Hash { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Vector2 Position => new(this.X, this.Y);
        public int Type { get; private set; }
        public int TempAvoidTries { get; set; } = 0;
        public double CurrentDistance => this.HasApi ? HelpTools.CalculateDistance(this.api.Hero.X, this.api.Hero.Y, this.X, this.Y) : double.MaxValue;
        public double TargetDistance => this.HasApi ? HelpTools.CalculateDistance(this.api.Hero.TargetX, this.api.Hero.TargetY, this.X, this.Y) : double.MaxValue;
        public double PathDistance => this.HasApi ? HelpTools.CalculateDistance(this.api.Hero.LastPathElement, this.Position) : double.MaxValue;
        public bool HasApi => this.api != null;

        public bool IsFrozenLabyrinthMine => this.Type == 21;

        public Mine(Api api, string hash, int x, int y, int type)
        {
            this.api = api;
            this.Hash = hash;
            this.X = x;
            this.Y = y;
            this.Type = type;
        }

        public static Mine Empty()
        {
            return new(null, string.Empty, 0, 0, -1);
        }
    }
}
