using DarksideApi.DarkOrbit.Commands.Attack;
using System.Diagnostics;

namespace DarksideApi.DarkOrbit.Handlers.Attack
{
    internal class AttackRangeExceededHandler : ICommandHandler
    {
        private readonly Api api;

        public AttackRangeExceededHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            AttackRangeExceededCommand attackRangeExceeded = new();
            attackRangeExceeded.Read(@byte);

            //userId => target
            //rocket: 650, laser: 600
            Debug.WriteLine("Attack Range Exceed: " + attackRangeExceeded.userId + ", type: " + attackRangeExceeded.type + ", max: " + attackRangeExceeded.maxRange);
        }
    }
}
