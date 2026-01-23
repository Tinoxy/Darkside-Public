using DarksideApi.DarkOrbit.Commands;
using DarksideApi.Util;
using DarksideApi.Util.Extensions;
using System.Numerics;

namespace DarksideApi.DarkOrbit.Objects.Map
{
    public class PoiZone : IGameObject
    {
        private readonly Api api;

        public string ZoneId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Vector2 Center => new(this.TopLeft.X + (this.Width / 2), this.TopLeft.Y + (this.Height / 2));
        public Vector2 Position => new(this.X, this.Y);
        public int Shape { get; set; }
        public bool IsActive { get; set; }
        public bool IsInverse { get; set; }
        public int Type { get; set; }
        public float Radius { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }
        public Vector2 TopLeft { get; private set; }
        public Vector2 TopRight { get; private set; }
        public Vector2 BottomLeft { get; private set; }
        public Vector2 BottomRight { get; private set; }
        public Vector2 LargeTopLeft => new(this.TopLeft.X - 50, this.TopLeft.Y - 50);
        public Vector2 LargeTopRight => new(this.TopRight.X + 50, this.TopRight.Y - 50);
        public Vector2 LargeBottomLeft => new(this.BottomLeft.X - 50, this.BottomLeft.Y + 50);
        public Vector2 LargeBottomRight => new(this.BottomRight.X + 50, this.BottomRight.Y + 50);
        private List<int> Coordinates { get; set; }
        public List<Vector2> CornerPositions { get; set; }
        private List<Vector2> LargeCornerPositions { get; set; }
        public List<LineIntersectionTool.Line> Borders { get; set; }
        public PoiZone ConnectedZone { get; private set; }
        public double CurrentDistance => this.HasApi ? this.GetDistance(this.api.Hero.X, this.api.Hero.Y) : double.MaxValue;
        public double TargetDistance => this.HasApi ? this.GetDistance(this.api.Hero.TargetX, this.api.Hero.TargetY) : double.MaxValue;
        public double PathDistance => this.HasApi ? HelpTools.CalculateDistance(this.api.Hero.LastPathElement, this.Position) : double.MaxValue;
        public bool HasApi => this.api != null;
        public bool HasConnectedZone => this.ConnectedZone != null;
        public bool IsNoAccess => this.Type == CommandConstants.POI_TYPE_NO_ACCESS || this.Type == CommandConstants.POI_TYPE_FACTION_NO_ACCESS || this.Type == CommandConstants.POI_TYPE_NO_ACCESS_CUSTOM;
        public bool IsNoAccessCustom => this.Type == CommandConstants.POI_TYPE_NO_ACCESS_CUSTOM;

        public PoiZone(Api api, string zoneId, int x, int y, int shape, bool isActive, bool inverse, int type, List<int> coordinates)
        {
            this.api = api;
            this.ZoneId = zoneId;
            this.X = x;
            this.Y = y;
            this.Shape = shape;
            this.IsActive = isActive;
            this.IsInverse = inverse;
            this.Type = type;
            this.Coordinates = coordinates;
            this.CornerPositions = [];
            this.LargeCornerPositions = [];
            this.ConnectedZone = null!;
            this.Borders = [];

            if (shape < 0)
            {
                return;
            }

            this.CreatePositions();
            this.CreateBorders();
            this.GetLargeCornerPositions();
        }

        public int GetQuarterToZone(int x, int y)
        {
            if (x > this.TopRight.X)
            {
                return y < this.TopRight.Y
                    ? 2
                    : y > this.BottomRight.Y
                    ? 3
                    : HelpTools.CalculateDistance(x, y, this.TopRight.X, this.TopRight.Y) < HelpTools.CalculateDistance(x, y, this.BottomRight.X, this.BottomRight.Y) ? 2 : 3;
            }
            if (x < this.TopRight.X)
            {
                if (y < this.TopRight.Y)
                    return HelpTools.CalculateDistance(x, y, this.TopRight.X, this.TopRight.Y) < HelpTools.CalculateDistance(x, y, this.TopLeft.X, this.TopLeft.Y) ? 2 : 1;
                if (y > this.BottomRight.Y)
                    return HelpTools.CalculateDistance(x, y, this.BottomLeft.X, this.BottomLeft.Y) < HelpTools.CalculateDistance(x, y, this.BottomRight.X, this.BottomRight.Y) ? 4 : 3;
            }
            if (x < this.TopLeft.X)
            {
                return y < this.TopLeft.Y
                    ? 1
                    : y > this.BottomLeft.Y
                    ? 4
                    : HelpTools.CalculateDistance(x, y, this.TopLeft.X, this.TopLeft.Y) < HelpTools.CalculateDistance(x, y, this.BottomLeft.X, this.BottomLeft.Y) ? 1 : 4;
            }
            if (x > this.TopLeft.X)
            {
                if (y < this.TopLeft.Y)
                    return HelpTools.CalculateDistance(x, y, this.TopRight.X, this.TopRight.Y) < HelpTools.CalculateDistance(x, y, this.TopLeft.X, this.TopLeft.Y) ? 2 : 1;
                if (y > this.BottomLeft.Y)
                    return HelpTools.CalculateDistance(x, y, this.BottomLeft.X, this.BottomLeft.Y) < HelpTools.CalculateDistance(x, y, this.BottomRight.X, this.BottomRight.Y) ? 4 : 3;
            }
            return this.CornerPositions.IndexOf(this.CornerPositions.OrderBy(c => HelpTools.CalculateDistance(x, y, c.X, c.Y)).FirstOrDefault()) + 1;
        }

        private double GetDistance(int x, int y)
        {
            if (this.Shape == CommandConstants.POI_SHAPE_RECTANGLE)
            {
                if (this.IsInZone(x, y))
                {
                    return 0;
                }
                var minX = this.CornerPositions.ElementAt(0)[0];
                var minY = this.CornerPositions.ElementAt(0)[1];
                var maxX = minX + this.Width;
                var maxY = minY + this.Height;
                double[] dXL = [minX - x, 0, x - maxX];
                double[] dYL = [minY - y, 0, y - maxY];
                var dx = dXL.Max();
                var dy = dYL.Max();
                //var dx = Math.Max(minX - x, Math.Max(0, x - maxX));
                //var dy = Math.Max(minY - y, Math.Max(0, y - maxY));
                return Math.Sqrt(dx * dx + dy * dy);
            }
            return this.Shape == CommandConstants.POI_SHAPE_CIRCLE
                ? HelpTools.CalculateDistance(x, y, this.X, this.Y) - this.Radius
                : HelpTools.CalculateDistance(x, y, this.X, this.Y);
        }

        public bool IsInZone(Vector2 point)
        {
            return this.IsInZone((int)point.X, (int)point.Y);
        }

        public bool IsInNoAccessZone(int x, int y)
        {
            return this.IsInNoAccessZone(new(x, y));
        }

        public bool IsInNoAccessZone(Vector2 point)
        {
            return this.IsNoAccess && this.IsInZone(point);
        }

        public bool IsInZone(int x, int y)
        {
            if (this.Shape == CommandConstants.POI_SHAPE_RECTANGLE)
            {
                if (x >= this.LargeTopLeft.X && y >= this.LargeTopLeft.Y)
                {
                    return (x <= this.LargeBottomRight.X && y <= this.LargeBottomRight.Y);
                }
            }
            if (this.Shape == CommandConstants.POI_SHAPE_POLYGON)
            {
                bool result = false;
                int j = this.CornerPositions.Count - 1;
                for (int i = 0; i < this.CornerPositions.Count; i++)
                {
                    if (this.CornerPositions[i].Y < y && this.CornerPositions[j].Y >= y || this.CornerPositions[j].Y < y && this.CornerPositions[i].Y >= y)
                    {
                        if (this.CornerPositions[i].X + (y - this.CornerPositions[i].Y) / (this.CornerPositions[j].Y - this.CornerPositions[i].Y) * (this.CornerPositions[j].X - this.CornerPositions[i].X) < x)
                        {
                            result = !result;
                        }
                    }
                    j = i;
                }
                return result;
            }
            return this.Shape == CommandConstants.POI_SHAPE_CIRCLE && HelpTools.CalculateDistance(x, y, this.X, this.Y) <= this.Radius;
        }

        public Vector2 GetPointOutOfZone(Vector2 corner)
        {
            var closestCorner = this.CornerPositions.OrderBy(c => HelpTools.CalculateDistance(c.X, c.Y, corner.X, corner.Y)).First();
            var random = Random.Shared;
            var point = new Vector2(0, 0);
            if (closestCorner == this.TopLeft)
            {
                point = new Vector2(random.Next((int)LargeTopLeft.X, (int)closestCorner.X - 20), random.Next((int)LargeTopLeft.Y, (int)closestCorner.Y - 20));
            } else
            if (closestCorner == this.TopRight)
            {
                point = new Vector2(random.Next((int)closestCorner.X + 20, (int)LargeTopRight.X), random.Next((int)LargeTopRight.Y, (int)closestCorner.Y - 20));
            } else
            if (closestCorner == this.BottomLeft)
            {
                point = new Vector2(random.Next((int)LargeBottomLeft.X, (int)closestCorner.X - 20), random.Next((int)closestCorner.Y + 20, (int)LargeBottomLeft.Y));
            } else
            {
                point = new Vector2(random.Next((int)closestCorner.X + 20, (int)LargeBottomRight.X), random.Next((int)closestCorner.Y + 20, (int)LargeBottomRight.Y));
            }
            if (this.HasConnectedZone && this.ConnectedZone.IsInZone(point))
            {
                point = this.ConnectedZone.GetPointOutOfZone(this.ConnectedZone.GetNextCorner(point, corner));
            }
            return point;
        }

        public bool Intersects(Vector2 from, Vector2 to)
        {
            var path = new LineIntersectionTool.Line(from, to);
            foreach (LineIntersectionTool.Line border in this.Borders)
            {
                if (LineIntersectionTool.FindIntersection(path, border) != LineIntersectionTool.FAILED)
                    return true;
            }
            return false;
        }

        public Vector2 GetNextCorner(Vector2 position, Vector2 targetPosition)
        {
            var closestCorner = this.CornerPositions.OrderBy(c => HelpTools.CalculateDistance(position.X, position.Y, c.X, c.Y)).FirstOrDefault();
            var nextCorner = this.CornerPositions.OrderBy(c => HelpTools.CalculateDistance(targetPosition.X, targetPosition.Y, c.X, c.Y)).FirstOrDefault();
            return this.CornerPositions.ElementAtOrDefault(((this.CornerPositions.IndexOf(closestCorner) + this.CornerPositions.IndexOf(nextCorner)) / 2) % 4);
        }

        public Vector2 GetClosestCornerToTarget(Vector2 targetPosition)
        {
            return this.CornerPositions.OrderBy(c => HelpTools.CalculateDistance(targetPosition.X, targetPosition.Y, c.X, c.Y)).FirstOrDefault();
        }

        public Vector2 GetFarthestCornerToTarget(Vector2 targetPosition)
        {
            return this.CornerPositions.OrderBy(c => HelpTools.CalculateDistance(targetPosition.X, targetPosition.Y, c.X, c.Y)).LastOrDefault();
        }

        public void SetConnectedZone()
        {
            var zones = this.api.POIZones.Values.Where(p => p.IsNoAccess);
            foreach (var zone in zones)
            {
                if (zone == this)
                    continue;

                foreach (var corner in this.LargeCornerPositions)
                {
                    if (zone.IsInZone(corner))
                    {
                        this.ConnectedZone = zone;
                        return;
                    }
                }
            }
        }

        public void RemoveConnectedZone()
        {
            this.ConnectedZone = null!;
        }

        private void CreateBorders()
        {
            //return [topLeft, topRight, bottomLeft, bottomRight];
            if (this.CornerPositions.Count <= 1)
            {
                return;
            }
            var borderTop = new LineIntersectionTool.Line(this.CornerPositions.ElementAt(0), this.CornerPositions.ElementAt(1));
            var borderLeft = new LineIntersectionTool.Line(this.CornerPositions.ElementAt(0), this.CornerPositions.ElementAt(2));

            var borderBottom = new LineIntersectionTool.Line(this.CornerPositions.ElementAt(2), this.CornerPositions.ElementAt(3));
            var borderRight = new LineIntersectionTool.Line(this.CornerPositions.ElementAt(1), this.CornerPositions.ElementAt(3));

            this.Borders.Add(borderTop);
            this.Borders.Add(borderLeft);
            this.Borders.Add(borderBottom);
            this.Borders.Add(borderRight);
        }

        private void CreatePositions()
        {
            List<Vector2> result = [];
            var rad = float.NaN;
            if (this.Shape == CommandConstants.POI_SHAPE_CIRCLE)
            {
                this.Radius = this.Coordinates.ElementAt(2);
                rad = this.Radius;
                this.X = this.Coordinates.ElementAt(0);
                this.Y = this.Coordinates.ElementAt(1);
                result.Add(new(this.X, this.Y));
            }
            else
            {
                List<int> copyOfCoordiantes = [.. this.Coordinates];
                while (copyOfCoordiantes.Count >= 2)
                {
                    result.Add(new(copyOfCoordiantes.Pop(), copyOfCoordiantes.Pop()));
                }
            }
            this.CornerPositions = result;
            if (float.IsNaN(rad))
            {
                this.CornerPositions = this.FindAllCornerPoints();
                this.Width = Math.Abs(this.CornerPositions[0].X - this.CornerPositions[1].X);
                this.Height = Math.Abs(this.CornerPositions[0].Y - this.CornerPositions[2].Y);
            }
        }

        private List<Vector2> GetLargeCornerPositions()
        {
            return [LargeTopLeft, LargeTopRight, LargeBottomLeft, LargeBottomRight];
        }

        private List<Vector2> FindAllCornerPoints()
        {
            Vector2 topLeft = this.CornerPositions[0];
            Vector2 topRight = this.CornerPositions[0];
            Vector2 bottomLeft = this.CornerPositions[0];
            Vector2 bottomRight = this.CornerPositions[0];
            var i = 0;
            while (i < this.CornerPositions.Count)
            {
                Vector2 _loc6_;
                if ((_loc6_ = this.CornerPositions[i]).X < topLeft.X || _loc6_.Y < topLeft.Y)
                {
                    topLeft = _loc6_;
                }
                if (_loc6_.X > topRight.X || _loc6_.Y < topRight.Y)
                {
                    topRight = _loc6_;
                }
                if (_loc6_.X < bottomLeft.X || _loc6_.Y > bottomLeft.Y)
                {
                    bottomLeft = _loc6_;
                }
                if (_loc6_.X > bottomRight.X || _loc6_.Y > bottomRight.Y)
                {
                    bottomRight = _loc6_;
                }
                i++;
            }

            this.TopLeft = topLeft;
            this.TopRight = topRight;
            this.BottomLeft = bottomLeft;
            this.BottomRight = bottomRight;

            return [topLeft, topRight, bottomLeft, bottomRight];
        }

        public static PoiZone Empty()
        {
            return new PoiZone(null!, string.Empty, 0, 0, -1, false, false, 0, []);
        }
    }
}
