using DarksideApi.DarkOrbit.Commands.Mines;

namespace DarksideApi.DarkOrbit.Handlers.Mines
{
    internal class RemoveMineHandler : ICommandHandler
    {
        private readonly Api api;

        public RemoveMineHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            RemoveMineCommand mineRemove = new();
            mineRemove.Read(@byte);

            this.api.Mines.Remove(mineRemove.hash, out _);
        }
    }
}
