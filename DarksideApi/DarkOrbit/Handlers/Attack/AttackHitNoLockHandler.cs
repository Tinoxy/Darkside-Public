using DarksideApi.DarkOrbit.Commands.Attack;
using DarksideApi.DarkOrbit.Objects.Ship;

namespace DarksideApi.DarkOrbit.Handlers.Attack
{
    internal class AttackHitNoLockHandler : ICommandHandler
    {
        private readonly Api api;

        public AttackHitNoLockHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            AttackHitNoLockCommand attackHitNoLock = new();
            attackHitNoLock.Read(@byte);

            if (attackHitNoLock.victimId == this.api.Hero.UserId)
            {
                this.api.Hero.Hitpoints = attackHitNoLock.victimHitpoints;
                this.api.Hero.Shield = attackHitNoLock.victimShield;
                this.api.Hero.NanoShield = attackHitNoLock.vicitimNanonShield;
                this.api.Hero.MaxHitpoints = attackHitNoLock.victimMaxHitpoints;
                this.api.Hero.MaxShield = attackHitNoLock.victimMaxShield;
                this.api.Hero.MaxNanoShield = attackHitNoLock.victimMaxNanonShield;
                return;
            }
            if (attackHitNoLock.attackerId == this.api.Hero.UserId
                && attackHitNoLock.attackType.attackType == AttackTypeModule.AttackType.Laser)
            {
                this.api.Statistics.AddDamage(this.api.Hero.CurrentLaser.Id, attackHitNoLock.damage);
            }
            if (this.api.Ships.TryGetValue(attackHitNoLock.victimId, out Objects.Ship.Ship? shipUpdated))
            {
                shipUpdated.Hitpoints = attackHitNoLock.victimHitpoints;
                shipUpdated.Shield = attackHitNoLock.victimShield;
                shipUpdated.NanoShield = attackHitNoLock.vicitimNanonShield;
                shipUpdated.MaxHitpoints = attackHitNoLock.victimMaxHitpoints;
                shipUpdated.MaxShield = attackHitNoLock.victimMaxShield;
                shipUpdated.MaxNanoShield = attackHitNoLock.victimMaxNanonShield;
                shipUpdated.DamageReceived = attackHitNoLock.damage;
                if (shipUpdated.Hitpoints <= 0)
                {

                    lock (this.api.ObjectLocker)
                        if (this.api.Ships.Remove(attackHitNoLock.victimId, out Objects.Ship.Ship _))
                        {
                            if (this.api.Logic.Target is Npc targetRemoveNpc
                                && attackHitNoLock.victimId == targetRemoveNpc.UserId
                                && attackHitNoLock.attackerId == this.api.Hero.UserId)
                            {
                                this.api.AddLootNpc(targetRemoveNpc.Username, 1);
                                this.api.Logic.Target = null;
                            }
                        }
                    //return;
                }
                if (shipUpdated == this.api.Logic.Target
                    && this.api.Hero.IsAttacking
                    && attackHitNoLock.attackerId == this.api.Hero.UserId
                    && this.api.Settings.LogLogDamage)
                {
                    shipUpdated.tempSelectTries = 0;
                    if (attackHitNoLock.attackType.attackType != AttackTypeModule.AttackType.Rocket
                        && attackHitNoLock.attackType.attackType != AttackTypeModule.AttackType.Laser
                        && attackHitNoLock.attackType.attackType != AttackTypeModule.AttackType.Kamikaze)
                    {
                        return;
                    }
                    if (attackHitNoLock.attackType.attackType == AttackTypeModule.AttackType.Kamikaze)
                    {
                        this.api.WriteLog($"Kamikaze damage to {shipUpdated.Username}: {attackHitNoLock.damage}");
                        return;
                    }
                    this.api.WriteLog($"Damage to {shipUpdated.Username} with {attackHitNoLock.attackType.attackType}: {attackHitNoLock.damage}, HP: {Math.Round(shipUpdated.HitpointsPercentage, 0, MidpointRounding.AwayFromZero)}%, SHD: {Math.Round(shipUpdated.ShieldPercentage, 0, MidpointRounding.AwayFromZero)}%");
                    return;
                }
                return;
            }
            if (attackHitNoLock.victimId == this.api.Hero.UserId
                && this.api.Settings.LogLogDamage
                && this.api.Ships.TryGetValue(attackHitNoLock.attackerId, out Objects.Ship.Ship? attacker) && attacker is Player player)
            {
                if (attackHitNoLock.attackType.attackType != AttackTypeModule.AttackType.Rocket
                        && attackHitNoLock.attackType.attackType != AttackTypeModule.AttackType.Laser
                        && attackHitNoLock.attackType.attackType != AttackTypeModule.AttackType.Kamikaze)
                {
                    return;
                }
                this.api.WriteLog($"Damage from {player.Username} with {attackHitNoLock.attackType.attackType}: {attackHitNoLock.damage}");
                return;
            }
        }
    }
}
