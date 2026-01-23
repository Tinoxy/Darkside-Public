using DarksideApi.DarkOrbit.Commands.Hero;

namespace DarksideApi.DarkOrbit.Handlers.Hero
{
    internal class MoveUpdateHandler : ICommandHandler
    {
        private readonly Api api;

        public MoveUpdateHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            MoveUpdateCommand moveUpdate = new();
            moveUpdate.Read(@byte);

            this.api.Logging.Log("Move Update: " + moveUpdate.x + "|" + moveUpdate.y);

            if (moveUpdate.x != this.api.Hero.TargetX && moveUpdate.y != this.api.Hero.TargetY)
            {
                this.api.Movement.ResetPath();
                this.api.Movement.StopMovement(this.api.Hero);
                this.api.Hero.FlyingTimeMs = 0;
            }
            this.api.Hero.TargetX = moveUpdate.x;
            this.api.Hero.TargetY = moveUpdate.y;
        }
    }
}
