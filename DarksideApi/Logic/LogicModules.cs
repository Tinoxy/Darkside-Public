using DarksideApi.DarkOrbit;
using DarksideApi.Logic.Modules;

namespace DarksideApi.Logic
{
    internal class LogicModules
    {
        private static List<ILogicModule> Create(Api api)
        {
            return [
            new CaptchaModule(api),
            new ReviveModule(api),
            new SettingsCheckModule(api),
            new AvoidModule(api),
            new JumpModule(api),
            new ShootbackModule(api),
            new FleeModule(api),
            new RepairModule(api),
            new TradeModule(api),
            new EnemyOnMapModule(api),
            new GalaxyGateModule(api),
            new DefaultModule(api),
            new CollectibleModule(api),
            new NpcModule(api),
            new RoamModule(api),
            ];
        }

        internal static List<ILogicModule> GetModules(Api api)
        {
            return Create(api);
        }
    }
}
