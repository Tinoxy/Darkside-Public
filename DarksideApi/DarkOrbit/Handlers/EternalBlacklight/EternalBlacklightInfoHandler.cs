using DarksideApi.DarkOrbit.Commands.EternalBlackLight;
using DarksideApi.Util.Extensions;

namespace DarksideApi.DarkOrbit.Handlers.EternalBlacklight
{
    internal class EternalBlacklightInfoHandler : ICommandHandler
    {
        private readonly Api api;

        public EternalBlacklightInfoHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            EternalBlacklightInfoCommand updateInfoCommand = new();
            updateInfoCommand.Read(@byte);


            this.api.Map.CurrentGGWave = updateInfoCommand.waveInfoCommand.currentWave;

            EternalBlacklightBoosterInfoCommand boosterUpdate = updateInfoCommand.boosterInfoCommand;

            if (this.api.Map.IsGalaxyGate)
            {
                var activeBoosters = string.Empty;
                //this.api.Hero.EBGActiveBoosters.Clear();
                foreach (EternalBlacklightBoosterCommand booster in boosterUpdate.currentBoosters)
                {
                    activeBoosters += $", {booster.boosterType} {booster.value}%";
                    //this.api.Hero.ActiveBoosters.TryAdd(booster.boosterType.ToString(), new Objects.Boosters.EternalBlacklightBooster(booster.value.ToString(), booster.value));
                    api.AddBoosters([new DarksideApi.DarkOrbit.Objects.Boosters.EternalBlacklightBooster(booster.boosterType.ToString(), booster.value)]);
                }

                this.api.WriteLog("EBG Active Boosters:" + (activeBoosters.Length > 2 ? activeBoosters.Remove(0, 1) : " None"));
            }

            if (boosterUpdate.skillPointInfo.points <= 0)
                return;

            this.api.Logic.EBGSelectBooster([boosterUpdate.boosterOption1, boosterUpdate.boosterOption2, boosterUpdate.boosterOption3]);
        }
    }
}
