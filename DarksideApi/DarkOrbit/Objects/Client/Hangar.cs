namespace DarksideApi.DarkOrbit.Objects.Client
{
    public class Hangar
    {
        public long HangarId { get; private set; }
        public bool IsActive { get; set; }
        public string Shipname { get; private set; }
        public int MapId { get; private set; }
        public bool IsFavorite { get; set; }

        public Hangar(long hangarId, bool isActive, string shipname, bool isFavorite, int mapId = -1)
        {
            this.HangarId = hangarId;
            this.IsActive = isActive;
            this.Shipname = shipname;
            this.IsFavorite = isFavorite;
            this.MapId = mapId;
        }
    }
}
