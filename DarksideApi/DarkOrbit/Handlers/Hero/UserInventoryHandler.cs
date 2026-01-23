using DarksideApi.DarkOrbit.Commands.Hero;

namespace DarksideApi.DarkOrbit.Handlers.Hero
{
    internal class UserInventoryHandler : ICommandHandler
    {
        private readonly Api api;

        public UserInventoryHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            UserInventoryModule inventoryInit = new();
            inventoryInit.Read(@byte);

            //api.Logging.WriteLine("INV INIT: " + inventoryInit.ToString());

            foreach (InventoryItemModule item in inventoryInit.inventoryItemModules)
            {
                this.api.Inventory.Add(item.lootId, (long)item.amount);
                if (item.lootId == "currency_uridium")
                {
                    this.api.Statistics.UpdateUridiumTotal((long)item.amount);
                    continue;
                }
                if (item.lootId == "currency_credits")
                {
                    this.api.Statistics.UpdateCreditsTotal((long)item.amount);
                    continue;
                }
                //var a = string.Empty;
                //item.attributes.ForEach(x => a += x.ToString() + ";");
                //api.Logging.WriteLine("Added inventory: " +  item.lootId + ", attributes: " + a);
                //var statsName = item.lootId.Split('_').Last().ToUpper();
                //api.AddLoot(statsName, 0L, (long)item.amount);
            }
        }
    }
}
