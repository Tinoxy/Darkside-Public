using DarksideApi.DarkOrbit.Objects.Map;
using System.Numerics;

namespace DarksideApi.DarkOrbit.Movement
{
    internal readonly struct ZoneRect
    {
        public readonly float MinX, MinY, MaxX, MaxY;
        public readonly Vector2 Center;
        public readonly PoiZone Zone;

        public ZoneRect(PoiZone zone, float pad)
        {
            Zone = zone;
            MinX = Math.Min(Math.Min(zone.TopLeft.X, zone.TopRight.X), Math.Min(zone.BottomLeft.X, zone.BottomRight.X)) - pad;
            MaxX = Math.Max(Math.Max(zone.TopLeft.X, zone.TopRight.X), Math.Max(zone.BottomLeft.X, zone.BottomRight.X)) + pad;
            MinY = Math.Min(Math.Min(zone.TopLeft.Y, zone.TopRight.Y), Math.Min(zone.BottomLeft.Y, zone.BottomRight.Y)) - pad;
            MaxY = Math.Max(Math.Max(zone.TopLeft.Y, zone.TopRight.Y), Math.Max(zone.BottomLeft.Y, zone.BottomRight.Y)) + pad;
            Center = zone.Center;
        }

        public bool Contains(Vector2 p) => p.X >= MinX && p.X <= MaxX && p.Y >= MinY && p.Y <= MaxY;
    }

    internal static class PathfinderHelpers
    {
        public static ZoneRect[] PrecomputeZoneRects(List<PoiZone> zones, float pad)
        {
            var arr = new ZoneRect[zones.Count];
            for (int i = 0; i < zones.Count; i++)
                arr[i] = new ZoneRect(zones[i], pad);
            return arr;
        }

        // Alternative Segment-Check, verwendet ZoneRect[] (no allocations, less math per-zone)
        public static bool IsSegmentBlockedByAnyZone(Vector2 a, Vector2 b, ZoneRect[] zoneRects, float pad)
        {
            for (int zi = 0; zi < zoneRects.Length; zi++)
            {
                var zr = zoneRects[zi];

                // quick bbox reject using precomputed padded rect
                if (Math.Max(a.X, b.X) < zr.MinX || Math.Min(a.X, b.X) > zr.MaxX ||
                    Math.Max(a.Y, b.Y) < zr.MinY || Math.Min(a.Y, b.Y) > zr.MaxY)
                    continue;

                // If endpoint inside padded rect and not corner -> blocked
                bool aInside = zr.Contains(a);
                bool bInside = zr.Contains(b);
                if (aInside && !IsPointCornerOfZone(a, zr.Zone)) return true;
                if (bInside && !IsPointCornerOfZone(b, zr.Zone)) return true;

                // precise intersection test (reuse existing SegmentIntersectsRectangleWithParams)
                if (Pathfinder.SegmentIntersectsRectangleWithParams(a, b, zr.MinX, zr.MinY, zr.MaxX, zr.MaxY, out float tEnter, out float tExit))
                {
                    float entry = Math.Max(tEnter, 0f);
                    float exit = Math.Min(tExit, 1f);
                    if (entry > exit) continue;
                    if (entry > 0f && exit < 1f) return true;

                    if ((Math.Abs(tEnter) <= 1e-4f && IsPointCornerOfZone(a, zr.Zone)) ||
                        (Math.Abs(tExit - 1f) <= 1e-4f && IsPointCornerOfZone(b, zr.Zone)))
                        continue;

                    if (!(Math.Abs(tEnter) <= 1e-4f || Math.Abs(tExit - 1f) <= 1e-4f))
                        return true;

                    if (!(IsPointCornerOfZone(a, zr.Zone) || IsPointCornerOfZone(b, zr.Zone)))
                        return true;
                }

                // midpoint safety
                var midpoint = (a + b) / 2;
                if (zr.Contains(midpoint)) return true;
            }
            return false;
        }

        // proxy to internal helpers in Pathfinder (move small helpers into internal PathfinderInternal)
        private static bool IsPointCornerOfZone(Vector2 point, PoiZone z) => Pathfinder.IsPointCornerOfZone(point, z);
    }
}