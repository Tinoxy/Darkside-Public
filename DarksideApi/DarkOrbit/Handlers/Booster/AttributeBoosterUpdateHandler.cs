using DarksideApi.DarkOrbit.Commands.Booster;
using DarksideApi.DarkOrbit.Objects.Boosters;
using DarksideApi.Util.Extensions;
using System.Diagnostics;

namespace DarksideApi.DarkOrbit.Handlers.Booster
{
    internal class AttributeBoosterUpdateHandler : ICommandHandler
    {
        private readonly Api api;

        public AttributeBoosterUpdateHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            AttributeBoosterUpdateCommand attributeBoosterUpdate = new();
            attributeBoosterUpdate.Read(@byte);

            var boosterList = new List<Objects.Boosters.Booster>();
            foreach (BoosterDetailModule booster in attributeBoosterUpdate.boostedDetailsUpdate)
            {
                if (booster is BoosterPlayerDetailModule playerBooster)
                {
                    /*if (this.api.Hero.ActiveBoosters.TryGetValue(playerBooster.lootId, out Objects.Boosters.Booster? activeBooster) && activeBooster is PlayerBooster)
                    {
                        ((PlayerBooster)this.api.Hero.ActiveBoosters[playerBooster.lootId]).Duration = playerBooster.secondsLeft;
                        continue;
                    }*/
                    //this.api.Hero.ActiveBoosters.TryAdd(booster.lootId, new PlayerBooster(booster.lootId, playerBooster.secondsLeft));
                    boosterList.Add(new PlayerBooster(booster.lootId, playerBooster.secondsLeft));
                    continue;
                }
                if (booster is BoosterSharedDetailModule sharedBooster)
                {
                    /*if (this.api.Hero.ActiveBoosters.TryGetValue(sharedBooster.lootId, out Objects.Boosters.Booster? activeBooster) && activeBooster is SharedBooster)
                    {
                        ((SharedBooster)this.api.Hero.ActiveBoosters[sharedBooster.lootId]).Amount = sharedBooster.numberOfBoosters;
                        continue;
                    }*/
                    //this.api.Hero.ActiveBoosters.TryAdd(booster.lootId, new SharedBooster(booster.lootId, sharedBooster.numberOfBoosters));
                    boosterList.Add(new SharedBooster(booster.lootId, sharedBooster.numberOfBoosters));
                    continue;
                }
                Debug.WriteLine("Unknown Booster Type: " + booster.ToString());
            }

            api.AddBoosters(boosterList);

            return;
            foreach (BoosterDetailModule booster in attributeBoosterUpdate.boostedDetailsRemove)
            {
                if (this.api.Hero.ActiveBoosters.Remove(booster.lootId))
                {
                    Debug.WriteLine("Removed Booster: " + booster.ToString());
                }
            }
        }
    }
}
