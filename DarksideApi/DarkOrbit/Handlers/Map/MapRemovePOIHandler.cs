using DarksideApi.DarkOrbit.Commands.Map;

namespace DarksideApi.DarkOrbit.Handlers.Map
{
    internal class MapRemovePOIHandler : ICommandHandler
    {
        private readonly Api api;

        public MapRemovePOIHandler(Api api) { this.api = api; }

        public void Handle(ByteArray @byte)
        {
            MapRemovePOICommand poiZoneRemove = new();
            poiZoneRemove.Read(@byte);

            this.api.POIZones.Remove(poiZoneRemove.zoneId, out var removedZone);
            this.api.POIZones.Values.Where(z => z.ConnectedZone == removedZone).FirstOrDefault()?.RemoveConnectedZone();

            this.api.Movement.ResetZones();
        }
    }
}
