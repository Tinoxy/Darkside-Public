using DarksideApi.DarkOrbit.Commands.Abilities;

namespace DarksideApi.DarkOrbit.Handlers.Abilities
{
    internal class AbilityStartHandler : ICommandHandler
    {
        private readonly Api api;

        public AbilityStartHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            AbilityStartCommand abilityStart = new();
            abilityStart.Read(@byte);

            if (abilityStart.activatorId == this.api.Hero.UserId)
            {
                this.api.WriteLog("Ability Activated!");
            }

            this.api.Logging.Log($"Ability Started by: {abilityStart.activatorId}, abilityId: {abilityStart.selectedAbilityId}, noStopCmd: {abilityStart.noStopCommand}");
        }
    }
}
