package net.bigpoint.darkorbit.com.module
{
   import net.bigpoint.com.module.IModule;
   import net.bigpoint.com.module.IModuleFactory;
   import net.bigpoint.darkorbit.com.module.autopilot.AutopilotStatus;
   import net.bigpoint.darkorbit.com.module.debug.EchoRequest;
   import net.bigpoint.darkorbit.com.module.debug.SummonNPCRequest;
   import net.bigpoint.darkorbit.com.module.liveops.europeanCup.ReqDroneBoostInfo;
   import net.bigpoint.darkorbit.com.module.liveops.seasonpass.RefreshQuestInfo;
   import net.bigpoint.darkorbit.com.module.liveops.seasonpass.SeasonPassRefreshCountCommand;
   import net.bigpoint.darkorbit.com.module.liveops.seasonpass.SeasonPassRefreshRequest;
   import net.bigpoint.darkorbit.com.module.porting.AllowUnityOnlyCommand;
   import net.bigpoint.darkorbit.com.module.porting.NormalTipsCommand;
   import net.bigpoint.darkorbit.com.module.porting.SpaceSearch.EventCommand;
   import net.bigpoint.darkorbit.com.module.porting.SpaceSearch.PlanetCommand;
   import net.bigpoint.darkorbit.com.module.porting.SpaceSearch.SpaceSearchEventsCommand;
   import net.bigpoint.darkorbit.com.module.porting.SpaceSearch.SpaceSearchInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.SpaceSearch.SpaceSearchMissionListCommand;
   import net.bigpoint.darkorbit.com.module.porting.SpaceSearch.SpaceSearchRequest;
   import net.bigpoint.darkorbit.com.module.porting.SpaceSearch.SpaceSearchSectorRewards;
   import net.bigpoint.darkorbit.com.module.porting.SpaceSearch.SpaceSearchSectorRewardsCommand;
   import net.bigpoint.darkorbit.com.module.porting.SpaceSearch.UpdateSpaceSearchKeyCommand;
   import net.bigpoint.darkorbit.com.module.porting.TupleThreeModule;
   import net.bigpoint.darkorbit.com.module.porting.TupleTwoModule;
   import net.bigpoint.darkorbit.com.module.porting.achievement.AchievementInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.achievement.AchievementListCommand;
   import net.bigpoint.darkorbit.com.module.porting.achievement.AchievementListRequest;
   import net.bigpoint.darkorbit.com.module.porting.achievement.AchievementTitleCommand;
   import net.bigpoint.darkorbit.com.module.porting.achievement.AchievementValueCommand;
   import net.bigpoint.darkorbit.com.module.porting.activityevent.EventQuestCompletedCommand;
   import net.bigpoint.darkorbit.com.module.porting.activityevent.EventQuestDefinitionModule;
   import net.bigpoint.darkorbit.com.module.porting.activityevent.EventQuestListCommand;
   import net.bigpoint.darkorbit.com.module.porting.activityevent.anewhope.EventANewHopeActionCommand;
   import net.bigpoint.darkorbit.com.module.porting.activityevent.anewhope.EventANewHopeActionRequest;
   import net.bigpoint.darkorbit.com.module.porting.activityevent.anewhope.EventANewHopeContributorCommand;
   import net.bigpoint.darkorbit.com.module.porting.activityevent.anewhope.EventANewHopeContributorListCommand;
   import net.bigpoint.darkorbit.com.module.porting.activityevent.anewhope.EventANewHopeQuestCommand;
   import net.bigpoint.darkorbit.com.module.porting.activityevent.anewhope.EventANewHopeStatusCommand;
   import net.bigpoint.darkorbit.com.module.porting.activityevent.returnofhero.EventReturnOfHeroActionCommand;
   import net.bigpoint.darkorbit.com.module.porting.activityevent.returnofhero.EventReturnOfHeroActionRequest;
   import net.bigpoint.darkorbit.com.module.porting.activityevent.returnofhero.EventReturnOfHeroContributorCommand;
   import net.bigpoint.darkorbit.com.module.porting.activityevent.returnofhero.EventReturnOfHeroContributorListCommand;
   import net.bigpoint.darkorbit.com.module.porting.activityevent.returnofhero.EventReturnOfHeroQuestCommand;
   import net.bigpoint.darkorbit.com.module.porting.activityevent.returnofhero.EventReturnOfHeroStatusCommand;
   import net.bigpoint.darkorbit.com.module.porting.activityevent.unityclientreward.ClaimUnityClientRewardRequest;
   import net.bigpoint.darkorbit.com.module.porting.activityevent.unityclientreward.EventUnityClientRewardQuestCommand;
   import net.bigpoint.darkorbit.com.module.porting.armoryunlock.ArmoryUnlockClaimCommand;
   import net.bigpoint.darkorbit.com.module.porting.armoryunlock.ArmoryUnlockClaimInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.armoryunlock.ArmoryUnlockClaimRequest;
   import net.bigpoint.darkorbit.com.module.porting.armoryunlock.ArmoryUnlockDrawCommand;
   import net.bigpoint.darkorbit.com.module.porting.armoryunlock.ArmoryUnlockDrawRequest;
   import net.bigpoint.darkorbit.com.module.porting.armoryunlock.ArmoryUnlockInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.armoryunlock.ArmoryUnlockInfoRequest;
   import net.bigpoint.darkorbit.com.module.porting.armoryunlock.ArmoryUnlockLootShowCommand;
   import net.bigpoint.darkorbit.com.module.porting.armoryunlock.ArmoryUnlockLootShowListCommand;
   import net.bigpoint.darkorbit.com.module.porting.auction.AuctionBidCommand;
   import net.bigpoint.darkorbit.com.module.porting.auction.AuctionBidRequest;
   import net.bigpoint.darkorbit.com.module.porting.auction.AuctionDataCommand;
   import net.bigpoint.darkorbit.com.module.porting.auction.AuctionHistoryCommand;
   import net.bigpoint.darkorbit.com.module.porting.auction.AuctionListCommand;
   import net.bigpoint.darkorbit.com.module.porting.auction.AuctionListRequest;
   import net.bigpoint.darkorbit.com.module.porting.auction.AuctionQuickBuyCommand;
   import net.bigpoint.darkorbit.com.module.porting.auction.AuctionQuickBuyRequest;
   import net.bigpoint.darkorbit.com.module.porting.auction.AuctionTestCommand;
   import net.bigpoint.darkorbit.com.module.porting.auction.AuctionTestRequest;
   import net.bigpoint.darkorbit.com.module.porting.auction.GetNamFIagRequest;
   import net.bigpoint.darkorbit.com.module.porting.auction.GetNamFlagCommand;
   import net.bigpoint.darkorbit.com.module.porting.auction.UpdateNamFIagCommand;
   import net.bigpoint.darkorbit.com.module.porting.auction.UpdateNamFIagRequest;
   import net.bigpoint.darkorbit.com.module.porting.autopilot.AutopilotInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.autopilot.AutopilotInfoRequest;
   import net.bigpoint.darkorbit.com.module.porting.autopilot.AutopilotInfoSaveCommand;
   import net.bigpoint.darkorbit.com.module.porting.autopilot.AutopilotInfoSaveRequest;
   import net.bigpoint.darkorbit.com.module.porting.balance.BalanceInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.balance.BalanceInfoListCommand;
   import net.bigpoint.darkorbit.com.module.porting.balance.BalanceInfoListRequest;
   import net.bigpoint.darkorbit.com.module.porting.battlestation.BattleStationHistoryInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.battlestation.BattleStationHistoryListCommand;
   import net.bigpoint.darkorbit.com.module.porting.battlestation.BattleStationHistoryListRequest;
   import net.bigpoint.darkorbit.com.module.porting.battlestation.BattleStationOverviewInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.battlestation.ClanBattleStationInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.battlestation.ClanBattleStationInfoRequest;
   import net.bigpoint.darkorbit.com.module.porting.birthday2025.Birthday2025ClaimRequest;
   import net.bigpoint.darkorbit.com.module.porting.birthday2025.Birthday2025GateEnterRequest;
   import net.bigpoint.darkorbit.com.module.porting.birthday2025.Birthday2025GateRewardCommand;
   import net.bigpoint.darkorbit.com.module.porting.birthday2025.Birthday2025GateWindowCommand;
   import net.bigpoint.darkorbit.com.module.porting.birthday2025.Birthday2025PointRewardCommand;
   import net.bigpoint.darkorbit.com.module.porting.birthday2025.Birthday2025PointRewardRequest;
   import net.bigpoint.darkorbit.com.module.porting.cannon.CannonAttackCommand;
   import net.bigpoint.darkorbit.com.module.porting.cannon.CannonUnstableModuleModule;
   import net.bigpoint.darkorbit.com.module.porting.captain.CaptainActionRequest;
   import net.bigpoint.darkorbit.com.module.porting.captain.CaptainCommand;
   import net.bigpoint.darkorbit.com.module.porting.captain.CaptainDismissBackCommand;
   import net.bigpoint.darkorbit.com.module.porting.captain.CaptainGetResourcesBackCommand;
   import net.bigpoint.darkorbit.com.module.porting.captain.CaptainPerkCommand;
   import net.bigpoint.darkorbit.com.module.porting.captain.CaptainPerkUpgradLevelCommand;
   import net.bigpoint.darkorbit.com.module.porting.captain.CaptainPerkUpgradLevelCostCommand;
   import net.bigpoint.darkorbit.com.module.porting.captain.CaptainRosterListCommand;
   import net.bigpoint.darkorbit.com.module.porting.captain.CaptainUpdateActiveBackCommand;
   import net.bigpoint.darkorbit.com.module.porting.captain.CaptainUpgradPerkBackCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ApplyAcceptCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ApplyAcceptRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.ApplyClanCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ApplyClanRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.ApplyDeclineCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ApplyDeclineRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.ChangeClanDescriptionCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ChangeClanDescriptionRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.ChangeClanFirmaCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ChangeClanFirmaRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.ChangeClanLeaderCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ChangeClanLeaderRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.ChangeClanTaxCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ChangeClanTaxRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanAddNewsCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanAddNewsRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanAllInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanApplyInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanApplyInfoListCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanApplyInfoListRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanDeleteAllMessageCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanDeleteAllMessageRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanDeleteDiploCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanDeleteDiploRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanDeleteMessageCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanDeleteMessageRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanDeleteOpenDiploCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanDeleteOpenDiploRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanDeleteRankCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanDeleteRankRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanDiploInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanDiploReqInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanEditDiploOfferCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanEditDiploOfferRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanFilterSearchInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanFilterSearchRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanListCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanListRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanMemberInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanMemberListCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanMemberListRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanMessageInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanMessageListCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanMessageListRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanNameTagInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanNewDiplomacyCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanNewDiplomacyRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanNewRankCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanNewRankRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanNewsInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanNewsListCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanNewsListRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanRankInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanRankListCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanRankListRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanReportAdminCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanReportAdminRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanSearchNameTagCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanSearchNameTagRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanSendAllMessageCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanSendAllMessageRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanSendMessageCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanSendMessageRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanTaxInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanTaxLogCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClanTestCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ClearClanCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.CompanyDateInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.DelClanCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.DelClanRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.DeleteClanApplyCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.DeleteClanApplyRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.DeleteClanCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.DeleteClanMemberCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.DeleteClanMemberRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.DeleteClanRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.DeleteMyRankCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.DeleteMyRankRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.EditClanInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.EditClanInfoRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.EditClanMemberCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.EditClanMemberRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.EditClanRanksCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.EditClanRanksRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.FoundClanCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.FoundClanRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.JoinClanCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.LeaveClanCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.LeaveClanRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.LoginInitClanCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.LoginInitClanRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.LoginInitMyApplyCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.NotifyNewDiploCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.NotifyNewDiploReqCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.PayoutClanCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.PayoutClanRequest;
   import net.bigpoint.darkorbit.com.module.porting.clan.ShowClanInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.clan.ShowClanInfoRequest;
   import net.bigpoint.darkorbit.com.module.porting.dailylogin.DailyLoginClaimTodayCommand;
   import net.bigpoint.darkorbit.com.module.porting.dailylogin.DailyLoginClaimTodayRequest;
   import net.bigpoint.darkorbit.com.module.porting.dailylogin.DailyLoginRewardDayCommand;
   import net.bigpoint.darkorbit.com.module.porting.dailylogin.DailyLoginRewardDayRequest;
   import net.bigpoint.darkorbit.com.module.porting.events.EndEventSummaryScoreCommand;
   import net.bigpoint.darkorbit.com.module.porting.events.EventLootShowCommand;
   import net.bigpoint.darkorbit.com.module.porting.events.EventPaymentActorCommand;
   import net.bigpoint.darkorbit.com.module.porting.events.EventSummaryScoreCommand;
   import net.bigpoint.darkorbit.com.module.porting.events.LootProbabilityModule;
   import net.bigpoint.darkorbit.com.module.porting.events.SpecialEventPaymentActorCommand;
   import net.bigpoint.darkorbit.com.module.porting.events.UpdateEventSummaryScoreCommand;
   import net.bigpoint.darkorbit.com.module.porting.evoucher.EvoucherHistoryInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.evoucher.EvoucherHistoryListCommand;
   import net.bigpoint.darkorbit.com.module.porting.evoucher.EvoucherHistoryListRequest;
   import net.bigpoint.darkorbit.com.module.porting.expedition.ExpeditionActivateShipRequest;
   import net.bigpoint.darkorbit.com.module.porting.expedition.ExpeditionBackRequest;
   import net.bigpoint.darkorbit.com.module.porting.expedition.ExpeditionBuyTicketCommand;
   import net.bigpoint.darkorbit.com.module.porting.expedition.ExpeditionBuyTicketRequest;
   import net.bigpoint.darkorbit.com.module.porting.expedition.ExpeditionClearanceTimeCommand;
   import net.bigpoint.darkorbit.com.module.porting.expedition.ExpeditionInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.expedition.ExpeditionInfoRequest;
   import net.bigpoint.darkorbit.com.module.porting.expedition.ExpeditionJumpLevelInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.expedition.ExpeditionJumpTargetLevelCommand;
   import net.bigpoint.darkorbit.com.module.porting.expedition.ExpeditionJumpTargetLevelRequest;
   import net.bigpoint.darkorbit.com.module.porting.expedition.ExpeditionRankUserInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.expedition.ExpeditionResetRequest;
   import net.bigpoint.darkorbit.com.module.porting.expedition.ExpeditionRewardCommand;
   import net.bigpoint.darkorbit.com.module.porting.expedition.ExpeditionSelectShipCommand;
   import net.bigpoint.darkorbit.com.module.porting.expedition.ExpeditionSelectShipRequest;
   import net.bigpoint.darkorbit.com.module.porting.expedition.ExpeditionSelectedShipListCommand;
   import net.bigpoint.darkorbit.com.module.porting.expedition.ExpeditionShipCommand;
   import net.bigpoint.darkorbit.com.module.porting.expedition.ExpeditionShipListCommand;
   import net.bigpoint.darkorbit.com.module.porting.expedition.ExpeditionShipListRequest;
   import net.bigpoint.darkorbit.com.module.porting.expedition.ExpeditionShowUiCommand;
   import net.bigpoint.darkorbit.com.module.porting.expedition.ExpeditionStartInstanceCommand;
   import net.bigpoint.darkorbit.com.module.porting.halloffame.HallOfFameDataCommand;
   import net.bigpoint.darkorbit.com.module.porting.halloffame.HallOfFameDataListCommand;
   import net.bigpoint.darkorbit.com.module.porting.halloffame.HallOfFameDataListRequest;
   import net.bigpoint.darkorbit.com.module.porting.halloffame.HallOfFamePages;
   import net.bigpoint.darkorbit.com.module.porting.halloffame.HallOfFamePagesRequest;
   import net.bigpoint.darkorbit.com.module.porting.halloffame.HallOfFameRankingPoints;
   import net.bigpoint.darkorbit.com.module.porting.halloffame.HallOfFameTopDataListCommand;
   import net.bigpoint.darkorbit.com.module.porting.halloffame.HallOfFameTopRequest;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarDroneEquipmentMsg;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarListRequest;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarPetEquipmentMsg;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipActivateShipCommand;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipActivateShipRequest;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipAutoBuyCpuAmmoCommand;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipAutoBuyCpuAmmoRequest;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipBriefCommand;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipChangeShipModelCommand;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipChangeShipModelRequest;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipClearConfigCommand;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipClearConfigRequest;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipDetailCommand;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipDetailRequest;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipDroneView;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipEquipmentCommand;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipEquipmentInventoryItem;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipEquipmentMsg;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipEquipmentRequest;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipFavouriteCommand;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipFavouriteRequest;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipImportConfigCommand;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipImportConfigRequest;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipLoadConfigCommand;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipLoadConfigRequest;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipMoveItemsCommand;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipMoveItemsRequest;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipPetView;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipRenamePetCommand;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipRenamePetRequest;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipRepairDroneCommand;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipRepairDroneRequest;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipRepairModuleCommand;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipRepairModuleRequest;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipSaveConfigCommand;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipSaveConfigRequest;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipSellDroneCommand;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipSellDroneRequest;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipSellItemCommand;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipSellItemRequest;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipUnlockSlotCommand;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipUnlockSlotRequest;
   import net.bigpoint.darkorbit.com.module.porting.hangar.HangarShipsCommand;
   import net.bigpoint.darkorbit.com.module.porting.hangar.InventoryItemModifyCommand;
   import net.bigpoint.darkorbit.com.module.porting.hangar.InventoryItemPack;
   import net.bigpoint.darkorbit.com.module.porting.hangar.InventoryItemSellPack;
   import net.bigpoint.darkorbit.com.module.porting.hangar.SetBonusInfo;
   import net.bigpoint.darkorbit.com.module.porting.hangar.ShipDesignPack;
   import net.bigpoint.darkorbit.com.module.porting.hangar.SingleDroneEquipmentMsg;
   import net.bigpoint.darkorbit.com.module.porting.logbook.LogbookDataCommand;
   import net.bigpoint.darkorbit.com.module.porting.logbook.LogbookDataListCommand;
   import net.bigpoint.darkorbit.com.module.porting.logbook.LogbookDataListRequest;
   import net.bigpoint.darkorbit.com.module.porting.lottery.LotteryEventFlushCommand;
   import net.bigpoint.darkorbit.com.module.porting.lottery.LotteryLogCommand;
   import net.bigpoint.darkorbit.com.module.porting.lottery.LotteryLogRewardCommand;
   import net.bigpoint.darkorbit.com.module.porting.lottery.LotteryRequest;
   import net.bigpoint.darkorbit.com.module.porting.lottery.LotteryRequestCommand;
   import net.bigpoint.darkorbit.com.module.porting.messagestack.ArgStackCommand;
   import net.bigpoint.darkorbit.com.module.porting.messagestack.BaseStackCommand;
   import net.bigpoint.darkorbit.com.module.porting.pilotsheet.InternalPilotBioRequest;
   import net.bigpoint.darkorbit.com.module.porting.pilotsheet.InternalPilotBioUpdateRequest;
   import net.bigpoint.darkorbit.com.module.porting.pilotsheet.InternalPilotBioUpdateResultsCommand;
   import net.bigpoint.darkorbit.com.module.porting.pilotsheet.InternalPilotBioUpdateStatusRequest;
   import net.bigpoint.darkorbit.com.module.porting.pilotsheet.InternalPilotBioUpdateUserNameRequest;
   import net.bigpoint.darkorbit.com.module.porting.pilotsheet.InternalPilotBioUserProfileCommand;
   import net.bigpoint.darkorbit.com.module.porting.publicWorldBoss.PublicWorldBossInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.publicWorldBoss.PublicWorldBossRaidCommand;
   import net.bigpoint.darkorbit.com.module.porting.publicWorldBoss.PublicWorldBossRaidIntensity;
   import net.bigpoint.darkorbit.com.module.porting.publicWorldBoss.PublicWorldBossRankCommand;
   import net.bigpoint.darkorbit.com.module.porting.publicWorldBoss.PublicWorldBossRankReward;
   import net.bigpoint.darkorbit.com.module.porting.publicWorldBoss.PublicWorldBossRequest;
   import net.bigpoint.darkorbit.com.module.porting.publicWorldBoss.PublicWorldBossTier;
   import net.bigpoint.darkorbit.com.module.porting.shop.ShopItemBuyCommand;
   import net.bigpoint.darkorbit.com.module.porting.shop.ShopItemBuyRequest;
   import net.bigpoint.darkorbit.com.module.porting.shop.ShopItemCommand;
   import net.bigpoint.darkorbit.com.module.porting.shop.ShopItemLevelDataCommand;
   import net.bigpoint.darkorbit.com.module.porting.shop.ShopItemListCommand;
   import net.bigpoint.darkorbit.com.module.porting.shop.ShopItemListRequest;
   import net.bigpoint.darkorbit.com.module.porting.shop.ShopLabelPrompt;
   import net.bigpoint.darkorbit.com.module.porting.shop.exchange.ExchangeShopCommand;
   import net.bigpoint.darkorbit.com.module.porting.shop.exchange.ExchangeShopRequest;
   import net.bigpoint.darkorbit.com.module.porting.shop.exchange.ExchangesShopItem;
   import net.bigpoint.darkorbit.com.module.porting.skillTree.ConvertResearchPointsCommand;
   import net.bigpoint.darkorbit.com.module.porting.skillTree.ConvertResearchPointsRequest;
   import net.bigpoint.darkorbit.com.module.porting.skillTree.ResetSkillTreeCommand;
   import net.bigpoint.darkorbit.com.module.porting.skillTree.ResetSkillTreeRequest;
   import net.bigpoint.darkorbit.com.module.porting.skillTree.SkillTreeInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.skillTree.SkillTreeListCommand;
   import net.bigpoint.darkorbit.com.module.porting.skillTree.SkillTreeListRequest;
   import net.bigpoint.darkorbit.com.module.porting.skillTree.UpLevelSkillTreeCommand;
   import net.bigpoint.darkorbit.com.module.porting.skillTree.UpLevelSkillTreeRequest;
   import net.bigpoint.darkorbit.com.module.porting.skillTree.UpdateCargoSpaceCommand;
   import net.bigpoint.darkorbit.com.module.porting.skylab.SkylabActionRequest;
   import net.bigpoint.darkorbit.com.module.porting.skylab.SkylabActiveRobotsLeftTimeCommand;
   import net.bigpoint.darkorbit.com.module.porting.skylab.SkylabCollectorInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.skylab.SkylabMainViewCommand;
   import net.bigpoint.darkorbit.com.module.porting.skylab.SkylabMainViewModuleCommand;
   import net.bigpoint.darkorbit.com.module.porting.skylab.SkylabModuleProductivityCommand;
   import net.bigpoint.darkorbit.com.module.porting.skylab.SkylabModuleUpgradeCommand;
   import net.bigpoint.darkorbit.com.module.porting.skylab.SkylabPurchaseRobotRequest;
   import net.bigpoint.darkorbit.com.module.porting.skylab.SkylabRobotPriceCommand;
   import net.bigpoint.darkorbit.com.module.porting.skylab.SkylabStorageInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.skylab.SkylabStorageOreCommand;
   import net.bigpoint.darkorbit.com.module.porting.skylab.SkylabUnifiedReceiptCommand;
   import net.bigpoint.darkorbit.com.module.porting.skylab.SkylabViewModuleBackCommand;
   import net.bigpoint.darkorbit.com.module.porting.skylab.SkylabViewModuleRequest;
   import net.bigpoint.darkorbit.com.module.porting.skylab.SkylabXenoInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.strikeandexit.SpotLightPositionCommand;
   import net.bigpoint.darkorbit.com.module.porting.strikeandexit.StrikeAndExitInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.strikeandexit.StrikeAndExitInfoRequest;
   import net.bigpoint.darkorbit.com.module.porting.strikeandexit.StrikeAndExitMapInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.strikeandexit.StrikeAndExitRemoveScoreCommand;
   import net.bigpoint.darkorbit.com.module.porting.strikeandexit.StrikeAndExitScoreCommand;
   import net.bigpoint.darkorbit.com.module.porting.synthesis.SynthesisCostLootCommand;
   import net.bigpoint.darkorbit.com.module.porting.synthesis.SynthesisGroupCommand;
   import net.bigpoint.darkorbit.com.module.porting.synthesis.SynthesisInventoryCommand;
   import net.bigpoint.darkorbit.com.module.porting.synthesis.SynthesisInventoryRequest;
   import net.bigpoint.darkorbit.com.module.porting.synthesis.SynthesisItemRequest;
   import net.bigpoint.darkorbit.com.module.porting.synthesis.SynthesisListCommand;
   import net.bigpoint.darkorbit.com.module.porting.synthesis.SynthesisModuleCommand;
   import net.bigpoint.darkorbit.com.module.porting.trinitytrials.PassItemModule;
   import net.bigpoint.darkorbit.com.module.porting.trinitytrials.TrinityTrialsClaimPassRewardRequest;
   import net.bigpoint.darkorbit.com.module.porting.trinitytrials.TrinityTrialsDropLevelCommand;
   import net.bigpoint.darkorbit.com.module.porting.trinitytrials.TrinityTrialsDropLevelOptionalCommand;
   import net.bigpoint.darkorbit.com.module.porting.trinitytrials.TrinityTrialsDropLevelOptionalRequest;
   import net.bigpoint.darkorbit.com.module.porting.trinitytrials.TrinityTrialsGateEnterRequest;
   import net.bigpoint.darkorbit.com.module.porting.trinitytrials.TrinityTrialsGateWindowCommand;
   import net.bigpoint.darkorbit.com.module.porting.trinitytrials.TrinityTrialsPassCommand;
   import net.bigpoint.darkorbit.com.module.porting.trinitytrials.TrinityTrialsRewardDescriptionCommand;
   import net.bigpoint.darkorbit.com.module.porting.trinitytrials.TrinityTrialsUIRequest;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.AbsUserMessageInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.BlackListAddCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.BlackListAddRequest;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.BlackListDeleteCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.BlackListDeleteRequest;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.BlackListGetCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.BlackListGetRequest;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.ContactBaseInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.ContactFriendAcceptTipsCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.ContactFriendInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.ContactInvitationInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.ContactListBlockInvitationsTipsCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.ContactListCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.ContactListRequest;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageAuctionInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageClanInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageDeleteCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageDeleteRequest;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageGetTargetCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageGetTargetRequest;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageInboxListCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageInboxListRequest;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageInfoInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageInitCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageInitRequest;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageLootCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageLootRequest;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageNewCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageNoteAddCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageNoteAddRequest;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageNoteDeleteCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageNoteDeleteRequest;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageNoteInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageNoteListCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageNoteListRequest;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageNoteUpdateCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageNoteUpdateRequest;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageOutboxListCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageOutboxListRequest;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageParentInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageSendCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageSendRequest;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageSystemInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageTargetInfoCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageUpdateStatusCommand;
   import net.bigpoint.darkorbit.com.module.porting.userMessage.UserMessageUpdateStatusRequest;
   
   public class ModuleFactory implements IModuleFactory
   {
      private static const fac:IModuleFactory = new ModuleFactory();
      
      public function ModuleFactory()
      {
         super();
      }
      
      public static function get() : IModuleFactory
      {
         return fac;
      }
      
      public function createInstance(param1:int) : IModule
      {
         switch(param1)
         {
            case -32697:
               return new ReplacementModule();
            case -32615:
               return new TrainingGroundRegistrationUnavailableLowGearScoreCommand();
            case -32594:
               return new ContactListEntryAttributeOnlineModule();
            case -32539:
               return new RzoneCancelSignUpRequest();
            case -32529:
               return new MonthlyDeluxeInfoCommand();
            case -32520:
               return new UserMessageInboxListCommand();
            case -32512:
               return new ClaimUnityClientRewardRequest();
            case -32344:
               return new PetFuelCargoFullMessage();
            case -32288:
               return new GalaxyGateBuilderBuyLifeRequest();
            case -32284:
               return new WildcardMessageModule();
            case -32240:
               return new DiminishingQuestRemoveQuestCommand();
            case -32215:
               return new AssemblySetFilterRequest();
            case -32056:
               return new RzoneRegistrationStatusModule();
            case -32003:
               return new TrainingGroundSignUpRequest();
            case -31830:
               return new UnstableModulesListCommand();
            case -31600:
               return new TitleFrenzyInfoCommand();
            case -31597:
               return new CaptchaExplodeCommand();
            case -31554:
               return new ReturneeLoginHideUIItemRequest();
            case -31536:
               return new DispatchModule();
            case -31213:
               return new SkillTreeListRequest();
            case -31192:
               return new AutopilotStatus();
            case -31176:
               return new SeasonPassMissionInfoCommand();
            case -31022:
               return new UserMessageDeleteRequest();
            case -30877:
               return new ExpeditionShipListRequest();
            case -30853:
               return new EggTriggerRequest();
            case -30830:
               return new MessageModule();
            case -30742:
               return new ResetSkillTreeCommand();
            case -30460:
               return new EternalGateBoosterInfoCommand();
            case -30415:
               return new CaptainPerkUpgradLevelCommand();
            case -30273:
               return new TrainingArenaSeasonRewardEntryCommand();
            case -30256:
               return new RefreshSelectedUICategoryCommand();
            case -30163:
               return new Birthday2025PointRewardRequest();
            case -30027:
               return new TrinityTrialsClaimPassRewardRequest();
            case -30008:
               return new LotteryEventFlushCommand();
            case -29986:
               return new BattlePassKeyExchangeRequest();
            case -29973:
               return new WorldBossRewardsTierUpdateCommand();
            case -29755:
               return new FightActivityModule();
            case -29751:
               return new ClanDeleteRankCommand();
            case -29520:
               return new SeasonPassMissionModule();
            case -29355:
               return new RzoneUnavailibleStatusModule();
            case -29266:
               return new NameChangeCommand();
            case -29232:
               return new ReturneeLoginItemRewardModule();
            case -29184:
               return new ClanTaxInfoCommand();
            case -29179:
               return new RogueLiteSelectRewardCommand();
            case -29040:
               return new UnstableModuleRecycleRequest();
            case -29026:
               return new ArmoryUnlockClaimInfoCommand();
            case -28989:
               return new EventSummaryScoreCommand();
            case -28947:
               return new EventANewHopeActionRequest();
            case -28833:
               return new TargetModule();
            case -28782:
               return new FrozenLabyrinthTopRankerInfoCommand();
            case -28580:
               return new SeasonPassRewardInfoCommand();
            case -28577:
               return new MileageMarketPackageModule();
            case -28558:
               return new VIPMerchantExchangeModule();
            case -28510:
               return new DispatchCollectGateRewardRequest();
            case -28246:
               return new NotifyNewDiploCommand();
            case -28187:
               return new MenuCategoryInitializationCommand();
            case -28186:
               return new InventoryItemUpdatedCommand();
            case -28169:
               return new ClanAddNewsCommand();
            case -28117:
               return new RewardContentModule();
            case -28099:
               return new DispatchBuySlotRequest();
            case -28076:
               return new TrainingGroundLookingForOpponentStatusCommand();
            case -28066:
               return new DailyLoginRewardDayCommand();
            case -28016:
               return new HallOfFameTopDataListCommand();
            case -27894:
               return new UserMessageInitCommand();
            case -27889:
               return new GroupMemberInitializationCommand();
            case -27824:
               return new MemberLoggedOutModule();
            case -27795:
               return new ClanEditDiploOfferCommand();
            case -27794:
               return new UserMessageNoteUpdateCommand();
            case -27784:
               return new RaidGateGameStageModule();
            case -27626:
               return new Birthday2025GateRewardCommand();
            case -27417:
               return new HintWindowAddComand();
            case -27383:
               return new VIPMerchantExchangeRequest();
            case -27348:
               return new AssemblyRecipeTemporaryTypeModule();
            case -27283:
               return new VIPMerchantExchangeBuyCommand();
            case -27280:
               return new InvitationErrorCommand();
            case -27157:
               return new LevelModule();
            case -27097:
               return new ArmoryUnlockClaimRequest();
            case -26912:
               return new HangarShipClearConfigCommand();
            case -26803:
               return new TitleFrenzyRankingInfoModule();
            case -26731:
               return new DispatchActiveEventCommand();
            case -26639:
               return new CCDateRangeModule();
            case -26497:
               return new PublicWorldBossRequest();
            case -26487:
               return new WorldBossCompletionModule();
            case -26430:
               return new EventQuestListCommand();
            case -26413:
               return new SeasonPointsInfoCommand();
            case -26187:
               return new StrikeAndExitScoreCommand();
            case -26169:
               return new SeasonPassMissionStatusModule();
            case -25962:
               return new BlackListAddRequest();
            case -25903:
               return new WorldBossInfoCommand();
            case -25765:
               return new HintWindowModule();
            case -25655:
               return new RogueLiteSelectShipRequest();
            case -25575:
               return new SoundTriggerModule();
            case -25523:
               return new AttackRangeExceededCommand();
            case -25493:
               return new RzoneClosingMapStatusModule();
            case -25271:
               return new SkylabStorageInfoCommand();
            case -25212:
               return new VIPMerchantExchangeInfoCommand();
            case -25182:
               return new WarGameScoreUpdateCommand();
            case -25135:
               return new HangarShipImportConfigRequest();
            case -25122:
               return new HallOfFameDataListCommand();
            case -25017:
               return new TitleFrenzyOwnerUpdateCommand();
            case -24921:
               return new EventActivationFactionAttributeModule();
            case -24707:
               return new ExpeditionActivateShipRequest();
            case -24666:
               return new ContactListEntryAttributeNameModule();
            case -24642:
               return new EventRewardConfirmationCommand();
            case -24639:
               return new VIPMerchantExchangeBuyRequest();
            case -24629:
               return new ArmoryUnlockInfoCommand();
            case -24477:
               return new PreBlackFridaySalesInfoCommand();
            case -24446:
               return new UnstableModuleRecycleResultCommand();
            case -24437:
               return new EternalBlacklightRankingEventInfoCommand();
            case -24349:
               return new SkylabMainViewCommand();
            case -24344:
               return new UridiumBankClaimRequest();
            case -24339:
               return new RogueLiteSelectShipCommand();
            case -24121:
               return new HangarShipDetailCommand();
            case -24076:
               return new ReturneeLoginRewardClaimRequest();
            case -24032:
               return new UnstableAttributeModule();
            case -23991:
               return new InvitationCreationRequest();
            case -23986:
               return new ApplyDeclineRequest();
            case -23954:
               return new QuestLocalizationModule();
            case -23941:
               return new AssemblyCollectAllRequest();
            case -23867:
               return new HangarListRequest();
            case -23746:
               return new EventANewHopeActionCommand();
            case -23722:
               return new ClanEditDiploOfferRequest();
            case -23691:
               return new BlackListAddCommand();
            case -23475:
               return new AuctionBidRequest();
            case -23469:
               return new ClanMemberListCommand();
            case -23464:
               return new ChangeClanLeaderRequest();
            case -23334:
               return new FrozenLabyrinthRankModule();
            case -23326:
               return new FrozenLabyrinthKeyInfoCommand();
            case -23311:
               return new PVPSummerEventFeverInfoCommand();
            case -23229:
               return new CommandCenterNewsModule();
            case -23147:
               return new ChangeClanDescriptionRequest();
            case -23053:
               return new ShipBoostEventInfoCommand();
            case -22942:
               return new HangarShipImportConfigCommand();
            case -22785:
               return new ExpeditionShowUiCommand();
            case -22724:
               return new ExpeditionShipCommand();
            case -22692:
               return new SpaceSearchSectorRewards();
            case -22637:
               return new InternalPilotBioUpdateStatusRequest();
            case -22625:
               return new ChatAuthenticateUserRequest();
            case -22607:
               return new CommandCenterNewsInfoRequest();
            case -22583:
               return new ChangeClanTaxCommand();
            case -22322:
               return new MarkPositionCommand();
            case -22316:
               return new EternalBlacklightBoosterInfoCommand();
            case -22291:
               return new HangarShipAutoBuyCpuAmmoRequest();
            case -22273:
               return new RogueLiteCpuInfoCommand();
            case -22229:
               return new ConvertResearchPointsRequest();
            case -22209:
               return new EquipReadyCommand();
            case -22197:
               return new ContactRequest();
            case -22100:
               return new QuestSettingsRequest();
            case -22055:
               return new MileageMarketInfoCommand();
            case -22013:
               return new PingMemberRequest();
            case -21995:
               return new KickMemberRequest();
            case -21891:
               return new EventReturnOfHeroContributorCommand();
            case -21686:
               return new CaptainPerkCommand();
            case -21666:
               return new BattlePassQuestModule();
            case -21616:
               return new InternalPilotBioUpdateResultsCommand();
            case -21530:
               return new RemoveCollectableCommand();
            case -21521:
               return new ClanDeleteAllMessageCommand();
            case -21344:
               return new HangarShipRenamePetCommand();
            case -21287:
               return new SummonNPCRequest();
            case -21275:
               return new CaptainRosterListCommand();
            case -21183:
               return new LeaveClanRequest();
            case -21180:
               return new RankRewardCommand();
            case -21082:
               return new ChangeLeaderRequest();
            case -21033:
               return new GetNamFlagCommand();
            case -20997:
               return new ShipBoostEventBoostInfoCommand();
            case -20975:
               return new Birthday2025GateEnterRequest();
            case -20936:
               return new RzoneQueueingStatusModule();
            case -20779:
               return new InstantRepairRequest();
            case -20702:
               return new DispatchMessageCommand();
            case -20656:
               return new RzoneConfirmRequest();
            case -20538:
               return new ClanNameTagInfoCommand();
            case -20517:
               return new ClanMessageListRequest();
            case -20430:
               return new AllowUnityOnlyCommand();
            case -20224:
               return new SynthesisModuleCommand();
            case -20198:
               return new DispatchSlotCostCommand();
            case -20176:
               return new SkylabActionRequest();
            case -20149:
               return new AuctionListRequest();
            case -20009:
               return new UserMessageNoteListCommand();
            case -19950:
               return new LeaderChangeCommand();
            case -19949:
               return new ArmoryUnlockDrawCommand();
            case -19860:
               return new TrackStringVariableModule();
            case -19793:
               return new ClanSendMessageRequest();
            case -19470:
               return new SeasonPassRewardClaimCommand();
            case -19420:
               return new ShopItemListRequest();
            case -19326:
               return new GalaxyGateBuilderGetGatePartsRequest();
            case -19207:
               return new UserMessageUpdateStatusRequest();
            case -19087:
               return new TieredArenaEventSelfReputationUpdateCommand();
            case -19053:
               return new AssemblyRecipeTypeModule();
            case -18926:
               return new NpcEventBossInfoCommand();
            case -18917:
               return new RewardContentCommand();
            case -18831:
               return new EquipAttributeModule();
            case -18813:
               return new QuestSettingsModule();
            case -18727:
               return new DeleteClanMemberRequest();
            case -18716:
               return new EvoucherHistoryListRequest();
            case -18701:
               return new AddBoxCommand();
            case -18621:
               return new SingleDroneEquipmentMsg();
            case -18599:
               return new ShopItemLevelDataCommand();
            case -18533:
               return new ClanDeleteRankRequest();
            case -18513:
               return new TrainingGroundCancelRequest();
            case -18410:
               return new PVPSummerEventReputationUpdateCommand();
            case -18286:
               return new SkylabUnifiedReceiptCommand();
            case -18259:
               return new TimerStateModule();
            case -18139:
               return new HallOfFameTopRequest();
            case -18088:
               return new ReturneeLoginRewardInfoCommand();
            case -17966:
               return new UnstableModuleRestabiliseRequest();
            case -17940:
               return new WarGameJoinRequest();
            case -17846:
               return new UserMessageGetTargetRequest();
            case -17742:
               return new TrainingGroundStatusCommand();
            case -17670:
               return new AssemblyRecipeReadyToCollectStatusModule();
            case -17430:
               return new ReloginCommand();
            case -17289:
               return new UridiumBankInfoCommand();
            case -17156:
               return new LogMessageCommand();
            case -17131:
               return new RerollWindowSettingsModule();
            case -17087:
               return new GroupMemberUpdateCommand();
            case -17068:
               return new CommandCenterInfoCommand();
            case -16971:
               return new ShopItemBuyCommand();
            case -16958:
               return new SeasonPassLevelPurchaseCommand();
            case -16955:
               return new HangarShipDroneView();
            case -16869:
               return new ChangeClanFirmaCommand();
            case -16810:
               return new ClanFilterSearchInfoCommand();
            case -16721:
               return new ClanSearchNameTagCommand();
            case -16669:
               return new ChrominUpdateCommand();
            case -16584:
               return new ReturneeLoginBundleBuyRequest();
            case -16507:
               return new GalaxyGateBuilderBonusGGRewardModule();
            case -16413:
               return new PassItemModule();
            case -16375:
               return new CombatMusicCommand();
            case -16324:
               return new SkylabRobotPriceCommand();
            case -16109:
               return new MonthlyDeluxeDayInfoCommand();
            case -16047:
               return new ClanSearchNameTagRequest();
            case -16031:
               return new UserMessageGetTargetCommand();
            case -16009:
               return new UIItemAddHighlightCommand();
            case -15922:
               return new SpaceStationCommand();
            case -15892:
               return new AgatusEventInfoCommand();
            case -15872:
               return new ContactListRequest();
            case -15724:
               return new ApplyClanRequest();
            case -15687:
               return new DelClanRequest();
            case -15681:
               return new HangarShipSellDroneRequest();
            case -15671:
               return new ExpeditionClearanceTimeCommand();
            case -15616:
               return new ExpeditionRankUserInfoCommand();
            case -15561:
               return new ChangeClanFirmaRequest();
            case -15362:
               return new ResetSkillTreeRequest();
            case -15282:
               return new NpcEventInfoCommand();
            case -15252:
               return new SpaceSearchInfoCommand();
            case -15195:
               return new SeasonPassRewardClaimRequest();
            case -15101:
               return new RogueLiteChangeEquipmentRequest();
            case -15010:
               return new EpicFightMessageTypeModule();
            case -14674:
               return new GalaxyGateBuilderMultiEnergyRequest();
            case -14599:
               return new ExchangeShopCommand();
            case -14564:
               return new InvitationInitializationCommand();
            case -14561:
               return new MemberJumpCommand();
            case -14550:
               return new ExpeditionRewardCommand();
            case -14454:
               return new TrinityTrialsPassCommand();
            case -14449:
               return new ExpeditionBuyTicketRequest();
            case -14417:
               return new DisplayOfferCommand();
            case -14189:
               return new GalaxyGateBuilderMultiEnergyCommand();
            case -14112:
               return new PublicWorldBossRaidIntensity();
            case -14090:
               return new EditClanMemberRequest();
            case -14083:
               return new MenuItemCooldownGroupTimerCommand();
            case -14035:
               return new EternalBlacklightBoosterCommand();
            case -13900:
               return new DispatchSlotModule();
            case -13884:
               return new EternalGateKeyInfoCommand();
            case -13763:
               return new QuantumFluidCoreData();
            case -13758:
               return new EditClanRanksCommand();
            case -13744:
               return new MileageMarketExchangeRequest();
            case -13599:
               return new TupleTwoModule();
            case -13507:
               return new StartCapsuleCollectCommand();
            case -13339:
               return new AuctionDataCommand();
            case -13299:
               return new ExchangeShopRequest();
            case -13204:
               return new HintModule();
            case -13008:
               return new CCommandModule();
            case -12979:
               return new ReturneeLoginRewardDayInfoCommand();
            case -12971:
               return new InventoryItemModule();
            case -12953:
               return new ApplyAcceptRequest();
            case -12940:
               return new EventReturnOfHeroActionCommand();
            case -12911:
               return new AgatusEventBossInfoCommand();
            case -12870:
               return new EternalGateInfoCommand();
            case -12697:
               return new GalaxyGateBuilderBuyLifeCommand();
            case -12691:
               return new UpdateCargoSpaceCommand();
            case -12288:
               return new CaptainUpgradPerkBackCommand();
            case -12138:
               return new UserMessageClanInfoCommand();
            case -12016:
               return new MapAddControlPOIZoneCommand();
            case -11940:
               return new HangarShipLoadConfigCommand();
            case -11937:
               return new UpLevelSkillTreeRequest();
            case -11821:
               return new HangarShipSellItemCommand();
            case -11763:
               return new PlaySoundCommand();
            case -11729:
               return new AchievementInfoCommand();
            case -11672:
               return new PublicWorldBossTier();
            case -11573:
               return new CCommandRequest();
            case -11508:
               return new HangarShipFavouriteCommand();
            case -11375:
               return new HallOfFameDataCommand();
            case -11262:
               return new WarGameStatusUpdateCommand();
            case -11173:
               return new AuctionQuickBuyCommand();
            case -11170:
               return new MapAssetIconAddHighlightCommand();
            case -11108:
               return new ShipBoostEventUpdateInfoCommand();
            case -11085:
               return new BalanceInfoListCommand();
            case -11056:
               return new RzoneCancelRequest();
            case -11029:
               return new InventoryItemAttributeModule();
            case -11017:
               return new ContactListEntryAttributeBaseModule();
            case -10977:
               return new ContactListSettingsModule();
            case -10966:
               return new SkylabModuleUpgradeCommand();
            case -10965:
               return new BoosterPlayerDetailModule();
            case -10933:
               return new SpacePlagueStatusCommand();
            case -10772:
               return new DelClanCommand();
            case -10766:
               return new CaptainDismissBackCommand();
            case -10662:
               return new PlutusPointsUpdateCommand();
            case -10557:
               return new UserMessageSendCommand();
            case -10457:
               return new MessageCommand();
            case -10407:
               return new FrozenLabyrinthInfoCommand();
            case -10328:
               return new DominationFactionInfluenceUpdateCommand();
            case -10278:
               return new GroupErrorCommand();
            case -10112:
               return new ShopItemCommand();
            case -9920:
               return new MinimapStrategyModule();
            case -9875:
               return new RankInfoCommand();
            case -9828:
               return new ShipBoostEventAttributeModule();
            case -9702:
               return new StrikeAndExitRemoveScoreCommand();
            case -9567:
               return new WindowItemCommand();
            case -9482:
               return new CCBooleanValueModule();
            case -9335:
               return new KillUiInfoMessageCommand();
            case -9260:
               return new LotteryRequestCommand();
            case -9244:
               return new HangarShipPetView();
            case -9214:
               return new AttackAbortLaserCommand();
            case -9107:
               return new SpacePlagueCounterDataModule();
            case -8974:
               return new WarGameLobbyStatusModule();
            case -8940:
               return new ExpeditionShipListCommand();
            case -8932:
               return new ClanListRequest();
            case -8737:
               return new RewardContentRequest();
            case -8683:
               return new ShipBoostEventModule();
            case -8659:
               return new ExpeditionJumpTargetLevelRequest();
            case -8650:
               return new ClanDeleteAllMessageRequest();
            case -8636:
               return new HangarShipEquipmentRequest();
            case -8622:
               return new CompanyDateInfoCommand();
            case -8605:
               return new ContactListEntryAttributeClanTagModule();
            case -8375:
               return new EchoRequest();
            case -8316:
               return new ShopItemListCommand();
            case -8262:
               return new VIPMerchantExchangePackageUpdateCommand();
            case -8246:
               return new SkylabModuleProductivityCommand();
            case -8239:
               return new BalanceInfoCommand();
            case -8235:
               return new QuantumFluidCoreDataListCommand();
            case -8224:
               return new RzoneOnRZoneStatusModule();
            case -8216:
               return new UserMessageTargetInfoCommand();
            case -7984:
               return new WorldBossRewardsInfoCommand();
            case -7923:
               return new BoosterBattleStationDetailModule();
            case -7787:
               return new TrainingArenaSeasonInfoCommand();
            case -7639:
               return new PVPSummerEventSelfReputationUpdateCommand();
            case -7612:
               return new ExpeditionBackRequest();
            case -7504:
               return new FoundClanRequest();
            case -7495:
               return new AbsUserMessageInfoCommand();
            case -7437:
               return new CaptchaTriggerCommand();
            case -7406:
               return new EternalBlacklightRankModule();
            case -7385:
               return new AssemblyRecipeRegularTypeModule();
            case -7254:
               return new GalaxyGateBuilderItemModule();
            case -7211:
               return new CaptchaBoxModule();
            case -7060:
               return new LeaveClanCommand();
            case -6972:
               return new TrainingArenaPerformanceInfoCommand();
            case -6940:
               return new KillUiFullMessageCommand();
            case -6914:
               return new CPUInitializationCommand();
            case -6907:
               return new LogbookDataListCommand();
            case -6856:
               return new EventUnityClientRewardQuestCommand();
            case -6739:
               return new RewardAchievementModule();
            case -6724:
               return new SeasonPassRefreshRequest();
            case -6699:
               return new HangarShipLoadConfigRequest();
            case -6633:
               return new FactionInfluenceUpdateCommand();
            case -6627:
               return new TitleFrenzyRankingInfoCommand();
            case -6612:
               return new HangarShipClearConfigRequest();
            case -6569:
               return new BattleStationOverviewInfoCommand();
            case -6477:
               return new CCStringValueModule();
            case -6090:
               return new TrinityTrialsGateEnterRequest();
            case -6072:
               return new MileageMarketPackageBuyRequest();
            case -6016:
               return new EventQuestCompletedCommand();
            case -5975:
               return new DispatchGetRewardPackageRequest();
            case -5906:
               return new WarGameInitLobbyRequest();
            case -5901:
               return new CaptainPerkUpgradLevelCostCommand();
            case -5889:
               return new CCAttributeModule();
            case -5873:
               return new EventQuestDefinitionModule();
            case -5792:
               return new SkylabStorageOreCommand();
            case -5777:
               return new InvitationRemovalCommand();
            case -5758:
               return new ContactListEntryAttributeClanModule();
            case -5657:
               return new RefreshQuestInfo();
            case -5613:
               return new MileageMarketPackageUpdateCommand();
            case -5459:
               return new ClanDeleteDiploRequest();
            case -5404:
               return new GalaxyGateBuilderGateModule();
            case -5385:
               return new TrainingGroundFightInfoCommand();
            case -5367:
               return new EmperorSibelonBuffRingCommand();
            case -5287:
               return new CommandCenterNewsClickedRequest();
            case -4926:
               return new TrackVariableModule();
            case -4850:
               return new EventReturnOfHeroStatusCommand();
            case -4824:
               return new EventProgressInfoCommand();
            case -4726:
               return new ClanTestCommand();
            case -4712:
               return new EventProgressCollectRequest();
            case -4640:
               return new UserMessageNoteDeleteCommand();
            case -4610:
               return new ClanDeleteDiploCommand();
            case -4472:
               return new GalaxyGateBuilderGetGatePartsCommand();
            case -4412:
               return new EventANewHopeContributorCommand();
            case -4382:
               return new TrainingGroundFightStatusCommand();
            case -4273:
               return new ClanNewRankRequest();
            case -4023:
               return new PlayerInfluenceUpdateCommand();
            case -3982:
               return new TrainingArenaRankInfoCommand();
            case -3973:
               return new SlotInfoCommand();
            case -3953:
               return new EternalBlacklightCpuInfoCommand();
            case -3866:
               return new SpaceSearchSectorRewardsCommand();
            case -3825:
               return new ClearMarkersCommand();
            case -3821:
               return new SynthesisListCommand();
            case -3647:
               return new BattleStationHistoryInfoCommand();
            case -3471:
               return new ClanInfoCommand();
            case -3300:
               return new RankingsEventsInfoRequest();
            case -3213:
               return new EventReturnOfHeroQuestCommand();
            case -3212:
               return new ArenaSurvivorsUpdateCommand();
            case -3160:
               return new StayinAlive();
            case -3122:
               return new JackpotArenaMatchResultModule();
            case -3105:
               return new WarGameLeaveQueueRequest();
            case -3035:
               return new EternalGateWaveInfoCommand();
            case -2923:
               return new RogueLiteModifierModule();
            case -2891:
               return new UserMessageOutboxListCommand();
            case -2839:
               return new SpacePlagueRatingStatusCommand();
            case -2656:
               return new UserMessageNoteUpdateRequest();
            case -2654:
               return new GalaxyGateBuilderMultiGateProgressModule();
            case -2456:
               return new TieredArenaEventScheduleInfoCommand();
            case -2325:
               return new GalaxyGateBuilderProbabilityModule();
            case -2272:
               return new HallOfFameDataListRequest();
            case -2259:
               return new AssemblyRecipeKeyModule();
            case -2125:
               return new ArmoryUnlockClaimCommand();
            case -2120:
               return new UserMessageNoteAddRequest();
            case -2075:
               return new PlanetCommand();
            case -2063:
               return new ClanNewRankCommand();
            case -1992:
               return new MessageKeyListWildcardModule();
            case -1964:
               return new ClanDiploReqInfoCommand();
            case -1955:
               return new RewardTitleModule();
            case -1887:
               return new AutopilotInfoRequest();
            case -1869:
               return new RankingsEventsInfoModule();
            case -1785:
               return new UnstableModuleRerollRequest();
            case -1748:
               return new NewQuestsAvailableCommand();
            case -1741:
               return new PVPSummerEventInfoCommand();
            case -1645:
               return new ClanNewsListRequest();
            case -1636:
               return new BalanceInfoListRequest();
            case -1515:
               return new SkillTreeInfoCommand();
            case -1502:
               return new DiminishingQuestAcceptQuestRequest();
            case -1464:
               return new WarGameExitRequest();
            case -1352:
               return new FoundClanCommand();
            case -1279:
               return new CommandCenterGetDontShowRequest();
            case -1102:
               return new InvitationAcceptRequest();
            case -1012:
               return new EditClanInfoRequest();
            case -932:
               return new RemoveMineCommand();
            case -826:
               return new EternalGateRankInfoCommand();
            case -807:
               return new AssemblyRecipeStatusUpdateCommand();
            case -805:
               return new QuestGiverWindowClosedRequest();
            case -598:
               return new HangarShipDetailRequest();
            case -576:
               return new MemberLeaveCommand();
            case -548:
               return new SpaceSearchMissionListCommand();
            case -543:
               return new MileageMarketInsufficientResourcesCommand();
            case -451:
               return new ExpeditionInfoCommand();
            case -424:
               return new PublicWorldBossRankReward();
            case -209:
               return new InventoryItemModifyCommand();
            case -131:
               return new PetGearActivationChangeCommand();
            case -100:
               return new AttackHitNoLockCommand();
            case -63:
               return new DispatchCollectRetrieverRequest();
            case -16:
               return new TitleFrenzyScheduleUpdateCommand();
            case 1:
               return new LegacyModule();
            case 2:
               return new KeepAlive();
            case 3:
               return new AttackLaserRunCommand();
            case 4:
               return new AttackLaserRequest();
            case 5:
               return new AttackHitAssetCommand();
            case 6:
               return new AttackRocketCommand();
            case 7:
               return new LoginRequest();
            case 8:
               return new AttributeHitpointUpdateCommand();
            case 9:
               return new AttributeCreditsUpdateCommand();
            case 10:
               return new JumpRequest();
            case 11:
               return new CombatStateCommand();
            case 12:
               return new AmmunitionEmptyCommand();
            case 13:
               return new AmmunitionFallbackCommand();
            case 14:
               return new ReadyRequest();
            case 15:
               return new LogoutCancelRequest();
            case 16:
               return new LogoutCommand();
            case 17:
               return new ShipDeselectionCommand();
            case 18:
               return new JumpDeniedCommand();
            case 19:
               return new JumpInitiatedCommand();
            case 20:
               return new JumpToServerCommand();
            case 21:
               return new ServerChangeCommand();
            case 25:
               return new AttackAbortLaserRequest();
            case 26:
               return new CollectBoxRequest();
            case 27:
               return new AttackTypeModule();
            case 28:
               return new AttackHitCommand();
            case 29:
               return new HeroMoveCommand();
            case 30:
               return new ShipDestroyedCommand();
            case 31:
               return new HealCommand();
            case 32:
               return new ActivationStatedRequest();
            case 33:
               return new ActivationRequest();
            case 34:
               return new DropRequest();
            case 35:
               return new ConfigChangeRequest();
            case 36:
               return new JumpCPUUpdateRequest();
            case 37:
               return new JumpCPUSelectRequest();
            case 38:
               return new AmmunitionTypeModule();
            case 39:
               return new AmmunitionPrizeItem();
            case 40:
               return new AmmunitionPrizeInfoCommand();
            case 41:
               return new AmmunitionBuyRequest();
            case 42:
               return new AmmunitionBuyCommand();
            case 46:
               return new MessageLocalizedCommand();
            case 47:
               return new MessageLocalizedWildcardCommand();
            case 48:
               return new MessageWildcardReplacementModule();
            case 49:
               return new ShipInitializationCommand();
            case 50:
               return new ShipSelectionCommand();
            case 51:
               return new AssetDeselection();
            case 52:
               return new ShipRemoveCommand();
            case 56:
               return new ExtraInfoCommand();
            case 57:
               return new CPUInfoHomeJumpCommand();
            case 58:
               return new CPUInfoAimCommand();
            case 59:
               return new CPUInfoCloakCommand();
            case 60:
               return new CPUInfoAutoRocketCommand();
            case 61:
               return new LoginErrorCommand();
            case 62:
               return new CPUInfoTradeDroneCommand();
            case 63:
               return new JumpCPUSelectCommand();
            case 64:
               return new JumpCPUPowerupStatusCommand();
            case 65:
               return new JumpCPUUpdateCommand();
            case 66:
               return new JumpCPUPriceMappingModule();
            case 67:
               return new AdvertisementViableCommand();
            case 68:
               return new TechTypeModule();
            case 69:
               return new TechActivationRequest();
            case 70:
               return new StatusModule();
            case 71:
               return new TechStatusItemModule();
            case 72:
               return new TechStatusCommand();
            case 73:
               return new TechActivationCommand();
            case 74:
               return new TechDeactivationCommand();
            case 80:
               return new AssetInfoCommand();
            case 81:
               return new TradeRequest();
            case 82:
               return new MoveRequest();
            case 83:
               return new ShipCreateCommand();
            case 84:
               return new SettingSetRequest();
            case 85:
               return new SettingSetCommand();
            case 86:
               return new CPUInfoAutoRocketLauncherCommand();
            case 87:
               return new SettingRemoveRequest();
            case 88:
               return new SettingsOldFlashCommand();
            case 89:
               return new AttackRocketRequest();
            case 90:
               return new MoveCommand();
            case 91:
               return new CooldownReachedCommand();
            case 92:
               return new CooldownTypeModule();
            case 93:
               return new CooldownStartedCommand();
            case 94:
               return new HarvestRequest();
            case 95:
               return new AttributeLevelUpUpdateCommand();
            case 96:
               return new AttributeShipSpeedUpdateCommand();
            case 97:
               return new AttributeSpaceUpdateCommand();
            case 98:
               return new AttributeConfigCountUpdateCommand();
            case 100:
               return new BoosterUpdateModule();
            case 101:
               return new AttributeBoosterUpdateCommand();
            case 102:
               return new AttributeSkillRepairUpdateCommand();
            case 103:
               return new AttributeSkillShieldUpdateCommand();
            case 105:
               return new OreCountModule();
            case 106:
               return new AttributeOreCountUpdateCommand();
            case 107:
               return new AchievementEarnedCommand();
            case 108:
               return new AchievementEndCommand();
            case 112:
               return new UICamLockToHeroCommand();
            case 113:
               return new UICamLockToShipCommand();
            case 114:
               return new UICamLockToCoordinatesCommand();
            case 115:
               return new UIMinimapNoiseCommand();
            case 116:
               return new UIMinimapMarkerCommand();
            case 117:
               return new UIWindowCreateCommand();
            case 118:
               return new UIWindowDestroyCommand();
            case 119:
               return new UIWindowNextPageRequest();
            case 120:
               return new UIArrowCommand();
            case 121:
               return new WindowIDModule();
            case 122:
               return new UIWindowActionCommand();
            case 123:
               return new UIWindowActionModule();
            case 124:
               return new ButtonIDModule();
            case 125:
               return new UIButtonActionModule();
            case 126:
               return new UIButtonActionCommand();
            case 127:
               return new UILayerAdvertisementCommand();
            case 129:
               return new OrePriceModule();
            case 130:
               return new TradeReplyCommand();
            case 131:
               return new TradeSellOreRequest();
            case 132:
               return new LabRefinementRequest();
            case 133:
               return new LabUpdateItemRequest();
            case 134:
               return new LabItemModule();
            case 135:
               return new LabUpdateItemCommand();
            case 136:
               return new UpdateItemModule();
            case 137:
               return new LabUpdateRequest();
            case 138:
               return new MapChangedCommand();
            case 141:
               return new JumpGateTypeModule();
            case 143:
               return new MapEventOreCommand();
            case 146:
               return new AssetTypeModule();
            case 147:
               return new AddOreCommand();
            case 149:
               return new PerformanceTrackingGetCommand();
            case 150:
               return new PerformanceTrackingSendInfoRequest();
            case 151:
               return new DebuggingServerShipPositionCommand();
            case 152:
               return new AlignmentModule();
            case 153:
               return new PetRequest();
            case 154:
               return new PetGearActivationRequest();
            case 155:
               return new PetHeroActivationCommand();
            case 156:
               return new PetDeactivationCommand();
            case 157:
               return new PetStatusCommand();
            case 158:
               return new PetHitpointsUpdateCommand();
            case 159:
               return new PetExperiencePointsUpdateCommand();
            case 160:
               return new PetLevelUpdateCommand();
            case 161:
               return new PetShieldUpdateCommand();
            case 162:
               return new PetFuelUpdateCommand();
            case 163:
               return new AttackMissedCommand();
            case 164:
               return new PetGearTypeModule();
            case 165:
               return new ShipSelectRequest();
            case 166:
               return new PetLocatorGearInitializationCommand();
            case 174:
               return new MessageMapEventCommand();
            case 176:
               return new MessageGameEventCommand();
            case 177:
               return new MapEventAttackedObjectOwnedCommand();
            case 178:
               return new MapEventAttackedObjectReleasedCommand();
            case 179:
               return new MapEventCloakStateCommand();
            case 180:
               return new MapEventDroneInfoCommand();
            case 181:
               return new MapEventEnemyWarningCommand();
            case 182:
               return new ShipSpawnChildSwarmCommand();
            case 183:
               return new MapEventTitleUpdateCommand();
            case 184:
               return new MapEventSmartbombExplosionCommand();
            case 185:
               return new MapEventInstashieldActivationCommand();
            case 186:
               return new MapEventEMPActivationCommand();
            case 187:
               return new BoosterFoundOnMapCommand();
            case 188:
               return new MapEventMapTitleUpdateCommand();
            case 189:
               return new MapEventSetShipAttackableCommand();
            case 190:
               return new SpaceBallInitializeScoreCommand();
            case 191:
               return new SpaceBallUpdateScoreCommand();
            case 192:
               return new SpaceBallUpdateSpeedCommand();
            case 194:
               return new HuntEventRankingPointsUpdateCommand();
            case 197:
               return new HuntEventNotificationCommand();
            case 198:
               return new CTBSetHomeZonesCommand();
            case 199:
               return new LMJackpotLeftShipsCommand();
            case 201:
               return new LMDiscountInfoCommand();
            case 202:
               return new LogMessengerPriorityModule();
            case 203:
               return new LMCollectResourcesCommand();
            case 204:
               return new LMColletJackpotCommand();
            case 205:
               return new LMCollectLogFilesCommand();
            case 206:
               return new LMCollectCreditsCommand();
            case 207:
               return new LMCollectUridiumCommand();
            case 208:
               return new LMCollectExperiencePointsCommand();
            case 209:
               return new LMCollectHonorPointsCommand();
            case 210:
               return new LMCollectAmmunitionCommand();
            case 211:
               return new LMCollectBankingMultiplierCommand();
            case 212:
               return new LMGGEnergyCollectedCommand();
            case 213:
               return new LMItemCollectedCommand();
            case 214:
               return new LMLootCollectedCommand();
            case 215:
               return new LMMineExplodedCommand();
            case 216:
               return new LMBoxCollectionCancelledCommand();
            case 217:
               return new LMInvisibilityDetectorInfoCommand();
            case 218:
               return new CTBScoreUpdateCommand();
            case 219:
               return new CTBMatchStateCommand();
            case 220:
               return new CTBBeaconCapturedCommand();
            case 221:
               return new CTBBeaconReleasedCommand();
            case 222:
               return new CTBBeaconPositionCommand();
            case 223:
               return new InvasionEventInitCommand();
            case 226:
               return new PetOutOfFuelCommand();
            case 227:
               return new PetIsDestroyedCommand();
            case 228:
               return new PetEvasionCommand();
            case 229:
               return new PetInitializationCommand();
            case 230:
               return new PetRepairCompleteCommand();
            case 231:
               return new PetPurchasedCommand();
            case 232:
               return new PetActivationCommand();
            case 233:
               return new PetGearRemoveCommand();
            case 234:
               return new PetGearAddCommand();
            case 235:
               return new PetGearResetCommand();
            case 236:
               return new PetGearSelectCommand();
            case 237:
               return new PetGearLocatorUpdate();
            case 239:
               return new PetBuffCommand();
            case 240:
               return new UIWindowBlockModeCommand();
            case 241:
               return new PetHarvestEffectCommand();
            case 242:
               return new PetIdleModeCommand();
            case 243:
               return new PetLaserAttackStopCommand();
            case 244:
               return new PetUIRepairButtonCommand();
            case 245:
               return new ClientLoggedInCommand();
            case 246:
               return new PetBlockUICommand();
            case 247:
               return new PetTradeWindowShowCommand();
            case 248:
               return new AttributeShieldUpdateCommand();
            case 249:
               return new MessageQueueModule();
            case 250:
               return new CollectionBeamStartCommand();
            case 251:
               return new CollectionBeamStopCommand();
            case 252:
               return new KillScreenPostCommand();
            case 253:
               return new KillScreenOptionModule();
            case 254:
               return new KillScreenOptionTypeModule();
            case 255:
               return new KillScreenRepairRequest();
            case 257:
               return new DroneFormationChangeCommand();
            case 258:
               return new DroneFormationAvailableFormationsCommand();
            case 259:
               return new DestructionTypeModule();
            case 260:
               return new ChannelCloseRequest();
            case 264:
               return new QualitySettingsRequest();
            case 265:
               return new GameplaySettingsRequest();
            case 266:
               return new DisplaySettingsRequest();
            case 267:
               return new AudioSettingsRequest();
            case 268:
               return new WindowSettingsRequest();
            case 269:
               return new QualitySettingsModule();
            case 270:
               return new DisplaySettingsModule();
            case 271:
               return new AudioSettingsModule();
            case 272:
               return new WindowSettingsModule();
            case 273:
               return new GameplaySettingsModule();
            case 275:
               return new AbilityEffectActivationCommand();
            case 276:
               return new AbilityStatusSingleCommand();
            case 277:
               return new AbilityStatusFullCommand();
            case 278:
               return new AbilityStopCommand();
            case 279:
               return new AbilityStartCommand();
            case 280:
               return new VisualModifierCommand();
            case 281:
               return new MarkTargetCommand();
            case 282:
               return new AbilityEffectDeActivationCommand();
            case 287:
               return new HellstormAttackCommand();
            case 288:
               return new ShipExtensionsCommand();
            case 290:
               return new ShipWarpRequest();
            case 291:
               return new ShipWarpWindowCommand();
            case 293:
               return new ShipWarpModule();
            case 333:
               return new TieredArenaEventTierUpdateCommand();
            case 340:
               return new UIWindowSettingsCommand();
            case 341:
               return new UIWindowSettingsRequest();
            case 342:
               return new UIWindowSettingModule();
            case 343:
               return new ResetSettingsRequest();
            case 439:
               return new DispatchGetGateListRequest();
            case 468:
               return new SpacePlagueBossSummoningProgressCommand();
            case 501:
               return new PlayerUpdateCommand();
            case 520:
               return new SectorControlMatchOverviewRequest();
            case 521:
               return new SectorControlMatchOverviewCommand();
            case 523:
               return new SectorControlMatchOverviewModule();
            case 524:
               return new SectorControlMatchDetailRequest();
            case 525:
               return new SectorControlMatchJoinRequest();
            case 526:
               return new GalaxyGateBuilderInfoCommand();
            case 527:
               return new SectorControlMatchDetailCommand();
            case 542:
               return new KillUiCloseMessageCommand();
            case 602:
               return new DiminishingQuestInitializationCommand();
            case 666:
               return new VersionRequest();
            case 667:
               return new VersionCommand();
            case 683:
               return new ArenaMatchResultModule();
            case 692:
               return new ClanRelationChangedCommand();
            case 696:
               return new SeasonPassCaptainLimitCommand();
            case 774:
               return new RogueLiteInfoCommand();
            case 800:
               return new UserKeyBindingsModule();
            case 801:
               return new UserKeyBindingsUpdate();
            case 814:
               return new AuctionBidCommand();
            case 833:
               return new NpcEventTimeInfoCommand();
            case 876:
               return new TrainingGroundRemainingTimeToTimeoutCommand();
            case 953:
               return new ContactFriendAcceptTipsCommand();
            case 1001:
               return new ShipWarpCompletedCommand();
            case 1002:
               return new ShipWarpNotAllowedCommand();
            case 1010:
               return new ShipWarpCanceledCommand();
            case 1011:
               return new ShipWarpWindowCloseCommand();
            case 1150:
               return new StarmapStationCommand();
            case 1151:
               return new StarmapStationInfo();
            case 1198:
               return new UpdateNamFIagRequest();
            case 1300:
               return new HitpointInfoCommand();
            case 1311:
               return new BoostedAttributeTypeModule();
            case 1313:
               return new AuctionHistoryCommand();
            case 1377:
               return new ContactBaseInfoCommand();
            case 1392:
               return new AutopilotInfoCommand();
            case 1439:
               return new ClanReportAdminCommand();
            case 1446:
               return new SkylabActiveRobotsLeftTimeCommand();
            case 1485:
               return new UpLevelSkillTreeCommand();
            case 1486:
               return new TrackIntVariableModule();
            case 1520:
               return new TeamDeathMatchOverviewInitRequest();
            case 1521:
               return new TeamDeathMatchOverviewCommand();
            case 1523:
               return new TeamDeathMatchOverviewModule();
            case 1524:
               return new TeamDeathMatchDetailRequest();
            case 1525:
               return new TeamDeathMatchJoinRequest();
            case 1527:
               return new TeamDeathMatchDetailCommand();
            case 1610:
               return new BattlePassKeyExchangeRewardModule();
            case 1635:
               return new ContactListEntryTypeModule();
            case 1667:
               return new TeamDeathMatchStatusUpdateCommand();
            case 1685:
               return new ClanApplyInfoListRequest();
            case 1707:
               return new HintWindowListInitCommand();
            case 1724:
               return new TeamDeathMatchResultScreenPlayerModule();
            case 1725:
               return new RewardModule();
            case 1727:
               return new NotifyNewDiploReqCommand();
            case 1744:
               return new ArmoryUnlockLootShowListCommand();
            case 1748:
               return new MiniClientRewardClaimRequest();
            case 1844:
               return new AssemblyRecipeBuildStatusModule();
            case 1855:
               return new UserMessageNoteListRequest();
            case 1869:
               return new AchievementTitleCommand();
            case 1922:
               return new OreTypeModule();
            case 1947:
               return new EternalGateSelectionCommand();
            case 2000:
               return new QuestCaseModule();
            case 2001:
               return new QuestElementModule();
            case 2002:
               return new QuestConditionModule();
            case 2004:
               return new QuestCompletedCommand();
            case 2005:
               return new QuestFailedCommand();
            case 2006:
               return new QuestCancelledCommand();
            case 2014:
               return new QuestConditionUpdateCommand();
            case 2016:
               return new LootModule();
            case 2017:
               return new QuestInitializationCommand();
            case 2019:
               return new TrainingArenaPerformanceEntryCommand();
            case 2022:
               return new QuestPrivilegeCommand();
            case 2023:
               return new QuestPrivilegeRequest();
            case 2024:
               return new QuestCancelRequest();
            case 2035:
               return new QuestAcceptRequest();
            case 2037:
               return new QuestConditionStateModule();
            case 2073:
               return new QuestInfoRequest();
            case 2074:
               return new QuestListRequest();
            case 2075:
               return new QuestListCommand();
            case 2078:
               return new QuestSlimInfoModule();
            case 2085:
               return new QuestGiverModule();
            case 2090:
               return new QuestInfoCommand();
            case 2099:
               return new QuestDefinitionModule();
            case 2100:
               return new QuestIconModule();
            case 2103:
               return new SynthesisGroupCommand();
            case 2210:
               return new ItemCommand();
            case 2211:
               return new EventActivationStateCommand();
            case 2270:
               return new VideoWindowCreateCommand();
            case 2271:
               return new VideoWindowNextPageCommand();
            case 2272:
               return new VideoWindowRemoveCommand();
            case 2280:
               return new CameraLockToCoordinatesCommand();
            case 2281:
               return new CameraLockToHeroCommand();
            case 2282:
               return new CameraLockToShipCommand();
            case 2345:
               return new QuestGiversAvailableCommand();
            case 2414:
               return new GalaxyGateBuilderGateCompleteCommand();
            case 2518:
               return new OreTradeInfoCommand();
            case 2521:
               return new TeamDeathMatchOverviewInitCommand();
            case 2554:
               return new EternalGateSkillPointInfoCommand();
            case 2679:
               return new BattlePassKeyExchangeInfoCommand();
            case 2698:
               return new WorldBossInfoUpdateCommand();
            case 2729:
               return new EventActivationAttributeModule();
            case 2748:
               return new AchievementListRequest();
            case 3115:
               return new ContactFriendInfoCommand();
            case 3163:
               return new ActivePerkUpdateCommand();
            case 3245:
               return new AssetRemoveCommand();
            case 3260:
               return new MapAssetAddBillboardCommand();
            case 3261:
               return new PartnerTypeModule();
            case 3331:
               return new EternalBlacklightChooseOptionRequest();
            case 3333:
               return new GroupInvitationRequest();
            case 3336:
               return new ClanMemberModule();
            case 3350:
               return new QuestGiverApproachedRequest();
            case 3366:
               return new QuestRequirementModule();
            case 3389:
               return new JumpgateCreateCommand();
            case 3420:
               return new DeleteMyRankCommand();
            case 3430:
               return new AssetInformationModule();
            case 3530:
               return new POIVisualModifierCommand();
            case 3563:
               return new SynthesisInventoryRequest();
            case 3621:
               return new UserMessageSystemInfoCommand();
            case 3651:
               return new ContactListUpdateContactsCommand();
            case 3719:
               return new LoginInitClanCommand();
            case 3787:
               return new EternalBlacklightRankInfoCommand();
            case 3807:
               return new TrainingGroundSummingUpStatusCommand();
            case 3836:
               return new CaptainActionRequest();
            case 4021:
               return new ArmoryUnlockLootShowCommand();
            case 4048:
               return new JackpotArenaMatchResultCommand();
            case 4061:
               return new DeleteClanMemberCommand();
            case 4079:
               return new DeleteClanApplyCommand();
            case 4141:
               return new QuestTypeModule();
            case 4216:
               return new ShopItemBuyRequest();
            case 4230:
               return new GalaxyGateBuilderBatteryItemModule();
            case 4238:
               return new EventUpdateCommand();
            case 4274:
               return new RankRewardRequest();
            case 4302:
               return new MapAssetActivationRequest();
            case 4321:
               return new QuestChallengeRatingModule();
            case 4345:
               return new AssemblyBuildRequest();
            case 4441:
               return new EternalBlacklightWaveInfoCommand();
            case 4444:
               return new ClanTagChangedCommand();
            case 4656:
               return new DispatchGetRetrieverListRequest();
            case 4712:
               return new InfiltrationStatusCommand();
            case 4713:
               return new CountdownStatusCommand();
            case 4714:
               return new CountdownStatusTypeModule();
            case 4715:
               return new CountdownStatusCloseWindowCommand();
            case 4740:
               return new UIItemRemoveHighlightCommand();
            case 4840:
               return new ExpeditionResetRequest();
            case 4844:
               return new ActivePerkModule();
            case 4853:
               return new BeaconCommand();
            case 4929:
               return new SynthesisInventoryCommand();
            case 5040:
               return new PublicWorldBossRankCommand();
            case 5061:
               return new MonthlyDeluxeRewardModule();
            case 5121:
               return new SkylabXenoInfoCommand();
            case 5181:
               return new AchievementValueCommand();
            case 5255:
               return new SectorControlMatchLeaveQueueRequest();
            case 5262:
               return new EpicNumberedFightMessageTypeModule();
            case 5330:
               return new UserSettingsCommand();
            case 5394:
               return new UserMessageNewCommand();
            case 5447:
               return new MarkRangeCommand();
            case 5474:
               return new MemberDestructionCommand();
            case 5522:
               return new UnstableModuleRestabilizeResultCommand();
            case 5546:
               return new ExpeditionJumpLevelInfoCommand();
            case 5550:
               return new TeamDeathMatchExitRequest();
            case 5575:
               return new ClanMemberInfoCommand();
            case 5613:
               return new AddFireworkBoxCommand();
            case 5687:
               return new EventActivationIntegerAttributeModule();
            case 5866:
               return new ReqDroneBoostInfo();
            case 5868:
               return new UserInventoryModule();
            case 5871:
               return new DispatchGetRetrieverListCommand();
            case 5932:
               return new HangarShipRepairModuleCommand();
            case 6001:
               return new AvailableModulesCommand();
            case 6002:
               return new AsteroidProgressCommand();
            case 6003:
               return new BattleStationBuildingStateCommand();
            case 6004:
               return new BattleStationManagementUiInitializationCommand();
            case 6005:
               return new BattleStationStatusCommand();
            case 6006:
               return new BuildStationRequest();
            case 6007:
               return new CompanyHierarchyInitializationCommand();
            case 6008:
               return new CompanyHierarchyRankingModule();
            case 6009:
               return new CompanyHierarchyRequest();
            case 6010:
               return new DeflectorShieldStatusChangeRequest();
            case 6011:
               return new EmergencyRepairRequest();
            case 6012:
               return new BattleStationErrorCommand();
            case 6013:
               return new EquipModuleRequest();
            case 6014:
               return new EquippedModulesModule();
            case 6015:
               return new OutOfBattleStationRangeCommand();
            case 6016:
               return new BattleStationBuildingUiInitializationCommand();
            case 6017:
               return new StationModuleModule();
            case 6018:
               return new UnequipModuleRequest();
            case 6029:
               return new GalaxyGateBuilderMultiplierItemModule();
            case 6030:
               return new BattleStationNoClanUiInitializationCommand();
            case 6039:
               return new StrikeAndExitInfoRequest();
            case 6048:
               return new HangarShipActivateShipRequest();
            case 6087:
               return new CaptchaCollectRequest();
            case 6096:
               return new InvitationCancelRequest();
            case 6134:
               return new DispatchGateModule();
            case 6206:
               return new EventReturnOfHeroContributorListCommand();
            case 6277:
               return new WordPuzzleLetterAchievedCommand();
            case 6278:
               return new WordPuzzleWindowInitCommand();
            case 6279:
               return new WordPuzzleLetterModule();
            case 6337:
               return new TrainingGroundConfirmationStatusCommand();
            case 6462:
               return new CCListRangeModule();
            case 6521:
               return new SectorControlMatchExitRequest();
            case 6526:
               return new ExpeditionStartInstanceCommand();
            case 6530:
               return new MapAssetActionAvailableCommand();
            case 6599:
               return new ClanBattleStationInfoCommand();
            case 6619:
               return new ArenaStatusCommand();
            case 6667:
               return new SectorControlMatchStateInfoCommand();
            case 6668:
               return new SectorControlBonusCommand();
            case 6669:
               return new SectorControlTicketCountCommand();
            case 6671:
               return new PlayerQueueRegistrationCommand();
            case 6672:
               return new PlayerQueueAlreadyRegisteredCommand();
            case 6674:
               return new POITypeModule();
            case 6675:
               return new POIDesignModule();
            case 6676:
               return new MapRemovePOICommand();
            case 6677:
               return new POIReadyCommand();
            case 6678:
               return new SectorControlCaptureParticipationCommand();
            case 6777:
               return new KillScreenUpdateCommand();
            case 6807:
               return new HighlightTypeModule();
            case 6832:
               return new AgatusEventTimeInfoCommand();
            case 6851:
               return new MileageMarketEventBuyRequest();
            case 6951:
               return new PVPSummerEventShuffleInfoCommand();
            case 7002:
               return new ModuleStatusUpdateCommand();
            case 7010:
               return new ClanRelationModule();
            case 7036:
               return new PlutusInfoCommand();
            case 7123:
               return new SectorControlErrorCommand();
            case 7124:
               return new SectorControlDraftConfirmationCommand();
            case 7125:
               return new SectorControlDraftConfirmationRequest();
            case 7143:
               return new UserMessageNoteInfoCommand();
            case 7199:
               return new TeamDeathMatchDraftConfirmationRequest();
            case 7217:
               return new SkillTreeListCommand();
            case 7227:
               return new ContactListCommand();
            case 7361:
               return new UpdateSpaceSearchKeyCommand();
            case 7441:
               return new SkylabViewModuleRequest();
            case 7501:
               return new StrikeAndExitInfoCommand();
            case 7513:
               return new LevelUpCommand();
            case 7770:
               return new SectorControlPlayerCountModule();
            case 7779:
               return new HealBeamCommand();
            case 7789:
               return new AccountStatusChangeCommand();
            case 7956:
               return new ChangeClanLeaderCommand();
            case 8024:
               return new MenuItemStatusCommand();
            case 8096:
               return new LoginInitMyApplyCommand();
            case 8113:
               return new LogbookDataListRequest();
            case 8145:
               return new HangarShipRepairDroneRequest();
            case 8169:
               return new CommandCenterDontShowInfoCommand();
            case 8307:
               return new GetNamFIagRequest();
            case 8372:
               return new EternalGateChooseOptionRequest();
            case 8375:
               return new InternalPilotBioUserProfileCommand();
            case 8508:
               return new LoginInitClanRequest();
            case 8511:
               return new TrinityTrialsRewardDescriptionCommand();
            case 8628:
               return new ExpeditionSelectShipRequest();
            case 8670:
               return new MenuItemTimerCommand();
            case 8778:
               return new GalaxyGateBuilderGateWaveUpdateCommand();
            case 8782:
               return new ClanSendAllMessageCommand();
            case 8847:
               return new EventANewHopeStatusCommand();
            case 8936:
               return new DispatchSlotInfoCommand();
            case 9131:
               return new TeamDeathMatchErrorCommand();
            case 9219:
               return new TrainingGroundRegistrationUnavailableStatusCommand();
            case 9246:
               return new UIWindowUpdateRequest();
            case 9288:
               return new ShipIconModule();
            case 9343:
               return new ClanDeleteOpenDiploRequest();
            case 9413:
               return new EventANewHopeQuestCommand();
            case 9603:
               return new UIButtonTriggerRequest();
            case 9604:
               return new MileageMarketBuyCommand();
            case 9608:
               return new SeasonPassRewardUnclaimedInfoCommand();
            case 9624:
               return new SectorControlSectorStateCommand();
            case 9667:
               return new FactionScoreModule();
            case 9701:
               return new TrinityTrialsDropLevelOptionalRequest();
            case 9767:
               return new HintWindowClosedRequest();
            case 9769:
               return new AssemblyRecipeSelectRequest();
            case 9785:
               return new TrinityTrialsDropLevelOptionalCommand();
            case 9797:
               return new BattlePassKeyExchangeLootModule();
            case 9805:
               return new WorldBossRankingInfoCommand();
            case 9868:
               return new BoosterDetailModule();
            case 9874:
               return new ClanNewsInfoCommand();
            case 9903:
               return new MenuItemInitializationCommand();
            case 9977:
               return new ClanDiploInfoCommand();
            case 9990:
               return new InvitationModeModule();
            case 9999:
               return new FactionModule();
            case 10041:
               return new ClanDeleteMessageRequest();
            case 10101:
               return new QuestAcceptabilityStatusModule();
            case 10116:
               return new PlayVisualEffectCommand();
            case 10240:
               return new ClanFilterSearchRequest();
            case 10249:
               return new ClanSendAllMessageRequest();
            case 10253:
               return new TrackVariableRequest();
            case 10325:
               return new ContactListEntryAttributeMapModule();
            case 10342:
               return new ClanRankListCommand();
            case 10505:
               return new BlackListGetRequest();
            case 10599:
               return new AddBeaconCommand();
            case 10637:
               return new ClanMessageListCommand();
            case 10642:
               return new AssemblyRecipeRemoveCommand();
            case 10831:
               return new CloakDeviceActivityModule();
            case 10836:
               return new WorldBossRankingInfoModule();
            case 10910:
               return new UserMessageParentInfoCommand();
            case 10973:
               return new HangarShipUnlockSlotCommand();
            case 11031:
               return new CaptainCommand();
            case 11091:
               return new TrainingGroundStatusDataModule();
            case 11273:
               return new ExchangesShopItem();
            case 11313:
               return new GroupInitializationCommand();
            case 11797:
               return new TeamDeathMatchResultScreenCommand();
            case 11836:
               return new CCInitCommand();
            case 11902:
               return new NpcUndockCommand();
            case 11922:
               return new RogueLiteCashOutCommand();
            case 12121:
               return new QuestListRemoveCommand();
            case 12122:
               return new QuestListUpdateCommand();
            case 12209:
               return new UridiumBankClaimCommand();
            case 12255:
               return new DeleteClanCommand();
            case 12265:
               return new MiniClientRewardDayInfoCommand();
            case 12422:
               return new ExpeditionSelectedShipListCommand();
            case 12439:
               return new BattlePassCollectRequest();
            case 12443:
               return new ContactListEntryAttributeClanRelationModule();
            case 12585:
               return new AssemblyRecipeUnavailableStatusModule();
            case 12696:
               return new RogueLiteInventoryCommand();
            case 12745:
               return new LootProbabilityModule();
            case 12830:
               return new BattlePassSetFilterRequest();
            case 12846:
               return new HangarShipMoveItemsRequest();
            case 12888:
               return new InvitationModeRequest();
            case 12910:
               return new FrozenLabyrinthTimeInfoCommand();
            case 12966:
               return new UnstableModuleModule();
            case 13023:
               return new DailyLoginClaimTodayRequest();
            case 13027:
               return new TrinityTrialsDropLevelCommand();
            case 13061:
               return new CCIntValueModule();
            case 13123:
               return new ProActionBarOpenStateChangeRequest();
            case 13131:
               return new PriceModule();
            case 13172:
               return new DeleteMyRankRequest();
            case 13231:
               return new HangarShipEquipmentCommand();
            case 13255:
               return new TeamDeathMatchLeaveQueueRequest();
            case 13259:
               return new SetBonusInfo();
            case 13334:
               return new PayloadEscortInfoCommand();
            case 13435:
               return new InventoryItemSellPack();
            case 13464:
               return new MiniClientItemRewardModule();
            case 13495:
               return new TrainingGroundRoundInfoCommand();
            case 13523:
               return new FollowRequest();
            case 13731:
               return new TrainingGroundRegistrationStatusCommand();
            case 13769:
               return new SkylabMainViewModuleCommand();
            case 13845:
               return new SpotLightPositionCommand();
            case 13994:
               return new DeleteClanRequest();
            case 14122:
               return new EventLootShowCommand();
            case 14136:
               return new InstantRepairAssetInformationModule();
            case 14232:
               return new Curcubitor2019Command();
            case 14249:
               return new ClanAllInfoCommand();
            case 14332:
               return new ContactListEntryTypeCommand();
            case 14484:
               return new UserMessageAuctionInfoCommand();
            case 14490:
               return new LogbookDataCommand();
            case 14534:
               return new SynthesisCostLootCommand();
            case 14647:
               return new AssemblySkipRequest();
            case 14665:
               return new GalaxyGateBuilderLootItemModule();
            case 14768:
               return new RzoneConfirmationStatusModule();
            case 14824:
               return new RerollWindowSettingsRequest();
            case 15017:
               return new FrozenLabyrinthRankInfoCommand();
            case 15126:
               return new ContactListBlockInvitationsModule();
            case 15182:
               return new ExpeditionSelectShipCommand();
            case 15186:
               return new SkylabCollectorInfoCommand();
            case 15201:
               return new SeasonPassLevelPurchaseRequest();
            case 15216:
               return new ContactListBlockInvitationsTipsCommand();
            case 15229:
               return new SpaceSearchEventsCommand();
            case 15361:
               return new SeasonPassRewardModule();
            case 15405:
               return new EndEventSummaryScoreCommand();
            case 15429:
               return new VIPMerchantExchangeInsufficientResourcesCommand();
            case 15453:
               return new MonthlyDeluxeTimeInfoCommand();
            case 15627:
               return new MonthlyDeluxeClaimRequest();
            case 15686:
               return new HangarShipSellItemRequest();
            case 15735:
               return new UserMessageInboxListRequest();
            case 15793:
               return new HangarShipFavouriteRequest();
            case 15888:
               return new PetHeatUpdateCommand();
            case 15937:
               return new AutopilotInfoSaveCommand();
            case 16059:
               return new RzoneLeaveMapRequest();
            case 16063:
               return new CannonAttackCommand();
            case 16078:
               return new AssemblyWindowSettingsModule();
            case 16088:
               return new GroupCloseCommand();
            case 16248:
               return new RogueLiteGateRewardModule();
            case 16278:
               return new ClanDeleteMessageCommand();
            case 16441:
               return new HangarPetEquipmentMsg();
            case 16460:
               return new HangarShipSellDroneCommand();
            case 16462:
               return new AutopilotInfoSaveRequest();
            case 16652:
               return new LogBoxInformationRequest();
            case 16709:
               return new HallOfFamePagesRequest();
            case 16811:
               return new Birthday2025ClaimRequest();
            case 16931:
               return new PublicWorldBossInfoCommand();
            case 17092:
               return new PageRefreshCommand();
            case 17102:
               return new UserMessageInfoCommand();
            case 17251:
               return new QuantumFluidCoreRequest();
            case 17470:
               return new LocationModule();
            case 17504:
               return new CannonUnstableModuleModule();
            case 17570:
               return new ContactListEntryAttributeLevelModule();
            case 17638:
               return new UIFeatureBarCommand();
            case 17795:
               return new CCInitRequest();
            case 17826:
               return new EventCommand();
            case 17847:
               return new EventActivationMapExchangeAttributeModule();
            case 17967:
               return new HangarShipBriefCommand();
            case 18003:
               return new ShipDesignPack();
            case 18051:
               return new ClanModule();
            case 18263:
               return new UserMessageInitRequest();
            case 18275:
               return new WarGameInitLobbyCommand();
            case 18354:
               return new ClanNewDiplomacyCommand();
            case 18395:
               return new DispatchGetGateListCommand();
            case 18403:
               return new RogueLiteScoreInfoCommand();
            case 18404:
               return new RogueLiteRankInfoCommand();
            case 18435:
               return new ClanMemberListRequest();
            case 18619:
               return new UserMessageLootCommand();
            case 18643:
               return new UserMessageNoteDeleteRequest();
            case 18867:
               return new FrozenLabyrinthSynkInfoCommand();
            case 18906:
               return new HangarShipAutoBuyCpuAmmoCommand();
            case 18952:
               return new LotteryLogRewardCommand();
            case 19136:
               return new ExpeditionInfoRequest();
            case 19165:
               return new HallOfFameRankingPoints();
            case 19201:
               return new DisassemblyRequest();
            case 19206:
               return new LoginResponse();
            case 19259:
               return new ClanRankListRequest();
            case 19300:
               return new ShowClanInfoCommand();
            case 19309:
               return new SpecialEventPaymentActorCommand();
            case 19324:
               return new ContactListRemoveContactsCommand();
            case 19360:
               return new Birthday2025GateWindowCommand();
            case 19521:
               return new TeamDeathMatchDraftConfirmationCommand();
            case 19588:
               return new EternalGateRankModule();
            case 19647:
               return new HealthModule();
            case 19699:
               return new SeasonPassRefreshCountCommand();
            case 19781:
               return new HangarShipMoveItemsCommand();
            case 19846:
               return new DesignSetUpdateCommand();
            case 19876:
               return new CCValueModule();
            case 19909:
               return new DispatchRewardModule();
            case 20025:
               return new ClanNewsListCommand();
            case 20034:
               return new CCCooldownValueModule();
            case 20041:
               return new ApplyAcceptCommand();
            case 20153:
               return new HangarShipUnlockSlotRequest();
            case 20292:
               return new EternalGateBoosterCommand();
            case 20353:
               return new HangarShipEquipmentMsg();
            case 20385:
               return new ClanApplyInfoCommand();
            case 20532:
               return new TrainingGroundPreparingForFightStatusCommand();
            case 20584:
               return new HuntInfoCommand();
            case 20585:
               return new DispatchInstantCompleteRequest();
            case 20609:
               return new MenuInitializationCommand();
            case 20628:
               return new PingCommand();
            case 20675:
               return new EventReturnOfHeroActionRequest();
            case 20697:
               return new AssemblyRecipeContentModule();
            case 20709:
               return new InventoryItemPack();
            case 20733:
               return new ClanRankInfoCommand();
            case 20740:
               return new NormalTipsCommand();
            case 20777:
               return new CCItemValueModule();
            case 20872:
               return new BoosterSharedDetailModule();
            case 20937:
               return new AssemblyRecipeAvailableStatusModule();
            case 21112:
               return new InternalPilotBioUpdateUserNameRequest();
            case 21203:
               return new GalaxyGateBuilderGateLifeLostCommand();
            case 21260:
               return new ArenaMatchEndRequest();
            case 21289:
               return new BattleStationHistoryListRequest();
            case 21324:
               return new ShowClanInfoRequest();
            case 21365:
               return new BlackListGetCommand();
            case 21427:
               return new RogueLiteSelectRewardRequest();
            case 21467:
               return new ContactUiUnblockCommand();
            case 21507:
               return new HangarShipsCommand();
            case 21639:
               return new EvoucherHistoryListCommand();
            case 21713:
               return new ArmoryUnlockInfoRequest();
            case 21797:
               return new SeasonPassRewardClaimAllRequest();
            case 21903:
               return new InvitationBlockModule();
            case 21929:
               return new EmoteActivationCommand();
            case 21941:
               return new HangarDroneEquipmentMsg();
            case 21957:
               return new AssetCreateCommand();
            case 21966:
               return new PayoutClanCommand();
            case 22059:
               return new ReturneeLoginInitialRewardClaimableInfoCommand();
            case 22080:
               return new RzoneStatusModule();
            case 22123:
               return new ClanDeleteOpenDiploCommand();
            case 22256:
               return new HangarShipRepairDroneCommand();
            case 22305:
               return new EternalBlacklightInfoCommand();
            case 22388:
               return new GalaxyGateBuilderInfoRequest();
            case 22424:
               return new AuctionListCommand();
            case 22480:
               return new BlackListDeleteCommand();
            case 22557:
               return new RzoneLeavingStatusModule();
            case 22603:
               return new LotteryLogCommand();
            case 22707:
               return new CreateMinimapMark();
            case 22718:
               return new UserMessageInfoInfoCommand();
            case 22731:
               return new ReturneeLoginInitialRewardInfoCommand();
            case 22788:
               return new ConvertResearchPointsCommand();
            case 22989:
               return new HangarShipChangeShipModelRequest();
            case 22991:
               return new ClearClanCommand();
            case 23191:
               return new DailyLoginClaimTodayCommand();
            case 23245:
               return new GroupMemberSelectRequest();
            case 23306:
               return new ContactListEntryModule();
            case 23463:
               return new CCBooleanRangeModule();
            case 23473:
               return new TieredArenaEventReputationUpdateCommand();
            case 23502:
               return new JoinClanCommand();
            case 23605:
               return new ChangeClanTaxRequest();
            case 23695:
               return new TrainingGroundConfirmRequest();
            case 23703:
               return new EditClanInfoCommand();
            case 23749:
               return new BattlePassInfoCommand();
            case 23869:
               return new HangarShipRenamePetRequest();
            case 24126:
               return new UserMessageSendRequest();
            case 24150:
               return new CommandCenterUpdateDontShowRequest();
            case 24222:
               return new ReplacementTypeModule();
            case 24234:
               return new MiniClientRewardInfoCommand();
            case 24249:
               return new CaptainUpdateActiveBackCommand();
            case 24289:
               return new RogueLiteShipModule();
            case 24301:
               return new TdmResultCommand();
            case 24357:
               return new EpicFightMessageCommand();
            case 24391:
               return new RogueLiteDisableEquipCommand();
            case 24499:
               return new EventPaymentActorCommand();
            case 24582:
               return new MapAssetIconRemoveHighlightCommand();
            case 24624:
               return new ClanBattleStationInfoRequest();
            case 24694:
               return new SlotBarInitializationCommand();
            case 24695:
               return new TupleThreeModule();
            case 24737:
               return new DispatchLootMessageCommand();
            case 24750:
               return new HangarShipChangeShipModelCommand();
            case 24781:
               return new EditClanMemberCommand();
            case 24881:
               return new MenuItemToolTipReplacementCommand();
            case 25136:
               return new GalaxyGateBuilderSetupGateRequest();
            case 25150:
               return new PayloadEscortScoreCommand();
            case 25159:
               return new Birthday2025PointRewardCommand();
            case 25190:
               return new BlackListDeleteRequest();
            case 25192:
               return new SeasonPassStatusCommand();
            case 25202:
               return new HangarShipActivateShipCommand();
            case 25218:
               return new StrikeAndExitMapInfoCommand();
            case 25220:
               return new ClanReportAdminRequest();
            case 25315:
               return new DiminishingQuestAcceptQuestResponse();
            case 25432:
               return new ClanAddNewsRequest();
            case 25558:
               return new MapAddPOICommand();
            case 25687:
               return new SpacePlagueInfectionStatusCommand();
            case 25758:
               return new ActivationZoneStatusCommand();
            case 25766:
               return new UserMessageUpdateStatusCommand();
            case 25778:
               return new EternalBlacklightSkillPointInfoCommand();
            case 25996:
               return new UpdateEventSummaryScoreCommand();
            case 26109:
               return new HallOfFamePages();
            case 26137:
               return new ContactListInitializationCommand();
            case 26313:
               return new PayoutClanRequest();
            case 26335:
               return new HuntRewardRequest();
            case 26553:
               return new ReturneeLoginBundleBuyCommand();
            case 26633:
               return new ChangeClanDescriptionCommand();
            case 26865:
               return new RzoneSignUpRequest();
            case 26884:
               return new ShipDesignAddedCommand();
            case 26896:
               return new UserMessageDeleteCommand();
            case 26992:
               return new GroupInvitationBlockToggleRequest();
            case 27021:
               return new ExpeditionBuyTicketCommand();
            case 27077:
               return new LeaveGroupRequest();
            case 27145:
               return new BaseStackCommand();
            case 27191:
               return new SkylabViewModuleBackCommand();
            case 27304:
               return new TieredArenaEventInfoCommand();
            case 27357:
               return new RankRewardRangeModule();
            case 27456:
               return new InvitationRejectRequest();
            case 27501:
               return new RemoveMinimapMark();
            case 27616:
               return new RankModule();
            case 27702:
               return new HangarShipSaveConfigCommand();
            case 27833:
               return new HuntRewardCommand();
            case 27915:
               return new ClanMessageInfoCommand();
            case 27934:
               return new CCNumericRangeModule();
            case 27954:
               return new PopupErrorMessageCommand();
            case 27964:
               return new UILayoutCommand();
            case 28076:
               return new UserMessageLootRequest();
            case 28085:
               return new EpicFightMessageModule();
            case 28091:
               return new SpaceSearchRequest();
            case 28132:
               return new EvoucherHistoryInfoCommand();
            case 28220:
               return new RzoneStatusCommand();
            case 28277:
               return new BattlePassWindowSettingsModule();
            case 28278:
               return new WorldBossRankingRewardsInfoCommand();
            case 28347:
               return new ClanApplyInfoListCommand();
            case 28469:
               return new ClanTaxLogCommand();
            case 28487:
               return new UpdateNamFIagCommand();
            case 28548:
               return new RogueLiteRankEventInfoCommand();
            case 28595:
               return new UiItemSourceModule();
            case 28603:
               return new DispatchSendGateRequest();
            case 28654:
               return new AuctionTestRequest();
            case 28769:
               return new PayloadEscortKeyInfoCommand();
            case 28825:
               return new WarGameTeamCountUpdateCommand();
            case 28901:
               return new PingPositionRequest();
            case 28959:
               return new TrinityTrialsGateWindowCommand();
            case 28963:
               return new GalaxyGateBuilderSetupGateCommand();
            case 28986:
               return new InternalPilotBioRequest();
            case 28990:
               return new StopCapsuleCollectCommand();
            case 28996:
               return new AddMineCommand();
            case 29123:
               return new HangarShipSaveConfigRequest();
            case 29135:
               return new PayloadEscortTimeInfoCommand();
            case 29178:
               return new AchievementListCommand();
            case 29197:
               return new ClanSendMessageCommand();
            case 29257:
               return new AuctionQuickBuyRequest();
            case 29281:
               return new AddMapEntityCommand();
            case 29336:
               return new ArgStackCommand();
            case 29382:
               return new CCDateValueModule();
            case 29446:
               return new GalaxyGateBuilderSaveSpinAmountRequest();
            case 29463:
               return new ContactListEntryAttributeCommand();
            case 29545:
               return new WorldBossRewardModule();
            case 29550:
               return new ClanListCommand();
            case 29584:
               return new LotteryRequest();
            case 29653:
               return new BattlePassKeyExchangeUpdateCommand();
            case 29682:
               return new ShopLabelPrompt();
            case 29698:
               return new UserMessageNoteAddCommand();
            case 29710:
               return new EventANewHopeContributorListCommand();
            case 29725:
               return new ReturneeLoginBundleInfoCommand();
            case 29734:
               return new TrinityTrialsUIRequest();
            case 29766:
               return new CCAbstractRangeModule();
            case 29890:
               return new ActivityModule();
            case 29950:
               return new AuctionTestCommand();
            case 30200:
               return new MessageKeyListWildcardCommand();
            case 30297:
               return new SynthesisItemRequest();
            case 30333:
               return new InternalPilotBioUpdateRequest();
            case 30350:
               return new ArmoryUnlockDrawRequest();
            case 30361:
               return new ExpeditionJumpTargetLevelCommand();
            case 30389:
               return new PlutusStatusCommand();
            case 30463:
               return new VIPMerchantExchangeItemModule();
            case 30499:
               return new EnergyUpdateCommand();
            case 30608:
               return new EventModule();
            case 30630:
               return new CCRequestAttributeModule();
            case 30664:
               return new DispatchSendRetrieverRequest();
            case 30744:
               return new ClanNewDiplomacyRequest();
            case 30763:
               return new HangarShipRepairModuleRequest();
            case 30768:
               return new DroneRemovedCommand();
            case 30783:
               return new AssemblyRecipeModule();
            case 30858:
               return new DeleteClanApplyRequest();
            case 30977:
               return new SkylabPurchaseRobotRequest();
            case 30988:
               return new TrainingGroundPlayerCombatStatusInfoCommand();
            case 31049:
               return new MileageMarketInfoUpdateCommand();
            case 31110:
               return new ContactInvitationInfoCommand();
            case 31140:
               return new CaptchaResolvedCommand();
            case 31152:
               return new UnstableModulesShardCommand();
            case 31259:
               return new BattlePassUpdateStatusCommand();
            case 31260:
               return new PublicWorldBossRaidCommand();
            case 31292:
               return new GalaxyGateBuilderPartItemModule();
            case 31335:
               return new RogueLiteDroneModule();
            case 31383:
               return new AssemblyCollectRequest();
            case 31541:
               return new ApplyDeclineCommand();
            case 31601:
               return new BoosterEquipmentDetailModule();
            case 31620:
               return new CaptainGetResourcesBackCommand();
            case 31654:
               return new DispatchGetRewardPackageCommand();
            case 31667:
               return new HangarShipEquipmentInventoryItem();
            case 31689:
               return new PlayerAttributeModule();
            case 31832:
               return new SlotBarConfigSetRequest();
            case 31940:
               return new EditClanRanksRequest();
            case 31942:
               return new UserMessageOutboxListRequest();
            case 32109:
               return new InventoryItemRemovedCommand();
            case 32151:
               return new CCQuestValueModule();
            case 32164:
               return new JumpGateRemoveCommand();
            case 32210:
               return new ApplyClanCommand();
            case 32241:
               return new Curcubitor2019TimeUpdateCommand();
            case 32247:
               return new PlutusScoreEntryModule();
            case 32307:
               return new OfferRequest();
            case 32320:
               return new EventProgressUpdateCommand();
            case 32321:
               return new EventProgressResetRequest();
            case 32358:
               return new DailyLoginRewardDayRequest();
            case 32369:
               return new AssemblyRecipeStatusModule();
            case 32474:
               return new MenuActionRequest();
            case 32478:
               return new ShipUpgradeModuleUpdateCommand();
            case 32669:
               return new BattleStationHistoryListCommand();
            default:
               return null;
         }
      }
   }
}

