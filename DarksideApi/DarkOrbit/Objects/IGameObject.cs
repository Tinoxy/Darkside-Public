using System.Numerics;

namespace DarksideApi.DarkOrbit.Objects
{
    public interface IGameObject
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Vector2 Position { get; }

        public double CurrentDistance { get; }
        public double TargetDistance { get; }
        public double PathDistance { get; }

        public bool HasApi { get; }
    }
}
