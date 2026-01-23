using DarksideApi.DarkOrbit.Commands.Abilities;
using System.Diagnostics;

namespace DarksideApi.DarkOrbit.Handlers.Abilities
{
    internal class AbilityEffectActivationHandler : ICommandHandler
    {
        private readonly Api api;

        public AbilityEffectActivationHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            AbilityEffectActivationCommand abilityEffectActivation = new();
            abilityEffectActivation.Read(@byte);

            Debug.WriteLine($"Ability Effect Activation: {abilityEffectActivation.activatorId}, {abilityEffectActivation.selectedAbilityId}, {abilityEffectActivation.isPresistent}, targets:{string.Join("; ", abilityEffectActivation.targetIds)}");
            if (abilityEffectActivation.activatorId == this.api.Hero.UserId)
            {
                if (abilityEffectActivation.selectedAbilityId == 1 && !abilityEffectActivation.isPresistent) //Heal Effect (Pusat+)
                {
                    this.api.Hero.RepairAmount = 0;
                    this.api.Hero.LastRepairTime = DateTime.MinValue;
                }
            }
        }
    }
}
