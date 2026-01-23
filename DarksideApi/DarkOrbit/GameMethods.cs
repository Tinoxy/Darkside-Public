using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Commands.Action;
using DarksideApi.DarkOrbit.Commands.Attack;
using DarksideApi.DarkOrbit.Commands.Auction;
using DarksideApi.DarkOrbit.Commands.Autopilot;
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
using DarksideApi.DarkOrbit.Commands.Map;
using DarksideApi.DarkOrbit.Commands.Pet;
using DarksideApi.DarkOrbit.Commands.Shop;
using DarksideApi.DarkOrbit.Commands.Skylab;
using DarksideApi.DarkOrbit.Objects;
using DarksideApi.DarkOrbit.Objects.Boxes;
using DarksideApi.DarkOrbit.Objects.Client;
using DarksideApi.DarkOrbit.Objects.Hero;
using DarksideApi.DarkOrbit.Objects.Ship;
using DarksideApi.Util;
using System.Diagnostics;
using System.Numerics;

namespace DarksideApi.DarkOrbit
{
    public class GameMethods
    {
        private readonly Api api;

        public GameMethods(Api api)
        {
            this.api = api;
        }

        public GameMethods() { }

        public void VersionRequest()
        {
            var version = DarkOrbitProtoLib.Version.GameVersion;
            this.api.GameClient.Send(new VersionRequest(version));
        }

        public void ReadyGame(short status = 1, bool isUnity = false)
        {
            this.api.GameClient.Send(new ReadyRequest(status, isUnity));
            this.api.Logging.Log("Game Init Stage: " + status);
        }

        public void AttackStop()
        {
            this.api.GameClient.Send(new AttackAbordRequest());
            this.api.Hero.LastAttackTime = DateTime.MinValue;
        }

        public void AttackStartLaser(int userId, int x, int y)
        {
            var attackCommand = new AttackLaserRequest(userId, x, y);
            if ((x + y) % 3 == 0)
            {
                attackCommand.y += 1;
            }
            this.api.GameClient.Send(attackCommand);
        }

        public void AttackStartRocket(int userId, int x, int y)
        {
            if (this.api.Cooldown.IsCooldownActive(Cooldown.CooldownType.ROCKET)
                || HelpTools.CalculateDistance(this.api.Hero.X, this.api.Hero.Y, x, y) >= 520
                || api.Hero.CurrentRocket.Amount <= 0)
            {
                return;
            }

            var attackCommand = new AttackRocketRequest(userId, x, y);
            if ((x + y) % 3 == 0)
            {
                attackCommand.y += 1;
            }
            this.api.GameClient.Send(attackCommand);

            this.api.Cooldown.AddCooldown(Cooldown.CooldownType.ROCKET, 3);
        }

        public void AttackStartRocketLauncher()
        {
            if (this.api.Hero.RocketLauncherLoadingStatus == (int)PlaySoundCommand.SoundType.HELLSTORM_EMPTY)
            {
                this.RocketLauncherLoad();
                return;
            }
            if (this.api.Hero.RocketLauncherLoadingStatus != (int)PlaySoundCommand.SoundType.HELLSTORM_FULLY_LOADED)
            {
                return;
            }
            if (this.api.Hero.CurrentRocketLauncher.Amount <= 0) 
            {
                return;
            }

            this.api.GameClient.Send(new MenuActionRequest(MenuActionRequest.BAR_TYPE_BAR, CommandConstants.EQUIPMENT_ROCKETLAUNCHER, MenuActionRequest.ACTION_STYLE_ACTIVATE));

            this.api.Hero.RocketLauncherLoadingStatus = (int)PlaySoundCommand.SoundType.HELLSTORM_EMPTY;
            this.api.Cooldown.AddCooldown(Cooldown.CooldownType.ROCKET_LAUNCHER, 4);
        }

        public void Jump()
        {
            if (this.api.Cooldown.IsCooldownActive(Cooldown.CooldownType.CPU_SPECIALJUMP) || this.api.Cooldown.IsCooldownActive(Cooldown.CooldownType.CPU_SPECIALJUMP))
            {
                return;
            }

            this.api.GameClient.Send(new JumpRequest());
        }

        public void SendLegacyCommand(string command)
        {
            if (string.IsNullOrEmpty(command))
                return;

            this.api.GameClient.Send(new LegacyCommand(command));
        }

        public void TargetSelect(Ship ship)
        {
            /* TryToSelectMapAssetCommand
              override public function execute(param1:INotification) : void
      {
         var _loc4_:class_2174 = null;
         var _loc2_:Array = param1.getBody() as Array;
         if(_loc2_ == null || _loc2_.length != 8)
         {
            return;
         }
         var _loc3_:int = int(_loc2_[0]);
         _loc2_[1] = int(_loc2_[1]);
         _loc2_[2] = int(_loc2_[2]);
         _loc2_[3] = int(_loc2_[3]);
         _loc2_[4] = int(_loc2_[4]);
         (_loc4_ = new class_2174(_loc3_)).name_156 = _loc2_[1];
         _loc4_.name_148 = _loc2_[2];
         _loc4_.var_2088 = _loc2_[3];
         _loc4_.var_2738 = _loc2_[4];
         _loc4_.var_2910 = _loc2_[5];
         _loc4_.var_2657 = _loc2_[6];
         _loc4_.radius = _loc2_[7];
         _loc4_.name_148 -= (_loc4_.name_156 + _loc4_.name_148 + _loc4_.var_2738 + _loc4_.var_2088) % 8;
         facade.sendNotification(ConnectionNotifications.SEND_NETTY_REQUEST,_loc4_);
      }
            0 -> npcId
            1 -> npcX
            2 -> npcY
            3 -> mouseX
            4 -> mouseY
            5 -> npcX + mouseX
            6 -> npcY + mouseY
            7 -> radius

            => npcY -= (npcX + npcY + mouseX + mouseY) % 8;
            */
            var screenX = (int)(ship.X / 9.882);
            var screenY = (int)(ship.Y / 11.380);

            var selectCommand = new ShipSelectRequest(ship.UserId, ship.X, ship.Y, screenX, screenY, ship.X + screenX, ship.Y + screenY, (ship.X + ship.Y) % 3 == 0 ? 48 : 45);
            selectCommand.targetY -= (selectCommand.targetX + selectCommand.targetY + screenY + screenX) % 8;

            this.api.GameClient.Send(selectCommand);
            this.api.AddVariableTrack();

            ship.tempSelectTries += 1;
        }

        public void Move(IGameObject gameObject)
        {
            this.Move(gameObject.X, gameObject.Y);
        }

        public void Move(IGameObject gameObject, int maxRandom)
        {
            var randX = gameObject.X + Random.Shared.Next(-maxRandom, maxRandom);
            var randY = gameObject.Y + Random.Shared.Next(-maxRandom, maxRandom);
            randX = randX < 0 ? 0 : randX > (this.api.Map.UsableX + this.api.Map.UsableWidth) ? (this.api.Map.UsableX + this.api.Map.UsableWidth) : randX;
            randY = randY < 0 ? 0 : randY > (this.api.Map.UsableY + this.api.Map.UsableHeight) ? (this.api.Map.UsableY + this.api.Map.UsableHeight) : randY;
            this.Move(randX, randY);
        }

        public void Move(Vector2 vector)
        {
            this.Move((int)vector.X, (int)vector.Y);
        }

        public void Move(int x, int y)
        {
            if (this.api.Hero.IsFrozen)
            {
                this.api.Movement.StopMovement(this.api.Hero);
                this.api.Hero.TargetX = this.api.Hero.X;
                this.api.Hero.TargetY = this.api.Hero.Y;
                this.api.Hero.FlyingTimeMs = 0;
                return;
            }

            if (!this.api.POIZones.IsEmpty)
            {
                if (!this.api.Hero.Path.Any(v => v.X == x && v.Y == y))
                {
                    this.api.Movement.ResetPath();
                    this.api.Hero.Path = this.api.Movement.GeneratePath(x, y);//api.Pathfinder.GenerateRoute(api.Hero.Position, new(x, y));
                    Debug.WriteLine("Generated Path: " + string.Join(", ", this.api.Hero.Path));
                    x = (int)this.api.Hero.FirstPathElement.X;
                    y = (int)this.api.Hero.FirstPathElement.Y;
                    this.api.Hero.Path.RemoveFirst();
                }
                else
                {
                    /*var tempPath = this.api.Movement.GeneratePath(x, y);
                    if (!this.api.Movement.IsPathSimilar(tempPath))
                    {
                        api.Hero.Path = tempPath;
                        x = (int)this.api.Hero.FirstPathElement.X;
                        y = (int)this.api.Hero.FirstPathElement.Y;
                    }*/
                    x = (int)this.api.Hero.FirstPathElement.X;
                    y = (int)this.api.Hero.FirstPathElement.Y;
                    if (!this.api.Hero.Path.Remove(new Vector2(x, y)))
                        this.api.Hero.Path.RemoveFirst();

                    Debug.WriteLine("Going to next Path point: " + x + ", " + y);
                }

                if (x == this.api.Hero.X && y == this.api.Hero.Y && this.api.Hero.Path.Count > 1)
                {
                    this.api.Hero.Path.RemoveFirst(); 
                    x = (int)this.api.Hero.FirstPathElement.X;
                    y = (int)this.api.Hero.FirstPathElement.Y;
                    this.api.Hero.Path.RemoveFirst();
                }
            }

            this.api.Hero.TargetX = x;
            this.api.Hero.TargetY = y;

            var moveCommand = new MoveRequest(x, y, this.api.Hero.X, this.api.Hero.Y);
            if ((moveCommand.x + moveCommand.y) % 3 == 0)
            {
                moveCommand.y++;
            }

            this.api.GameClient.Send(moveCommand);
            this.api.AddVariableTrack();

            var distance = HelpTools.CalculateDistance(this.api.Hero.X, this.api.Hero.Y, x, y);
            var duration = (distance / this.api.Hero.Speed);
            var durationMS = (float)duration * 1000f;

            this.api.Hero.FlyingTimeMs = durationMS;

            this.api.Movement.CompleteMovement(this.api.Hero);
            this.api.Movement.StartMovement(this.api.Hero, this.api.Hero.TargetX, this.api.Hero.TargetY, durationMS);
            this.api.AddLastPosition(this.api.Hero.X, this.api.Hero.Y);
        }

        public void BoxCollect(Box box)
        {
            var collectCommand = new CollectBoxRequest(box.Hash, this.api.Hero.X, this.api.Hero.Y, box.X, box.Y);
            if ((this.api.Hero.X + this.api.Hero.Y + box.Y) % 3 == 0)
            {
                collectCommand.y++;
            }

            box.tempStartCollectTime = DateTime.UtcNow;
            this.api.GameClient.Send(collectCommand);
        }

        public void OreCollect(Ore ore)
        {
            ore.tempStartCollectTime = DateTime.UtcNow;
            this.api.GameClient.Send(new HarvestRequest(ore.Hash));
        }

        public void KeepAlive(bool mouseClick = false)
        {
            this.api.GameClient.Send(new KeepAlive(mouseClick));
        }

        public void GameLogin()
        {
            this.api.GameClient.Send(new LoginRequest(this.api.Hero.UserId, (short)this.api.Hero.FactionId, this.api.Hero.SessionId!, DarkOrbitProtoLib.Version.GameVersion, this.api.Hero.InstanceId, true));
        }

        public void GameLogout()
        {
            this.api.WriteLog("Logging out...");
            this.api.GameClient.Send(new UIButtonTriggerRequest("logout"));
            this.api.Hero.LogoutStartTime = DateTime.UtcNow;
        }

        public void StatusSelect(int selection = 1)
        {
            if (selection > 1 && this.api.GameClient.GameState < GameState.Ready)
            {
                selection = 1;
            }
            this.api.GameClient.Send(new KillScreenRepairRequest((short)selection, this.api.Hero.UserId, (short)this.api.Hero.FactionId, this.api.Hero.SessionId!, string.Empty, 0, false));
        }

        public void ConfigurationChange(int toConfig)
        {
            if (this.api.Cooldown.IsCooldownActive(Cooldown.CooldownType.CONFIG))
            {
                return;
            }

            //class_25.getInstance().sendNotification(ConnectionNotifications.SEND_COMMAND,[class_108.SELECT,[class_108.const_4186,_loc3_.toString(),class_114.userID.toString(),class_114.sessionID.toString()]]);
            this.api.GameClient.Send(new LegacyCommand($"S|CFG|{toConfig}|{this.api.Hero.UserId}|{this.api.Hero.SessionId}"));
            this.api.Cooldown.AddCooldown(Cooldown.CooldownType.CONFIG, 5);
        }

        public void MenuRequest(short source, string menuItemId, short type)
        {
            this.api.GameClient.Send(new MenuActionRequest(source, menuItemId, type));
        }

        public void DroneFormationChange(string toFormation, int caller = 0)
        {
            if (string.IsNullOrEmpty(toFormation))
            {
                if (caller != 0)
                    return;

                this.DroneFormationChange(this.api.Settings.RoamFormation, 1);
                return;
            }

            if (this.api.Cooldown.IsCooldownActive(Cooldown.CooldownType.DRONE_FORMATION))
            {
                return;
            }

            this.MenuRequest(0, toFormation, 1);
            this.api.Cooldown.AddCooldown(Cooldown.CooldownType.DRONE_FORMATION, 5);
        }

        public void PetActivate()
        {
            this.api.GameClient.Send(new PetRequest(CommandConstants.PET_TOGGLE_ACTIVATION));
            this.api.Hero.PetStartTries += (this.api.Hero.IsPetDestroyed ? 0 : 1);
        }

        public void PetDeactivate()
        {
            this.api.GameClient.Send(new PetRequest(CommandConstants.PET_DEACTIVATE));
        }

        public void PetRevive()
        {
            this.api.GameClient.Send(new PetRequest(CommandConstants.PET_DESTROYED_REPAIR));
            this.api.Hero.IsPetDestroyed = false;
        }

        public void PetModeSet(int gearId = CommandConstants.PET_GEAR_ENEMY_LOCATOR, int options = 0)
        {
            if (gearId == CommandConstants.PET_GEAR_KAMIKAZE && this.api.Cooldown.IsCooldownActive(Cooldown.CooldownType.PET_KAMIKAZE_GEAR))
            {
                this.PetModeSet(CommandConstants.PET_GEAR_GUARD);
                return;
                //api.Cooldown.AddCooldown(Cooldown.CooldownType.PET_KAMIKAZE_GEAR, 35);
            }
            this.api.Logging.Log("Setting PetMode: " + gearId + ", " + options);
            this.api.Hero.Pet.Gear = gearId;
            this.api.Hero.Pet.ModeOptions = options;
            this.api.GameClient.Send(new PetGearActivateRequest((short)gearId, (short)options));
        }

        public void Repair()
        {
            this.MenuRequest(MenuActionRequest.BAR_TYPE_BAR, CommandConstants.EQUIPMENT_REPAIR, MenuActionRequest.ACTION_STYLE_ACTIVATE);
        }

        public void RepairInstant(int mapAssetId)
        {
            this.api.GameClient.Send(new MapAssetActivationRequest(mapAssetId));
        }

        public void Cloak()
        {
            this.MenuRequest(MenuActionRequest.BAR_TYPE_BAR, CommandConstants.EQUIPMENT_CLOAK, MenuActionRequest.ACTION_STYLE_ACTIVATE);
        }

        public void UseMine(string mineType)
        {
            if (string.IsNullOrEmpty(mineType))
                return;

            if (this.api.Cooldown.IsCooldownActive(Cooldown.CooldownType.MINE))
            {
                return;
            }

            this.MenuRequest(0, mineType, 0);

            this.api.Cooldown.AddCooldown(Cooldown.CooldownType.MINE, 30);
        }

        public void LaserChange(string laserType)
        {
            if (string.IsNullOrEmpty(laserType))
            {
                api.Logging.Log($"Laser type is empty [{laserType}], cannot change laser.");
                return;
            }

            //api.Hero.CurrentLaser = laserType;
            this.MenuRequest(MenuActionRequest.BAR_TYPE_BAR, laserType, MenuActionRequest.ACTION_STYLE_ACTIVATE);
        }

        public void RocketChange(string rocketType)
        {
            if (string.IsNullOrEmpty(rocketType))
                return;

            //api.Hero.CurrentRocket = rocketType;
            this.MenuRequest(MenuActionRequest.BAR_TYPE_BAR, rocketType, MenuActionRequest.ACTION_STYLE_DEACTIVATE);

            this.api.Cooldown.AddCooldown(Cooldown.CooldownType.ROCKET, 3);
        }

        public void RocketLauncherChange(string rocketLauncherType)
        {
            if (string.IsNullOrEmpty(rocketLauncherType))
                return;

            //api.Hero.CurrentRocketLauncher = rocketLauncherType;
            this.MenuRequest(MenuActionRequest.BAR_TYPE_BAR, rocketLauncherType, MenuActionRequest.ACTION_STYLE_ACTIVATE);

            this.api.Cooldown.AddCooldown(Cooldown.CooldownType.ROCKET_LAUNCHER, 4);
        }

        public void RocketLauncherLoad()
        {
            if (this.api.Cooldown.IsCooldownActive(Cooldown.CooldownType.ROCKET_LAUNCHER)
                || this.api.Hero.RocketLauncherLoadingStatus == (int)PlaySoundCommand.SoundType.HELLSTORM_LOADING
                || this.api.Hero.RocketLauncherLoadingStatus == (int)PlaySoundCommand.SoundType.HELLSTORM_FULLY_LOADED)
            {
                return;
            }
            this.MenuRequest(MenuActionRequest.BAR_TYPE_BAR, CommandConstants.EQUIPMENT_ROCKETLAUNCHER, MenuActionRequest.ACTION_STYLE_ACTIVATE);

            this.api.Cooldown.AddCooldown(Cooldown.CooldownType.ROCKET_LAUNCHER, 4);
        }

        public void OreUpgrade(Ore.OreType oreId, long amount)
        {
            this.api.GameClient.Send(new UpgradeOreRequest((short)oreId, amount));
        }

        public void LabUpdate(Cargo.LabType itemType, Ore.OreType oreId, long amount)
        {
            this.api.GameClient.Send(new LabUpdateItemRequest(new LabItemModule((short)itemType), new OreCountModule((short)oreId, amount)));
        }

        public void DispatcherSend(DispatcherObj dispatcher)
        {
            dispatcher.StartTime = DateTime.UtcNow;
            this.api.GameClient.Send(new DispatchSendRetrieverRequest(dispatcher.Id));
        }

        public void DispatcherUpdate(int type = 1)
        {
            this.api.GameClient.Send(new DispatchGetRetrieverListRequest((short)type));
        }

        public void DispatcherRetrieve(DispatcherObj dispatcher)
        {
            this.api.GameClient.Send(new DispatchCollectRetrieverRequest((short)dispatcher.SlotId));
        }

        public void DispatcherQuickFinish(DispatcherObj dispatcher)
        {
            this.api.GameClient.Send(new DispatchInstantCompleteRequest((short)dispatcher.SlotId));
        }

        public void TrackVariable(bool empty = false)
        {
            this.api.GameClient.Send(new TrackVariableRequest(empty ? [] : [this.api.VariableTrackModule]));

            this.api.Statistics.AddClick(empty ? 0 : this.api.VariableTrackModule.value);
            this.api.VariableTrackModule.value = 0;
        }

        public void CaptchaCollect(Box box)
        {
            this.api.GameClient.Send(new CaptchaCollectRequest(box.CaptchaIndex));
            this.api.Captcha.CollectBox(box.CaptchaIndex);
        }

        public void ChannelClose(bool close = false)
        {
            this.api.GameClient.Send(new ChannelCloseRequest(close));
        }

        public void CommandCenterHide()
        {
            if (this.api.Settings.IsUnity)
            {
                //api.GameClient.Send(new LoginInitClanUnityRequest());
                return;
            }
            this.api.GameClient.Send(new CommandCenterGetDontShowRequest());
        }

        public void AbilityActivate(string abilityId)
        {
            this.MenuRequest(MenuActionRequest.BAR_TYPE_BAR, abilityId, MenuActionRequest.ACTION_STYLE_ACTIVATE);
            //this.api.WriteLog("Activating " + abilitySetting.AbilityName);    //Spam
        }

        public void GroupInvite(string username)
        {
            if (string.IsNullOrEmpty(username))
                return;

            this.api.GameClient.Send(new InvitationCreationRequest(username));
        }

        public void GroupAccept(int senderUserId)
        {
            this.api.GameClient.Send(new InvitationAcceptRequest(senderUserId));
        }

        public void GroupDecline(int senderUserId)
        {
            this.api.GameClient.Send(new InvitationRejectRequest(senderUserId));
        }

        public void OreSell(Ore.OreType type, long amount, bool needTradeRequest = true)
        {
            if (amount <= 0)
                return;

            if (needTradeRequest)
                this.api.GameClient.Send(new TradeRequest());

            this.api.GameClient.Send(new SellOreRequest((short)type, amount));
        }

        public void OreTrade(double amount)
        {
            if (amount <= 0)
                return;

            //this.guiManager.method_4235().method_41().sendCommand(class_69.const_1888,[_loc2_]);
            this.api.GameClient.Send(new LegacyCommand("XCP|" + amount));
            this.api.Statistics.AmountTrades += 1;
        }

        public void GalaxyGateGeneratorSelectSpinAmount(byte amount)
        {
            this.api.GameClient.Send(new GalaxyGateBuilderSaveSpinAmountRequest(amount));
            this.api.Hero.GalaxyGeneratorSpinAmount = amount;
        }

        public void GalaxyGateGeneratorSpin(byte gateId, byte amount, bool useMultiplier)
        {
            this.api.GameClient.Send(new GalaxyGateBuilderMultiEnergyRequest(gateId, amount, useMultiplier));

            this.api.Inventory.ExtraEnergy -= (double)amount;
            this.api.Statistics.CollectedEE -= amount;

            if (this.api.Inventory.ExtraEnergy < 0)
            {
                this.api.Inventory.ExtraEnergy = 0;
            }
        }

        public void GalaxyGateGeneratorPlace(byte gateId)
        {
            this.api.GameClient.Send(new GalaxyGateBuilderSetupGateRequest(gateId));
        }

        public void GalaxyGateGeneratorOpen()
        {
            this.api.GameClient.Send(new GalaxyGateBuilderInfoRequest());
        }

        public void AdvancedJumpCPUSelect(int mapId)
        {
            this.api.GameClient.Send(new LegacyCommand($"JCPU|S|{mapId}"));
        }

        public void AdvancedJumpCPUJump()
        {
            this.api.GameClient.Send(new LegacyCommand("JCPU|J"));
            this.api.Cooldown.AddCooldown(Cooldown.CooldownType.CPU_SPECIALJUMP, 60);
        }

        public void EBGBoosterSelect(int selectedBooster)
        {
            this.api.GameClient.Send(new EternalBlacklightChooseOptionRequest(selectedBooster));
        }

        public void EBGPlace()
        {
            this.MenuRequest(MenuActionRequest.BAR_TYPE_BAR, CommandConstants.EQUIPMENT_EBG_CPU, MenuActionRequest.ACTION_STYLE_ACTIVATE);
        }

        public void QZActivate()
        {
            this.MenuRequest(MenuActionRequest.BAR_TYPE_BAR, CommandConstants.EQUIPMENT_QZ_CPU, MenuActionRequest.ACTION_STYLE_ACTIVATE);
        }

        public void ShopOpen(string category)
        {
            this.api.GameClient.Send(new ShopItemListRequest(category));
        }

        public void ShopBuyRocket(string itemId, int amount)
        {
            if (string.IsNullOrEmpty(itemId))
                return;

            this.ShopOpen("ammunition");

            var shopBuyItem = new ShopItemBuyRequest(itemId, "ammunition", "rocket", amount, string.Empty, "purchase", string.Empty);
            this.api.GameClient.Send(shopBuyItem);
        }

        public void ShopBuyLaser(string itemId, int amount)
        {
            if (string.IsNullOrEmpty(itemId))
                return;

            this.ShopOpen("ammunition");

            var shopBuyItem = new ShopItemBuyRequest(itemId, "ammunition", "battery", amount, string.Empty, "purchase", string.Empty);
            this.api.GameClient.Send(shopBuyItem);
        }

        public void ShopBuyPetFuel(int amount)
        {
            this.ShopOpen("petgear");

            var shopBuyItem = new ShopItemBuyRequest("resource_pet-fuel", "petgear", "resource", amount, string.Empty, "purchase", string.Empty);
            this.api.GameClient.Send(shopBuyItem);
        }

        public void LoginRewardClaim()
        {
            if (!this.api.Settings.IsUnity)
                return;

            this.api.GameClient.Send(new DailyLoginClaimTodayRequest());
        }

        public void MenuItemUse(string item)
        {
            if (string.IsNullOrEmpty(item))
                return;

            this.MenuRequest(MenuActionRequest.BAR_TYPE_BAR, item, MenuActionRequest.ACTION_STYLE_ACTIVATE);
        }

        public void SkylabBuyRobot(string module)
        {
            if (string.IsNullOrEmpty(module))
                return;

            this.api.GameClient.Send(new SkylabPurchaseRobotRequest(1, module));
        }

        public void SkylabUpgradeModule(string module)
        {
            if (string.IsNullOrEmpty(module))
                return;

            this.api.GameClient.Send(new SkylabActionRequest(string.Empty, module, "upgrade"));
        }

        public void AuctionGetList(string auctionType = "hour")
        {
            this.api.GameClient.Send(new AuctionListRequest(auctionType));
        }

        public void AuctionBid(string lootId, long price, string type = "hour")
        {
            this.api.GameClient.Send(new AuctionBidRequest(lootId, price, type));
        }

        public void AudioSettingsRequest()
        {
            this.api.GameClient.Send(new AudioSettingsRequest(0, 0, 0, false));
            this.api.Logging.Log("Request Audio Settings...");
        }

        public void ExpeditionSelectShips(List<long> hangarIds)
        {
            this.api.GameClient.Send(new ExpeditionSelectShipRequest(hangarIds));
            var hangars = "";
            hangarIds.ForEach(h => hangars += ", " + h);
            this.api.Logging.Log("Expedition Selected Hangars: " + hangars[2..]);
        }

        public void ExpeditionActivateShip(long hangarId)
        {
            this.api.GameClient.Send(new ExpeditionActivateShipRequest(hangarId));
            this.api.WriteLog($"Expedition Ship Activated: {this.api.Hangars.FirstOrDefault(h => h.HangarId == hangarId)?.Shipname}");
        }

        public void ExpeditionRequestShips()
        {
            this.api.GameClient.Send(new ExpeditionShipListRequest());
        }

        public void EasterEggTrigger(EggTriggerRequest.EggTriggerType eggTriggerType)
        {
            this.api.GameClient.Send(new EggTriggerRequest(eggTriggerType));
            this.api.WriteLog("Easter Egg Triggered: " + eggTriggerType);
        }

        public void AutopilotInfoRequest()
        {
            //this.api.GameClient.Send(new AutopilotInfoRequest());
            //this.api.Logging.Log("Requesting Autopilot Info...");
        }

        public void AutopilotSetInfo(string autoPilotInfos)
        {
            //Autopilot info: {"Lasers": ["ammunition_laser_lcb-10"], "Rockets": ["ammunition_rocket_plt-2026"], "LowToZone": true, "SkillDict": {"ship_orcus_design_orcus-blaze": ["ability_orcus_assimilate"]}, "CounterNPCs": true, "AttackAllNPCs": true, "Configuration": 1, "CounterPlayers": true, "CollectAllBoxes": true, "DoNotAttackNPCs": false, "NPCSelectedList": {"9": []}, "DoNotCollectBoxes": false, "AttackSelectedNPCs": false, "CollectSelectedList": {"9": []}, "PursueAttackingNPCs": true, "CollectSelectedBoxes": false}
            //RESULT: False
            //LASER RANGE: 600
            //REMAINING-TIME: 0
            //autoPilotInfos = "{\"Lasers\": [\"ammunition_laser_lcb-10\"], \"Rockets\": [\"ammunition_rocket_plt-2026\"], \"LowToZone\": true, \"SkillDict\": {\"ship_orcus_design_orcus-blaze\": [\"ability_orcus_assimilate\"]}, \"CounterNPCs\": true, \"AttackAllNPCs\": true, \"Configuration\": 1, \"CounterPlayers\": true, \"CollectAllBoxes\": true, \"DoNotAttackNPCs\": false, \"NPCSelectedList\": {\"9\": []}, \"DoNotCollectBoxes\": false, \"AttackSelectedNPCs\": false, \"CollectSelectedList\": {\"9\": []}, \"PursueAttackingNPCs\": true, \"CollectSelectedBoxes\": false}";
            //this.api.GameClient.Send(new AutopilotInfoSaveRequest(autoPilotInfos));
        }
    }
}
