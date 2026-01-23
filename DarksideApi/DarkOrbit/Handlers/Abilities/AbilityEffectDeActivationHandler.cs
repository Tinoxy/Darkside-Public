using DarksideApi.DarkOrbit.Commands.Abilities;

namespace DarksideApi.DarkOrbit.Handlers.Abilities
{
    internal class AbilityEffectDeActivationHandler : ICommandHandler
    {
        private readonly Api api;

        public AbilityEffectDeActivationHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            AbilityEffectDeActivationCommand abilityEffectDeActivation = new();
            abilityEffectDeActivation.Read(@byte);

            if (abilityEffectDeActivation.activatorId == this.api.Hero.UserId)
            {
                this.api.WriteLog("Ability Deactivated!");
            }
        }
    }
}
