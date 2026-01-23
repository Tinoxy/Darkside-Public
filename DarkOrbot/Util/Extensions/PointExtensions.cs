using System.Numerics;
using System.Windows;

namespace DarkOrbot.Util.Extensions
{
    public static class PointExtensions
    {
        public static Vector2 ToVector2(this Point point)
        {
            return new Vector2((float)point.X, (float)point.Y);
        }
    }
}
