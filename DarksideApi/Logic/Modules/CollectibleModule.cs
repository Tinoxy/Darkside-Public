using DarksideApi.DarkOrbit;
using DarksideApi.DarkOrbit.Objects.Boxes;

namespace DarksideApi.Logic.Modules
{
    internal class CollectibleModule(Api api) : ILogicModule
    {
        private const int COLLECTIBLE_COLLECT_RANGE = 10;
        private bool GotoCollectible()
        {
            if (api.Logic.Target == null)
            {
                api.Logic.SetState(this, LogicState.None);
                return true;
            }

            var targetCollectible = (Collectible)api.Logic.Target;

            if (!api.Collectibles.ContainsKey(targetCollectible.Hash))
            {
                api.Logic.SetState(this, LogicState.None);
                return true;
            }
            if ((DateTime.UtcNow - targetCollectible.tempStartMoveTime).TotalSeconds > 34
                    || (targetCollectible.CurrentDistance >= COLLECTIBLE_COLLECT_RANGE
                    && targetCollectible.TargetDistance >= COLLECTIBLE_COLLECT_RANGE
                    && !targetCollectible.IsOnTarget()))
            {
                api.WriteLog($"Moving to collectible: {targetCollectible.DisplayType}.");
                targetCollectible.tempStartMoveTime = DateTime.UtcNow;
                targetCollectible.IncreaseMoveTries();
                api.GameMethods.Move(targetCollectible.GetPosition());
                Thread.Sleep(Logic.GetRandomSleepTime(100, 200));
                return true;
            }
            if (targetCollectible.IsOnTarget()
                && targetCollectible.CurrentDistance >= COLLECTIBLE_COLLECT_RANGE
                && !api.Hero.IsFlying)
            {
                api.WriteLog($"Re-Moving to collectible: {targetCollectible.DisplayType}.");
                api.GameMethods.Move(targetCollectible.GetPosition());
                Thread.Sleep(Logic.GetRandomSleepTime(100, 200));
            }

            if (targetCollectible.CurrentDistance < COLLECTIBLE_COLLECT_RANGE)
            {
                api.Logic.SetState(this, LogicState.Collectible);
                return true;
            }

            Collectible possibleCollectible = api.Logic.GetClosestCollectible();

            if (possibleCollectible.Hash == targetCollectible.Hash || possibleCollectible.tempSelectedAsCloser)
            {
                return true;
            }

            if (possibleCollectible.CurrentDistance < targetCollectible.CurrentDistance)
            {
                api.WriteLog($"Going to closer: {possibleCollectible.DisplayType}.");
                api.Logic.SetState(this, LogicState.GotoCollectible);
                possibleCollectible.tempSelectedAsCloser = true;
                api.Logic.Target = possibleCollectible;
                return true;
            }
            return false;
        }

        private bool CollectCollectible()
        {
            if (api.Logic.Target == null)
            {
                api.Logic.SetState(this, LogicState.None);
                return true;
            }

            var targetCollectible = (Collectible)api.Logic.Target!;

            if (!api.Collectibles.ContainsKey(targetCollectible.Hash))
            {
                api.Logic.SetState(this, LogicState.None);
                return true;
            }
            if (targetCollectible.CurrentDistance >= COLLECTIBLE_COLLECT_RANGE)
            {
                api.Logic.SetState(this, LogicState.GotoCollectible);
                return true;
            }

            if (targetCollectible is Box box)
            {
                this.CollectBox(box);
            }
            else if (targetCollectible is Ore ore)
            {
                this.CollectOre(ore);
            }

            api.Logic.SetState(this, LogicState.None);
            return true;
        }

        private void CollectBox(Box targetBox)
        {
            api.GameMethods.BoxCollect(targetBox);
            Thread.Sleep(targetBox.GetWaitTime());

            //api.AddLootBox(targetBox.DisplayType, 1);
            api.Collectibles.Remove(targetBox.Hash, out _);
        }

        private void CollectOre(Ore targetOre)
        {
            api.GameMethods.OreCollect(targetOre);
            api.Hero.CollectStartTime = DateTime.UtcNow;


            while (api.Hero.IsCollecting)
            {
                Thread.Sleep(10);
                continue;
            }

            //api.AddLootBox(targetOre.DisplayType, 1);
            api.Collectibles.Remove(targetOre.Hash, out _);
        }

        public bool Run()
        {
            return api.Logic.State == LogicState.GotoCollectible
                && this.GotoCollectible() || api.Logic.State == LogicState.Collectible && this.CollectCollectible();
        }
    }
}
