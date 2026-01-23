using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Settings;
using System.Collections.ObjectModel;

namespace DarksideApi.DarkOrbit.Objects.Boxes
{
    public class Box(Api api, string hash, int x, int y, string type, int captchaIndex = -1) : Collectible(api, hash, x, y), IGameObject
    {
        public string Type { get; private set; } = type;
        public int CaptchaIndex { get; private set; } = captchaIndex;
        public bool IsShipCargo => this.Type == CommandConstants.BOX_SHIP;
        public bool IsBootyBox => this.Type.Contains("BOOTY");

        public bool IsValidBox(KeyedCollection<string, BoxSetting> boxCollectTypes)
        {
            if (this == null
                || boxCollectTypes == null
                || !this.HasApi
                || !boxCollectTypes.TryGetValue(this.Type, out BoxSetting? setting)
                || (this.IsShipCargo && api.Hero.Cargo.FreeCargoSpace <= 0))
            {
                return false;
            }

            var flag = this.Type switch
            {
                CommandConstants.BOX_PIRATE_DEFAULT or CommandConstants.BOX_PIRATE_GOLD => api.Inventory.BootyKeysGreen > 0,
                CommandConstants.BOX_PIRATE_BLUE => api.Inventory.BootyKeysBlue > 0,
                CommandConstants.BOX_PIRATE_RED => api.Inventory.BootyKeysRed > 0,
                CommandConstants.BOX_PIRATE_SILVER => api.Inventory.BootyKeysSilver > 0,
                CommandConstants.BOX_PIRATE_EGG => api.Inventory.BootyKeysLucientEgg > 0,
                CommandConstants.BOX_PIRATE_BLACK => api.Inventory.BootyKeysBlack > 0,
                CommandConstants.BOX_ASTRAL => api.Inventory.BootyKeysAstral > 0,
                CommandConstants.BOX_ASTRAL_PRIME => api.Inventory.BootyKeysAstralPrime > 0,
                CommandConstants.BOX_EMPYRIAN => api.Inventory.BootyKeysEmpyrian > 0,
                CommandConstants.BOX_PROMETHEUS => api.Inventory.BootyKeysPrometheus > 0,
                CommandConstants.BOX_LAUREL => api.Inventory.BootyKeysLaurel > 0,
                CommandConstants.BOX_PROSPEROUS => api.Inventory.BootyKeysProsperous > 0,
                _ => true,
            };
            return flag && setting.IsEnabled;
        }

        public int GetWaitTime()
        {
            return this.IsBootyBox
                ? 5555
                : this.Type switch
                {
                    CommandConstants.BOX_MUCOSUM or CommandConstants.BOX_SOLAR or CommandConstants.BOX_SCRAP or CommandConstants.BOX_BONUS or CommandConstants.BOX_ALIEN_EGG => 10,
                    CommandConstants.BOX_SHIP => Logic.Logic.GetRandomSleepTime(70, 120),
                    CommandConstants.BOX_PIRATE_BLUE or CommandConstants.BOX_PIRATE_RED or CommandConstants.BOX_PIRATE_GOLD or CommandConstants.BOX_PIRATE_SILVER or CommandConstants.BOX_PIRATE_DEFAULT or CommandConstants.BOX_ASTRAL or CommandConstants.BOX_BATTLEPASS => 5000,
                    _ => 100,
                };
        }

        public static Box Empty()
        {
            return new Box(null, string.Empty, 0, 0, string.Empty);
        }
    }
}
