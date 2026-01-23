using DarksideApi.DarkOrbit.Commands.Attack;

namespace DarksideApi.DarkOrbit.Handlers.Attack
{
    internal class ShipSelectionHandler : ICommandHandler
    {
        private readonly Api api;

        public ShipSelectionHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            ShipSelectionCommand targetSelected = new();
            targetSelected.Read(@byte);
            if (!this.api.Ships.ContainsKey(targetSelected.userId))
            {
                return;
            }
            this.api.Ships[targetSelected.userId].tempSelectTries = 0;
            this.api.Ships[targetSelected.userId].Shield = targetSelected.shield;
            this.api.Ships[targetSelected.userId].MaxShield = targetSelected.maxShield;
            this.api.Ships[targetSelected.userId].Hitpoints = targetSelected.hitpoints;
            this.api.Ships[targetSelected.userId].MaxHitpoints = targetSelected.maxHitpoints;
            this.api.Ships[targetSelected.userId].NanoShield = targetSelected.nanoShield;
            this.api.Ships[targetSelected.userId].MaxNanoShield = targetSelected.maxNanoShield;
            foreach (KeyValuePair<int, Objects.Ship.Ship> toBeDeselected in this.api.Ships)
            {
                toBeDeselected.Value.IsSelected = false;
            }
            this.api.Ships[targetSelected.userId].IsSelected = true;
            this.api.Ships[targetSelected.userId].ActionStartTime = DateTime.UtcNow;
        }
    }
}
