using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Commands.Map;
using DarksideApi.DarkOrbit.Objects.Map;
using System.Diagnostics;

namespace DarksideApi.DarkOrbit.Handlers.Map
{
    internal class JumpgateCreateHandler : ICommandHandler
    {
        private readonly Api api;

        public JumpgateCreateHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            JumpgateCreateCommand gateInit = new();
            gateInit.Read(@byte);

            Gate gate = new(this.api, gateInit.gateId, gateInit.X, gateInit.Y, gateInit.designId, gateInit.factionId, gateInit.isVisibleOnMinimap);
            if (!this.api.Gates.TryAdd(gateInit.gateId, gate))
            {
                this.api.Gates[gateInit.gateId] = gate;
            }

            if (this.api.Map.IsBlackLightMap && gateInit.designId == CommandConstants.GALAXY_GATE_EBG_DESING_ID)
            {
                this.api.Map.IsEternalBlackLightGatePlaced = true;
            }

            Debug.WriteLine(gateInit.ToString());
        }
    }
}
