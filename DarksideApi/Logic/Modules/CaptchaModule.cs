using DarksideApi.DarkOrbit;
using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Objects.Boxes;

namespace DarksideApi.Logic.Modules
{
    internal class CaptchaModule(Api api) : ILogicModule
    {
        public bool Run()
        {
            if (api.Captcha != null && api.Logic.State != LogicState.Captcha && api.Logic.State != LogicState.CaptchaSolve)
            {
                api.Logic.SetState(this, LogicState.Captcha);
            }
            if (api.Logic.State == LogicState.Captcha)
            {
                api.WriteLog("Captcha Found!");
                api.WriteLog($"Will collect {api.Captcha.Amount}x {(api.Captcha.BoxToCollect == CommandConstants.BOX_CAPTACHA_RED ? "Red" : "Black")} Boxes.");
                Thread.Sleep(1000);
                api.Logic.SetState(this, api.Captcha != null ? LogicState.CaptchaSolve : LogicState.None);
            }

            if (api.Logic.State == LogicState.CaptchaSolve)
            {
                api.Logic.Target = api.Logic.GetClosestCaptchaBox();

                if (api.Logic.Target == null || api.Captcha!.Amount <= 0)
                {
                    api.Logging.Log("Captcha target is Null, Amount: " + api.Captcha!.Amount);
                    Thread.Sleep(api.Captcha.WaitTime + Logic.GetRandomSleepTime(1000, 2000));
                    api.WriteLog("Captcha Solved!");
                    api.AddLootResource(Statistics.StatisticsConstants.Captcha, 1);
                    api.Captcha = null!;
                    api.Logic.SetState(this, LogicState.None);
                    return true;
                }

                if (api.Logic.Target.TargetDistance > 10)
                {
                    api.GameMethods.Move(api.Logic.Target);
                }

                if (api.Hero.FlyingTimeMs <= 0)
                {
                    api.GameMethods.CaptchaCollect((api.Logic.Target as Box)!);
                }

                return true;
            }

            return false;
        }
    }
}
