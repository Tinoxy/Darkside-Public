using DarksideApi.Util;
using System.Numerics;

namespace DarksideApi.DarkOrbit.Objects.Boxes
{
    public class Collectible(Api api, string hash, int x, int y) : IGameObject
    {
        //Api
        private readonly Api api = api;
        public bool HasApi => this.api != null;

        //Box Info
        public string Hash { get; private set; } = hash;
        public int X { get; set; } = x;
        public int Y { get; set; } = y;
        public Vector2 Position => new(this.X, this.Y);
        public double CurrentDistance => this.HasApi ? HelpTools.CalculateDistance(this.api.Hero.X, this.api.Hero.Y, this.X, this.Y) : double.MaxValue;
        public double TargetDistance => this.HasApi ? HelpTools.CalculateDistance(this.api.Hero.TargetX, this.api.Hero.TargetY, this.X, this.Y) : double.MaxValue;
        public double PathDistance => this.HasApi ? HelpTools.CalculateDistance(this.api.Hero.LastPathElement, this.Position) : double.MaxValue;
        public string DisplayType => (this is Box b) ? b.Type : (this is Ore o) ? o.Type.ToString() : "Item";

        //Temp
        public DateTime tempStartCollectTime;
        public DateTime tempStartMoveTime;
        public bool tempSelectedAsCloser;
        public int tempMoveTries;

        //Missclick
        private int missedX = HelpTools.GetChance(34) ? x + Random.Shared.Next(-7, 6) : x;
        private int missedY = HelpTools.GetChance(32) ? y + Random.Shared.Next(-4, 5) : y;

        public Collectible GetPosition()
        {
            var result = new Collectible(null!, null!, this.missedX, this.missedY);

            if (this.missedX != this.X
                || this.missedY != this.Y)
            {
                this.api.WriteLog("Misclicked next " + this.DisplayType);
                this.tempMoveTries -= 1;
            }

            this.missedX = this.X;
            this.missedY = this.Y;

            return result;
        }

        public bool IsOnTarget()
        {
            return this.PathDistance <= 10;
        }

        public void IncreaseMoveTries()
        {
            if (this.tempMoveTries > 3)
            {
                this.api.Logging.Log("Too many move tries for " + this.DisplayType + ", marking as invalid.");
                this.api.InvalidCollectibles.Add(this.Hash);
                return;
            }
            this.tempMoveTries += 1;
        }

        public static Collectible Empty()
        {
            return new(null!, string.Empty, 0, 0);
        }
    }
}
