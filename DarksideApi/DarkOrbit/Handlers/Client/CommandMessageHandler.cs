using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Objects.Boxes;
using DarksideApi.DarkOrbit.Objects.Map;
using DarksideApi.DarkOrbit.Objects.Ship;
using DarksideApi.DarkOrbit.Settings;
using DarksideApi.Util;
using DarksideApi.Util.Extensions;
using System.Xml.Linq;

namespace DarksideApi.DarkOrbit.Handlers.Client
{
    public class CommandMessageHandler
    {
        public static void HandleMessage(Api api, string message)
        {
            try
            {
                if (string.IsNullOrEmpty(message))
                {
                    return;
                }
                if (!message.Contains(SPLITTER))
                {
                    return;
                }

                var messageData = message.Split(SPLITTER);
                if (messageData.Length < 2)
                {
                    return;
                }

                if (messageData[0] != "0")
                {
                    switch (messageData[0])
                    {
                        case ERROR:
                            api.Logging.Log("Error: " + message);
                            break;
                        default:
                            api.Logging.Log("Unknown Prefix: " + message);
                            break;
                    }
                    return;
                }

                switch (messageData[1])
                {
                    case TRACKING:
                        api.GameMethods.SendLegacyCommand($"TR{SPLITTER}{Api.TrackVersion}{SPLITTER}0{SPLITTER}context3D not available!");
                        break;
                    case ATTRIBUTE:
                        if (messageData[2] == JCPU)
                        {
                            //0|A|JCPU|C|28|850|1
                            //0 | A | JCPU | S | 10 | 28
                            if (messageData[3] == "I")
                            {
                                api.Logging.Log("Advanced Jump CPU Init");
                                break;
                            }
                            if (messageData[3] == "C")
                            {
                                api.Logging.Log("Advanced Jump CPU Selected Map: " + messageData[4]);
                                break;
                            }
                            if (messageData[3] == "S")
                            {
                                api.Hero.IsJumping = true;
                                api.Hero.AdvancedJumpCpuDuration = int.Parse(messageData[4]);
                                api.Logging.Log("Advanced Jump CPU Jump starting to: " + messageData[5] + ", duration: " + messageData[4]);
                                api.WriteLog("Advanced Jump CPU, Jumping to: " + (CommandConstants.MAP_NAMES.TryGetValue(int.Parse(messageData[5]), out var mapName) ? mapName : "Map"));
                                break;
                            }
                        }
                        if (messageData[2] == HEALTH)
                        {
                            var userId = int.Parse(messageData[3]);
                            if (userId == api.Hero.UserId)
                            {
                                var value = int.Parse(messageData[6]);
                                if (value > 0)
                                {
                                    api.EntitiesUnderAttackBy.Clear();
                                }
                                if (messageData[5] == HITPOINTS)
                                {
                                    api.Hero.Hitpoints = value;
                                    api.Hero.RepairAmount = int.Parse(messageData[7]);
                                    api.Hero.LastRepairTime = DateTime.UtcNow;
                                    break;
                                }
                                if (messageData[5] == SHIELD)
                                {
                                    api.Hero.Shield = value;
                                    api.Logging.Log(message);
                                    break;
                                }
                            }
                        }
                        if (messageData[2] == ACHIEVEMENT)
                        {
                            if (!int.TryParse(messageData[4], out var amountUnlocked))
                            {
                                amountUnlocked = 1;
                            }
                            api.WriteLog($"Achievement unlocked: {amountUnlocked}x {messageData[3]}");
                            api.AddLootResource(messageData[3], amountUnlocked, 0);
                            break;
                        }
                        if (messageData[2] == LEVEL_UP)
                        {
                            //0|A|LUP|6|159850
                            var newLevel = int.Parse(messageData[3]);
                            var userId = int.Parse(messageData[4]);

                            if (userId == api.Hero.UserId)
                            {
                                api.WriteLog("Level Up to: " + newLevel);
                                api.AddLootResource("Level " + newLevel, 1);
                            }
                        }
                        if (messageData[2] == JUMP_VOUCHER)
                        {
                            api.Inventory.JumpVouchers = int.Parse(messageData[3]);
                            break;
                        }
                        if (messageData[2] == SERVER_MSG)
                        {
                            if (messageData[3] == "jump15")
                            {
                                api.Cooldown.AddCooldown(DarkOrbit.Cooldown.CooldownType.CPU_SPECIALJUMP, 15);
                                break;
                            }
                            if (messageData[3] == "msg_pet_activated")
                            {
                                api.WriteLog("Pet Activated!");
                                break;
                            }
                            if (messageData[3] == "msg_pet_deactivated")
                            {
                                api.WriteLog("Pet Deactivated!");
                                break;
                            }
                            if (messageData[3] == "msg_pet_out_of_fuel")
                            {
                                api.Hero.IsPetOutOfFuel = true;
                                api.WriteLog("Pet out of Fuel!");
                                break;
                            }
                            if (messageData[3] == "msg_pet_kamikaze-used" || messageData[3] == "msg_pet_is_dead")
                            {
                                api.WriteLog("Pet destroyed");
                                api.Logic.TryPetRevive();
                                api.Hero.IsPetDestroyed = true;
                                break;
                            }
                            if (messageData[3] == "config_change_failed_time")
                            {
                                api.Cooldown.AddCooldown(DarkOrbit.Cooldown.CooldownType.CONFIG, 5);
                                break;
                            }
                            if (messageData[3] == "jumpgate_failed_no_gate")
                            {
                                api.Hero.IsJumping = false;
                                api.Logic.Target = null;
                                break;
                            }
                            if (messageData[3] == "ammo_cpu_worked")
                            {
                                api.WriteLog("CPU Bought Ammo.");
                                break;
                            }
                            if (messageData[3] == "lab_cpu_rockets")
                            {
                                api.WriteLog("CPU Upgraded Rockets.");
                                break;
                            }
                            if (messageData[3] == "jumpgate_failed_pvp_map")
                            {
                                api.Hero.IsJumping = false;
                                break;
                            }
                            if (messageData[3] == "captcha_box_failed")
                            {
                                api.Logic.SetState(typeof(CommandMessageHandler), Logic.LogicState.CaptchaPending);
                                break;
                            }
                            if (messageData[3] == "sp_mission-completed")
                            {
                                api.AddLootResource("Season Mission", 1);
                                break;
                            }
                            if (messageData[3] == "sp_daily-mission-completed")
                            {
                                api.AddLootResource("Daily Mission", 1);
                                break;
                            }
                            if (messageData[3] == "sp_weekly-mission-completed")
                            {
                                api.AddLootResource("Weekly Mission", 1);
                                break;
                            }
                            if (messageData[3] == "lifes_left")
                            {
                                api.Map.CurrentGGLifeLeft = int.Parse(messageData[4]);
                                break;
                            }
                            if (messageData[3] == "server_restart_n_minutes")
                            {
                                api.GameClient.IsServerRestarting = true;
                                api.WriteLog("Server restarting in " + messageData[4] + "Min.");
                                break;
                            }
                            if (messageData[3] == "server_restart_n_seconds")
                            {
                                api.GameClient.IsServerRestarting = true;
                                api.WriteLog("Server restarting in " + messageData[4] + "Sec.");
                                if (messageData[4] == "0")
                                {
                                    api.GameClient.GameState = GameState.Disconnected;
                                }
                                break;
                            }
                            if (messageData[3].Contains("log_msg_gg_completed_"))
                            {
                                var ggName = messageData[3].Split("_").Last().ToUpper();
                                api.WriteLog("Completed GG " + ggName);
                                //api.AddLoot("GG " + ggName, 1);
                                api.GameObservers.GameTaskObserver.ForceLastGalaxyGateSpinnerUpdate();
                            }
                            if (messageData[3] == "msg_cpu_jump_sequence_complete")
                            {
                                api.Hero.IsJumping = false;
                                api.WriteLog("Advanced Jump CPU, Jump completed.");
                                break;
                            }
                            if (messageData[3] == "error_cpu_jump_cancelled_through_move")
                            {
                                //0|A|STM|error_cpu_jump_cancelled_through_move
                                api.Hero.IsJumping = false;
                                api.WriteLog("Advanced Jump CPU, Jump cancled through move.");
                                break;
                            }
                        }
                        if (messageData[2] == SERVER_DATA)
                        {
                            if (messageData[3].Contains("-=- 15 -=-"))
                            {
                                api.WriteLog("GG Wave starting...");
                                api.GameMethods.Move((int)(api.Map.CornerTopLeft.X * 1.4) + Random.Shared.Next(-100, 100),
                                                    (int)(api.Map.CornerTopLeft.Y * 2) + Random.Shared.Next(-100, 100));
                                break;
                            }
                            if (messageData[3].Contains("Eternal Blacklight Gate"))
                            {
                                api.Map.Name = "GG ∞";
                            }
                            if (int.TryParse(messageData[3].Split("-").Last().Trim(), out var wave))
                            {
                                api.Map.CurrentGGWave = wave;
                                api.WriteLog($"{api.Map.Name} {wave} starting...");
                            }
                            break;
                        }
                        if (messageData[2] == CURRENCY)
                        {
                            api.Inventory.AddCredits(long.Parse(messageData[3]));
                            api.Inventory.AddUridium(long.Parse(messageData[4]));

                            //api.AddLoot(StatisticsConstants.Credits, 0, long.Parse(messageData[3]));
                            //api.AddLoot(StatisticsConstants.Uridium, 0, long.Parse(messageData[4]));
                            break;
                        }
                        if (messageData[2] == SELECTED_CONFIG)
                        {
                            api.Hero.Config = int.Parse(messageData[3]);
                            break;
                        }
                        if (messageData[2] == CARGO)
                        {
                            api.Hero.Cargo.MaxCargoSpace = int.Parse(messageData[3]);
                            break;
                        }
                        if (messageData[2] == BOOTY_KEY_GREEN)
                        {
                            api.Inventory.BootyKeysGreen = int.Parse(messageData[3]);
                            break;
                        }
                        if (messageData[2] == BOOTY_KEY_BLUE)
                        {
                            api.Inventory.BootyKeysBlue = int.Parse(messageData[3]);
                            break;
                        }
                        if (messageData[2] == BOOTY_KEY_RED)
                        {
                            api.Inventory.BootyKeysRed = int.Parse(messageData[3]);
                            break;
                        }
                        if (messageData[2] == "BKS")
                        {
                            api.Inventory.BootyKeysSilver = int.Parse(messageData[3]);
                            break;
                        }
                        if (messageData[2] == "BKBLC")
                        {
                            api.Inventory.BootyKeysBlackLightChifre = int.Parse(messageData[3]);
                            break;
                        }
                        if (messageData[2] == "BKM")
                        {
                            api.Inventory.BootyKeysMasque = int.Parse(messageData[3]);
                            break;
                        }
                        if (messageData[2] == "BKBB")
                        {
                            api.Inventory.BootyKeysBlack = int.Parse(messageData[3]);
                            break;
                        }
                        if (messageData[2] == "BKA")
                        {
                            api.Inventory.BootyKeysAstral = int.Parse(messageData[3]);
                            break;
                        }
                        if (messageData[2] == "BKAP")
                        {
                            api.Inventory.BootyKeysAstralPrime = int.Parse(messageData[3]);
                            break;
                        }
                        if (messageData[2] == "BKL")
                        {
                            api.Inventory.BootyKeysLucientEgg = int.Parse(messageData[3]);
                            break;
                        }
                        if (messageData[2] == "BKE")
                        {
                            api.Inventory.BootyKeysEmpyrian = int.Parse(messageData[3]);
                            break;
                        }
                        if (messageData[2] == "BKPR")
                        {
                            api.Inventory.BootyKeysPrometheus = int.Parse(messageData[3]);
                            break;
                        }
                        if (messageData[2] == "BKLL")
                        {
                            api.Inventory.BootyKeysLaurel = int.Parse(messageData[3]);
                            break;
                        }
                        if (messageData[2] == "BKP")
                        {
                            api.Inventory.BootyKeysProsperous = int.Parse(messageData[3]);
                            break;
                        }
                        /*
                            0|A|BKPG|0
                            0|A|BKE|0
                            0|A|BKBLD|0
                        */
                        if (messageData[2].Contains("BK"))
                        {
                            //Debug.WriteLine("BOOTY-KEY: " + message);
                            break;
                        }
                        if (messageData[2] == LOOTING_CANCELLED)
                        {
                            //Collecting False
                            api.Hero.CollectStartTime = DateTime.MinValue;
                            break;
                        }
                        if (messageData[2] == BOX_COLLECT_INIT)
                        {
                            var userId = int.Parse(messageData[4]);
                            var collecting = messageData[3] == "1";
                            if (api.Hero.UserId == userId)
                            {
                                if (api.Logic.Target is Box)
                                {
                                    api.Hero.CollectStartTime = DateTime.MinValue;
                                    break;
                                }
                                api.Hero.CollectStartTime = DateTime.UtcNow;
                                break;
                            }
                            if (api.Hero.IsPetActive && api.Hero.Pet.UserId == userId && collecting)
                            {
                                api.AddLootBox("Pet-Boxes", 1);
                                break;
                            }
                            break;
                        }
                        api.Logging.Log(message);
                        break;
                    case LOG_MESSAGE:
                        if (messageData[2] == RECEIVED)
                        {
                            if (messageData[3] == CREDITS)
                            {
                                api.WriteLog("Received " + messageData[4] + " Credits.");
                                //api.AddLoot(StatisticsConstants.Credits, long.Parse(messageData[4]), long.Parse(messageData[5]));
                                api.Statistics.CollectedCredits += long.Parse(messageData[4]);
                                api.Statistics.CollectedCredits.Total = long.Parse(messageData[5]);
                                api.Inventory.AddCredits(long.Parse(messageData[5]));
                                break;
                            }
                            if (messageData[3] == BOX_ALREADY_COLLECTED && api.Logic.Target is Box collectingBox)
                            {
                                api.Collectibles.Remove(collectingBox.Hash!, out _);
                                break;
                            }
                            if (messageData[3] == EXPERIENCE_POINTS)
                            {
                                api.WriteLog("Received " + messageData[4] + " XP.");
                                //api.AddLoot(StatisticsConstants.XP, long.Parse(messageData[4]), long.Parse(messageData[5]));
                                api.Statistics.CollectedXP += long.Parse(messageData[4]);
                                api.Hero.ExperiencePoints = double.Parse(messageData[5]);
                                break;
                            }
                            if (messageData[3] == HONOR)
                            {
                                api.WriteLog("Received " + messageData[4] + " Honor.");
                                //api.AddLoot(StatisticsConstants.Honor, long.Parse(messageData[4]), long.Parse(messageData[5]));
                                api.Statistics.CollectedHonor += long.Parse(messageData[4]);
                                api.Hero.HonorPoints = double.Parse(messageData[5]);
                                break;
                            }
                            if (messageData[3] == EXTRA_ENEGRY)
                            {
                                api.WriteLog("Received " + messageData[4] + " Extra Energy.");
                                //api.AddLoot(StatisticsConstants.EE, long.Parse(messageData[4]), messageData.Length > 5 ? long.Parse(messageData[5]) : 0);
                                api.Statistics.CollectedEE += long.Parse(messageData[4]);
                                if (messageData.Length > 5)
                                    api.Inventory.ExtraEnergy = double.Parse(messageData.Last());
                                break;
                            }
                            if (messageData[3] == URIDIUM)
                            {
                                api.WriteLog("Received " + messageData[4] + " Uridium.");
                                //api.AddLoot(StatisticsConstants.Uridium, long.Parse(messageData[4]), long.Parse(messageData[5]));
                                api.Statistics.CollectedUridium += long.Parse(messageData[4]);
                                api.Statistics.CollectedUridium.Total = long.Parse(messageData[5]);
                                api.Inventory.AddUridium(long.Parse(messageData[5]));
                                break;
                            }
                            if (messageData[3] == LOGFILE)
                            {
                                api.WriteLog("Received " + messageData[4] + " Logfile.");
                                //api.AddLoot("LOGFILE", long.Parse(messageData[4]), long.Parse(messageData[5]));
                                break;
                            }
                            if (messageData[3] == LASER || messageData[3] == ROCKET || messageData[3] == ROCKET_LAUNCHER || messageData[3] == FIREWORK || messageData[3] == "AMI")
                            {
                                var itemName = messageData[4].Split('_').LastOrDefault().ToUpper();
                                //api.AddLoot(itemName, long.Parse(messageData[5]));
                                api.WriteLog("Received " + messageData[5] + "x " + itemName + ".");
                                break;
                            }
                            if (messageData[3] == JUMP_VOUCHER)
                            {
                                //api.AddLoot(StatisticsConstants.JumpVoucher, 1);
                                api.WriteLog("Received Jump Voucher.");
                                break;
                            }
                            if (messageData[3] == LOOT)
                            {
                                //resource_collectable_charopos-shard.
                                var itemName = messageData[4].Split('_').LastOrDefault().ToUpper();
                                var count = long.Parse(messageData[5]);
                                //api.AddLootResource(itemName, count);
                                api.Logging.Log("Received loot: " + message);
                                api.WriteLog("Received " + count + "x " + itemName + ".");
                                //if (api.Inventory.Get(itemName) <= 0)
                                //{
                                //api.Inventory.Add(itemName, count);
                                //}
                                break;
                            }
                            if (messageData[3] == PET_FUEL)
                            {
                                //0|LM|ST|PFL|123
                                api.AddLootResource("Pet Fuel", long.Parse(messageData[4]));
                                api.WriteLog("Received " + messageData[4] + "x Pet Fuel.");
                                break;
                            }
                            if (messageData[3] == BOX_TO_BIG)
                            {
                                /*var fromShipSetting = Logic.Logic.Settings.CollectBoxTypes.First(b => b.Type == CommandConstants.BOX_SHIP);
                                if (fromShipSetting != null)
                                {
                                    fromShipSetting.IsEnabled = false;
                                }*/
                                if (api.Logic.Target is Collectible c)
                                {
                                    api.Collectibles.Remove(c.Hash, out _);
                                }
                                api.Hero.CollectStartTime = DateTime.MinValue;
                                api.WriteLog("Can not collect any more Cargo.");
                                break;
                            }
                            if (messageData[3] == "echo-green_init")
                            {
                                if (api.Logic.Target is not Collectible c)
                                {
                                    break;
                                }
                                api.Hero.CollectStartTime = DateTime.UtcNow;
                                break;
                            }
                        }
                        if (messageData[2] == "HP")
                        {
                            //0|LM|HP|DIS|RD|0
                        }
                        api.Logging.Log(message);
                        break;
                    case STATUS:
                        if (messageData[2] == CONFIG)
                        {
                            api.Hero.Config = int.Parse(messageData[3]);
                            api.WriteLog("Changed Config to: " + api.Hero.Config);
                            break;
                        }
                        api.Logging.Log(message);
                        break;
                    case MAP_EVENT:                       
                        if (messageData[2] == LASER_FADE_IN || messageData[2] == LASER_FADE_OUT)
                        {
                            break;
                        }
                        if (messageData[2] == MESSAGE)
                        {
                            if (messageData[5] == "msg_booty-key-green_auto_buy_not_active")
                            {
                                if (api.Logic.Target is Box box && api.Settings.CollectBoxTypes.Contains(box.Type))
                                {
                                    api.Settings.CollectBoxTypes[box.Type].IsEnabled = false;
                                    api.WriteLog("No more " + box.Type + " Keys, disabeling...");
                                    break;
                                }
                                api.WriteLog("Auto buy booty keys is disabled.");
                                break;
                            }
                            if (messageData[5] == "msg_switchship_success")
                            {
                                api.WriteLog("Hangar Change Successful!");
                                break;
                            }
                            if (messageData[5] == "log_galaxygate_wave_name:base")
                            {
                                var mapName = HelpTools.Between(messageData[6], "%NAME%,v:", "},{");
                                var wave = HelpTools.Between(messageData[6], "%WAVE%,v:", "}");
                                //api.Map.Name = mapName;
                                api.Map.CurrentGGWave = int.Parse(wave);
                                api.WriteLog($"{mapName} Wave {wave} started...");
                                break;
                            }
                            if (messageData[5] == "msg_gg_move_next_gate")
                            {
                                api.WriteLog("Galaxy Gate Wave finished!");
                                break;
                            }
                            if (messageData[5] == "log_msg_gather_repair-voucher_s")
                            {
                                api.WriteLog("Received 1x Repair Voucher.");
                                api.AddLootResource("Repair Voucher", 1);
                                break;
                            }
                            if (messageData[5] == "msg_booty-key-lucent_received_s")
                            {
                                var amount = int.Parse(HelpTools.Between(messageData[6], "{w:%AMOUNT%,v:", "}").Trim());
                                api.WriteLog($"Received {amount}x Lucent Egg Booty Key");
                                api.AddLootResource("BOOTY-KEY-LUCENT", amount);
                                break;
                            }
                            if (messageData[5] == "msg_booty-key-green_received_p" || messageData[5] == "msg_booty-key-green_received_s")
                            {
                                var amount = int.Parse(HelpTools.Between(messageData[6], "{w:%AMOUNT%,v:", "}").Trim());
                                api.WriteLog($"Received {amount}x Booty Key");
                                api.AddLootResource("BOOTY-KEY", amount);
                            }
                            if (messageData[5] == "msg_galaxy_gate_cooldown_active")
                            {
                                //0|n|MSG|1|0|msg_galaxy_gate_cooldown_active|{w:%HOURS%,v:11},{w:%MINUTES%,v:48}
                                api.Hero.IsJumping = false;
                                Gate galaxyGate = api.Logic.GetClosestGateWithoutArea();
                                if (galaxyGate != null && galaxyGate.GateId > 0)
                                {
                                    GalaxyGateSetting? ggSetting = null;
                                    if (galaxyGate.GateDesignId == CommandConstants.GALAXY_GATE_VOT_DESIGN_ID
                                        && api.Settings.GalaxyGateSettings.TryGetValue(CommandConstants.GALAXY_GATE_VOT_DESIGN_ID, out ggSetting))
                                    {
                                        ggSetting.IsEnabled = false;
                                    }
                                    api.LockedGateIds.Add(galaxyGate.GateId);
                                    var h = int.Parse(HelpTools.Between(messageData[6], "{w:%HOURS%,v:", "},{w:"));
                                    var m = int.Parse(HelpTools.Between(messageData[6], "{w:%MINUTES%,v:", "}"));
                                    api.WriteLog($"Gate is locked, for: {h}hours {m}min");
                                    Task.Delay(TimeSpan.FromHours(h).Add(TimeSpan.FromMinutes(m))).ContinueWith(_ =>
                                    {
                                        api.LockedGateIds.Remove(galaxyGate.GateId);
                                        api.WriteLog($"Gate is no longer locked");
                                        if (ggSetting != null)
                                            ggSetting.IsEnabled = true;
                                    });
                                    api.Logic.Target = null;
                                }
                                api.Logging.Log($"Jump forbidden: " + message);
                                break;
                            }
                            if (messageData[5] == "msg_npc_skoll-labyrinth_spawn_now")
                            {
                                //0|n|MSG|1|1|msg_npc_skoll-labyrinth_spawn_now|{w:%MAP%,v:Eridani},{w:%ZONE%,v:3}
                                var info = messageData[6];
                                var map = HelpTools.Between(info, "{w:%MAP%,v:", "},");
                                var zone = HelpTools.Between(info, "{w:%ZONE%,v:", "}");
                                api.WriteLog($"Skoll spawned on Map {map}, Zone {zone}");
                                break;
                            }
                            if (messageData[5] == "msg_npc_spawned_map")
                            {
                                //[{w:%MAP%,v:4-2 PvP},{w:%NPC_NAME%,v:-=[ Deadly Battleray ]=-}]
                                var map = HelpTools.Between(messageData[6], "%MAP%,v:", "},{w").Replace("PvP", "").Split(' ').First().Trim();
                                var npc = HelpTools.Between(messageData[6], "%NPC_NAME%,v:", "}]").Trim();
                                if (!api.Settings.NpcSettings.TryGetValue(npc, out Settings.NpcSetting? npcSetting))
                                {
                                    api.Logging.Log(npc + " spawned not found in Settings");
                                    break;
                                }
                                if (!CommandConstants.MAP_NAMES.TryGetKey(map, out var mapId))
                                {
                                    api.Logging.Log(npc + " spawned not map " + map + "not found");
                                    break;
                                }
                                if (!npcSetting.IsEnabled || npcSetting.Priority <= 4)
                                {
                                    break;
                                }
                                api.WriteLog("Important Npc " + npc + " spawned on Map " + map + ", hunting...");
                                api.Settings.MapSelect = mapId;
                                break;
                            }
                            if (messageData[5] == "msg_pending_mails_p")
                            {
                                //0|n|MSG|1|1|msg_pending_mails_p|{w:%COUNT%,v:3457,t:1}
                                var count = HelpTools.Between(messageData[6], "%COUNT%,v:", ",t:");
                                api.WriteLog("Pending Messages: " + count);
                                break;
                            }
                            if (messageData[5] == "log_msg_titlefrenzy_playertakeover")
                            {
                                //0|n|MSG|1|1|log_msg_titlefrenzy_playertakeover|{w:%NAME%,v:BOXER},{w:%MAP%,v:1-3 MMO}
                                var playerName = HelpTools.Between(messageData[6], "%NAME%,v:", "},");
                                var mapName = HelpTools.Between(messageData[6], "%MAP%,v:", "}").Split(' ').FirstOrDefault()?.Trim();
                                if (playerName != api.Hero.Username)
                                {
                                    if (api.Map.Name != mapName)
                                    {
                                        break;
                                    }
                                    api.WriteLog($"{playerName} took over a Prestige Title on your map!");
                                    break;
                                }
                                api.WriteLog($"You now have a Prestige Title!");
                                break;
                            }
                        }
                        if (messageData[2] == ENEMIES_ON_MAP)
                        {
                            var amount = int.Parse(messageData[3]);

                            if (amount > 0)
                            {
                                api.WriteLog($"{amount} {(amount > 1 ? "Enemies" : "Enemy")} on map!");
                            }

                            api.Map.EnemysCount = amount;
                            break;
                        }
                        if (messageData[2] == SHIP_CLOAK_UPDATE)
                        {
                            var isCloaked = messageData[4] == "1";
                            var userId = int.Parse(messageData[3]);
                            if (userId == api.Hero.UserId)
                            {
                                api.Hero.IsCloaked = isCloaked;
                                break;
                            }
                            if (api.Ships.TryGetValue(userId, out Objects.Ship.Ship? ship) && ship is Player playerToCloak)
                            {
                                playerToCloak.Cloaked = isCloaked;
                                break;
                            }
                            if (api.Pets.TryGetValue(userId, out Objects.Pets.Pet? pet))
                            {
                                pet.IsVisible = !isCloaked;
                                break;
                            }
                        }
                        if (messageData[2] == MINE_EXPLODE)
                        {
                            api.Mines.Remove(messageData[3], out _);
                            break;
                        }
                        if (messageData[2] == ORE_REMOVE)
                        {
                            api.Collectibles.Remove(messageData[3], out _);
                            break;
                        }
                        if (messageData[2] == DRONE_UPDATE)
                        {
                            /*int userId = int.Parse(messageData[3]);

                            if (userId == api.Hero.UserId)
                            {
                                if (CommandConstants.DRONE_FORMATION_IDS.TryGetValue(int.Parse(messageData[4]), out var formation))
                                {
                                    api.Hero.DroneFormation = formation;
                                    api.WriteLog("Changed Formation to " + formation);
                                    break;
                                }
                                api.WriteLog("Changed Formation to " + messageData[4]);
                            }*/
                            break;
                        }
                        if (messageData[2] == TITLE)
                        {
                            var userId = int.Parse(messageData[3]);

                            if (userId == api.Hero.UserId)
                            {
                                api.Hero.Title = messageData[5];
                            }
                            if (api.Ships.TryGetValue(userId, out Objects.Ship.Ship? ship))
                            {
                                ship.Title = messageData[5];
                            }
                            break;
                        }
                        if (messageData[2] == BURNING_TRAIL)
                        {
                            api.MapAssets.Remove(int.Parse(messageData[3]), out _);
                            break;
                        }
                        if (messageData[2] == EMP)//
                        {
                            var userId = int.Parse(messageData[3]);
                            var x = int.Parse(messageData[4]);
                            var y = int.Parse(messageData[5]);
                            if (api.Logic.Target is Objects.Ship.Ship ship && userId == ship.UserId)
                            {
                                ship.IsSelected = false;
                            }
                            if (userId == api.Hero.UserId)
                            {
                                foreach (Npc possibleNpc in api.Ships.Values.OfType<Npc>())
                                {
                                    possibleNpc.IsSelected = false;
                                }
                            }
                        }
                        if (messageData[2] == FX)
                        {
                            //0|n|fx|start|ICY_CUBE|170671934
                            //0|n|fx|end|SABOTEUR_DEBUFF|80138602
                            if (messageData[3] == START)
                            {
                                if (messageData[4] == "ICY_CUBE")
                                {
                                    var userId = int.Parse(messageData[5]);
                                    if (userId == api.Hero.UserId)
                                    {
                                        api.Hero.IsFrozen = true;
                                        Task.Delay(6000).ContinueWith(_ => api.Hero.IsFrozen = false);
                                        //Movement.Movement.StopMovementComplete(GameObjects.Hero);
                                    }
                                    break;
                                }
                                if (messageData[4] == "SABOTEUR_DEBUFF")
                                {
                                    var userId = int.Parse(messageData[5]);
                                    if (userId == api.Hero.UserId)
                                    {
                                        api.Hero.IsSaboteurDebuffed = true;
                                    }
                                    break;
                                }
                            }
                            if (messageData[3] == END)
                            {
                                if (messageData[4] == "ICY_CUBE")
                                {
                                    var userId = int.Parse(messageData[5]);
                                    if (userId == api.Hero.UserId)
                                    {
                                        api.Hero.IsFrozen = false;
                                    }
                                    break;
                                }
                                if (messageData[4] == "SABOTEUR_DEBUFF")
                                {
                                    var userId = int.Parse(messageData[5]);
                                    if (userId == api.Hero.UserId)
                                    {
                                        api.Hero.IsSaboteurDebuffed = false;
                                    }
                                    break;
                                }
                            }
                        }
                        if (messageData[2] == MINION_SPAWN)
                        {
                            /*
                    Received Command: 0|n|s|150201431|20|150201543|150200807|150201182|150201246|150200438|150201003|150200119|150200362|150200637|150201583|150200740|150200600|150201445|150200698|150200440|150200232|150200243|150200173|150200746|150200677
                     */
                            var bossUserId = int.Parse(messageData[3]);
                            if (!api.Ships.TryGetValue(bossUserId, out Objects.Ship.Ship? ship) || ship is not Npc npc)
                            {
                                break;
                            }

                            var minionsCount = int.Parse(messageData[4]);
                            List<int> minionUserIds = [];
                            for (var i = 5; i < minionsCount; i++)
                            {
                                minionUserIds.Add(int.Parse(messageData[i]));
                            }

                            npc.MinioinNpcs = minionUserIds;
                            api.Logging.Log("Added " + minionUserIds.Count + " Minions to " + bossUserId);
                            break;
                        }
                        api.Logging.Log($"Received Command: {message}");
                        break;
                    case HELLSTORM:
                        //Received Command: 0|v|150200423|170671934|H|2|0|0
                        var targetId = int.Parse(messageData[3]);
                        var attackerId = int.Parse(messageData[2]);
                        var typeId = int.Parse(messageData[5]);
                        var hit = messageData[4] == "H";

                        if (attackerId == api.Hero.UserId)
                        {
                            //api.Hero.RocketLauncherLoadingStatus = RocketLauncherLoadCommand.EMPTY;
                            api.Logging.Log($"Rocket Attack({typeId}) on {targetId} {(hit ? "Successful" : "Missed")}!");
                        }
                        break;
                    case CPU_REQUEST:
                        api.GameMethods.SendLegacyCommand(JCPU + SPLITTER + GET);
                        break;
                    case MINE_REMOVE:
                    case MINE_EXPLODE:
                    case ORE_REMOVE:
                        var toRemove = messageData[2];
                        api.Mines.Remove(toRemove, out _);
                        api.Collectibles.Remove(toRemove, out _);
                        break;
                    case PET_DESTROYED:
                        api.WriteLog("Pet destroyed!");
                        api.Hero.IsPetDestroyed = true;
                        api.Logic.TryPetRevive();
                        break;
                    case KICK:
                        api.GameClient.Disconnect();
                        break;
                    case LOGOUT:
                        api.WriteLog("Logged out.");
                        api.Hero.LogoutStartTime = DateTime.MinValue;
                        api.GameClient.GameState = GameState.Disconnected;
                        break;
                    case BOX_DISABLED:
                        api.Hero.CollectStartTime = DateTime.MinValue;
                        break;
                    case PORTAL_JUMP:
                        api.Map = new(int.Parse(messageData[2]), 0, 0);
                        break;
                    case PORTAL_BLOCKED:
                        //messageData[2] == Level Neccessary -- $Jump forbidden: 0|k|24
                        api.Hero.IsJumping = false;
                        if (api.Logic.Target is Gate gate)
                        {
                            api.LockedGateIds.Add(gate.GateId);
                            api.WriteLog($"Gate is locked, necessary Level: {messageData[2]}");
                        }
                        api.Logging.Log($"Jump forbidden: " + message);
                        api.Logic.SetState(typeof(CommandMessageHandler), Logic.LogicState.None);
                        break;
                    case NO_AMMUNITION:
                        var type = messageData[2];
                        var ammoId = short.Parse(messageData[3]);
                        if (!CommandConstants.AMMUNITION_TYPES.TryGetKey(ammoId, out var ammoName))
                        {
                            ammoName = "current";
                        }
                        switch (type)
                        {
                            case "ROCKET":
                                api.WriteLog($"Out of {ammoName} Rockets!");
                                break;
                            case "LASER":
                                api.WriteLog($"Out of {ammoName} Ammunition!");
                                break;
                            default:
                                api.Logging.Log(message);
                                break;
                        }
                        break;
                    case PET:
                        if (messageData[2] == PET_FUEL)
                        {
                            api.Hero.IsPetOutOfFuel = false;
                        }
                        if (messageData[2] == PET_GEAR)
                        {
                            if (messageData[3] == TARGET_LIST)
                            {
                                api.Logging.Log("PET TARGET LIST: " + message);
                            }
                        }
                        break;
                    default:
                        api.Logging.Log($"Received Command: {message}");
                        break;
                }
            }
            catch (Exception ex)
            {
                api.Logging.Log(ex.ToString());
            }
        }

        public const string SPLITTER = "|";

        public const string KICK = "KIK";
        public const string JCPU = "JCPU";
        public const string GET = "GET";
        public const string CPU_REQUEST = "7";
        public const string MAP_EVENT = "n";
        public const string STATUS = "S";
        public const string LOG_MESSAGE = "LM";
        public const string ATTRIBUTE = "A";
        public const string RECEIVED = "ST";
        public const string TRACKING = "TR";
        public const string HELLSTORM = "v";
        public const string LOGOUT = "l";
        public const string LASER_FADE_IN = "LSH";
        public const string LASER_FADE_OUT = "USH";
        public const string PORTAL_JUMP = "i";
        public const string PORTAL_BLOCKED = "k";
        public const string MINION_SPAWN = "s";
        public const string NO_AMMUNITION = "W";
        public const string PET = "PET";
        public const string PET_GEAR = "G";
        public const string ERROR = "ERR";

        public const string BOX_ALREADY_COLLECTED = "BAH";
        public const string JUMP_VOUCHER = "JV";
        public const string FIREWORK = "FW";
        public const string ROCKET_LAUNCHER = "RL";
        public const string ROCKET = "ROK";
        public const string LASER = "BAT";
        public const string EXTRA_ENEGRY = "XEN";
        public const string HONOR = "HON";
        public const string EXPERIENCE_POINTS = "EP";
        public const string URIDIUM = "URI";
        public const string CREDITS = "CRE";
        public const string CURRENCY = "C";
        public const string LOGFILE = "LOG";
        public const string LOOT = "LOT";
        public const string PET_FUEL = "PFL";
        public const string TARGET_LIST = "TL";
        public const string LEVEL_UP = "LUP";

        public const string MESSAGE = "MSG";
        public const string ENEMIES_ON_MAP = "w";
        public const string SHIP_CLOAK_UPDATE = "INV";
        public const string MINE_EXPLODE = "MIN";
        public const string ORE_REMOVE = "q";
        public const string MINE_REMOVE = "2";
        public const string DRONE_UPDATE = "d";
        public const string BOX_COLLECT_INIT = "SLA";
        public const string LOOTING_CANCELLED = "SLC";
        public const string FX = "fx";
        public const string START = "start";
        public const string END = "end";

        public const string CONFIG = "CFG";
        public const string SELECTED_CONFIG = "CC";
        public const string CARGO = "c";
        public const string HEALTH = "HL";
        public const string HITPOINTS = "HPT";
        public const string SHIELD = "SHD";
        public const string ACHIEVEMENT = "ACG";
        public const string TITLE = "t";
        public const string BURNING_TRAIL = "BT";
        public const string EMP = "EMP";

        public const string SERVER_MSG = "STM";
        public const string SERVER_DATA = "STD";
        public const string PET_DESTROYED = "Z";
        public const string BOOTY_KEY_GREEN = "BK";
        public const string BOOTY_KEY_BLUE = "BKB";
        public const string BOOTY_KEY_RED = "BKR";
        public const string BOOTY_KEY_MASQUE = "BKM";
        public const string BOX_TO_BIG = "BTB";
        public const string BOX_DISABLED = "h";
    }
}
