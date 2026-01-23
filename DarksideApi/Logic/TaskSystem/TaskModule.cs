using DarksideApi.DarkOrbit;
using DarksideApi.DarkOrbit.Settings;

namespace DarksideApi.Logic.TaskSystem
{
    public class TaskModule
    {
        private readonly Api api;
        private TaskSettingsCopy settingsCopy;
        private readonly TaskSetting taskSetting;
        public int Id { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime StartTime { get; private set; }

        public double Duration => this.taskSetting.Duration;

        public TaskModule(Api api, TaskSetting taskSetting, int id = -1)
        {
            this.api = api;
            this.settingsCopy = new();
            this.taskSetting = taskSetting;
            this.Id = id < 0 ? Random.Shared.Next(1000, 100000) : id;
            this.StartTime = DateTime.MinValue;
            this.IsActive = false;
        }

        public void Activate()
        {
            this.IsActive = true;
            this.CopySettings();
            this.OverrideSettings();
            this.StartTime = DateTime.UtcNow;
            this.api.WriteLog($"Starting next Task: {this.taskSetting.TaskTypeDisplay}, {Math.Round(this.Duration, 0)}Min");
        }

        public void Deactivate(bool forced = false)
        {
            this.IsActive = false;
            this.RevertSettings();

            if (!forced)
                this.api.WriteLog($"Finished Task: {this.taskSetting.TaskTypeDisplay}");
        }

        private void OverrideSettings()
        {
            if (!this.taskSetting.IsEnabled)
            {
                this.Deactivate();
                return;
            }
            this.DeactivateAllSettings();
            switch (this.taskSetting.TaskType)
            {
                case TaskType.CollectBoxes:
                    this.api.Settings.GeneralCollectBoxes = true;
                    break;
                case TaskType.DoGalaxyGates:
                    this.api.Settings.GeneralDoGalaxyGates = true;
                    break;
                case TaskType.KillNpc:
                    this.api.Settings.GeneralShootNpcs = true;
                    break;
                case TaskType.ChangeWorkingMap:
                    if (this.taskSetting.Option < 1)
                    {
                        this.Deactivate();
                        return;
                    }
                    this.api.Settings.UseMap = true;
                    this.api.Settings.MapSelect = this.taskSetting.Option;
                    break;
                case TaskType.Pause:
                    this.api.Settings.GeneralPauseUsePause = true;
                    this.api.Settings.GeneralPauseAfterPlayTime = 0.0;
                    this.api.Settings.GeneralPausePauseTime = this.taskSetting.Duration / 60;
                    break;
                case TaskType.None:
                default:
                    this.Deactivate();
                    return;
            }
        }

        private void DeactivateAllSettings()
        {
            this.api.Settings.GeneralShootNpcs = false;
            this.api.Settings.GeneralCollectBoxes = false;
            this.api.Settings.GeneralDoGalaxyGates = false;
            this.api.Settings.GeneralPauseUsePause = false;
        }

        private void CopySettings()
        {
            this.settingsCopy = new(this.api.Settings.GeneralShootNpcs, this.api.Settings.GeneralCollectBoxes, this.api.Settings.GeneralDoGalaxyGates, this.api.Settings.MapSelect, this.api.Settings.GeneralPauseUsePause, this.api.Settings.GeneralPauseAfterPlayTime, this.api.Settings.GeneralPausePauseTime);
        }

        private void RevertSettings()
        {
            this.api.Settings.GeneralShootNpcs = this.settingsCopy.shootNpcs;
            this.api.Settings.GeneralCollectBoxes = this.settingsCopy.collectBoxes;
            this.api.Settings.GeneralDoGalaxyGates = this.settingsCopy.doGalaxyGates;
            //DO NOT REVERT MAP -> TO NOT BE STUCK IN POSSIBLE MAP SWITCHING LOOP
            this.api.Settings.GeneralPauseUsePause = this.settingsCopy.usePause;
            this.api.Settings.GeneralPauseAfterPlayTime = this.settingsCopy.pauseAfterPlayTime;
            this.api.Settings.GeneralPausePauseTime = this.settingsCopy.pausePauseTime;
        }
    }
}
