using DarksideApi.DarkOrbit.Settings;
using System.Collections.ObjectModel;
using static DarksideApi.DarkOrbit.Objects.Boxes.Ore;

namespace DarksideApi.DarkOrbit.Objects.Boxes
{
    public class Ore(Api api, string hash, int x, int y, OreType type) : Collectible(api, hash, x, y), IGameObject
    {
        public OreType Type { get; private set; } = type;

        public enum OreType
        {
            EMPTY = -1, PROMETIUM, ENDURIUM, TERBIUM, XENOMIT, PROMETID, DURANIUM, PROMERIUM, SEPROM, PALLADIUM, OSMIUM = 28
        }

        public static Ore Empty()
        {
            return new Ore(null!, string.Empty, 0, 0, OreType.EMPTY);
        }

        public bool IsTradable => this.Type == OreType.TERBIUM
            || this.Type == OreType.PROMETID
            || this.Type == OreType.PROMETIUM
            || this.Type == OreType.PROMERIUM
            || this.Type == OreType.DURANIUM
            || this.Type == OreType.ENDURIUM;

        public bool IsValidOre(ObservableCollection<OreSetting> oreCollectTypes)
        {
            return this != null
                && oreCollectTypes != null
                && this.HasApi
                && api.Hero.Cargo.FreeCargoSpace > 0 && oreCollectTypes.Any(o => o.IsEnabled && o.Type == (int)this.Type);
        }
    }
}
