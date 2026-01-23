using DarksideApi.DarkOrbit.Commands.Attack;
using DarksideApi.DarkOrbit.Objects.Map;
using System.Diagnostics;

namespace DarksideApi.DarkOrbit.Handlers.Attack
{
    internal class AttackHitAssetHandler : ICommandHandler
    {
        private readonly Api api;

        public AttackHitAssetHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            AttackHitAssetCommand attackHitAsset = new();
            attackHitAsset.Read(@byte);

            if (this.api.MapAssets.TryGetValue(attackHitAsset.assetId, out MapAsset? targetAsset))
            {
                //targetAsset.Hitpoints = attackHitAsset.assetHitpoints;
                //targetAsset.Shield = attackHitAsset.assetShield;
                Debug.WriteLine("Asset Hit: " + targetAsset.AssetId + " - Hitpoints: " + attackHitAsset.hitpointsNow + ", Max: " + attackHitAsset.hitpointsMax);
            }
        }
    }
}
