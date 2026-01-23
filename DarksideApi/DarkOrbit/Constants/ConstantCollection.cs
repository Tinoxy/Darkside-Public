using DarksideApi.DarkOrbit.Objects.Client;
using DarksideApi.Logic.TaskSystem;

namespace DarksideApi.DarkOrbit.Constants
{
    public class ConstantCollection
    {

        public static Dictionary<TaskType, string> TASK_TYPES { get; private set; } = new()
        {
            { TaskType.CollectBoxes, "Collect Boxes" },
            { TaskType.KillNpc, "Kill NPCs" },
            { TaskType.DoGalaxyGates, "Do Galaxy Gates" },
            { TaskType.ChangeWorkingMap, "Change Working Map" },
            { TaskType.Pause, "Pause" },
        };

        public static List<DispatcherObj> DISPATCHER_DEFAULT_LIST => [
        (new(1, 10800, 0)),
                (new(2, 21600, 0)),
                (new(3, 21600, 0)),
                (new(4, 32400, 0)),
                (new(5, 43200, 0)),
                (new(6, 43200, 0)),
                (new(7, 10800, 0)),
                (new(8, 21600, 0)),
                (new(9, 21600, 0)),
                (new(10, 32400, 0)),
                (new(11, 43200, 0)),
                (new(12, 43200, 0)),
                (new(19, 18000, 0)),
                (new(20, 25200, 0)),
                (new(21, 32400, 0)),
                (new(13, 10800, 0)),
                (new(14, 21600, 0)),
                (new(15, 21600, 0)),
                (new(16, 32400, 0)),
                (new(17, 43200, 0)),
                (new(18, 43200, 0)),
            ];
    }
}
