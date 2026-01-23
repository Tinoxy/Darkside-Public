using DarksideApi.DarkOrbit;
using DarksideApi.DarkOrbit.Objects.Boxes;
using DarksideApi.DarkOrbit.Objects.Ship;

namespace DarksideApi.Logic.Modules
{
    internal class DefaultModule(Api api) : ILogicModule
    {
        public bool Run()
        {
            if (api.Logic.State != LogicState.None)
            {
                return false;
            }

            api.Statistics.StartSearchTime();

            Collectible closestCollectible = api.Settings.GeneralCollectBoxes ? api.Logic.GetClosestCollectible() : Collectible.Empty();
            Npc closestNpc = api.Settings.GeneralShootNpcs ? api.Logic.GetClosestNpc() : Npc.Empty();

            if (api.Settings.GeneralCollectBoxes && !string.IsNullOrEmpty(closestCollectible.Hash!) && closestCollectible.CurrentDistance < closestNpc.CurrentDistance)
            {
                api.Statistics.AddSearchTime();
                api.Logic.SetState(this, LogicState.GotoCollectible);
                api.Logic.Target = closestCollectible;
                api.WriteLog($"Going to next {closestCollectible.DisplayType}.");
                return true;
            }

            if (api.Settings.GeneralShootNpcs && closestNpc.UserId > 0)
            {
                api.Statistics.AddSearchTime();
                api.Logic.SetState(this, LogicState.GotoNpc);
                api.Logic.Target = closestNpc;
                api.WriteLog($"Going to next {closestNpc.Username}");
                return true;
            }

            if (!api.Hero.IsRepairing
                && api.Hero.HitpointsPercentage < 99
                && api.EntitiesUnderAttackBy.IsEmpty)
            {
                api.GameMethods.Repair();
            }
            if (!api.Hero.IsFlying)// || ((HelpTools.GetChance(2) && api.Hero.FlyingTimeMs < Random.Shared.Next(1000, 2000) && api.Hero.LastPathElement == api.Hero.Position)))
            {
                api.Logic.SetState(this, LogicState.Roaming);
            }
            return false;
        }
    }
}
