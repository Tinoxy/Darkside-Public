namespace DarksideApi.DarkOrbit.GameObservers
{
    public class SessionObserver : IGameObserver
    {
        private readonly Api api;
        private System.Timers.Timer? timer;

        public bool IsSessionValid { get; private set; }

        public SessionObserver(Api api)
        {
            this.api = api;
        }

        public void InitSessionObserver()
        {
            this.StopObserver();
            this.IsSessionValid = true;
            this.timer = new System.Timers.Timer(TimeSpan.FromMinutes(30))
            {
                AutoReset = true,
            };
            this.timer.Elapsed += (s, e) => this.Observe();
            this.StartObserver();
        }

        public void Observe()
        {
            //LOGOUT CHECK
            if (this.api.Hero.IsLoggingOut
                && (DateTime.UtcNow - this.api.Hero.LogoutStartTime).TotalSeconds > 120
                && this.api.GameClient.GameState == GameState.Disconnected
                && this.api.Settings.IsRunning)
            {
                this.api.Logic.SetState(this, Logic.LogicState.None);
                this.api.GameClient.GameState = GameState.Disconnected;
                this.api.WriteLog("Game Logout error! Reconnecting...");
            }

            //SESSION CHECK
            if (this.api.Settings.IsUnity)
            {
                return;
            }
            if (!this.IsSessionValid)
            {
                return;
            }
            if (this.api.BackpageApi.IsSessionValid())
            {
                this.IsSessionValid = true;
                return;
            }
            this.api.WriteLog("Session " + this.api.Hero.SessionId + " expired...");
            this.IsSessionValid = false;
        }

        public void StartObserver()
        {
            this.timer?.Start();
        }

        public void StopObserver()
        {
            this.timer?.Stop();
        }
    }
}
