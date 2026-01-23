using DarksideApi.DarkOrbit.Commands.Mines;
using DarksideApi.DarkOrbit.Objects.Boxes;

namespace DarksideApi.DarkOrbit.Handlers.Mines
{
    internal class AddMineHandler : ICommandHandler
    {
        private readonly Api api;

        public AddMineHandler(Api api) { this.api = api; }

        public void Handle(ByteArray @byte)
        {
            AddMineCommand mineInit = new();
            mineInit.Read(@byte);

            Mine mine = new(this.api, mineInit.Hash, mineInit.X, mineInit.Y, mineInit.typeId);
            if (!this.api.Mines.TryAdd(mineInit.Hash, mine))
            {
                this.api.Mines[mineInit.Hash] = mine;
            }
        }
    }
}
