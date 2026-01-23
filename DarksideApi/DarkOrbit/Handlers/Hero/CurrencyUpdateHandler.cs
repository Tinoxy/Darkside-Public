using DarksideApi.DarkOrbit.Commands.Hero;

namespace DarksideApi.DarkOrbit.Handlers.Hero
{
    internal class CurrencyUpdateHandler : ICommandHandler
    {
        private readonly Api api;

        public CurrencyUpdateHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            CurrencyUpdated currencyUpdated = new();
            currencyUpdated.Read(@byte);

            this.api.Inventory.AddCredits(currencyUpdated.credits);
            this.api.Inventory.AddUridium(currencyUpdated.uridium);
            //api.Hero.Jackpot = currencyUpdated.jackpot;
        }
    }
}
