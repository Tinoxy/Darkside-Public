using DarksideApi.DarkOrbit.Commands;

namespace DarksideApi.DarkOrbit.Settings
{
    public class MapAreaSetting : Setting
    {
        public MapAreaType AreaType { get; set; }
        public double Range { get; set; }
        public int GateId { get; set; }
        public int MapId { get; set; }
        public string TypeName { get; set; }
        public string MapName => CommandConstants.MAP_NAMES.TryGetValue(this.MapId, out var name) ? name : this.MapId.ToString();

        public enum MapAreaType
        {
            Portal, QuestGiver, Clanbase
        }
    }
}
