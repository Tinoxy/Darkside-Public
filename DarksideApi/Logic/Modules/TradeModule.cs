using DarksideApi.DarkOrbit;
using DarksideApi.DarkOrbit.Objects.Boxes;
using DarksideApi.Util;
using System.Numerics;

namespace DarksideApi.Logic.Modules
{
    internal class TradeModule(Api api) : ILogicModule
    {
        public bool Run()
        {
            if (api.Logic.State == LogicState.SellOre)
            {
                Vector2 tradePos = new((int)(100.5 * 100) + Random.Shared.Next(-4, 4), (int)(66 * 100) + Random.Shared.Next(-4, 4));

                if (HelpTools.CalculateDistance(api.Hero.TargetX, api.Hero.TargetY, tradePos.X, tradePos.Y) > 25
                    && HelpTools.CalculateDistance(api.Hero.X, api.Hero.Y, tradePos.X, tradePos.Y) > 25)
                {
                    api.GameMethods.Move(tradePos);
                    Thread.Sleep(Logic.GetRandomSleepTime());
                    return true;
                }

                if (HelpTools.CalculateDistance(api.Hero.X, api.Hero.Y, tradePos.X, tradePos.Y) > 25)
                {
                    Thread.Sleep(1000);
                    return true;
                }

                api.GameMethods.OreTrade(api.Hero.Cargo.Get(Ore.OreType.PALLADIUM));
                Thread.Sleep(250);

                if (api.Settings.OreSellEverything)
                {
                    foreach (KeyValuePair<Ore.OreType, long> ore in api.Hero.Cargo.GetTradeableOres)
                    {
                        api.GameMethods.OreSell(ore.Key, ore.Value);
                        Thread.Sleep(Logic.GetRandomSleepTime());
                    }
                }

                api.Logic.SetState(this, LogicState.SellOreDone);
                return true;
            }

            return false;
        }
    }
}
