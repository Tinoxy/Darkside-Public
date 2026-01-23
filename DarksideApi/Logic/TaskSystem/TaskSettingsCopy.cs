namespace DarksideApi.Logic.TaskSystem
{
    internal class TaskSettingsCopy
    {
        public readonly bool shootNpcs;
        public readonly bool collectBoxes;
        public readonly bool doGalaxyGates;
        public readonly int workingMap;
        public readonly bool usePause;
        public readonly double pauseAfterPlayTime;
        public readonly double pausePauseTime;

        public TaskSettingsCopy(bool shootNpcs, bool collectBoxes, bool doGalaxyGates, int workingMap, bool usePause, double pauseAfterPlayTime, double pausePauseTime)
        {
            this.shootNpcs = shootNpcs;
            this.collectBoxes = collectBoxes;
            this.doGalaxyGates = doGalaxyGates;
            this.workingMap = workingMap;
            this.usePause = usePause;
            this.pauseAfterPlayTime = pauseAfterPlayTime;
            this.pausePauseTime = pausePauseTime;
        }

        public TaskSettingsCopy()
        {
        }
    }
}
