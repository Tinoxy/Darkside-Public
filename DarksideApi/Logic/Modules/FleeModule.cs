using DarksideApi.DarkOrbit;
using DarksideApi.DarkOrbit.Objects.Map;
using DarksideApi.DarkOrbit.Objects.Pets;
using DarksideApi.DarkOrbit.Objects.Ship;
using DarksideApi.Util;

namespace DarksideApi.Logic.Modules
{
    internal class FleeModule(Api api) : ILogicModule
    {
        public bool Run()
        {
            if (api.Logic.State == LogicState.GotoFleeSpotted)
            {
                if (!api.Ships.Values.OfType<Player>().Any(p => p.IsEnemy)
                    && !api.Ships.Values.OfType<Player>().Where(p => p.IsEnemy && p.CurrentDistance < 4242).Any()
                    && api.Logic.Target is not Player)
                {
                    api.Logic.SetState(this, LogicState.None);
                    return false;
                }
                Gate gate = api.Logic.GetClosestGate(1);

                if (HelpTools.CalculateDistance(api.Hero.LastPathElement, gate.Position) > 30)
                {
                    api.GameMethods.Move(gate, 10);
                    Thread.Sleep(Logic.GetRandomSleepTime());
                    return true;
                }
                if (gate.CurrentDistance < 30)// || GameObjects.Hero.FlyingTimeMs < 50)
                {
                    api.WriteLog($"Waiting for spotted Enemies to leave Map...");
                    api.GameMethods.Move(gate.X + Random.Shared.Next(-30, 30), gate.Y + Random.Shared.Next(-30, 30));
                    Thread.Sleep(2000);
                    return true;
                }
                return false;
            }
            if (api.Logic.State == LogicState.GotoFlee)
            {
                if (api.Logic.Target is not Player && api.Logic.Target is not Pet)
                {
                    api.Logic.SetState(this, LogicState.None);
                    return false;
                }
                if (api.Logic.Target is Player player && !player.IsSelected)
                {
                    api.GameMethods.TargetSelect(player);
                    Thread.Sleep(Logic.GetRandomSleepTime(100, 300));
                }

                Gate targetGate = api.Logic.GetClosestGate(1);
                api.Logic.Target = targetGate;

                if (targetGate.TargetDistance > 30)
                {
                    api.GameMethods.Move(targetGate);
                    Thread.Sleep(Logic.GetRandomSleepTime());
                }

                api.Logic.SetState(this, LogicState.FleeJump);
                return true;
            }
            return false;
        }
    }
}
