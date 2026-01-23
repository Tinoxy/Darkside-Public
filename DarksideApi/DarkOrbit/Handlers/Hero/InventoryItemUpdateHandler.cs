using DarksideApi.DarkOrbit.Commands.Hero;

namespace DarksideApi.DarkOrbit.Handlers.Hero
{
    internal class InventoryItemUpdateHandler : ICommandHandler
    {
        private readonly Api api;

        public InventoryItemUpdateHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            InventoryItemUpdatedCommand inventoryItemUpdate = new();
            inventoryItemUpdate.Read(@byte);

            var current = this.api.Inventory.Get(inventoryItemUpdate.item.lootId);
            var newCurrentValue = 0L;
            if (current > 0)
            {
                newCurrentValue = (long)inventoryItemUpdate.item.amount - current;
            }

            this.api.Inventory.Add(inventoryItemUpdate.item.lootId, (long)inventoryItemUpdate.item.amount);
            if (inventoryItemUpdate.item.lootId == "currency_uridium")
            {
                this.api.Statistics.CollectedUridium += newCurrentValue;
                this.api.Statistics.UpdateUridiumTotal((long)inventoryItemUpdate.item.amount);
                return;
            }
            if (inventoryItemUpdate.item.lootId == "currency_credits")
            {
                this.api.Statistics.CollectedCredits += newCurrentValue;
                this.api.Statistics.UpdateCreditsTotal((long)inventoryItemUpdate.item.amount);
                return;
            }

            var statsName = inventoryItemUpdate.item.lootId.Split('_').Last().ToUpper();
            this.api.AddLootResource(statsName, newCurrentValue, (long)inventoryItemUpdate.item.amount);
            this.api.Logging.Log(inventoryItemUpdate.ToString());
        }
    }
}
