using DarksideApi.DarkOrbit.Commands.Captcha;
using DarksideApi.Logic;

namespace DarksideApi.DarkOrbit.Handlers.Captcha
{
    internal class CaptchaTriggerHandler : ICommandHandler
    {
        private readonly Api api;

        public CaptchaTriggerHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            CaptchaTriggerCommand captchaInit = new();
            captchaInit.Read(@byte);

            Objects.Client.Captcha captcha = new(this.api, captchaInit.captchaParam, captchaInit.captchaTimer, captchaInit.type, captchaInit.x, captchaInit.y, captchaInit.radius, captchaInit.blackBoxesCount, captchaInit.redBoxesCount);
            this.api.Captcha = captcha;
            this.api.Logic.SetState(this, LogicState.Captcha);
            this.api.Movement.StopMovement(this.api.Hero);
            //CAPTCHA: 1, 6, 2000, 2, 920, 500, 4, 0
            //amount + time + x + var_261 + y + radius + var_4812 + type
        }
    }
}
