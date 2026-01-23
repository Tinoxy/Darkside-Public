using DarksideApi.DarkOrbit.Commands.Abilities;
using DarksideApi.DarkOrbit.Commands.Attack;
using DarksideApi.DarkOrbit.Commands.Auction;
using DarksideApi.DarkOrbit.Commands.Autopilot;
using DarksideApi.DarkOrbit.Commands.Booster;
using DarksideApi.DarkOrbit.Commands.Boxes;
using DarksideApi.DarkOrbit.Commands.Captcha;
using DarksideApi.DarkOrbit.Commands.Client;
using DarksideApi.DarkOrbit.Commands.DailyLogin;
using DarksideApi.DarkOrbit.Commands.Dispatch;
using DarksideApi.DarkOrbit.Commands.EternalBlackLight;
using DarksideApi.DarkOrbit.Commands.Expedition;
using DarksideApi.DarkOrbit.Commands.GalaxyGates;
using DarksideApi.DarkOrbit.Commands.Group;
using DarksideApi.DarkOrbit.Commands.Hero;
using DarksideApi.DarkOrbit.Commands.Loot;
using DarksideApi.DarkOrbit.Commands.Map;
using DarksideApi.DarkOrbit.Commands.Mines;
using DarksideApi.DarkOrbit.Commands.Pet;
using DarksideApi.DarkOrbit.Commands.Quests;
using DarksideApi.DarkOrbit.Commands.Ship;
using DarksideApi.DarkOrbit.Commands.Shop;
using DarksideApi.DarkOrbit.Handlers.Abilities;
using DarksideApi.DarkOrbit.Handlers.Attack;
using DarksideApi.DarkOrbit.Handlers.Auction;
using DarksideApi.DarkOrbit.Handlers.Autopilot;
using DarksideApi.DarkOrbit.Handlers.Booster;
using DarksideApi.DarkOrbit.Handlers.Boxes;
using DarksideApi.DarkOrbit.Handlers.Captcha;
using DarksideApi.DarkOrbit.Handlers.Client;
using DarksideApi.DarkOrbit.Handlers.DailyLogin;
using DarksideApi.DarkOrbit.Handlers.Dispatch;
using DarksideApi.DarkOrbit.Handlers.EternalBlacklight;
using DarksideApi.DarkOrbit.Handlers.Expedition;
using DarksideApi.DarkOrbit.Handlers.GalaxyGateBuilder;
using DarksideApi.DarkOrbit.Handlers.Group;
using DarksideApi.DarkOrbit.Handlers.Hero;
using DarksideApi.DarkOrbit.Handlers.Loot;
using DarksideApi.DarkOrbit.Handlers.Map;
using DarksideApi.DarkOrbit.Handlers.Mines;
using DarksideApi.DarkOrbit.Handlers.Pet;
using DarksideApi.DarkOrbit.Handlers.Quest;
using DarksideApi.DarkOrbit.Handlers.Ship;
using DarksideApi.DarkOrbit.Handlers.Shop;
using System.Diagnostics;

namespace DarksideApi.DarkOrbit.Handlers
{
    public class CommandHandler
    {
        private readonly Dictionary<short, ICommandHandler> handlerLookup;
        private readonly UnknownCommandHandler unknownCommandHandler;
        private ICommandHandler? commandHandler;

        public CommandHandler(Api api)
        {
            this.handlerLookup = [];
            this.unknownCommandHandler = new();
            this.Create(api);
        }

        public ICommandHandler Lookup(short id)
        {
            if (!this.handlerLookup.TryGetValue(id, out this.commandHandler))
            {
                //Debug.WriteLine($"CommandHandler: {id} not found!");
                return this.unknownCommandHandler;
            }
            //Debug.WriteLine($"CommandHandler: {this.commandHandler} found!");
            return this.commandHandler;
        }

        private void Create(Api api)
        {
            this.handlerLookup.Add(AbilityStartCommand.Id, new AbilityStartHandler(api));
            this.handlerLookup.Add(AbilityEffectActivationCommand.Id, new AbilityEffectActivationHandler(api));
            this.handlerLookup.Add(AbilityEffectDeActivationCommand.Id, new AbilityEffectDeActivationHandler(api));

            this.handlerLookup.Add(ShipSelectionCommand.Id, new ShipSelectionHandler(api));
            this.handlerLookup.Add(AttackAbortLaserCommand.Id, new AttackAbortLaserHandler(api));
            this.handlerLookup.Add(ShipDeselectionCommand.Id, new ShipDeselectionHandler(api));
            this.handlerLookup.Add(AttackLaserRunCommand.Id, new AttackLaserRunHandler(api));
            this.handlerLookup.Add(AttackHitCommand.Id, new AttackHitHandler(api));
            this.handlerLookup.Add(AttackHitNoLockCommand.Id, new AttackHitNoLockHandler(api));
            this.handlerLookup.Add(AttackHitAssetCommand.Id, new AttackHitAssetHandler(api));
            this.handlerLookup.Add(AttackRangeExceededCommand.Id, new AttackRangeExceededHandler(api));

            this.handlerLookup.Add(AutopilotInfoCommand.Id, new AutopilotInfoHandler(api));
            this.handlerLookup.Add(AutoPilotStatus.Id, new AutopilotStatusHandler(api));

            this.handlerLookup.Add(AttributeBoosterUpdateCommand.Id, new AttributeBoosterUpdateHandler(api));
            this.handlerLookup.Add(BoosterFoundOnMapCommand.Id, new BoosterFoundOnMapHandler(api));

            this.handlerLookup.Add(AddBoxCommand.Id, new AddBoxHandler(api));
            this.handlerLookup.Add(RemoveCollectableCommand.Id, new RemoveCollectableHandler(api));
            this.handlerLookup.Add(OreInitCommand.Id, new OreInitHandler(api));

            this.handlerLookup.Add(AuctionListCommand.Id, new AuctionListHandler(api));
            this.handlerLookup.Add(AuctionBidCommand.Id, new AuctionBidHandler(api));

            this.handlerLookup.Add(CaptchaTriggerCommand.Id, new CaptchaTriggerHandler(api));

            this.handlerLookup.Add(LoginResponseCommand.Id, new LoginResponseHandler(api));
            this.handlerLookup.Add(KillScreenPostCommand.Id, new KillScreenPostCommandHandler(api));
            this.handlerLookup.Add(ServerVersionCheck.Id, new ServerVersionCheckHandler(api));
            this.handlerLookup.Add(LegacyCommand.Id, new LegacyCommandHandler(api));
            this.handlerLookup.Add(DroneFormationChangeCommand.Id, new DroneFormationChangeHandler(api));
            this.handlerLookup.Add(StayinAliveCommand.Id, new StayinAliveHandler(api));
            this.handlerLookup.Add(ReloginCommand.Id, new ReloginHandler(api));
            this.handlerLookup.Add(MenuInitializationCommand.Id, new MenuInitializationHandler(api));
            this.handlerLookup.Add(MenuItemStatusCommand.Id, new MenuItemStatusHandler(api));
            this.handlerLookup.Add(PlaySoundCommand.Id, new PlaySoundHandler(api));
            this.handlerLookup.Add(ClientLoggedInCommand.Id, new ClientLoggedInHandler());
            this.handlerLookup.Add(HealCommand.Id, new HealHandler(api));

            this.handlerLookup.Add(DailyLoginRewardDayCommand.Id, new DailyLoginRewardDayHandler(api));

            this.handlerLookup.Add(DispatchSlotInfoCommand.Id, new DispatchSlotInfoHandler(api));
            this.handlerLookup.Add(DispatchGetRetrieverListCommand.Id, new DispatchGetRetrieverListHandler(api));
            this.handlerLookup.Add(DispatchActiveEventCommand.Id, new DispatchActiveEventHandler(api));

            this.handlerLookup.Add(EternalBlacklightBoosterInfoCommand.Id, new EternalBlacklightBoosterInfoHandler(api));
            this.handlerLookup.Add(EternalBlacklightInfoCommand.Id, new EternalBlacklightInfoHandler(api));
            this.handlerLookup.Add(EternalBlacklightWaveInfoCommand.Id, new EternalBlacklightWaveInfoHandler(api));

            this.handlerLookup.Add(ExpeditionStartInstanceCommand.Id, new ExpeditionStartInstanceHandler(api));
            this.handlerLookup.Add(ExpeditionSelectShipCommand.Id, new ExpeditionSelectShipHandler(api));
            this.handlerLookup.Add(ExpeditionShowUiCommand.Id, new ExpeditionShowUiHandler(api));

            this.handlerLookup.Add(GalaxyGateBuilderInfoCommand.Id, new GalaxyGateBuilderInfoHandler(api));
            this.handlerLookup.Add(GalaxyGateBuilderGateCompleteCommand.Id, new GalaxyGateBuilderGateCompleteHandler(api));

            this.handlerLookup.Add(InvitationInitializationCommand.Id, new InvitationInitalizationHandler(api));
            this.handlerLookup.Add(GroupMemberUpdateCommand.Id, new GroupMemberUpdateHandler(api));
            this.handlerLookup.Add(GroupCloseCommand.Id, new GroupCloseHandler(api));
            this.handlerLookup.Add(GroupInitalizationCommand.Id, new GroupInitHandler(api));
            this.handlerLookup.Add(MemberLeaveCommand.Id, new MemberLeaveHandler(api));

            this.handlerLookup.Add(ShipInitializationCommand.Id, new ShipInitializationHandler(api));
            this.handlerLookup.Add(CargoUpdateCommand.Id, new CargoUpdateHandler(api));
            this.handlerLookup.Add(CurrencyUpdated.Id, new CurrencyUpdateHandler(api));
            this.handlerLookup.Add(HitpointInfoCommand.Id, new HitpointInfoHandler(api));
            this.handlerLookup.Add(ShieldUpdateCommand.Id, new ShieldUpdateHandler(api));
            this.handlerLookup.Add(MoveUpdateCommand.Id, new MoveUpdateHandler(api));
            this.handlerLookup.Add(SpeedUpdateCommand.Id, new SpeedConfirmHandler(api));
            this.handlerLookup.Add(UserInventoryModule.Id, new UserInventoryHandler(api));
            this.handlerLookup.Add(InventoryItemUpdatedCommand.Id, new InventoryItemUpdateHandler(api));
            this.handlerLookup.Add(EnergyUpdateCommand.Id, new EnergyUpdateHandler(api));

            this.handlerLookup.Add(PayloadEscortKeyInfoCommand.Id, new PayloadEscortKeyInfoHandler(api));

            this.handlerLookup.Add(AssetCreateCommand.Id, new AssetCreateHandler(api));
            this.handlerLookup.Add(AssetRemoveCommand.Id, new AssetRemoveHandler(api));
            this.handlerLookup.Add(JumpgateCreateCommand.Id, new JumpgateCreateHandler(api));
            this.handlerLookup.Add(MapAddPOICommand.Id, new MapAddPOIHandler(api));
            this.handlerLookup.Add(MapRemovePOICommand.Id, new MapRemovePOIHandler(api));
            this.handlerLookup.Add(JumpInitiatedCommand.Id, new JumpConfirmHandler(api));
            this.handlerLookup.Add(JumpGateRemoveCommand.Id, new JumpgateRemoveHandler(api));
            this.handlerLookup.Add(MapAssetActionAvailableCommand.Id, new MapAssetActionAvailableHandler(api));

            this.handlerLookup.Add(AddMineCommand.Id, new AddMineHandler(api));
            this.handlerLookup.Add(RemoveMineCommand.Id, new RemoveMineHandler(api));

            this.handlerLookup.Add(PetHeroActivationCommand.Id, new PetHeroActivationHandler(api));
            this.handlerLookup.Add(PetOutOfFuelCommand.Id, new PetOutOfFuelHandler(api));
            this.handlerLookup.Add(PetFuelUpdateCommand.Id, new PetFuelUpdateHandler(api));
            this.handlerLookup.Add(PetActivationCommand.Id, new PetInitHandler(api));
            this.handlerLookup.Add(PetLevelUpCommand.Id, new PetLevelUpHandler(api));
            this.handlerLookup.Add(PetGearAddCommand.Id, new PetGearAddHandler(api));
            this.handlerLookup.Add(PetGearRemoveCommand.Id, new PetGearRemoveHandler(api));
            this.handlerLookup.Add(PetGearLocatorUpdateCommand.Id, new PetGearLocatorUpdateHandler(api));
            this.handlerLookup.Add(PetIsDestroyedCommand.Id, new PetIsDestroyedHandler(api));
            this.handlerLookup.Add(PetTradeWindowShowCommand.Id, new PetTradeWindowShowHandler(api));
            this.handlerLookup.Add(PetUIRepairButtonCommand.Id, new PetUIRepairButtonHandler(api));
            this.handlerLookup.Add(PetInitializationCommand.Id, new PetInitializationHandler(api));
            this.handlerLookup.Add(PetStatusCommand.Id, new PetStatusHandler(api));

            this.handlerLookup.Add(DiminishingQuestInitializationCommand.Id, new DiminishingQuestInfoHandler(api));
            this.handlerLookup.Add(QuestInitializationCommand.Id, new QuestInitalizationHandler(api));
            this.handlerLookup.Add(QuestConditionUpdateCommand.Id, new QuestConditionUpdateHandler(api));
            this.handlerLookup.Add(QuestCompletedCommand.Id, new QuestCompletedHandler(api));
            this.handlerLookup.Add(QuestFailedCommand.Id, new QuestFailedHandler(api));

            this.handlerLookup.Add(ShipDestroyedCommand.Id, new ShipDestroyedHandler(api));
            this.handlerLookup.Add(ShipCreateCommand.Id, new ShipCreateHandler(api));
            this.handlerLookup.Add(ShipMovementCommand.Id, new ShipMovementHandler(api));
            this.handlerLookup.Add(ShipRemoveCommand.Id, new ShipRemoveHandler(api));

            this.handlerLookup.Add(ShopItemListCommand.Id, new ShopItemListHandler());

            api.Logging.Log($"Created {this.handlerLookup.Count} Handlers!");
        }
    }
}
