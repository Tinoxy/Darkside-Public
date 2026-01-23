using DarksideApi.DarkOrbit;
using DarksideApi.DarkOrbit.Objects.Map;

namespace DarksideApi.Logic.Modules
{
    internal class EnemyOnMapModule(Api api) : ILogicModule
    {
        public bool Run()
        {
            if (api.Logic.State == LogicState.EnemyOnMap)
            {
                if (api.Settings.MaxEnemysOnMap > 0 && (api.Map.EnemysCount < api.Settings.MaxEnemysOnMap || api.Map.EnemysCount <= 0))
                {
                    api.Logic.SetState(this, LogicState.None);
                    return true;
                }
                Gate gate = api.Logic.GetClosestGate();

                if (gate.TargetDistance >= 30)
                {
                    api.WriteLog($"{(api.Map.EnemysCount > 1 ? "Enemies" : "Enemy")} on Map, going to Gate...");
                    api.GameMethods.Move(gate, 10);
                    Thread.Sleep(Logic.GetRandomSleepTime());
                    return true;
                }
                if (gate.CurrentDistance < 30)// || GameObjects.Hero.FlyingTimeMs < 50)
                {
                    api.WriteLog($"Waiting for {(api.Map.EnemysCount > 1 ? "Enemies" : "Enemy")} to leave Map...");
                    Thread.Sleep(Logic.GetRandomSleepTime(16000, 36000));
                    api.GameMethods.Move(gate.X + Random.Shared.Next(-30, 30), gate.Y + Random.Shared.Next(-30, 30));
                    return true;
                }
            }

            if (api.Settings.MaxEnemysOnMap > 0 && api.Map.EnemysCount >= api.Settings.MaxEnemysOnMap && api.Logic.State != LogicState.EnemyOnMap)
            {
                api.Logic.SetState(this, LogicState.EnemyOnMap);
                return true;
            }
            return false;
        }
    }
}
