using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Commands.Boxes;
using DarksideApi.DarkOrbit.Objects.Boxes;

namespace DarksideApi.DarkOrbit.Handlers.Boxes
{
    internal class RemoveCollectableHandler : ICommandHandler
    {
        private readonly Api api;

        public RemoveCollectableHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            RemoveCollectableCommand boxRemove = new();
            boxRemove.Read(@byte);

            if (this.api.Logic.Target is Box boxTarget && boxTarget.Hash == boxRemove.hash)
            {
                //GameObjects.Hero.X = boxTarget.X;
                //GameObjects.Hero.Y = boxTarget.Y;
                if (!boxRemove.collected)
                    return;

                this.api.Logic.Target = null;

                if (boxTarget.CurrentDistance > 5)
                    return;

                this.api.AddLootBox(boxTarget.DisplayType, 1);
            }
            if (this.api.Logic.Target is Ore oreTarget && oreTarget.Hash == boxRemove.hash)
            {
                if (!boxRemove.collected)
                    return;

                this.api.Logic.Target = null;

                if (oreTarget.CurrentDistance >= 10)
                    return;

                this.api.Hero.CollectStartTime = DateTime.MinValue;
                this.api.AddLootBox(oreTarget.DisplayType, 1);
            }
            if (this.api.Map.IsGalaxyGate
                && this.api.Collectibles.TryGetValue(boxRemove.hash, out Collectible? collectible) && (collectible is not Box b || (b.Type != CommandConstants.BOX_SHIP && b.Type != CommandConstants.CANDY_CARGO))
                && collectible.CurrentDistance > 600 && !boxRemove.collected)
            {
                return;
            }
            this.api.Collectibles.Remove(boxRemove.hash, out _);
            this.api.Mines.Remove(boxRemove.hash, out _);
        }
    }
}
