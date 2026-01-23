using DarksideApi.DarkOrbit.Commands.Expedition;
using System.Diagnostics;

namespace DarksideApi.DarkOrbit.Handlers.Expedition
{
    internal class ExpeditionSelectShipHandler : ICommandHandler
    {
        private readonly Api api;

        public ExpeditionSelectShipHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            ExpeditionSelectShipCommand expeditionSelectShip = new();
            expeditionSelectShip.Read(@byte);

            Debug.WriteLine(expeditionSelectShip.ToString());
        }
    }
}
