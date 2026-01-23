using DarksideApi.DarkOrbit.Commands.Map;
using DarksideApi.DarkOrbit.Objects.Map;
using System.Diagnostics;

namespace DarksideApi.DarkOrbit.Handlers.Map
{
    internal class MapAddPOIHandler : ICommandHandler
    {
        private readonly Api api;

        public MapAddPOIHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            MapAddPOICommand poiZoneInit = new();
            poiZoneInit.Read(@byte);

            PoiZone poiZone = new(this.api, poiZoneInit.poiId, 0, 0, poiZoneInit.shape, poiZoneInit.active, poiZoneInit.inverted, poiZoneInit.poiType, poiZoneInit.shapeCoordinates);
            poiZone.SetConnectedZone();
            if (!this.api.POIZones.TryAdd(poiZoneInit.poiId, poiZone))
            {
                this.api.POIZones[poiZoneInit.poiId] = poiZone;
            }
          
            this.api.Movement.ResetZones();

            Debug.WriteLine(poiZoneInit.ToString());
        }
    }
}
