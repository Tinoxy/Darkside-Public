using DarksideApi.DarkOrbit.Commands.Client;

namespace DarksideApi.DarkOrbit.Handlers.Client
{
    internal class LegacyCommandHandler : ICommandHandler
    {
        private readonly Api api;

        public LegacyCommandHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            LegacyCommand simpleCommand = new();
            simpleCommand.Read(@byte);

            CommandMessageHandler.HandleMessage(this.api, simpleCommand.message);
        }
    }
}
