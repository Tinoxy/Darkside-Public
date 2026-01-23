using DarksideApi.DarkOrbit.Commands.Attack;

namespace DarksideApi.DarkOrbit.Handlers.Attack
{
    internal class AttackAbortLaserHandler : ICommandHandler
    {
        private readonly Api api;

        public AttackAbortLaserHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            AttackAbortLaserCommand shipDestroy = new();
            shipDestroy.Read(@byte);

            if (shipDestroy.userId == this.api.Hero.UserId)
            {
                this.api.Hero.LastAttackTime = DateTime.MinValue;
            }
        }
    }
}
