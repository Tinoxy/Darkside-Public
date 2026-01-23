namespace DarksideApi.DarkOrbit.Cooldown
{
    public class Cooldown
    {
        private readonly Api api;
        private readonly Dictionary<CooldownType, DateTime> cooldowns;

        public Cooldown(Api api)
        {
            this.api = api;
            this.cooldowns = [];
        }

        public bool IsCooldownActive(CooldownType type)
        {
            this.CooldownDurationCheck(type);
            return this.cooldowns.ContainsKey(type);
        }

        public void CooldownDurationCheck(CooldownType type)
        {
            if (!this.cooldowns.TryGetValue(type, out DateTime finishTime))
            {
                return;
            }
            if ((DateTime.UtcNow - finishTime).TotalSeconds <= 0)
            {
                return;
            }
            this.cooldowns.Remove(type);
        }

        public void AddCooldown(CooldownType type, int secondsUntilFinished)
        {
            this.CooldownDurationCheck(type);
            if (!this.cooldowns.TryAdd(type, DateTime.UtcNow.AddSeconds(secondsUntilFinished)))
            {
                return;
            }
        }

        public bool RemoveCooldown(CooldownType type)
        {
            return this.cooldowns.Remove(type);
        }

        public void HeroInitCooldowns()
        {
            if (this.api.GameClient.GameState >= GameState.Ready)
            {
                return;
            }
            this.AddCooldown(CooldownType.DRONE_FORMATION, 5);
            this.AddCooldown(CooldownType.CPU_SPECIALJUMP, 15);
            this.AddCooldown(CooldownType.CONFIG, 5);
            this.AddCooldown(CooldownType.LOGIN, 10);
        }
    }
}
