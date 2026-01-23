using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Commands.Map;

namespace DarksideApi.DarkOrbit.Handlers.Map
{
    internal class JumpgateRemoveHandler : ICommandHandler
    {
        private readonly Api api;

        public JumpgateRemoveHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            JumpGateRemoveCommand gateRemove = new();
            gateRemove.Read(@byte);

            this.api.Gates.Remove(gateRemove.gateId, out Objects.Map.Gate? removedGate);

            if (removedGate != null && this.api.Map.IsBlackLightMap && removedGate.GateDesignId == CommandConstants.GALAXY_GATE_EBG_DESING_ID)
            {
                this.api.Map.IsEternalBlackLightGatePlaced = false;
            }

            this.api.Logging.Log("Gate Removed: " + gateRemove.gateId);
        }
    }
}
