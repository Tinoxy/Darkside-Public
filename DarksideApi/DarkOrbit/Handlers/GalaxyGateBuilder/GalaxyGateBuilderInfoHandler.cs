using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Commands.GalaxyGates;
using DarksideApi.DarkOrbit.Objects.Client;

namespace DarksideApi.DarkOrbit.Handlers.GalaxyGateBuilder
{
    public class GalaxyGateBuilderInfoHandler : ICommandHandler
    {
        private readonly Api api;

        public GalaxyGateBuilderInfoHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            GalaxyGateBuilderInfoCommand galaxyGateGeneratorInit = new();
            galaxyGateGeneratorInit.Read(@byte);

            //Debug.WriteLine("GG init: " + galaxyGateGeneratorInit.ToString());
            foreach (GalaxyGateBuilderGateModule gate in galaxyGateGeneratorInit.gates)
            {
                if (!CommandConstants.GG_SPINNER_ID_MAX_PARTS.TryGetValue((byte)gate.id, out var maxParts))
                {
                    continue;
                }
                var gg = new GalaxyGateGenerator(gate.mode, gate.id, gate.state, gate.currentParts, maxParts, gate.lifesLeft, gate.lifePrice, gate.currentWave);
                if (!this.api.GalaxyGateGenerators.TryAdd(gate.id, gg))
                {
                    this.api.GalaxyGateGenerators[gate.id] = gg;
                }
            }

            CommandConstants.GG_SPINNER_AMOUNT.Clear();
            CommandConstants.GG_SPINNER_AMOUNT.AddRange(galaxyGateGeneratorInit.spinAmounts);

            if (this.api.Inventory.ExtraEnergy < galaxyGateGeneratorInit.samples)
            {
                this.api.Inventory.ExtraEnergy = galaxyGateGeneratorInit.samples;
            }
            //Debug.WriteLine("New GG Energy: " + galaxyGateGeneratorInit.samples.ToString());
            //api.AddLoot(Statistics.StatisticsConstants.EE, 0L, (long)galaxyGateGeneratorInit.extraEnergy);
        }
    }
}
