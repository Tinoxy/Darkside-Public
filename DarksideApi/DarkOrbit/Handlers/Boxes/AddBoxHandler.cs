using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Commands.Boxes;
using DarksideApi.DarkOrbit.Objects.Boxes;

namespace DarksideApi.DarkOrbit.Handlers.Boxes
{
    internal class AddBoxHandler : ICommandHandler
    {
        private readonly Api api;

        public AddBoxHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            AddBoxCommand boxInit = new();
            boxInit.Read(@byte);

            Box box = new(this.api, boxInit.Hash, boxInit.X, boxInit.Y, boxInit.type!);
            if (!this.api.Collectibles.TryAdd(boxInit.Hash, box))
            {
                this.api.Collectibles[boxInit.Hash] = box;
            }
            if (!CommandConstants.BOX_TYPES.Contains(boxInit.type))
            {
                CommandConstants.AddBoxType(boxInit.type);
            }
        }
    }
}
