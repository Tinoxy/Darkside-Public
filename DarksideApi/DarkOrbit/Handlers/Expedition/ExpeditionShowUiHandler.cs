using DarksideApi.DarkOrbit.Commands.Expedition;
using DarksideApi.Util;
using System.Diagnostics;

namespace DarksideApi.DarkOrbit.Handlers.Expedition
{
    internal class ExpeditionShowUiHandler : ICommandHandler
    {
        private readonly Api api;

        public ExpeditionShowUiHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            ExpeditionShowUiCommand expeditionShowUi = new();
            expeditionShowUi.Read(@byte);

            Debug.WriteLine(expeditionShowUi.ToString());

            switch (expeditionShowUi.type)
            {
                case ExpeditionShowUiCommand.UIType.SELECT_ERR:

                    this.api.GameMethods.ExpeditionRequestShips();

                    var currentId = (this.api.Hero.CurrentHangar ?? this.api.Hangars.FirstOrDefault(h => h.IsActive)).HangarId;
                    var faves = this.api.Hangars.Where(h => h.IsFavorite && h.HangarId != currentId).Take(4).Select(h => h.HangarId).ToList();
                    var count = faves.Count + 1;

                    if (count < 5)
                    {
                        faves.AddRange(this.api.Hangars.Where(h => h.HangarId != currentId && !faves.Contains(h.HangarId)).Take(5 - count).Select(h => h.HangarId));
                    }

                    List<long> hangars = [
                        currentId,
                    ];
                    hangars.AddRange(faves);

                    if (HelpTools.CalculateDistance(this.api.Hero.Position, new((int)(100 * 100), (int)(69 * 100))) > 30)
                    {
                        this.api.GameMethods.Move(100 * 100, 69 * 100);
                    }

                    Task.Delay((int)this.api.Hero.FlyingTimeMs + 100).ContinueWith(_ => this.SetExpeditionHangar(hangars, currentId));
                    break;
                case ExpeditionShowUiCommand.UIType.TICKET:

                    if (!this.api.Settings.GalaxyGateSettings.Contains(301))
                    {
                        return;
                    }
                    this.api.Settings.GalaxyGateSettings[301].IsEnabled = false;
                    this.api.WriteLog("Deep Space Echoes Galaxy Gate disabled: No more keys available.");
                    break;
            }
        }

        private void SetExpeditionHangar(List<long> hangars, long currentId)
        {

            this.api.GameMethods.ExpeditionSelectShips(hangars);
            this.api.WriteLog("Selecting Deep Space Echoes Ships...");

            this.api.GameMethods.ExpeditionActivateShip(currentId);
        }
    }
}
