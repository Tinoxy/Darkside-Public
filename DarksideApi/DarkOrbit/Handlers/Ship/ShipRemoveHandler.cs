using DarksideApi.DarkOrbit.Commands.Ship;
using DarksideApi.DarkOrbit.Objects.Ship;

namespace DarksideApi.DarkOrbit.Handlers.Ship
{
    internal class ShipRemoveHandler : ICommandHandler
    {
        private readonly Api api;

        public ShipRemoveHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            ShipRemoveCommand shipRemove = new();
            shipRemove.Read(@byte);

            var wasTarget = false;

            if (this.api.Logic.Target is Npc targetRemoveNpc
                && shipRemove.userId == targetRemoveNpc.UserId
                && this.api.Hero.IsAttacking)
            {
                wasTarget = true;
                this.api.Logic.Target = null;
            }
            if (this.api.Logic.Target is Player targetRemovePlayer
                && shipRemove.userId == targetRemovePlayer.UserId
                && (this.api.Logic.State == Logic.LogicState.Shootback || this.api.Logic.State == Logic.LogicState.GotoShootback))
            {
                wasTarget = true;
                this.api.Logic.Target = null;
            }

            lock (this.api.ObjectLocker)
            {
                if (this.api.Ships.Remove(shipRemove.userId, out Objects.Ship.Ship? shipRemoved))
                {
                    if (!wasTarget)
                    {
                        return;
                    }
                    if (shipRemoved is Player)
                    {
                        this.api.Statistics.AmountPlayersDestroyed += 1;
                        if (!this.api.AutoAttackKillAmounts.TryAdd(shipRemoved.UserId, 1))
                            this.api.AutoAttackKillAmounts[shipRemoved.UserId] += 1;
                    }
                    if (shipRemoved is Npc)
                    {
                        this.api.AddLootNpc(shipRemoved.Username, 1);
                    }
                    return;
                }
                if (this.api.Pets.Remove(shipRemove.userId, out _))//var petRemoveValue))
                {
                    //api.Movement.StopMovement(petRemoveValue);
                    return;
                }
            }
        }
    }
}
