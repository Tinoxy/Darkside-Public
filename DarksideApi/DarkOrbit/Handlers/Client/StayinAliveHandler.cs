namespace DarksideApi.DarkOrbit.Handlers.Client
{
    internal class StayinAliveHandler : ICommandHandler
    {
        private readonly Api api;

        public StayinAliveHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            this.api.GameClient.PingAllowed = true;
            this.api.GameClient.LastPong = DateTime.UtcNow;
        }
    }
}
