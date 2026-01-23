using DarksideApi.DarkOrbit.Commands.Client;
using System.Diagnostics;

namespace DarksideApi.DarkOrbit.Handlers.Client
{
    internal class HealHandler : ICommandHandler
    {
        private readonly Api api;

        public HealHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            HealCommand heal = new HealCommand();
            heal.Read(@byte);

            Debug.WriteLine($"[HealHandler] HealType: {heal.healType}, HealerId: {heal.healerId}, HealedId: {heal.healedId}, CurrentHitpoints: {heal.currentHitpoints}, HealAmount: {heal.healAmount}");
        }
    }
}
