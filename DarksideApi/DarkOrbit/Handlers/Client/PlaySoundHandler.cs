using DarksideApi.DarkOrbit.Commands.Client;

namespace DarksideApi.DarkOrbit.Handlers.Client
{
    internal class PlaySoundHandler : ICommandHandler
    {
        private readonly Api api;

        public PlaySoundHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            PlaySoundCommand playSound = new();
            playSound.Read(@byte);

            if ((int)playSound.status < 3)
                this.api.Hero.RocketLauncherLoadingStatus = (int)playSound.status;

            this.api.Logging.Log("Play Sound: " + playSound.status);
        }
    }
}
