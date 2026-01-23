namespace DarksideApi.DarkOrbit.Objects.StarSystem
{
    public class StarMap(int mapId, List<StarPortal> portals)
    {
        public int MapId { get; private set; } = mapId;
        public List<StarPortal> Portals { get; private set; } = portals;
    }
}
