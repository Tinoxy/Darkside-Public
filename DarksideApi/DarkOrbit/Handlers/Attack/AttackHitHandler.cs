using DarksideApi.DarkOrbit.Commands.Attack;
using DarksideApi.DarkOrbit.Objects.Ship;
using System.Diagnostics;

namespace DarksideApi.DarkOrbit.Handlers.Attack
{
    internal class AttackHitHandler : ICommandHandler
    {
        private readonly Api api;

        public AttackHitHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            AttackHitCommand attackHit = new();
            attackHit.Read(@byte);

            if (attackHit.victimId == this.api.Hero.UserId)
            {
                this.api.Hero.Hitpoints = attackHit.victimHitpoints;
                this.api.Hero.Shield = attackHit.victimShield;
                this.api.Hero.NanoShield = attackHit.vicitimNanonShield;
                return;
            }
            if (attackHit.attackerId == this.api.Hero.UserId
                && attackHit.attackType.attackType == AttackTypeModule.AttackType.Laser)
            {
                this.api.Statistics.AddDamage(this.api.Hero.CurrentLaser.Id, attackHit.damage);
            }
            if (this.api.Ships.TryGetValue(attackHit.victimId, out Objects.Ship.Ship? shipUpdated))
            {
                shipUpdated.Hitpoints = attackHit.victimHitpoints;
                shipUpdated.Shield = attackHit.victimShield;
                shipUpdated.NanoShield = attackHit.vicitimNanonShield;
                shipUpdated.DamageReceived = attackHit.damage;
                if (shipUpdated.Hitpoints <= 0)
                {

                    lock (this.api.ObjectLocker)
                        if (this.api.Ships.Remove(attackHit.victimId, out Objects.Ship.Ship _))
                        {
                            if (this.api.Logic.Target is Npc targetRemoveNpc
                                && attackHit.victimId == targetRemoveNpc.UserId
                                && attackHit.attackerId == this.api.Hero.UserId)
                            {
                                this.api.AddLootNpc(targetRemoveNpc.Username, 1);
                                this.api.Logic.Target = null;
                            }
                        }
                    //return;
                }
                if (shipUpdated == this.api.Logic.Target
                    && this.api.Hero.IsAttacking
                    && attackHit.attackerId == this.api.Hero.UserId
                    && this.api.Settings.LogLogDamage)
                {
                    shipUpdated.tempSelectTries = 0;
                    if (attackHit.attackType.attackType != AttackTypeModule.AttackType.Rocket
                        && attackHit.attackType.attackType != AttackTypeModule.AttackType.Laser
                        && attackHit.attackType.attackType != AttackTypeModule.AttackType.Kamikaze)
                    {
                        return;
                    }
                    if (attackHit.attackType.attackType == AttackTypeModule.AttackType.Kamikaze)
                    {
                        this.api.WriteLog($"Kamikaze damage to {shipUpdated.Username}: {attackHit.damage}");
                        return;
                    }
                    var type = attackHit.attackType.attackType == AttackTypeModule.AttackType.Rocket ? "Rocket" : "Laser";
                    this.api.WriteLog($"Damage to {shipUpdated.Username} with {type}: {attackHit.damage}, HP: {Math.Round(shipUpdated.HitpointsPercentage, 0, MidpointRounding.AwayFromZero)}%, SHD: {Math.Round(shipUpdated.ShieldPercentage, 0, MidpointRounding.AwayFromZero)}%");
                    return;
                }
                return;
            }
            if (attackHit.victimId == this.api.Hero.UserId
                && this.api.Settings.LogLogDamage
                && this.api.Ships.TryGetValue(attackHit.attackerId, out Objects.Ship.Ship? attacker) && attacker is Player player)
            {
                if (attackHit.attackType.attackType != AttackTypeModule.AttackType.Rocket
                        && attackHit.attackType.attackType != AttackTypeModule.AttackType.Laser
                        && attackHit.attackType.attackType != AttackTypeModule.AttackType.Kamikaze)
                {
                    return;
                }
                var type = attackHit.attackType.attackType == AttackTypeModule.AttackType.Rocket ? "Rocket" : "Laser";
                this.api.WriteLog($"Damage from {player.Username} with {type}: {attackHit.damage}");
                return;
            }
        }
    }
}
