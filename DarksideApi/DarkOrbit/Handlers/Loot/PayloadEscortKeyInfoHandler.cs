using DarksideApi.DarkOrbit.Commands.Loot;
using System.Diagnostics;

namespace DarksideApi.DarkOrbit.Handlers.Loot
{
    internal class PayloadEscortKeyInfoHandler : ICommandHandler
    {
        private readonly Api api;
        public PayloadEscortKeyInfoHandler(Api api)
        {
            this.api = api;
        }
        public void Handle(ByteArray @byte)
        {
            PayloadEscortKeyInfoCommand payloadEscortKeyInfo = new();
            payloadEscortKeyInfo.Read(@byte);

            Debug.WriteLine("Escort key count: " + payloadEscortKeyInfo.keyCount);

            api.Inventory.Add("ESCORT_ACCESS_KEY", payloadEscortKeyInfo.keyCount);
        }
    }
}
