using DarksideApi.DarkOrbit;
using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Objects.Map;

namespace DarksideApi.Logic.Modules
{
    internal class JumpModule(Api api) : ILogicModule
    {
        public bool Run()
        {
            if (api.Logic.State == LogicState.GotoGateJump)
            {
                if (!api.Logic.MoveToPortalToJumpToMap(api.Settings.MapSelect))
                {
                    api.Logic.SetState(this, LogicState.None);
                    return false;
                }
                api.Logic.SetState(this, LogicState.GateJump);
            }
            if (api.Logic.State == LogicState.GotoGateJumpToAccessGG)
            {
                if (!api.Logic.MoveToPortalToJumpToMap(api.Hero.GGAccessTargetMapId))
                {
                    api.Logic.SetState(this, LogicState.None);
                    return false;
                }
                api.Logic.SetState(this, LogicState.GateJump);
            }
            if (api.Logic.State == LogicState.FleeJump || api.Logic.State == LogicState.FleeJumpback)
            {
                if (api.Map.IsPirateMap)
                {
                    api.Logic.SetState(this, LogicState.GotoFleeSpotted);
                    return true;
                }
                Gate targetGate = api.Logic.GetClosestGate(1, api.Settings.OnAttackedIgnorePortalsToPvP);
                if (targetGate.CurrentDistance > 30 && targetGate.TargetDistance > 30)
                {
                    api.GameMethods.Move(targetGate);
                    Thread.Sleep(Logic.GetRandomSleepTime());
                }

                if (targetGate.CurrentDistance <= 30)
                {
                    api.GameMethods.Jump();
                    api.Logic.SetState(this, LogicState.FleeJumpback);
                    Thread.Sleep(1000);
                    return true;
                }

                if (api.Hero.Config != api.Settings.OnAttackedFleeConfig)
                {
                    api.GameMethods.ConfigurationChange(api.Settings.OnAttackedFleeConfig);
                }
                if (api.Hero.DroneFormation != api.Settings.OnAttackedFleeFormation)
                {
                    api.GameMethods.DroneFormationChange(api.Settings.OnAttackedFleeFormation);
                }

                if (api.Settings.OnAttackedFleeUseMine)
                {
                    api.GameMethods.UseMine(api.Settings.OnAttackedFleeMine);
                }
                Thread.Sleep(1000);
                return true;
            }
            if (api.Logic.State == LogicState.GateJump || api.Logic.State == LogicState.RepairJump)
            {
                if (api.Logic.Target == null)
                {
                    api.Logic.SetState(this, LogicState.None);
                    return true;
                }
                var targetGate = (Gate)api.Logic.Target;
                /*
                if (!GameObjects.Gates.ContainsKey(targetGate.GateId))
                {
                    api.Logging.WriteLineIf(App.IsDebugger, "Did not Find Gate with Id: " + targetGate.GateId);
                    State = LogicState.None;
                    return true;
                }*/
                if (targetGate == null || targetGate == Gate.Empty())
                {
                    api.Logic.SetState(this, LogicState.None);
                    return true;
                }
                if (targetGate.IsLocked)
                {
                    api.Logic.SetState(this, api.Logic.State == LogicState.RepairJump ? LogicState.Repair : LogicState.None);
                    return true;
                }
                if ((targetGate.PathDistance > 50 && targetGate.TargetDistance > 50) || !api.Hero.IsFlying)
                {
                    api.GameMethods.Move(targetGate, 50);
                    Thread.Sleep(Logic.GetRandomSleepTime());
                }
                if (targetGate.CurrentDistance < 50)
                {
                    if (!CommandConstants.MAP_NAMES.TryGetValue(targetGate.LeadsToMapId(), out var nextMap))
                    {
                        nextMap = "next Map...";
                    }
                    Thread.Sleep(2000);
                    api.WriteLog($"Jumping to {nextMap}");
                    api.Logic.SetState(this, api.Logic.State == LogicState.RepairJump ? LogicState.Repair : LogicState.None);
                    api.GameMethods.Jump();
                    Thread.Sleep(2000);
                    if (!api.Hero.IsJumping)
                    {
                        api.GameMethods.Move(targetGate, 50);
                    }
                    Thread.Sleep(3000);
                    return true;
                }
                if (api.Hero.DroneFormation != api.Settings.RoamFormation)
                {
                    api.GameMethods.DroneFormationChange(api.Settings.RoamFormation);
                }
                if (api.Hero.Config != api.Settings.RoamConfig)
                {
                    api.GameMethods.ConfigurationChange(api.Settings.RoamConfig);
                }

                Thread.Sleep(1000);
                return true;
            }
            return false;
        }
    }
}
