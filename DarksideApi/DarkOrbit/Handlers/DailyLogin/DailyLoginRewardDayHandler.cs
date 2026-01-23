using DarksideApi.DarkOrbit.Commands.DailyLogin;

namespace DarksideApi.DarkOrbit.Handlers.DailyLogin
{
    internal class DailyLoginRewardDayHandler : ICommandHandler
    {
        private readonly Api api;

        public DailyLoginRewardDayHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            if (!this.api.Settings.IsUnity)
            {
                return;
            }
            DailyLoginRewardDayCommand dailyLoginRewardDayUnity = new();
            dailyLoginRewardDayUnity.Read(@byte);

            if (!dailyLoginRewardDayUnity.claimed)
            {
                this.api.GameMethods.LoginRewardClaim();
                this.api.WriteLog("Claiming Daily Login Reward for Day " + dailyLoginRewardDayUnity.day + ", Month: " + dailyLoginRewardDayUnity.month);
            }
        }
    }
}
