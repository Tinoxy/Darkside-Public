using DarksideApi.Logic.TaskSystem;

namespace DarksideApi.DarkOrbit.Settings
{
    public class TaskSetting : Setting
    {
        public TaskType TaskType { get; set; }
        public double Duration { get; set; }
        public int Option { get; set; } = -1;

        public string TaskTypeDisplay => TaskManager.TaskTypeToDisplay(this.TaskType);

    }
}
