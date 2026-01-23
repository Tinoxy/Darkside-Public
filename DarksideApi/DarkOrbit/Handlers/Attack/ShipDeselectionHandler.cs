using DarksideApi.DarkOrbit.Commands.Attack;
using DarksideApi.DarkOrbit.Objects.Ship;

namespace DarksideApi.DarkOrbit.Handlers.Attack
{
    public class ShipDeselectionHandler : ICommandHandler
    {
        private readonly Api api;
        public ShipDeselectionHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            ShipDeselectionCommand deselectTarget = new();
            deselectTarget.Read(@byte);

            if (this.api.Hero.IsAttacking && this.api.Logic.Target is Npc npc)
            {
                npc.IsSelected = false;
            }
        }
    }
}
