using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Commands.GalaxyGates;
using DarksideApi.Util.Extensions;

namespace DarksideApi.DarkOrbit.Handlers.GalaxyGateBuilder
{
    internal class GalaxyGateBuilderGateCompleteHandler : ICommandHandler
    {
        private readonly Api api;

        public GalaxyGateBuilderGateCompleteHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            GalaxyGateBuilderGateCompleteCommand galaxyGateGeneratorGateComplete = new();
            galaxyGateGeneratorGateComplete.Read(@byte);

            if (this.api.GalaxyGateGenerators.TryGetValue(galaxyGateGeneratorGateComplete.gateId, out Objects.Client.GalaxyGateGenerator? ggg))
            {
                ggg.CurrentParts = galaxyGateGeneratorGateComplete.currentPart;
            }


            if (CommandConstants.GG_SPINNER_NAME_ID.TryGetKey(galaxyGateGeneratorGateComplete.gateId, out var ggName))
            {
                Settings.GalaxyGateSetting setting = this.api.Logic.GetGalaxyGateSetting(ggName);
                if (setting.IsEnabled && setting.SpinPlaceGate)
                {
                    this.api.GameMethods.GalaxyGateGeneratorPlace(galaxyGateGeneratorGateComplete.gateId);
                    this.api.WriteLog("Placing " + ggName);
                }
                this.api.AddLootResource("Build " + ggName, 1, 0);
            }
        }
    }
}
