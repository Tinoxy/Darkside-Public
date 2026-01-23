using System.Numerics;

namespace DarksideApi.Util
{
    public class LineIntersectionTool
    {
        public class Line
        {
            public float x1;
            public float y1;
            public float x2;
            public float y2;
            public Vector2 Start => new(x1, y1);
            public Vector2 End => new(x2, y2);
            public double Distance => HelpTools.CalculateDistance(Start, End);

            public Line(Vector2 start, Vector2 end)
            {
                this.x1 = start.X;
                this.y1 = start.Y;
                this.x2 = end.X;
                this.y2 = end.Y;
            }

            public Line(float x1, float y1, float x2, float y2)
            {
                this.x1 = x1;
                this.y1 = y1;
                this.x2 = x2;
                this.y2 = y2;
            }

            public IList<Vector2> SplitLine(int count)
            {

                count += 1;

                var d = Math.Sqrt((Start.X - End.X) * (Start.X - End.X) + (Start.Y - End.Y) * (Start.Y - End.Y)) / count;
                var fi = Math.Atan2(End.Y - Start.Y, End.X - Start.X);

                List<Vector2> points = new List<Vector2>(count + 1);

                for (int i = 0; i <= count; ++i)
                    points.Add(new ((int)(Start.X + i * d * Math.Cos(fi)), (int)(Start.Y + i * d * Math.Sin(fi))));

                return points;
            }
        }

        public static readonly Vector2 FAILED = new(-1, -1);

        public static Vector2 FindIntersection(Line lineA, Line lineB, double tolerance = 0.0001)
        {
            float x1 = lineA.x1, y1 = lineA.y1;
            float x2 = lineA.x2, y2 = lineA.y2;

            float x3 = lineB.x1, y3 = lineB.y1;
            float x4 = lineB.x2, y4 = lineB.y2;

            // equations of the form x=c (two vertical lines) with overlapping
            if (Math.Abs(x1 - x2) < tolerance && Math.Abs(x3 - x4) < tolerance && Math.Abs(x1 - x3) < tolerance)
            {
                //throw new Exception("Both lines overlap vertically, ambiguous intersection points.");
                return FAILED;
            }

            //equations of the form y=c (two horizontal lines) with overlapping
            if (Math.Abs(y1 - y2) < tolerance && Math.Abs(y3 - y4) < tolerance && Math.Abs(y1 - y3) < tolerance)
            {
                //throw new Exception("Both lines overlap horizontally, ambiguous intersection points.");
                return FAILED;
            }

            //equations of the form x=c (two vertical parallel lines)
            if (Math.Abs(x1 - x2) < tolerance && Math.Abs(x3 - x4) < tolerance)
            {
                //return default (no intersection)
                return FAILED;
            }

            //equations of the form y=c (two horizontal parallel lines)
            if (Math.Abs(y1 - y2) < tolerance && Math.Abs(y3 - y4) < tolerance)
            {
                //return default (no intersection)
                return FAILED;
            }

            //general equation of line is y = mx + c where m is the slope
            //assume equation of line 1 as y1 = m1x1 + c1 
            //=> -m1x1 + y1 = c1 ----(1)
            //assume equation of line 2 as y2 = m2x2 + c2
            //=> -m2x2 + y2 = c2 -----(2)
            //if line 1 and 2 intersect then x1=x2=x & y1=y2=y where (x,y) is the intersection point
            //so we will get below two equations 
            //-m1x + y = c1 --------(3)
            //-m2x + y = c2 --------(4)

            float x, y;

            //lineA is vertical x1 = x2
            //slope will be infinity
            //so lets derive another solution
            if (Math.Abs(x1 - x2) < tolerance)
            {
                //compute slope of line 2 (m2) and c2
                var m2 = (y4 - y3) / (x4 - x3);
                var c2 = -m2 * x3 + y3;

                //equation of vertical line is x = c
                //if line 1 and 2 intersect then x1=c1=x
                //subsitute x=x1 in (4) => -m2x1 + y = c2
                // => y = c2 + m2x1 
                x = x1;
                y = c2 + m2 * x1;
            }
            //lineB is vertical x3 = x4
            //slope will be infinity
            //so lets derive another solution
            else if (Math.Abs(x3 - x4) < tolerance)
            {
                //compute slope of line 1 (m1) and c2
                var m1 = (y2 - y1) / (x2 - x1);
                var c1 = -m1 * x1 + y1;

                //equation of vertical line is x = c
                //if line 1 and 2 intersect then x3=c3=x
                //subsitute x=x3 in (3) => -m1x3 + y = c1
                // => y = c1 + m1x3 
                x = x3;
                y = c1 + m1 * x3;
            }
            //lineA & lineB are not vertical 
            //(could be horizontal we can handle it with slope = 0)
            else
            {
                //compute slope of line 1 (m1) and c2
                var m1 = (y2 - y1) / (x2 - x1);
                var c1 = -m1 * x1 + y1;

                //compute slope of line 2 (m2) and c2
                var m2 = (y4 - y3) / (x4 - x3);
                var c2 = -m2 * x3 + y3;

                //solving equations (3) & (4) => x = (c1-c2)/(m2-m1)
                //plugging x value in equation (4) => y = c2 + m2 * x
                x = (c1 - c2) / (m2 - m1);
                y = c2 + m2 * x;

                //verify by plugging intersection point (x, y)
                //in orginal equations (1) & (2) to see if they intersect
                //otherwise x,y values will not be finite and will fail this check
                if (!(Math.Abs(-m1 * x + y - c1) < tolerance
                    && Math.Abs(-m2 * x + y - c2) < tolerance))
                {
                    //return default (no intersection)
                    return FAILED;
                }
            }

            //x,y can intersect outside the line segment since line is infinitely long
            //so finally check if x, y is within both the line segments
            if (IsInsideLine(lineA, x, y) &&
                IsInsideLine(lineB, x, y))
            {
                return new(x, y);
            }

            //return default (no intersection)
            return FAILED;

        }

        // Returns true if given point(x,y) is inside the given line segment
        private static bool IsInsideLine(Line line, float x, float y)
        {
            return (x >= line.x1 && x <= line.x2
                        || x >= line.x2 && x <= line.x1)
                   && (y >= line.y1 && y <= line.y2
                        || y >= line.y2 && y <= line.y1);
        }
    }
}
