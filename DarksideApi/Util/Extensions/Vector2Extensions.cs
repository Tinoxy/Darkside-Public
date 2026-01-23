using System.Numerics;

namespace DarksideApi.Util.Extensions
{
    public static class Vector2Extensions
    {
        public static double Angle(this Vector2 vector, Vector2 to)
        {
            return Math.Atan2(vector.Y - to.Y, vector.X - to.X);
        }

        public static Vector2 ToAngle(this Vector2 center, double angle, double distance)
        {
            return new Vector2
            {
                X = (float)(center.X - Math.Cos(angle) * distance),
                Y = (float)(center.Y - Math.Sin(angle) * distance)
            };
        }

        public static double DistanceTo(this Vector2 from, Vector2 to)
        {
            return HelpTools.CalculateDistance(from, to);
        }

        public static Vector2 Of(this Vector2 from, double angle, double distance)
        {
            return new((float)(from.X - Math.Cos(angle) * distance), (float)(from.Y - Math.Sin(angle) * distance));
        }
    }
}
