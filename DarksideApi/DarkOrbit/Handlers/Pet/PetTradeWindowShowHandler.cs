using DarksideApi.DarkOrbit.Commands.Pet;

namespace DarksideApi.DarkOrbit.Handlers.Pet
{
    internal class PetTradeWindowShowHandler : ICommandHandler
    {
        private readonly Api api;

        public PetTradeWindowShowHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            PetTradeWindowShowCommand petTradeWindowShow = new();
            petTradeWindowShow.Read(@byte);

            if (!this.api.Settings.OreSellEverything)
            {
                return;
            }
            this.api.WriteLog("Pet trade activated.");
            foreach (KeyValuePair<Objects.Boxes.Ore.OreType, long> ore in this.api.Hero.Cargo.GetTradeableOres)
            {
                Task.Delay(Logic.Logic.GetRandomSleepTime(100, 300))    //Too many instant messages are not being processed by the server, so we need to add a delay here.
                    .ContinueWith(_ => this.api.GameMethods.OreSell(ore.Key, ore.Value, false));
            }
            this.api.Statistics.AmountTrades += 1;
            this.api.Cooldown.AddCooldown(DarkOrbit.Cooldown.CooldownType.PET_TRADE_GEAR, (60 * 2));
        }
    }
}
