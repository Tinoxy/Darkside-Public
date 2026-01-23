using DarksideApi.DarkOrbit.Commands.Booster;
using DarksideApi.DarkOrbit.Objects.Boosters;

namespace DarksideApi.DarkOrbit.Handlers.Booster
{
    internal class BoosterFoundOnMapHandler : ICommandHandler
    {
        private readonly Api api;

        public BoosterFoundOnMapHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            BoosterFoundOnMapCommand boosterFoundOnMap = new();
            boosterFoundOnMap.Read(@byte);

            var hoursInSeconds = boosterFoundOnMap.hours * 3600;
            var boosterName = boosterFoundOnMap.lootId.Split('_').Last().ToUpper();
            this.api.WriteLog($"Received {boosterName} for {boosterFoundOnMap.hours} hours");
            this.api.AddLootResource(boosterName, boosterFoundOnMap.hours);

            /*if (this.api.Hero.ActiveBoosters.TryGetValue(boosterFoundOnMap.lootId, out Objects.Boosters.Booster? booster) && booster is PlayerBooster)
            {
                ((PlayerBooster)this.api.Hero.ActiveBoosters[boosterFoundOnMap.lootId]).Duration += hoursInSeconds;
                return;
            }

            this.api.Hero.ActiveBoosters.TryAdd(boosterFoundOnMap.lootId, new PlayerBooster(boosterFoundOnMap.lootId, hoursInSeconds));*/
            api.AddBoosters([new PlayerBooster(boosterFoundOnMap.lootId, hoursInSeconds)]);
        }
    }
}
