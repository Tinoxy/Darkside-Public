using DarksideApi.DarkOrbit.Commands.Map;

namespace DarksideApi.DarkOrbit.Handlers.Map
{
    internal class AssetRemoveHandler : ICommandHandler
    {
        private readonly Api api;

        public AssetRemoveHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            AssetRemoveCommand assetRemove = new();
            assetRemove.Read(@byte);

            this.api.MapAssets.Remove(assetRemove.uid, out var asset);

            if (!api.POIZones.ContainsKey(assetRemove.uid.ToString()))
            {
                return;
            }
            api.POIZones.Remove(assetRemove.uid.ToString(), out _);
            api.Logging.Log("Asset Remove Handler: Removed avoid zone for clan base " + assetRemove.uid + " at " + asset?.Position);
        }
    }
}
