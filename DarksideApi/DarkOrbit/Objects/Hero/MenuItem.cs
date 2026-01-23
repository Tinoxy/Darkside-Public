using DarksideApi.DarkOrbit.Commands;
using DarksideApi.Util.Extensions;

namespace DarksideApi.DarkOrbit.Objects.Hero
{
    public class MenuItem
    {
        public string Id { get; set; }
        public long Amount { get; set; }
        public bool IsSelected { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsActivatable { get; set; }
        public MenuItemType Type { get; set; }
        public string DisplayName
        {
            get
            {
                var name = string.Empty;
                if (this.Type == MenuItemType.DRONE && CommandConstants.DRONE_FORMATIONS.TryGetKey(this.Id, out name))
                    return name;

                if (this.Type == MenuItemType.BUY)
                    name = "Buy ";

                return (name + this.Id.Split('_').Last().ToUpper());
            }
        }

        public MenuItem(string id, long amount, bool isSelected, bool isAvailable, bool isActivatable, MenuItemType type = MenuItemType.DEFAULT)
        {
            this.Id = id;
            this.Amount = amount;
            this.IsSelected = isSelected;
            this.IsAvailable = isAvailable;
            this.IsActivatable = isActivatable;
            this.Type = (type != MenuItemType.DEFAULT ? type : GetMenuItemType(id));
        }

        public static MenuItemType GetMenuItemType(string menuItemId)
        {
            if (menuItemId.Contains("buy_ammunition_"))
            {
                return MenuItemType.BUY;
            }
            if (menuItemId.Contains("ability_"))
            {
                return MenuItemType.ABILITY;
            }
            if (menuItemId.Contains("ammunition_laser"))
            {
                return MenuItemType.AMMUNITION;
            }
            if (menuItemId.Contains("equipment_pet"))
            {
                return MenuItemType.PET;
            }
            if (menuItemId.Contains("ammunition_rocketlauncher"))
            {
                return MenuItemType.ROCKETLAUNCHER;
            }
            if (menuItemId.Contains("ammunition_rocket_"))
            {
                return MenuItemType.ROCKET;
            }
            if (menuItemId.Contains("equipment_extra_cpu") || menuItemId.Contains("ammunition_specialammo"))
            {
                return MenuItemType.CPU;
            }
            if (menuItemId.Contains("drone_formation"))
            {
                return MenuItemType.DRONE;
            }
            return menuItemId.Contains("tech_")
                ? MenuItemType.TECH
                : menuItemId.Contains("ammunition_mine") ? MenuItemType.MINE : MenuItemType.DEFAULT;
        }

        public static MenuItem Empty()
        {
            return new(string.Empty, 0, false, false, false, MenuItemType.DEFAULT);
        }

        public static MenuItem EmptyType(MenuItemType type)
        {
            return new(string.Empty, 0, false, false, false, type);
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
