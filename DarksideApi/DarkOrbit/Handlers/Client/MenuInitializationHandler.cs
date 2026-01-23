using DarksideApi.DarkOrbit.Commands.Client;
using DarksideApi.DarkOrbit.Objects.Hero;
using System.Diagnostics;

namespace DarksideApi.DarkOrbit.Handlers.Client
{
    internal class MenuInitializationHandler : ICommandHandler
    {
        private readonly Api api;

        public MenuInitializationHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            MenuInitializationCommand menuInit = new();
            menuInit.Read(@byte);

            foreach (MenuCategoryInitializationCommand category in menuInit.categories)
            {
                Debug.WriteLine("Menu Category: " + category.categoryId);
                var list = new List<MenuItem>();
                foreach (MenuItemInitializationCommand menuItem in category.menuItems)
                {
                    var item = new MenuItem(menuItem.status.menuItemId, menuItem.status.counterValue, menuItem.status.selected, menuItem.status.available, menuItem.status.activatable);

                    switch (item.Type)
                    {
                        case MenuItemType.AMMUNITION:
                            if (item.IsSelected)
                                this.api.Hero.CurrentLaser = item;
                            break;
                        case MenuItemType.ROCKETLAUNCHER:
                            if (item.IsSelected)
                                this.api.Hero.CurrentRocketLauncher = item;
                            break;
                        case MenuItemType.ROCKET:
                            if (item.IsSelected)
                                this.api.Hero.CurrentRocket = item;
                            break;
                        case MenuItemType.ABILITY:
                            if (item.IsAvailable)
                            {
                                if (this.api.Hero.CurrentAbilities.ContainsKey(item.Id))
                                {
                                    this.api.Hero.CurrentAbilities[item.Id] = item;
                                    this.api.Logging.Log("Updated usable Ability: " + item.Id);
                                    break;
                                }
                                this.api.Hero.CurrentAbilities.TryAdd(item.Id, item);
                                this.api.Logging.Log("Added usable Ability: " + item.Id);
                            }
                            break;
                    }

                    list.Add(item);
                    //api.Menu.Add(menuItem.status.menuItemId, item);
                }

                this.api.AddMenuItems(list);
            }
        }
    }
}
