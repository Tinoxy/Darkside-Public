namespace DarksideApi.DarkOrbit.GameObservers
{
    interface IGameObserver
    {
        public void StartObserver();
        public void StopObserver();
        public void Observe();
    }
}
