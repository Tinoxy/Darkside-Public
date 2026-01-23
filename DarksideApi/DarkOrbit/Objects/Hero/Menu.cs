using DarksideApi.Util.Collections;
using DarksideApi.Util.Extensions;

namespace DarksideApi.DarkOrbit.Objects.Hero
{
    public class Menu
    {
        public ObservableKeyedCollection<string, MenuItem> MenuItems { get; set; }
        public ObservableKeyedCollection<string, MenuItem> AvailableMenuItems { get; set; }

        public Menu()
        {
            this.MenuItems = new ObservableKeyedCollection<string, MenuItem>(x => x.Id, "Id");
            this.AvailableMenuItems = new ObservableKeyedCollection<string, MenuItem>(x => x.Id, "Id");
        }

        public void Add(MenuItem item)
        {
            if (this.MenuItems.TryAdd(item.Id, item))
            {
                return;
            }
            this.MenuItems[item.Id].Id = item.Id;
            this.MenuItems[item.Id].Amount = item.Amount;
            this.MenuItems[item.Id].IsActivatable = item.IsActivatable;
            this.MenuItems[item.Id].IsAvailable = item.IsAvailable;
            this.MenuItems[item.Id].IsSelected = item.IsSelected;
            this.MenuItems[item.Id].Type = item.Type;
        }

        public void AddAvailableMenuItem(MenuItem item)
        {
            if (!item.IsAvailable)
                return;

            if (this.AvailableMenuItems.TryAdd(item.Id, item))
            {
                return;
            }
            this.AvailableMenuItems[item.Id].Id = item.Id;
            this.AvailableMenuItems[item.Id].Amount = item.Amount;
            this.AvailableMenuItems[item.Id].IsActivatable = item.IsActivatable;
            this.AvailableMenuItems[item.Id].IsAvailable = item.IsAvailable;
            this.AvailableMenuItems[item.Id].IsSelected = item.IsSelected;
            this.AvailableMenuItems[item.Id].Type = item.Type;
        }

        public MenuItem Get(string key)
        {
            return this.MenuItems.TryGetValue(key, out MenuItem? value) ? value : MenuItem.Empty();
        }

        public bool IsSelected(string key)
        {
            return this.MenuItems.TryGetValue(key, out MenuItem? value) && value.IsSelected;
        }

        public bool IsCloakAvailable => this.MenuItems.TryGetValue(Commands.CommandConstants.EQUIPMENT_CLOAK, out MenuItem? cloak) && cloak.IsAvailable;
    }
}
