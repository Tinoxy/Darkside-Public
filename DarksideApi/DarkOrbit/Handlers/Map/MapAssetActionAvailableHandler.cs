using DarksideApi.DarkOrbit.Commands.Map;

namespace DarksideApi.DarkOrbit.Handlers.Map
{
    internal class MapAssetActionAvailableHandler : ICommandHandler
    {
        private readonly Api api;

        public MapAssetActionAvailableHandler(Api api)
        {
            this.api = api;
        }
        public void Handle(ByteArray @byte)
        {
            MapAssetActionAvailableCommand mapAssetActionAvailable = new();
            mapAssetActionAvailable.Read(@byte);

            if (mapAssetActionAvailable.additionalInformation is InstantRepairAssetInformationModule instantRepairAssetInformation &&
                this.api.MapAssets.TryGetValue(mapAssetActionAvailable.mapAssetId, out Objects.Map.MapAsset? mapAsset)
                && mapAsset.IsRepairDock
                && mapAssetActionAvailable.activatable)
            {
                mapAsset.InstantRepairCount = instantRepairAssetInformation.instantRepairCount;
                return;
            }

            if (this.api.Gates.TryGetValue(mapAssetActionAvailable.mapAssetId, out Objects.Map.Gate? gate))
            {
                gate.CanJump = mapAssetActionAvailable.activatable;
            }

            this.api.Logging.Log("Map Asset Action for Asset: " + mapAssetActionAvailable.mapAssetId
                + ", activatable: " + mapAssetActionAvailable.activatable
                + ", additinal Info: " + mapAssetActionAvailable.additionalInformation.ToString()
                + " status: " + mapAssetActionAvailable.state);
        }
    }
}
