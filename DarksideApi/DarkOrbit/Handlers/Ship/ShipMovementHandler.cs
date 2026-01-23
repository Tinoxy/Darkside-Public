using DarksideApi.DarkOrbit.Commands.Ship;
using DarksideApi.DarkOrbit.Objects.Ship;
using DarksideApi.Util;
using System.Diagnostics;

namespace DarksideApi.DarkOrbit.Handlers.Ship
{
    internal class ShipMovementHandler : ICommandHandler
    {
        private readonly Api api;

        public ShipMovementHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            ShipMovementCommand shipMovement = new();
            shipMovement.Read(@byte);

            if (shipMovement.userId == this.api.Hero.UserId)
            {
                this.api.Hero.TargetX = shipMovement.x;
                this.api.Hero.TargetY = shipMovement.y;
                this.api.Movement.StartMovement(this.api.Hero, this.api.Hero.TargetX, this.api.Hero.TargetY, shipMovement.timeToTarget);
                this.api.Logging.Log("Received Travel Info from Server: " + shipMovement.x + "|" + shipMovement.y + ", duration: " + shipMovement.timeToTarget);
                if (shipMovement.timeToTarget <= 0)
                {
                    this.api.Movement.StopMovement(this.api.Hero);
                    this.api.Hero.FlyingTimeMs = 0;
                    this.api.Logic.IncreaseMoveQuarter();
                }
                return;
            }
            lock (this.api.ObjectLocker)
            {
                if (this.api.Ships.TryGetValue(shipMovement.userId, out Objects.Ship.Ship? shipMoveValue))
                {
                    if (shipMoveValue is Npc npcMove && (npcMove.IsHitac || npcMove.IsHitacMinion))
                        Debug.WriteLine("ShipMovementHandler: Moving ship " + shipMoveValue.Username + " to " + shipMovement.x + "|" + shipMovement.y);
                    
                    shipMoveValue.TargetX = shipMovement.x;
                    shipMoveValue.TargetY = shipMovement.y;
                    shipMoveValue.Speed = (HelpTools.CalculateDistance(shipMoveValue.X, shipMoveValue.Y, shipMoveValue.TargetX, shipMoveValue.TargetY) / shipMovement.timeToTarget);
                    if (shipMoveValue.tempLastPositions.Count > 7)
                    {
                        shipMoveValue.tempLastPositions.RemoveAt(0);
                    }
                    shipMoveValue.tempLastPositions.Add(new(shipMovement.x, shipMovement.y));
                    this.api.Movement.StartMovement(shipMoveValue, shipMoveValue.TargetX, shipMoveValue.TargetY, shipMovement.timeToTarget);

                    if (this.api.Group != null && shipMoveValue is Player player
                        && this.api.Group.Members.TryGetValue(player.UserId, out Objects.Group.GroupMemberObj? groupMemberValue))
                    {
                        groupMemberValue.Player = player;
                        groupMemberValue.MapId = this.api.Map.Id;
                    }

                    return;
                }
                if (this.api.Pets.TryGetValue(shipMovement.userId, out Objects.Pets.Pet? petMoveValue))
                {
                    petMoveValue.TargetX = shipMovement.x;
                    petMoveValue.TargetY = shipMovement.y;
                    this.api.Movement.StartMovement(petMoveValue, petMoveValue.TargetX, petMoveValue.TargetY, shipMovement.timeToTarget);
                }
            }
        }
    }
}
