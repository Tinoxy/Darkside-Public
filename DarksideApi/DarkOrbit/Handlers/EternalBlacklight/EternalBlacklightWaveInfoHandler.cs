using DarksideApi.DarkOrbit.Commands.EternalBlackLight;

namespace DarksideApi.DarkOrbit.Handlers.EternalBlacklight
{
    internal class EternalBlacklightWaveInfoHandler : ICommandHandler
    {
        private readonly Api api;

        public EternalBlacklightWaveInfoHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            EternalBlacklightWaveInfoCommand updateWaveCommand = new();
            updateWaveCommand.Read(@byte);

            this.api.Map.CurrentGGWave = updateWaveCommand.currentWave;
            this.api.WriteLog($"EBG Wave: {updateWaveCommand.currentWave}, furthest: {updateWaveCommand.furthestWave}");
        }
    }
}
