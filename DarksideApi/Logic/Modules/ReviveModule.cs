using DarksideApi.DarkOrbit;

namespace DarksideApi.Logic.Modules
{
    internal class ReviveModule(Api api) : ILogicModule
    {
        public bool Run()
        {
            if (api.Logic.State != LogicState.Revive)
            {
                return false;
            }

            Thread.Sleep(Logic.GetRandomSleepTime(api.Settings.ReviveSleep * 1000, (api.Settings.ReviveSleep * 1000) + 1000));

            api.WriteLog("Reviving...");
            api.GameMethods.StatusSelect(api.Hero.HasSpawnedDestroyed ? 1 : api.Settings.ReviveOption + 1);

            api.GameClient.RestartTimer();
            api.Hero.HasSpawnedDestroyed = false;
            api.Movement.ResetPath();

            Thread.Sleep(Logic.GetRandomSleepTime(2000, 4000));

            api.GameClient.GameState = GameState.Reconnect;

            api.Logic.SetState(this, LogicState.None);

            return true;
        }
    }
}
