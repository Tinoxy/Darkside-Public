using DarksideApi.DarkOrbit.Objects.Map;
using System.Diagnostics;
using System.Numerics;

namespace DarksideApi.DarkOrbit.Movement
{
    public static class Pathfinder
    {
        //Higher Offset -> Corners out of zone
        private const float CornerOffset = 12.0f;
        private const float Epsilon = 0.0001f;
        private const float CornerTolerance = 2.0f;
        private const float MinStepDistance = 1.0f; //minimum distance between two path points

        //Overflow protection / Out of Memory prevention
        private const int MaxPathNodesForSmoothing = 4000; //if path gets too long, skip smoothing
        private const int TargetSampleSize = 1000; //max samples

        public static LinkedList<Vector2> FindShortestPath(Vector2 start, Vector2 target, IEnumerable<PoiZone> obstacles, Map map, float zonePadding = 5.0f)
        {
            var pad = Math.Abs(zonePadding);
            var zones = obstacles?.Where(z => z.IsNoAccess).ToList() ?? new List<PoiZone>();

            if (zones.Count == 0)
            {
                Debug.WriteLine("Pathfinder: keine Hindernisse - gebe Direktverbindung zurück.");
                return new LinkedList<Vector2>(new[] { start, target });
            }

            //Precompute ZoneRects
            var zoneRects = zones.Select(z => new ZoneRect(z, pad)).ToArray();

            //If start/target in NoAccess -> Get point outside of zone
            foreach (var z in zones)
            {
                if (z.IsInNoAccessZone((int)start.X, (int)start.Y))
                    start = z.GetPointOutOfZone(start);
                if (z.IsInNoAccessZone((int)target.X, (int)target.Y))
                    target = z.GetPointOutOfZone(target);
            }

            if (!IsSegmentBlockedByAnyZone(start, target, zoneRects))
            {
                Debug.WriteLine("Pathfinder: Direct Route Start->Target possible!");
                return new LinkedList<Vector2>(new[] { start, target });
            }

            //Visibility graph (using Corners)
            var nodes = BuildCornerNodes(zones, map, includeShift: true, pad: pad);
            nodes.Insert(0, target);
            nodes.Insert(0, start);
            var path = RunVisibilityDijkstra(nodes, zoneRects, pad);
            if (path != null)
            {
                var fixedPath = ValidateAndFixPath(path, zoneRects, pad, map);
                if (fixedPath != null)
                {
                    Debug.WriteLine("Pathfinder: Found route using visibility");
                    return fixedPath;
                }
            }

            //Fallbacks (moved Corners, detour points)
            var fallbackPad = Math.Max(pad, 12.0f);
            var fallbackNodes = BuildCornerNodes(zones, map, includeShift: false, pad: fallbackPad).ToList();
            foreach (var z in zones)
            {
                var corners = new List<Vector2> { z.TopLeft, z.TopRight, z.BottomLeft, z.BottomRight };
                foreach (var corner in corners)
                {
                    var dir = Vector2.Normalize(corner - z.Center);
                    if (float.IsNaN(dir.X) || float.IsNaN(dir.Y))
                        dir = new Vector2(0, -1);
                    var shiftedOut = corner + dir * (fallbackPad + CornerOffset);
                    if (map.IsInMap((int)shiftedOut.X, (int)shiftedOut.Y))
                        fallbackNodes.Add(shiftedOut);
                }
            }
            fallbackNodes.Insert(0, target);
            fallbackNodes.Insert(0, start);

            //Recompute zoneRects for fallbackPad (needed because padding changed)
            var fallbackZoneRects = zones.Select(z => new ZoneRect(z, fallbackPad)).ToArray();
            var fallbackPath = RunVisibilityDijkstra(fallbackNodes, fallbackZoneRects, fallbackPad);
            if (fallbackPath != null)
            {
                var fixedFallback = ValidateAndFixPath(fallbackPath, fallbackZoneRects, fallbackPad, map);
                if (fixedFallback != null)
                {
                    Debug.WriteLine("Pathfinder: Route found using Fallback-Graph.");
                    return fixedFallback;
                }
            }

            //Detour-Points
            var detourNodes = new List<Vector2> { start, target };
            foreach (var z in zones)
            {
                float minX = Math.Min(Math.Min(z.TopLeft.X, z.TopRight.X), Math.Min(z.BottomLeft.X, z.BottomRight.X)) - pad;
                float maxX = Math.Max(Math.Max(z.TopLeft.X, z.TopRight.X), Math.Max(z.BottomLeft.X, z.BottomRight.X)) + pad;
                float minY = Math.Min(Math.Min(z.TopLeft.Y, z.TopRight.Y), Math.Min(z.BottomLeft.Y, z.BottomRight.Y)) - pad;
                float maxY = Math.Max(Math.Max(z.TopLeft.Y, z.TopRight.Y), Math.Max(z.BottomLeft.Y, z.BottomRight.Y)) + pad;

                var rightAbove = new Vector2(maxX + CornerOffset, minY - CornerOffset);
                var rightBelow = new Vector2(maxX + CornerOffset, maxY + CornerOffset);
                var leftAbove = new Vector2(minX - CornerOffset, minY - CornerOffset);
                var leftBelow = new Vector2(minX - CornerOffset, maxY + CornerOffset);

                foreach (var c in new[] { rightAbove, rightBelow, leftAbove, leftBelow })
                    if (map.IsInMap((int)c.X, (int)c.Y))
                        detourNodes.Add(c);
            }
            var detourPath = RunVisibilityDijkstra(detourNodes, zoneRects, pad);
            if (detourPath != null)
            {
                var fixedDetour = ValidateAndFixPath(detourPath, zoneRects, pad, map);
                if (fixedDetour != null)
                {
                    Debug.WriteLine("Pathfinder: Route found using Detour-Graph.");
                    return fixedDetour;
                }
            }

            //Last Fallback: Aggressive A*
            var gridPath = RunGridAStar(start, target, zoneRects, map, cellSize: 64, pad: Math.Max(pad, 8f));
            if (gridPath != null)
            {
                Debug.WriteLine("Pathfinder: Route found using A*.");
                return gridPath;
            }

            Debug.WriteLine("Pathfinder: no possible route found!");
            return new LinkedList<Vector2>(new[] { start, target });
        }

        private static List<Vector2> BuildCornerNodes(List<PoiZone> zones, Map map, bool includeShift, float pad)
        {
            var nodes = new List<Vector2>();
            foreach (var z in zones)
            {
                var cornerSpecs = new (Vector2 corner, int sx, int sy)[]
                {
                    (z.TopLeft, -1, -1),
                    (z.TopRight, 1, -1),
                    (z.BottomLeft, -1, 1),
                    (z.BottomRight, 1, 1)
                };
                float zMinX = Math.Min(Math.Min(z.TopLeft.X, z.TopRight.X), Math.Min(z.BottomLeft.X, z.BottomRight.X));
                float zMaxX = Math.Max(Math.Max(z.TopLeft.X, z.TopRight.X), Math.Max(z.BottomLeft.X, z.BottomRight.X));
                float zMinY = Math.Min(Math.Min(z.TopLeft.Y, z.TopRight.Y), Math.Min(z.BottomLeft.Y, z.BottomRight.Y));
                float zMaxY = Math.Max(Math.Max(z.TopLeft.Y, z.TopRight.Y), Math.Max(z.BottomLeft.Y, z.BottomRight.Y));

                float paddedMinX = zMinX - pad;
                float paddedMaxX = zMaxX + pad;
                float paddedMinY = zMinY - pad;
                float paddedMaxY = zMaxY + pad;

                foreach (var (corner, sx, sy) in cornerSpecs)
                {
                    if (!map.IsInMap((int)corner.X, (int)corner.Y))
                        continue;

                    if (includeShift)
                    {
                        float mult = 1f;
                        Vector2 shifted;
                        int iter = 0;
                        do
                        {
                            shifted = new Vector2(corner.X + sx * CornerOffset * mult, corner.Y + sy * CornerOffset * mult);
                            mult *= 1.5f;
                            iter++;
                        }
                        while (iter < 6 && (shifted.X >= paddedMinX && shifted.X <= paddedMaxX && shifted.Y >= paddedMinY && shifted.Y <= paddedMaxY));

                        if (shifted.X >= paddedMinX && shifted.X <= paddedMaxX && shifted.Y >= paddedMinY && shifted.Y <= paddedMaxY)
                        {
                            Vector2 dir = corner - z.Center;
                            if (Math.Abs(dir.X) < Epsilon && Math.Abs(dir.Y) < Epsilon)
                                dir = new Vector2(0, -1);
                            else
                                dir = Vector2.Normalize(dir);
                            shifted = corner + dir * CornerOffset;

                            if (shifted.X >= paddedMinX && shifted.X <= paddedMaxX && shifted.Y >= paddedMinY && shifted.Y <= paddedMaxY)
                            {
                                shifted = new Vector2(corner.X + sx * (CornerOffset + pad + 4f), corner.Y + sy * (CornerOffset + pad + 4f));
                            }
                        }

                        if (map.IsInMap((int)shifted.X, (int)shifted.Y))
                            nodes.Add(shifted);
                        else
                            nodes.Add(corner);
                    }
                    else
                    {
                        if (!(corner.X >= paddedMinX && corner.X <= paddedMaxX && corner.Y >= paddedMinY && corner.Y <= paddedMaxY))
                            nodes.Add(corner);
                        else
                        {
                            var smallShift = new Vector2(corner.X + sx * (CornerOffset + pad / 2f), corner.Y + sy * (CornerOffset + pad / 2f));
                            if (map.IsInMap((int)smallShift.X, (int)smallShift.Y))
                                nodes.Add(smallShift);
                        }
                    }
                }
            }
            return nodes.Distinct(new Vector2Comparer(0.5f)).ToList();
        }

        private static LinkedList<Vector2>? RunVisibilityDijkstra(List<Vector2> nodes, ZoneRect[] zoneRects, float pad)
        {
            var adjacency = new Dictionary<int, List<(int to, float cost)>>(nodes.Count);
            for (int i = 0; i < nodes.Count; i++) adjacency[i] = new List<(int to, float cost)>();

            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = i + 1; j < nodes.Count; j++)
                {
                    var a = nodes[i];
                    var b = nodes[j];
                    bool blocked = IsSegmentBlockedByAnyZone(a, b, zoneRects);
                    if (!blocked)
                    {
                        float d = Vector2.Distance(a, b);
                        adjacency[i].Add((j, d));
                        adjacency[j].Add((i, d));
                    }
                }
            }

            int startIndex = 0;
            int goalIndex = 1; //FindShortestPath -> nodes[1] = target
            var count = nodes.Count;
            var gScore = Enumerable.Repeat(float.PositiveInfinity, count).ToArray();
            var prev = new int?[count];

            float Heuristic(int idx) => Vector2.Distance(nodes[idx], nodes[goalIndex]);

            gScore[startIndex] = 0f;
            var open = new PriorityQueue<int, float>();
            open.Enqueue(startIndex, Heuristic(startIndex));

            while (open.Count > 0)
            {
                var current = open.Dequeue();

                //target reached
                if (current == goalIndex)
                    break;

                //no neighbours -> continue
                if (!adjacency.TryGetValue(current, out var neighbors) || neighbors.Count == 0)
                    continue;

                float gCurrent = gScore[current];

                foreach (var (to, cost) in neighbors)
                {
                    float tentativeG = gCurrent + cost;
                    if (tentativeG + 1e-6 < gScore[to])
                    {
                        gScore[to] = tentativeG;
                        prev[to] = current;
                        float f = tentativeG + Heuristic(to);
                        open.Enqueue(to, f);
                    }
                }
            }

            if (float.IsPositiveInfinity(gScore[goalIndex]))
                return null;

            var stack = new Stack<Vector2>();
            int? cur = goalIndex;
            while (cur != null)
            {
                stack.Push(nodes[cur.Value]);
                cur = prev[cur.Value];
            }

            var result = new LinkedList<Vector2>();
            while (stack.Count > 0) result.AddLast(stack.Pop());
            return result;
        }

        private static LinkedList<Vector2>? ValidateAndFixPath(LinkedList<Vector2> path, ZoneRect[] zoneRects, float pad, Map map)
        {
            if (path == null || path.Count < 2) return null;

            var points = path.ToList();
            for (int i = 0; i < points.Count; i++)
            {
                var p = points[i];

                if (i > 0 && Vector2.Distance(points[i - 1], p) < MinStepDistance)
                {
                    points[i] = points[i - 1];
                    continue;
                }

                foreach (var zr in zoneRects)
                {
                    bool inside = (p.X >= zr.MinX && p.X <= zr.MaxX && p.Y >= zr.MinY && p.Y <= zr.MaxY);
                    if (!inside) continue;

                    if (IsPointCornerOfZone(p, zr.Zone)) continue;

                    var nudged = NudgePointOutOfZone(p, zr.Zone, pad, map);
                    if (nudged == null)
                    {
                        return null;
                    }
                    points[i] = nudged.Value;
                }
            }

            var result = new LinkedList<Vector2>();
            Vector2? last = null;
            foreach (var pt in points)
            {
                if (last != null && Vector2.Distance(last.Value, pt) < MinStepDistance) continue;
                result.AddLast(pt);
                last = pt;
            }

            if (result.Count < 2) return null;
            return result;
        }

        private static Vector2? NudgePointOutOfZone(Vector2 point, PoiZone z, float pad, Map map)
        {
            float paddedMinX = Math.Min(Math.Min(z.TopLeft.X, z.TopRight.X), Math.Min(z.BottomLeft.X, z.BottomRight.X)) - pad;
            float paddedMaxX = Math.Max(Math.Max(z.TopLeft.X, z.TopRight.X), Math.Max(z.BottomLeft.X, z.BottomRight.X)) + pad;
            float paddedMinY = Math.Min(Math.Min(z.TopLeft.Y, z.TopRight.Y), Math.Min(z.BottomLeft.Y, z.BottomRight.Y)) - pad;
            float paddedMaxY = Math.Max(Math.Max(z.TopLeft.Y, z.TopRight.Y), Math.Max(z.BottomLeft.Y, z.BottomRight.Y)) + pad;

            var dir = point - z.Center;
            if (Math.Abs(dir.X) < Epsilon && Math.Abs(dir.Y) < Epsilon)
                dir = new Vector2(0, -1);
            else
                dir = Vector2.Normalize(dir);

            float step = CornerOffset;
            var candidate = point;
            for (int i = 0; i < 12; i++)
            {
                candidate = candidate + dir * step;
                step *= 1.3f;
                if (candidate.X < paddedMinX || candidate.X > paddedMaxX || candidate.Y < paddedMinY || candidate.Y > paddedMaxY)
                {
                    if (map.IsInMap((int)candidate.X, (int)candidate.Y))
                        return candidate;
                    else
                        return null;
                }
            }

            int sx = point.X < z.Center.X ? -1 : 1;
            int sy = point.Y < z.Center.Y ? -1 : 1;
            var fallback = new Vector2(point.X + sx * (CornerOffset + pad + 8f), point.Y + sy * (CornerOffset + pad + 8f));
            if (map.IsInMap((int)fallback.X, (int)fallback.Y)) return fallback;
            return null;
        }

        private static bool IsSegmentBlockedByAnyZone(Vector2 a, Vector2 b, ZoneRect[] zoneRects)
        {
            for (int zi = 0; zi < zoneRects.Length; zi++)
            {
                var zr = zoneRects[zi];

                if (Math.Max(a.X, b.X) < zr.MinX || Math.Min(a.X, b.X) > zr.MaxX ||
                    Math.Max(a.Y, b.Y) < zr.MinY || Math.Min(a.Y, b.Y) > zr.MaxY)
                    continue;

                //endpoint inside padded rect and not corner -> blocked
                bool aInside = (a.X >= zr.MinX && a.X <= zr.MaxX && a.Y >= zr.MinY && a.Y <= zr.MaxY);
                bool bInside = (b.X >= zr.MinX && b.X <= zr.MaxX && b.Y >= zr.MinY && b.Y <= zr.MaxY);
                if (aInside && !IsPointCornerOfZone(a, zr.Zone)) return true;
                if (bInside && !IsPointCornerOfZone(b, zr.Zone)) return true;

                //precise intersection test
                if (SegmentIntersectsRectangleWithParams(a, b, zr.MinX, zr.MinY, zr.MaxX, zr.MaxY, out float tEnter, out float tExit))
                {
                    float entry = Math.Max(tEnter, 0f);
                    float exit = Math.Min(tExit, 1f);
                    if (entry > exit) continue;

                    if (entry > 0f && exit < 1f) return true;

                    if ((Math.Abs(tEnter) <= Epsilon && IsPointCornerOfZone(a, zr.Zone)) ||
                        (Math.Abs(tExit - 1f) <= Epsilon && IsPointCornerOfZone(b, zr.Zone)))
                    {
                        continue;
                    }

                    if (!(Math.Abs(tEnter) <= Epsilon || Math.Abs(tExit - 1f) <= Epsilon))
                        return true;

                    if (!(IsPointCornerOfZone(a, zr.Zone) || IsPointCornerOfZone(b, zr.Zone)))
                        return true;
                }

                //midpoint safety
                var midpoint = (a + b) / 2;
                if (midpoint.X >= zr.MinX && midpoint.X <= zr.MaxX && midpoint.Y >= zr.MinY && midpoint.Y <= zr.MaxY)
                    return true;
            }
            return false;
        }

        private static bool IsSegmentBlockedByAnyZone(Vector2 a, Vector2 b, List<PoiZone> zones, float pad)
        {
            var zrs = zones.Select(z => new ZoneRect(z, pad)).ToArray();
            return IsSegmentBlockedByAnyZone(a, b, zrs);
        }

        private static LinkedList<Vector2>? RunGridAStar(Vector2 start, Vector2 target, ZoneRect[] zoneRects, Map map, int cellSize = 64, float pad = 8f)
        {
            //grid dimensions
            int cols = Math.Max(2, (map.Width + cellSize - 1) / cellSize);
            int rows = Math.Max(2, (map.Height + cellSize - 1) / cellSize);

            bool[,] blocked = new bool[cols, rows];
            for (int cx = 0; cx < cols; cx++)
            {
                for (int cy = 0; cy < rows; cy++)
                {
                    float cxCenter = cx * cellSize + cellSize / 2f;
                    float cyCenter = cy * cellSize + cellSize / 2f;
                    if (!map.IsInMap((int)cxCenter, (int)cyCenter))
                    {
                        blocked[cx, cy] = true;
                        continue;
                    }

                    for (int zi = 0; zi < zoneRects.Length; zi++)
                    {
                        var zr = zoneRects[zi];
                        if (cxCenter >= zr.MinX && cxCenter <= zr.MaxX && cyCenter >= zr.MinY && cyCenter <= zr.MaxY)
                        {
                            blocked[cx, cy] = true;
                            break;
                        }
                    }
                }
            }

            //start/target cells
            int sx = Clamp((int)(start.X / cellSize), 0, cols - 1);
            int sy = Clamp((int)(start.Y / cellSize), 0, rows - 1);
            int gx = Clamp((int)(target.X / cellSize), 0, cols - 1);
            int gy = Clamp((int)(target.Y / cellSize), 0, rows - 1);

            //start or goal cell blocked -> find nearest free cell within radius
            if (blocked[sx, sy])
            {
                if (!FindNearestFreeCell(blocked, cols, rows, sx, sy, out var nsx, out var nsy, maxRadius: 6))
                {
                    return null;
                }
                sx = nsx; sy = nsy;
            }
            if (blocked[gx, gy])
            {
                if (!FindNearestFreeCell(blocked, cols, rows, gx, gy, out var ngx, out var ngy, maxRadius: 6))
                {
                    return null;
                }
                gx = ngx; gy = ngy;
            }

            var open = new PriorityQueue<(int x, int y), float>();
            var gScore = new float[cols, rows];
            var cameFrom = new (int px, int py)[cols, rows];
            var visited = new bool[cols, rows];
            for (int i = 0; i < cols; i++)
                for (int j = 0; j < rows; j++)
                    gScore[i, j] = float.PositiveInfinity;

            gScore[sx, sy] = 0f;
            open.Enqueue((sx, sy), HeuristicCell(sx, sy, gx, gy));
            cameFrom[sx, sy] = (sx, sy);

            int[] dx = { -1, 1, 0, 0, -1, -1, 1, 1 };
            int[] dy = { 0, 0, -1, 1, -1, 1, -1, 1 };

            while (open.Count > 0)
            {
                var cur = open.Dequeue();
                int cx = cur.x, cy = cur.y;
                if (visited[cx, cy]) continue;
                visited[cx, cy] = true;

                if (cx == gx && cy == gy)
                {
                    //reconstruct path
                    var rev = new List<Vector2>();
                    int curx = gx, cury = gy;
                    while (!(curx == cameFrom[curx, cury].px && cury == cameFrom[curx, cury].py))
                    {
                        float cxCenter = curx * cellSize + cellSize / 2f;
                        float cyCenter = cury * cellSize + cellSize / 2f;
                        rev.Add(new Vector2(cxCenter, cyCenter));
                        var p = cameFrom[curx, cury];
                        curx = p.px; cury = p.py;
                    }
                    //add start cell center
                    rev.Add(new Vector2(sx * cellSize + cellSize / 2f, sy * cellSize + cellSize / 2f));
                    rev.Reverse();

                    var final = new List<Vector2> { new Vector2(start.X, start.Y) };
                    final.AddRange(rev);
                    final.Add(new Vector2(target.X, target.Y));

                    if (final.Count > MaxPathNodesForSmoothing)
                    {
                        Debug.WriteLine($"Pathfinder: Grid-A* produced large path (nodes={final.Count}), sampling nodes to avoid exception.");
                        final = ReducePathBySampling(final, TargetSampleSize);
                    }

                    var smoothed = SmoothPath(final, zoneRects, pad);
                    return new LinkedList<Vector2>(smoothed);
                }

                for (int k = 0; k < dx.Length; k++)
                {
                    int nx = cx + dx[k], ny = cy + dy[k];
                    if (nx < 0 || nx >= cols || ny < 0 || ny >= rows) continue;
                    if (blocked[nx, ny]) continue;
                    if (visited[nx, ny]) continue;

                    float tentativeG = gScore[cx, cy] + ((k < 4) ? 1f : 1.4142f);
                    if (tentativeG + 1e-6 < gScore[nx, ny])
                    {
                        gScore[nx, ny] = tentativeG;
                        cameFrom[nx, ny] = (cx, cy);
                        float f = tentativeG + HeuristicCell(nx, ny, gx, gy);
                        open.Enqueue((nx, ny), f);
                    }
                }
            }

            return null;
        }

        private static float HeuristicCell(int x, int y, int gx, int gy)
        {
            float dx = gx - x;
            float dy = gy - y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        private static bool FindNearestFreeCell(bool[,] blocked, int cols, int rows, int sx, int sy, out int fx, out int fy, int maxRadius = 6)
        {
            fx = sx; fy = sy;
            for (int r = 1; r <= maxRadius; r++)
            {
                for (int dx = -r; dx <= r; dx++)
                {
                    for (int dy = -r; dy <= r; dy++)
                    {
                        int nx = sx + dx, ny = sy + dy;
                        if (nx < 0 || nx >= cols || ny < 0 || ny >= rows) continue;
                        if (!blocked[nx, ny])
                        {
                            fx = nx; fy = ny;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private static int Clamp(int v, int lo, int hi) => v < lo ? lo : (v > hi ? hi : v);

        private static List<Vector2> SmoothPath(List<Vector2> path, ZoneRect[] zoneRects, float pad)
        {
            if (path == null || path.Count <= 2)
                return path is null ? new List<Vector2>() : new List<Vector2>(path);

            if (path.Count > MaxPathNodesForSmoothing)
            {
                Debug.WriteLine($"Pathfinder.SmoothPath: inserter Path {path.Count} too big, reducing to {TargetSampleSize}.");
                path = ReducePathBySampling(path, TargetSampleSize);
            }

            var result = new List<Vector2>();
            int n = path.Count;

            result.Add(path[0]);

            int i = 0;
            while (i < n - 1)
            {
                int j = i + 1;
                int best = i + 1;

                while (j < n)
                {
                    if (IsSegmentBlockedByAnyZone(path[i], path[j], zoneRects))
                    {
                        break;
                    }
                    best = j;
                    j++;
                }

                if (best == i)
                {
                    best = Math.Min(i + 1, n - 1);
                }

                var toAdd = path[best];
                var last = result.Count > 0 ? result[result.Count - 1] : (Vector2?)null;
                if (last == null || Vector2.Distance(last.Value, toAdd) >= MinStepDistance)
                    result.Add(toAdd);

                i = best;
            }

            if (result.Count == 0 || result[result.Count - 1] != path[n - 1])
            {
                var last = result.Count > 0 ? result[result.Count - 1] : (Vector2?)null;
                var end = path[n - 1];
                if (last == null || Vector2.Distance(last.Value, end) >= MinStepDistance)
                    result.Add(end);
            }

            if (result.Count > MaxPathNodesForSmoothing)
            {
                Debug.WriteLine($"Pathfinder.SmoothPath: result still large ({result.Count}), sampling down to {TargetSampleSize}.");
                result = ReducePathBySampling(result, TargetSampleSize);
            }

            return result;
        }

        private static List<Vector2> ReducePathBySampling(List<Vector2> path, int targetSize)
        {
            if (path == null || path.Count <= targetSize) return new List<Vector2>(path);
            if (targetSize < 3) targetSize = 3;

            var reduced = new List<Vector2>(targetSize);
            int n = path.Count;
            reduced.Add(path[0]);
            for (int k = 1; k < targetSize - 1; k++)
            {
                double t = k / (double)(targetSize - 1);
                int idx = (int)Math.Round(t * (n - 1));
                idx = Math.Clamp(idx, 1, n - 2);
                reduced.Add(path[idx]);
            }
            reduced.Add(path[n - 1]);
            return reduced;
        }

        internal static bool IsPointCornerOfZone(Vector2 point, PoiZone z)
        {
            if (Vector2.Distance(point, z.TopLeft) <= CornerTolerance) return true;
            if (Vector2.Distance(point, z.TopRight) <= CornerTolerance) return true;
            if (Vector2.Distance(point, z.BottomLeft) <= CornerTolerance) return true;
            if (Vector2.Distance(point, z.BottomRight) <= CornerTolerance) return true;

            if (Vector2.Distance(point, z.LargeTopLeft) <= CornerTolerance) return true;
            if (Vector2.Distance(point, z.LargeTopRight) <= CornerTolerance) return true;
            if (Vector2.Distance(point, z.LargeBottomLeft) <= CornerTolerance) return true;
            if (Vector2.Distance(point, z.LargeBottomRight) <= CornerTolerance) return true;

            if (z.CornerPositions != null)
            {
                foreach (var c in z.CornerPositions)
                    if (Vector2.Distance(point, c) <= CornerTolerance) return true;
            }
            return false;
        }

        private class Vector2Comparer : IEqualityComparer<Vector2>
        {
            private readonly float tol;
            public Vector2Comparer(float tolerance = 0.5f) => tol = tolerance;
            public bool Equals(Vector2 x, Vector2 y) => Vector2.Distance(x, y) <= tol;
            public int GetHashCode(Vector2 obj) => HashCode.Combine((int)(obj.X / tol), (int)(obj.Y / tol));
        }

        internal static bool SegmentIntersectsRectangleWithParams(Vector2 p0, Vector2 p1, float minX, float minY, float maxX, float maxY, out float tEnter, out float tExit)
        {
            tEnter = 0f;
            tExit = 1f;

            float dx = p1.X - p0.X;
            float dy = p1.Y - p0.Y;

            float t0 = 0.0f;
            float t1 = 1.0f;

            bool Clip(float p, float q, ref float t0Ref, ref float t1Ref)
            {
                //p == 0 => parallel to this edge
                if (Math.Abs(p) < Epsilon)
                {
                    //line is parallel and outside the boundary
                    if (q < 0f) return false;
                    return true;
                }

                float r = q / p;
                if (p < 0f)
                {
                    if (r > t1Ref) return false;
                    if (r > t0Ref) t0Ref = r;
                }
                else
                {
                    if (r < t0Ref) return false;
                    if (r < t1Ref) t1Ref = r;
                }
                return true;
            }

            //left
            if (!Clip(-dx, p0.X - minX, ref t0, ref t1)) return false;
            //right
            if (!Clip(dx, maxX - p0.X, ref t0, ref t1)) return false;
            //top
            if (!Clip(-dy, p0.Y - minY, ref t0, ref t1)) return false;
            //bottom
            if (!Clip(dy, maxY - p0.Y, ref t0, ref t1)) return false;

            if (t0 <= t1 && (t0 <= 1.0f && t1 >= 0.0f))
            {
                tEnter = t0;
                tExit = t1;
                return true;
            }

            return false;
        }
    }
}