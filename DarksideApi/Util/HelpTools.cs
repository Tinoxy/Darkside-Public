using DarksideApi.DarkOrbit;
using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Objects.StarSystem;
using Dijkstra.NET.Graph;
using Dijkstra.NET.ShortestPath;
using System.Diagnostics;
using System.Net;
using System.Numerics;
using System.Text.RegularExpressions;

namespace DarksideApi.Util
{
    public partial class HelpTools
    {
        public static string ToValidFilename(string fileName)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '_');
            }
            return fileName;
        }

        public static bool Trim(string stringToTrim, out string stringTrimmed)
        {
            stringTrimmed = string.Empty;
            if (string.IsNullOrEmpty(stringToTrim))
            {
                return false;
            }
            stringTrimmed = stringToTrim.Trim();
            return true;
            /*var _loc2_ = 0;
            while (HelpTools.IsWhitespace(stringTrimmed[_loc2_]))
            {
                _loc2_++;
            }
            var _loc3_ = stringTrimmed.Length - 1;
            while (HelpTools.IsWhitespace(stringTrimmed[_loc3_]))
            {
                _loc3_--;
            }
            if (_loc3_ >= _loc2_)
            {
                stringTrimmed = stringTrimmed.Substring(_loc2_, _loc3_ + 1);
                return true;
            }
            return false;*/
        }

        public static bool IsWhitespace(char charToCheck)
        {
            return charToCheck.ToString() switch
            {
                " " or "\t" or "\r" or "\n" or "\f" => true,
                _ => false,
            };
        }

        public static int GetConfig(int currentConfig)
        {
            return currentConfig == 2 ? 1 : 2;
        }

        public static double CalculateDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2.0) + Math.Pow(y2 - y1, 2.0));
        }

        public static double CalculateDistance(Vector2 from, Vector2 to)
        {
            return CalculateDistance(from.X, from.Y, to.X, to.Y);
        }

        public static double GetAngle(double x, double y, double x2, double y2)
        {
            /*    270
		           |
			180  --+-- 0
			       |
				  90
		    */
            var deltaX = x - x2;
            var deltaY = y - y2;
            return Math.Atan2(deltaY, deltaX) / Math.PI * 180 + 180;
        }

        public static Vector2 GetNextPosition(int currentX, int currentY, int targetX, int targetY, float rate = 0.9f)
        {
            Vector2 current = new(currentX, currentY);
            Vector2 target = new(targetX, targetY);
            Vector2 next = (target + Vector2.Multiply((current - target), rate));
            if (Vector2.Distance(current, next) < 420)
            {
                next = (target + Vector2.Multiply((current - target), 1));
            }
            return next;
        }

        public static bool IsMovingAwayFrom(int x, int y, List<Vector2> positions)
        {
            if (positions.Count < 6)
            {
                return false;
            }
            var lastDistance = HelpTools.CalculateDistance(x, y, positions.ElementAt(0).X, positions.ElementAt(0).Y);
            var flag = false;
            for (var i = 1; i < positions.Count; i++)
            {
                Vector2 pos = positions.ElementAt(i);
                var distance = HelpTools.CalculateDistance(x, y, pos.X, pos.Y);
                if (distance < lastDistance)
                {
                    flag = false;
                    break;
                }
                lastDistance = distance;
                flag = true;
                continue;
            }
            return flag;
        }

        public static bool IsMovingTowards(int x, int y, List<Vector2> positions)
        {
            if (positions.Count < 6)
            {
                return false;
            }
            var lastDistance = HelpTools.CalculateDistance(x, y, positions.ElementAt(0).X, positions.ElementAt(0).Y);
            var flag = false;
            for (var i = 1; i < positions.Count; i++)
            {
                Vector2 pos = positions.ElementAt(i);
                var distance = HelpTools.CalculateDistance(x, y, pos.X, pos.Y);
                if (distance > lastDistance)
                {
                    flag = false;
                    break;
                }
                lastDistance = distance;
                flag = true;
                continue;
            }
            return flag;
        }

        public static bool IsInTimespan(List<DateTime> times, double maxTime, int amount = 3)
        {
            return times.Where(t => (DateTime.UtcNow - t).TotalSeconds * 60 < maxTime).ToArray().Length >= amount;
        }

        public static bool IsUpperMap(int mapId)
        {
            return mapId > 16 && mapId < 30;//&& (mapId != 306 && mapId != 307 && mapId != 308) && (mapId != 91 && mapId != 92 && mapId != 93 && mapId != 94);
        }

        public static List<int> GetPath(int currentMapId, int targetMapId, int factionId)
        {
            if (currentMapId == targetMapId)
            {
                return [];
            }

            var graph = new Graph<uint, string>();
            var list = new Dictionary<int, uint>();

            foreach (KeyValuePair<int, StarMap> map in Api.StarSystem)
            {
                list.Add(map.Key, graph.AddNode((uint)map.Key));
            }

            foreach (KeyValuePair<int, uint> nodeId in list)
            {
                INode<uint, string> map = graph[nodeId.Value];
                foreach (StarPortal portal in Api.StarSystem[(int)map.Item].Portals)
                {
                    graph.Connect(nodeId.Value, list[portal.LeadsToMapId], (factionId == portal.FactionId || factionId == 0 ? 0 : 1), null);
                }
            }

            ShortestPathResult result = graph.Dijkstra(list[currentMapId], list[targetMapId]);

            IEnumerable<uint> toGoTo = result.GetPath();
            var mapsToGoTo = new List<int>();

            foreach (var node in toGoTo)
            {
                foreach (KeyValuePair<int, uint> i in list)
                {
                    if (i.Value != node)
                    {
                        continue;
                    }
                    mapsToGoTo.Add(i.Key);
                    break;
                }
            }

            mapsToGoTo.RemoveAt(0);

            var str = string.Empty;
            mapsToGoTo.ForEach(m => str += m.ToString() + " ");

            return mapsToGoTo;
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Shared.Next(s.Length)]).ToArray());
        }

        public static bool GetChance(int percentage = 50)
        {
            return Random.Shared.Next(0, 100) < percentage;
        }

        public static string Between(string content, string start, string end)
        {
            if (!content.Contains(start) || !content.Contains(end))
            {
                return string.Empty;
            }
            var _start = content.IndexOf(start);
            var _end = content.IndexOf(end, _start);
            return content.Substring(_start + start.Length, _end - _start - start.Length);
        }

        public static bool TryBetween(string content, string start, string end, out string result, Logging.Logging? logging = null)
        {
            try
            {
                if (!content.Contains(start) || !content.Contains(end))
                {
                    logging?.Log("Could not find '" + start + "' or '" + end + "' in content.");
                    result = string.Empty;
                    return false;
                }
                var _start = content.IndexOf(start);
                var _end = content.IndexOf(end, _start);
                result = content.Substring(_start + start.Length, _end - _start - start.Length);
                return true;
            } catch (Exception ex)
            {
                logging?.Log(ex);
                result = string.Empty;
            }
            return false;
        }

        public static List<string> LoadAbilityNames()
        {
            try
            {
                using HttpClient client = new();
                client.DefaultRequestHeaders.UserAgent.ParseAdd($"BigpointClient/{Api.BigpointClientVersion}");
                client.Timeout = TimeSpan.FromSeconds(120);
                client.DefaultRequestHeaders.Referrer = new Uri($"https://de4.darkorbit.com/spacemap");

                using var request = new HttpRequestMessage(HttpMethod.Get, "https://darkorbit-22.bpsecure.com/spacemap/templates/de/resource_items.xml?__cv=d0ced73136849721eec6f5aec3b71f00");
                using HttpResponseMessage response = client.Send(request);

                using StreamReader streamReader = new(response.Content.ReadAsStream());
                var data = streamReader.ReadToEnd();

                //items_ability_(.*?)_code
                MatchCollection matches = Regex.Matches(data, "items_ability_(.*?)_fullname");

                return [];
            }
            catch (Exception)
            {
            }
            return [];
        }

        private static int GetMapMultiplicator(int id)
        {
            return id == 91 || id == 92 || id == 93
                || id == 29 || id == 16
                ? 128
                : 64;
        }

        public static Dictionary<int, Dictionary<string, int>> LoadOreIds()
        {
            /*
<item name="quest_task_matches_ore_1">Prometium</item>
<item name="quest_task_matches_ore_2">Terbium</item>
<item name="quest_task_matches_ore_3">Endurium</item>
<item name="quest_task_matches_ore_4">Xenomit</item>
<item name="quest_task_matches_ore_5">Prometid</item>
<item name="quest_task_matches_ore_6">Duranium</item>
<item name="quest_task_matches_ore_7">Promerium</item>
*/
            var result = new Dictionary<int, Dictionary<string, int>>
            {
                //x-1
                {
                    1,
                    new Dictionary<string, int>()
            {
                { CommandConstants.ORE_PROMETIUM, 1 },
                { CommandConstants.ORE_ENDURIUM, 3 },
            }
                },
                {
                    5,
                    new Dictionary<string, int>()
            {
                { CommandConstants.ORE_PROMETIUM, 1 },
                { CommandConstants.ORE_ENDURIUM, 3 },
            }
                },
                {
                    9,
                    new Dictionary<string, int>()
            {
                { CommandConstants.ORE_PROMETIUM, 1 },
                { CommandConstants.ORE_ENDURIUM, 3 },
            }
                },

                //x-2
                {
                    2,
                    new Dictionary<string, int>()
            {
                { CommandConstants.ORE_PROMETIUM, 1 },
                { CommandConstants.ORE_TERBIUM, 2 },
                { CommandConstants.ORE_ENDURIUM, 3 },
            }
                },
                {
                    6,
                    new Dictionary<string, int>()
            {
                { CommandConstants.ORE_PROMETIUM, 1 },
                { CommandConstants.ORE_TERBIUM, 2 },
                { CommandConstants.ORE_ENDURIUM, 3 },
            }
                },
                {
                    10,
                    new Dictionary<string, int>()
            {
                { CommandConstants.ORE_PROMETIUM, 1 },
                { CommandConstants.ORE_TERBIUM, 2 },
                { CommandConstants.ORE_ENDURIUM, 3 },
            }
                },

                //x-3
                {
                    3,
                    new Dictionary<string, int>()
            {
                { CommandConstants.ORE_TERBIUM, 2 },
                { CommandConstants.ORE_ENDURIUM, 3 },
            }
                },
                {
                    7,
                    new Dictionary<string, int>()
            {
                { CommandConstants.ORE_TERBIUM, 2 },
                { CommandConstants.ORE_ENDURIUM, 3 },
            }
                },
                {
                    11,
                    new Dictionary<string, int>()
            {
                { CommandConstants.ORE_TERBIUM, 2 },
                { CommandConstants.ORE_ENDURIUM, 3 },
            }
                },

                //x-4
                {
                    4,
                    new Dictionary<string, int>()
            {
                { CommandConstants.ORE_TERBIUM, 2 },
                { CommandConstants.ORE_ENDURIUM, 3 },
            }
                },
                {
                    8,
                    new Dictionary<string, int>()
            {
                { CommandConstants.ORE_TERBIUM, 2 },
                { CommandConstants.ORE_ENDURIUM, 3 },
            }
                },
                {
                    12,
                    new Dictionary<string, int>()
            {
                { CommandConstants.ORE_TERBIUM, 2 },
                { CommandConstants.ORE_ENDURIUM, 3 },
            }
                }
            };
            return result;
        }

        public static string GetMapName(int mapId)
        {
            return mapId switch
            {
                1 or 5 or 9 => "X-1",
                2 or 6 or 10 => "X-2",
                3 or 7 or 11 => "X-3",
                4 or 8 or 12 => "X-4",
                13 or 14 or 15 or 16 => "PvP",
                17 or 21 or 25 => "X-5",
                18 or 22 or 26 => "X-6",
                19 or 23 or 27 => "X-7",
                20 or 24 or 28 => "X-8",
                29 => "4-5",
                306 or 307 or 308 => "X-BL",
                91 or 92 or 93 => "5-X",
                94 => "5-4",
                _ => "Other"
            };
        }

        public static Dictionary<int, Dictionary<string, int>> LoadNpcIds(Api api, string data)
        {
            var result = new Dictionary<int, Dictionary<string, int>>();
            try
            {
                //api.Logging.WriteLine("Npc Map Data: " + data);
                var maps = data.Split("<map ");

                foreach (var map in maps)
                {
                    var mapIdStr = Between(map, "ref=\"", "\">");
                    var npcs = map.Split("<!");

                    var mapNpcs = new Dictionary<string, int>();
                    foreach (var n in npcs)
                    {
                        var npcName = Between(n, "-- ", " -->");
                        var npcId = Between(n, "<npc id=\"", "\"/>");

                        if (string.IsNullOrEmpty(npcId) || !int.TryParse(npcId, out var id))
                        {
                            continue;
                        }

                        mapNpcs.TryAdd(npcName, id);
                    }

                    if (string.IsNullOrEmpty(mapIdStr) || !int.TryParse(mapIdStr, out var mapId))
                    {
                        api.Logging.Log("Could not Parse MapId from: " + map);
                        continue;
                    }

                    result.Add(mapId, mapNpcs);
                }
            }
            catch (Exception ex)
            {
                api.Logging.Log(ex);
            }
            return result;
        }

        public static Dictionary<int, StarMap> LoadGateData(Api api)
        {
            try
            {
                using HttpClient client = new();
                client.DefaultRequestHeaders.UserAgent.ParseAdd($"BigpointClient/{Api.BigpointClientVersion}");
                client.Timeout = TimeSpan.FromSeconds(120);
                client.DefaultRequestHeaders.Referrer = new Uri($"https://{api.Hero.ServerTag}.darkorbit.com/spacemap");

                using var request = new HttpRequestMessage(HttpMethod.Get, "https://darkorbit-22.bpsecure.com/spacemap/graphics/spacemap-config.xml");
                using HttpResponseMessage response = client.Send(request);

                using StreamReader streamReader = new(response.Content.ReadAsStream());
                var data = streamReader.ReadToEnd();

                var jumpGates = Between(data, "<jumpgates>", "</jumpgates>");
                var npcData = Between(data, "<npcs>", "</npcs>");

                Dictionary<int, Dictionary<string, int>> npcIdsByMap = LoadNpcIds(api, npcData);
                foreach (KeyValuePair<int, Dictionary<string, int>> map in npcIdsByMap)
                {
                    if (!Api.NpcIdsByMap.ContainsKey(map.Key))
                    {
                        Api.NpcIdsByMap.Add(map.Key, map.Value);
                    }
                    foreach (KeyValuePair<string, int> npc in map.Value)
                    {
                        if (!Api.NpcIdsByMap[map.Key].ContainsKey(npc.Key))
                        {
                            Api.NpcIdsByMap[map.Key].Add(npc.Key, npc.Value);
                        }
                    }
                }
                Api.OreIdsByMap = LoadOreIds();

                var gates = jumpGates.Split("<jumpgate key");

                Dictionary<int, StarMap> result = [];
                string gate, fac;
                int gateId, mapId, x, y, leadsToMapId, multiplicator, factionId;
                for (var i = 1; i < gates.Length; i++)
                {
                    gate = gates[i];
                    gateId = int.Parse(Between(gate, "=\"", "\" ref"));
                    mapId = int.Parse(Between(gate, "ref=\"", "\">"));
                    multiplicator = GetMapMultiplicator(mapId);
                    x = int.Parse(Between(gate, "<position x=\"", "\" y=")) * multiplicator;
                    y = int.Parse(Between(gate, "y=\"", "\"/>")) * multiplicator;
                    fac = Between(gate, "<faction>", "</faction>");
                    factionId = (fac == "mmo" ? 1 : fac == "eic" ? 2 : fac == "vru" ? 3 : 0);
                    leadsToMapId = int.Parse(Between(gate, "<leadsToMap>", "</leadsToMap>"));

                    if (!result.TryGetValue(mapId, out StarMap? value))
                    {
                        value = new StarMap(mapId, []);
                        result.Add(mapId, value);
                    }

                    value.Portals.Add(new StarPortal(api, gateId, x, y, factionId, leadsToMapId));
                    //api.Logging.WriteLine("Added StarPortal leading to " + leadsToMapId + " on map " + mapId);
                }
                //FROZEN LAB-->
                result[1].Portals.Add(new StarPortal(api, 83, 10500, 6500, 0, -2));
                result[5].Portals.Add(new StarPortal(api, 83, 10500, 6500, 0, -2));
                result[9].Portals.Add(new StarPortal(api, 83, 10500, 6500, 0, -2));

                result.Add(-2, new StarMap(-2, [
                    new(api, 1, 10500, 6500, 0, 1),
                    new(api, 1, 10500, 6500, 0, 5),
                    new(api, 1, 10500, 6500, 0, 9),
                ]));

                foreach (KeyValuePair<int, StarMap> frozenLabyrinthExits in GetFrozenLabyrinthExits(api))
                {
                    result.Add(frozenLabyrinthExits.Key, frozenLabyrinthExits.Value);
                }
                //<--FROZEN LAB
                //MIMESIS ESCORT-->
                result[1].Portals.Add(new StarPortal(api, 54, 10500, 6500, 0, -3));
                result[5].Portals.Add(new StarPortal(api, 54, 10500, 6500, 0, -3));
                result[9].Portals.Add(new StarPortal(api, 54, 10500, 6500, 0, -3));
                result[20].Portals.Add(new StarPortal(api, 54, 10500, 6500, 0, -3));
                result[24].Portals.Add(new StarPortal(api, 54, 10500, 6500, 0, -3));
                result[28].Portals.Add(new StarPortal(api, 54, 10500, 6500, 0, -3));

                result.Add(-3, new StarMap(-3, [
                    new(api, 1, 10500, 6500, 0, 1),
                    new(api, 1, 10500, 6500, 0, 5),
                    new(api, 1, 10500, 6500, 0, 9),
                    new(api, 1, 10500, 6500, 0, 20),
                    new(api, 1, 10500, 6500, 0, 24),
                    new(api, 1, 10500, 6500, 0, 28),
                ]));
                //NO EXITS
                //<--MIMESIS ESCORT

                return result;
            }
            catch (Exception ex)
            {
                api.Logging.Log(ex);
                return [];
            }
        }

        private static Dictionary<int, StarMap> GetFrozenLabyrinthExits(Api api)
        {
            return new Dictionary<int, StarMap>() {
            { 430, new StarMap(430, [
                    new (api, 55, 11000, 7200, 0, 0),
                    new (api, 55, 22000, 7200, 0, 0),
                    new (api, 55, 11000, 14400, 0, 0),
                    new (api, 55, 22000, 14400, 0, 0),
                ])
            },
            { 431, new StarMap(432, [
                    new (api, 55, 11000, 7200, 0, 0),
                    new (api, 55, 22000, 7200, 0, 0),
                    new (api, 55, 11000, 14400, 0, 0),
                    new (api, 55, 22000, 14400, 0, 0),
                ]) },
            { 432, new StarMap(432, [
                    new (api, 55, 11000, 7200, 0, 0),
                    new (api, 55, 22000, 7200, 0, 0),
                    new (api, 55, 11000, 14400, 0, 0),
                    new (api, 55, 22000, 14400, 0, 0),
                ])  },
            { 433, new StarMap(433, [
                    new (api, 55, 11000, 7200, 0, 0),
                    new (api, 55, 22000, 7200, 0, 0),
                    new (api, 55, 11000, 14400, 0, 0),
                    new (api, 55, 22000, 14400, 0, 0),
                ])  },
            { 434, new StarMap(434, [
                    new (api, 55, 11000, 7200, 0, 0),
                    new (api, 55, 22000, 7200, 0, 0),
                    new (api, 55, 11000, 14400, 0, 0),
                    new (api, 55, 22000, 14400, 0, 0),
                ])  },
            { 435, new StarMap(435, [
                    new (api, 55, 11000, 7200, 0, 0),
                    new (api, 55, 22000, 7200, 0, 0),
                    new (api, 55, 11000, 14400, 0, 0),
                    new (api, 55, 22000, 14400, 0, 0),
                ])  },
            { 436, new StarMap(436, [
                    new (api, 55, 11000, 7200, 0, 0),
                    new (api, 55, 22000, 7200, 0, 0),
                    new (api, 55, 11000, 14400, 0, 0),
                    new (api, 55, 22000, 14400, 0, 0),
                ])  },
            { 437, new StarMap(437, [
                    new (api, 55, 11000, 7200, 0, 0),
                    new (api, 55, 22000, 7200, 0, 0),
                    new (api, 55, 11000, 14400, 0, 0),
                    new (api, 55, 22000, 14400, 0, 0),
                ])  },
            { 438, new StarMap(438, [
                    new (api, 55, 11000, 7200, 0, 0),
                    new (api, 55, 22000, 7200, 0, 0),
                    new (api, 55, 11000, 14400, 0, 0),
                    new (api, 55, 22000, 14400, 0, 0),
                ])  },
            { 439, new StarMap(439, [
                    new (api, 55, 11000, 7200, 0, 0),
                    new (api, 55, 22000, 7200, 0, 0),
                    new (api, 55, 11000, 14400, 0, 0),
                    new (api, 55, 22000, 14400, 0, 0),
                ]) },
            { 440, new StarMap(440, [
                    new (api, 55, 11000, 7200, 0, 0),
                    new (api, 55, 22000, 7200, 0, 0),
                    new (api, 55, 11000, 14400, 0, 0),
                    new (api, 55, 22000, 14400, 0, 0),
                ])  },
            { 441, new StarMap(441, [
                    new (api, 55, 11000, 7200, 0, 0),
                    new (api, 55, 22000, 7200, 0, 0),
                    new (api, 55, 11000, 14400, 0, 0),
                    new (api, 55, 22000, 14400, 0, 0),
                ])  },
            { 442, new StarMap(442, [
                    new (api, 55, 11000, 7200, 0, 0),
                    new (api, 55, 22000, 7200, 0, 0),
                    new (api, 55, 11000, 14400, 0, 0),
                    new (api, 55, 22000, 14400, 0, 0),
                ])  },
            { 443, new StarMap(443, [
                    new (api, 55, 11000, 7200, 0, 0),
                    new (api, 55, 22000, 7200, 0, 0),
                    new (api, 55, 11000, 14400, 0, 0),
                    new (api, 55, 22000, 14400, 0, 0),
                ])  },
            { 444, new StarMap(444, [
                    new (api, 55, 11000, 7200, 0, 0),
                    new (api, 55, 22000, 7200, 0, 0),
                    new (api, 55, 11000, 14400, 0, 0),
                    new (api, 55, 22000, 14400, 0, 0),
                ])  },
            { 445, new StarMap(445, [
                    new (api, 55, 11000, 7200, 0, 0),
                    new (api, 55, 22000, 7200, 0, 0),
                    new (api, 55, 11000, 14400, 0, 0),
                    new (api, 55, 22000, 14400, 0, 0),
                ])  },
            };
        }

        public static Dictionary<int, IPEndPoint> LoadIpAdresses(string serverTag)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.UserAgent.ParseAdd($"BigpointClient/{Api.BigpointClientVersion}");
            client.Timeout = TimeSpan.FromSeconds(120);
            client.DefaultRequestHeaders.Referrer = new Uri($"https://{serverTag}.darkorbit.com/spacemap");
            using var request = new HttpRequestMessage(HttpMethod.Get, $"https://{serverTag}.darkorbit.com/spacemap/xml/maps.php");
            using HttpResponseMessage response = client.Send(request);
            using StreamReader streamReader = new(response.Content.ReadAsStream());
            var data = streamReader.ReadToEnd();
            var maps = data.Split("<map id=");
            Dictionary<int, IPEndPoint> ipAdresses = [];

            for (var i = 1; i < maps.Length; i++)
            {
                var ipAndMapData = maps[i].Trim();
                var mapId = int.Parse(HelpTools.Between(ipAndMapData, "\"", "\">"));
                var ipAsString = HelpTools.Between(ipAndMapData, "<gameserverIP>", "</gameserverIP>");
                var port = GameClient.PORT;

                if (string.IsNullOrEmpty(ipAsString))
                {
                    continue;
                }

                if (ipAsString.Contains(':'))
                {
                    var fullIpArray = ipAsString.Split(':');
                    ipAsString = fullIpArray[0];
                    port = int.Parse(fullIpArray[1]);
                }

                IPAddress resolvedIp;

                // Prüfen ob es bereits eine IP ist
                if (IPAddress.TryParse(ipAsString, out resolvedIp))
                {
                    // Ist bereits eine IP-Adresse
                }
                else
                {
                    // Domain auflösen
                    try
                    {
                        var hostEntry = Dns.GetHostEntry(ipAsString);
                        resolvedIp = hostEntry.AddressList.FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

                        if (resolvedIp == null)
                        {
                            Debug.WriteLine($"Keine IPv4-Adresse für {ipAsString} gefunden");
                            continue;
                        }

                        Debug.WriteLine($"Resolved {ipAsString} zu {resolvedIp}");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Fehler beim Auflösen von {ipAsString}: {ex.Message}");
                        continue;
                    }
                }

                if (ipAdresses.ContainsKey(mapId))
                {
                    ipAdresses[mapId] = new IPEndPoint(resolvedIp, port);
                    continue;
                }

                ipAdresses.Add(mapId, new IPEndPoint(resolvedIp, port));
            }

            Debug.WriteLine("IPs Loaded!");
            return ipAdresses;
        }

        public static void WriteErrorLog(string message, string data)
        {
            try
            {
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\error.log", "Unhandled exception occurred: \n" + message + "\n\nDetails:\n" + data + "\n\n\n");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error logging Excpetion:\n" + ex);
            }
        }
    }
}
