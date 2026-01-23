using DarksideApi.DarkOrbit.Commands.Expedition;

namespace DarksideApi.DarkOrbit.Handlers.Expedition
{
    internal class ExpeditionStartInstanceHandler : ICommandHandler
    {
        private readonly Api api;

        public ExpeditionStartInstanceHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            ExpeditionStartInstanceCommand expeditionStartInstance = new();
            expeditionStartInstance.Read(@byte);

            this.api.WriteLog("Deep Space Echoes Time: " + expeditionStartInstance.seconds);
        }
    }
}
