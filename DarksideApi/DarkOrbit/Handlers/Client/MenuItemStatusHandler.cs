using DarksideApi.DarkOrbit.Commands.Client;
using DarksideApi.DarkOrbit.Objects.Hero;
using System.Diagnostics;

namespace DarksideApi.DarkOrbit.Handlers.Client
{
    internal class MenuItemStatusHandler : ICommandHandler
    {
        private readonly Api api;

        public MenuItemStatusHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            MenuItemStatusCommand menuItemStatus = new();
            menuItemStatus.Read(@byte);

            var item = new MenuItem(menuItemStatus.menuItemId, menuItemStatus.counterValue, menuItemStatus.selected, menuItemStatus.available, menuItemStatus.activatable);
            //Debug.WriteLine(menuItemStatus.ToString());

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

            //api.Menu.Add(menuItemStatus.menuItemId, item);
            this.api.AddMenuItems([item]);
        }
    }
}
