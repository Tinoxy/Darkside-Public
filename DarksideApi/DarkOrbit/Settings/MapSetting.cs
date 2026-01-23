namespace DarksideApi.DarkOrbit.Settings
{
    internal class MapSetting : Setting
    {
        public List<string>? Npcs { get; set; }
        public int MapId { get; set; }
        public string? MapName { get; set; }

        public MapSetting()
        {
            this.Npcs = [];
            if (Api.NpcIdsByMap.TryGetValue(this.MapId, out Dictionary<string, int>? npcs))
            {
                foreach (KeyValuePair<string, int> npc in npcs)
                {
                    this.Npcs.Add(npc.Key);
                }
            }
        }
    }
}
