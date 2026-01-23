using DarksideApi.DarkOrbit.Commands.Map;
using DarksideApi.DarkOrbit.Objects.Map;

namespace DarksideApi.DarkOrbit.Handlers.Map
{
    internal class AssetCreateHandler : ICommandHandler
    {
        private readonly Api api;

        public AssetCreateHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            AssetCreateCommand assetInit = new();
            assetInit.Read(@byte);

            MapAsset asset = new(this.api, assetInit.assetId, assetInit.userName, assetInit.x, assetInit.y, (AssetType)assetInit.type.type, assetInit.clanRelation.type, assetInit.factionId, assetInit.clanTag);
            if (!this.api.MapAssets.TryAdd(assetInit.assetId, asset))
            {
                this.api.MapAssets[assetInit.assetId] = asset;
            }
            /*
            if (asset.IsClanBase && asset.IsEnemy)
            {
                var avoidZone = this.api.Logic.GeneratePoiZone(asset.AssetId.ToString(), asset.Position, 850, 850);
                api.POIZones.TryAdd(avoidZone.ZoneId, avoidZone);
                api.Logging.Log("Asset Create Handler: Added avoid zone for clan base " + asset.AssetId + " at " + asset.Position);
            }*/
        }
    }
}
