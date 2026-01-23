using DarksideApi.DarkOrbit;
using DarksideApi.Util;

namespace DarksideApi.Logic.Modules
{
    internal class RoamModule(Api api) : ILogicModule
    {
        public bool Run()
        {
            if (api.Logic.State != LogicState.Roaming)
            {
                return false;
            }
            if (api.Hero.DroneFormation != api.Settings.RoamFormation)
            {
                api.GameMethods.DroneFormationChange(api.Settings.RoamFormation);
            }
            if (api.Hero.Config != api.Settings.RoamConfig)
            {
                api.GameMethods.ConfigurationChange(api.Settings.RoamConfig);
            }

            Thread.Sleep(Logic.GetRandomSleepTime());

            if (!api.Hero.IsFlying)
            {
                api.WriteLog("Roaming...");
            }
            if (HelpTools.CalculateDistance(api.PointOfInterest, api.Hero.Position) < 1000)
            {
                api.PointOfInterest = new(-1, -1);
            }
            api.Logic.IncreaseMoveQuarter();
            api.GameMethods.Move(api.Logic.GetRandomPosition());

            api.Logic.SetState(this, LogicState.None);
            return false;
        }
    }
}
