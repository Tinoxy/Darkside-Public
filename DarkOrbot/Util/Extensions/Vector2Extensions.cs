using System.Numerics;
using System.Windows;

namespace DarkOrbot.Util.Extensions
{
    public static class Vector2Extensions
    {
        public static Point ToDrawingPoint(this Vector2 vector)
        {
            return new Point(vector.X, vector.Y);
        }
    }
}
