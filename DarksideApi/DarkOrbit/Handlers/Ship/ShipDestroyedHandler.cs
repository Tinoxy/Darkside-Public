using DarksideApi.DarkOrbit.Commands.Ship;
using DarksideApi.DarkOrbit.Objects.Ship;

namespace DarksideApi.DarkOrbit.Handlers.Ship
{
    internal class ShipDestroyedHandler : ICommandHandler
    {
        private readonly Api api;

        public ShipDestroyedHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            ShipDestroyedCommand shipExplode = new();
            shipExplode.Read(@byte);

            var wasTarget = false;

            if (this.api.Logic.Target is Npc targetExplodeNpc
                && shipExplode.userId == targetExplodeNpc.UserId
                && this.api.Hero.IsAttacking)
            {
                wasTarget = true;
                this.api.Logic.Target = null;
            }
            if (this.api.Logic.Target is Player targetExplodePlayer
                && shipExplode.userId == targetExplodePlayer.UserId
                && this.api.Hero.IsAttacking)
            {
                wasTarget = true;
                this.api.Logic.Target = null;
            }

            lock (this.api.ObjectLocker)
            {
                if (this.api.Ships.Remove(shipExplode.userId, out Objects.Ship.Ship? removedShip))
                {
                    if (!wasTarget)
                    {
                        return;
                    }
                    if (removedShip is Player)
                    {
                        this.api.Statistics.AmountPlayersDestroyed += 1;
                        if (!this.api.AutoAttackKillAmounts.TryAdd(removedShip.UserId, 1))
                            this.api.AutoAttackKillAmounts[removedShip.UserId] += 1;
                        return;
                    }
                    if (removedShip is Npc)
                    {
                        this.api.AddLootNpc(removedShip.Username, 1);
                        return;
                    }
                    return;
                }

                if (this.api.Pets.Remove(shipExplode.userId, out Objects.Pets.Pet? destroyedPet))
                {
                    //api.Movement.StopMovement(petExplodeValue);
                    if (shipExplode.userId == this.api.Hero.Pet.UserId || destroyedPet.OwnerId == this.api.Hero.UserId)
                    {
                        //api.Hero.IsPetAlive = false;
                        this.api.AddLootResource("Pet destroyed", 1);
                    }
                    return;
                }
            }
        }
    }
}
