using DarksideApi.DarkOrbit.Commands.Boxes;
using DarksideApi.DarkOrbit.Objects.Boxes;

namespace DarksideApi.DarkOrbit.Handlers.Boxes
{
    internal class OreInitHandler : ICommandHandler
    {
        private readonly Api api;

        public OreInitHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            OreInitCommand oreInit = new();
            oreInit.Read(@byte);

            Ore ore = new(this.api, oreInit.Hash, oreInit.X, oreInit.Y, (Ore.OreType)oreInit.Type);
            if (!this.api.Collectibles.TryAdd(oreInit.Hash, ore))
            {
                this.api.Collectibles[oreInit.Hash] = ore;
            }
        }
    }
}
