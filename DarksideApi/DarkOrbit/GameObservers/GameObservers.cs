using DarksideApi.Logic.TaskSystem;

namespace DarksideApi.DarkOrbit.GameObservers
{
    public class GameObservers
    {
        public GameTaskObserver GameTaskObserver { get; private set; }
        public SessionObserver SessionObserver { get; private set; }
        public TaskManager TaskManager { get; private set; }

        public GameObservers(Api api)
        {
            this.GameTaskObserver = new(api);
            this.SessionObserver = new(api);
            this.TaskManager = new(api);
        }

        public void Stop()
        {
            this.GameTaskObserver.StopObserver();
            this.SessionObserver.StopObserver();
            //this.TaskManager.StopTasks();
        }
    }
}
