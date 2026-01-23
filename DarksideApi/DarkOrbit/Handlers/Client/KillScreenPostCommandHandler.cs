using DarksideApi.DarkOrbit.Commands.Client;
using DarksideApi.DarkOrbit.Objects.Ship;
using DarksideApi.Logic;

namespace DarksideApi.DarkOrbit.Handlers.Client
{
    internal class KillScreenPostCommandHandler : ICommandHandler
    {
        private readonly Api api;

        public KillScreenPostCommandHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            KillScreenPostCommand screen = new();
            screen.Read(@byte);

            if (this.api.Map.IsGalaxyGate)
            {
                this.api.Hero.HasSpawnedDestroyed = true;
            }
            if (this.api.Logic.Target is Player player && this.api.Settings.OnAttackedShootback)
            {
                if (!this.api.AutoAttackKillAmounts.TryAdd(player.UserId, 1))
                    this.api.AutoAttackKillAmounts[player.UserId] += 1;
            }

            this.api.Movement.ResetZones();
            this.api.Hero.Reset();
            this.api.Logic.SetState(this, LogicState.Revive);
            this.api.Logic.Target = null;

            _ = this.api.LastDeaths.TryAdd(DateTime.UtcNow, screen.killerName);

            this.api.WriteLog("Destroyed by " + screen.killerName);
            this.api.AddLootDeath(screen.killerName, 1);
            this.api.AddLastPosition(-1, -1);

            if (this.api.Settings.AutoStart && !this.api.Settings.IsRunning)
            {
                //Task.Delay(3000).ContinueWith(_ => api.Logic.StartLogic());
            }
        }
    }
}
