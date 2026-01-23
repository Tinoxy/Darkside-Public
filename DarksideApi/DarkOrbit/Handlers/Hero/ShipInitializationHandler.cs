using DarksideApi.DarkOrbit.Commands.Hero;

namespace DarksideApi.DarkOrbit.Handlers.Hero
{
    public class ShipInitializationHandler : ICommandHandler
    {
        private readonly Api api;

        public ShipInitializationHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            ShipInitializationCommand heroInit = new();
            heroInit.Read(@byte);

            this.api.Clear();

            this.api.Logic.Target = null;
            this.api.Map = new(heroInit.mapId, this.api.Hero.X, this.api.Hero.Y);

            this.api.Hero.UserId = heroInit.userId;
            this.api.Hero.Username = heroInit.userName;
            this.api.Hero.ClanTag = heroInit.clanTag;
            this.api.Hero.Level = heroInit.level;
            this.api.Hero.FactionId = heroInit.factionId;

            this.api.Hero.Ship = heroInit.typeId;
            var speed = heroInit.speed * 0.97;
            this.api.Hero.Speed = (int)Math.Max(-2147483648, Math.Min(speed, 2147483647));
            this.api.Hero.X = heroInit.x;
            this.api.Hero.Y = heroInit.y;
            this.api.Hero.TargetX = heroInit.x;
            this.api.Hero.TargetY = heroInit.y;
            this.api.Hero.NextMapId = heroInit.mapId;
            this.api.Hero.IsCloaked = heroInit.cloaked;

            //api.Hero.Jackpot = heroInit.jackpot;
            this.api.Inventory.AddCredits((long)heroInit.credits);
            this.api.Inventory.AddUridium((long)heroInit.uridium);
            this.api.Hero.ExperiencePoints = heroInit.experiencePoints;
            this.api.Hero.HonorPoints = heroInit.honor;

            this.api.Hero.Hitpoints = heroInit.currentHitpoints;
            this.api.Hero.MaxHitpoints = heroInit.maxHitpoints;
            this.api.Hero.Shield = heroInit.shield;
            this.api.Hero.MaxShield = heroInit.maxShield;
            this.api.Hero.NanoShield = heroInit.nanoShield;
            this.api.Hero.MaxNanoShield = heroInit.maxNanoShield;
            this.api.Hero.CurrentCaptainEnergy = heroInit.captainEnergy;
            this.api.Hero.MaxCaptainEnergy = heroInit.maxCaptainEnergy;

            this.api.Hero.Cargo.MaxCargoSpace = heroInit.maxCargoSpace;
            this.api.Hero.IsJumping = false;
            this.api.Hero.HasPremium = heroInit.premium;

            this.api.GameMethods.ReadyGame(1, false);
            Task.Delay(100).ContinueWith(_ => this.api.GameMethods.ReadyGame(2, false));
            Task.Delay(200).ContinueWith(_ => this.api.GameMethods.TrackVariable(true));
            Task.Delay(300).ContinueWith(_ => this.api.GameMethods.AudioSettingsRequest());
            Task.Delay(400).ContinueWith(_ => this.api.GameMethods.AutopilotInfoRequest());

            this.api.Cooldown.HeroInitCooldowns();
            this.api.GameClient.GameState = GameState.Ready;


            if (this.api.Settings.AutoStart && !this.api.Settings.IsRunning)
            {
                Task.Delay(3000).ContinueWith(_ => this.api.Logic.StartLogic());
            }
        }
    }
}
