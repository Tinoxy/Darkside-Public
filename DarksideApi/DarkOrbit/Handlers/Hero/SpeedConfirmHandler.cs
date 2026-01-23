using DarksideApi.DarkOrbit.Commands.Hero;
using DarksideApi.Util;

namespace DarksideApi.DarkOrbit.Handlers.Hero
{
    internal class SpeedConfirmHandler : ICommandHandler
    {
        private readonly Api api;

        public SpeedConfirmHandler(Api api) { this.api = api; }

        public void Handle(ByteArray @byte)
        {
            SpeedUpdateCommand speedConfirm = new();
            speedConfirm.Read(@byte);

            this.api.Hero.Speed = (int)Math.Max(-2147483648, Math.Min(speedConfirm.speed, 2147483647));
            //api.Logging.WriteLine($"Speed confirmed: {speedConfirm.speed}({api.Hero.Speed}), {speedConfirm.var_4774}");

            if (this.api.Hero.IsFlying)
            {
                int[] lastTarget = [this.api.Hero.TargetX, this.api.Hero.TargetY];
                var distance = HelpTools.CalculateDistance(this.api.Hero.X, this.api.Hero.Y, lastTarget[0], lastTarget[1]);
                var duration = (distance / this.api.Hero.Speed);
                var durationMS = (float)duration * 1000f;

                this.api.Movement.StopMovement(this.api.Hero);
                this.api.Movement.StartMovement(this.api.Hero, lastTarget[0], lastTarget[1], durationMS);
            }
        }
    }
}
