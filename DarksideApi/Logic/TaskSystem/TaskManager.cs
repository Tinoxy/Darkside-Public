using DarksideApi.DarkOrbit;

namespace DarksideApi.Logic.TaskSystem
{
    public class TaskManager
    {
        private readonly Api api;
        private int CurrentTaskIndex;
        public TaskModule CurrentTask { get; private set; }

        public TaskManager(Api api)
        {
            this.api = api;
            this.CurrentTaskIndex = 0;
            this.CurrentTask = null!;
        }

        public void CheckTasks()
        {
            if (!this.api.Settings.IsRunning)
            {
                return;
            }
            if (!this.api.Settings.UseTaskSystem)
            {
                return;
            }
            if (this.CurrentTask == null || ((DateTime.UtcNow - this.CurrentTask.StartTime).TotalMinutes > this.CurrentTask.Duration))
            {
                this.StartNewTask();
                return;
            }
        }

        public void StopTasks()
        {
            if (this.CurrentTask == null)
            {
                return;
            }
            this.CurrentTask.Deactivate(true);
        }

        private void StartNewTask()
        {
            if (this.CurrentTask != null)
            {
                this.CurrentTask.Deactivate();
            }
            if (this.CurrentTaskIndex >= this.api.Settings.TaskSettings.Count)
            {
                this.CurrentTask = null!;
                this.CurrentTaskIndex = 0;
                this.api.WriteLog("All Tasks finished, restarting list...");
                return;
            }
            for (var i = this.CurrentTaskIndex; i < this.api.Settings.TaskSettings.Count; i++)
            {
                DarkOrbit.Settings.TaskSetting setting = this.api.Settings.TaskSettings.ElementAt(i);
                if (setting == null || !setting.IsEnabled)
                {
                    continue;
                }
                this.CurrentTask = new(this.api, setting, i);
                this.CurrentTaskIndex = i + 1;
                break;
            }
            this.CurrentTask?.Activate();
        }

        public static string TaskTypeToDisplay(TaskType type)
        {
            return type switch
            {
                TaskType.KillNpc => "Shoot Npcs",
                TaskType.DoGalaxyGates => "Do Galaxy Gates",
                TaskType.CollectBoxes => "Collect Boxes",
                TaskType.ChangeWorkingMap => "Change Working Map",
                TaskType.Pause => "Pause",
                _ => "None",
            };
        }
    }
}
