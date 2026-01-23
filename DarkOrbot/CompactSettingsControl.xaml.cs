using DarkOrbot.Util;
using DarksideApi;
using DarksideApi.DarkOrbit;
using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Constants;
using DarksideApi.DarkOrbit.Objects.Client;
using DarksideApi.DarkOrbit.Settings;
using DarksideApi.Logic.TaskSystem;
using DarksideApi.Statistics;
using DarksideApi.Util;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using HelpTools = DarksideApi.Util.HelpTools;

namespace DarkOrbot
{
    /// <summary>
    /// Interaktionslogik für CompactSettingsControl.xaml
    /// </summary>
    public partial class CompactSettingsControl : Window, IControl
    {
        public Api Api { get; private set; }
        public CompactApiControl ApiControl { get; private set; }
        private int selectedTabIndex;
        private WindowSnapUtil? snapUtil;

        public event EventHandler<CompactApiControl>? OnNewAccountAdded;

        public bool Ready { get; set; }
        public string ApiKey => this.Api.Settings.ApiKey;
        public DarkOrbotSettings Settings => this.Api.Settings;

        public CompactSettingsControl(CompactApiControl owner)
        {
            this.ApiControl = owner;
            this.Api = this.ApiControl.Api;
            this.InitializeComponent();
            this.Init();
        }

        private CompactSettingsControl() { }

        public static CompactSettingsControl Empty()
        {
            return new CompactSettingsControl();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Settings Control Loaded");
        }

        private void Init()
        {
            //this.snapUtil = new WindowSnapUtil(this, this.ApiControl.mainWindow);
            this.Api.OnAddHangars += (s, e) =>
            {
                if (!this.Dispatcher.CheckAccess())
                {
                    this.Dispatcher.Invoke(() => this.AddHangar(e));
                    return;
                }
                this.AddHangar(e);
            };
            if (this.Api.GameClient is not null)
            {
                this.Api.GameClient.GameStateChanged += (s, e) =>
                {
                    //SetHomepageLoggedInStatus(e);
                    if (e != GameState.LoggingIn)
                    {
                        this.SetLoginButtonEnabledState(true);
                    }
                    else
                    {
                        this.SetLoginButtonEnabledState(false);
                    }
                    if (e == GameState.CollectingLoginBonus)
                    {
                        //LoadHomepage();
                    }
                };
            }

            this.Ready = true;
        }

        private void AddHangar(List<Hangar> hangars)
        {
            var currentSelectedHangar = string.Empty;
            if (this.boxesOresSellHangarCombobox.SelectedItem != null)
            {
                currentSelectedHangar = this.boxesOresSellHangarCombobox.SelectedItem.ToString();
            }
            this.Api.Hangars.Clear();
            hangars.ForEach(h => this.Api.Hangars.Add(h));
            CollectionViewSource.GetDefaultView(this.boxesOresSellHangarCombobox.ItemsSource).Refresh();
            if (!string.IsNullOrEmpty(currentSelectedHangar))
            {
                this.boxesOresSellHangarCombobox.SelectedItem = currentSelectedHangar;
            }
        }

        public void AddStatisticsItem(StatisticsItem statisticsItem)
        {
            if (!this.Dispatcher.CheckAccess())
            {
                this.Dispatcher.Invoke(() => this.AddStatisticsItem(statisticsItem));
                return;
            }
            this.Api.Statistics.AddLoot(statisticsItem);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.SaveSettings();
        }

        private void boxesBoxesAddBtn_Click(object sender, RoutedEventArgs e)
        {
            var text = (string)this.boxesBoxesAddCombobox.SelectedValue;
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            var priority = (int)this.boxesBoxesPrioritySlider.Value;
            BoxSetting boxSetting = new()
            {
                IsEnabled = true,
                Type = text,
                Priority = priority,
            };
            if (this.Api.Settings.CollectBoxTypes.Contains(boxSetting.Type))
            {
                this.Api.Settings.CollectBoxTypes[boxSetting.Type].Priority = boxSetting.Priority;
            }
            else
            {
                this.Api.Settings.CollectBoxTypes.Add(boxSetting);
            }
            CollectionViewSource.GetDefaultView(this.boxesBoxesDataGrid.ItemsSource).Refresh();
            CollectionViewSource.GetDefaultView(this.boxesBoxesAddCombobox.ItemsSource).Refresh();
        }

        private void boxesOresAddBtn_Click(object sender, RoutedEventArgs e)
        {
            var text = (string)this.boxesOresAddCombobox.SelectedValue;
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            if (!CommandConstants.ORE_LOOKUP.TryGetValue(text, out var oreType))
            {
                return;
            }
            OreSetting oreSetting = new()
            {
                IsEnabled = true,
                Type = oreType,
                Name = text,
            };
            this.Api.Settings.CollectOreTypes.Add(oreSetting);
        }

        private void npcsNpcsAddBtn_Click(object sender, RoutedEventArgs e)
        {
            var text = this.npcsNpcsAddNameCombobox.Text;
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            if (this.npcsNpcsAddDroneFormationCombobox.SelectedValue == null
                || this.npcsNpcsAddLaserCombobox.SelectedValue == null
                || this.npcsNpcsAddRocketsCombobox.SelectedValue == null
                || this.npcsNpcsAddRocketLauncherCombobox.SelectedValue == null
                || this.npcsNpcsAddRebuyRocketTypeCheckbox == null
                || this.npcsNpcsAddRebuyRocketLauncherTypeCheckbox == null
                || this.npcsNpcsAddCircleCheckbox == null
                || this.npcsNpcsAddUseAbilityCheckbox == null
                || this.npcsNpcsAddFinishNpcCheckbox == null
                || this.npcsNpcsAddOnlyFreeCheckbox == null
                || this.npcsNpcsAddUsePetModeCheckbox == null
                || this.npcsNpcsAddDroneFormationBelowFifteenCombobox.SelectedValue == null
                || this.npcsNpcsAddDroneFormationBelowFifteenCheckbox == null)
            {
                return;
            }
            var cfg = this.npcsNpcsAddConfigBtn.Content;
            var petMode = this.npcsNpcsAddPetModeCombobox.SelectedValue ?? 0;
            NpcSetting npcSetting = new()
            {
                IsEnabled = true,
                Name = text,
                Config = cfg != null ? int.Parse(cfg.ToString()) : 1,
                DroneFormation = (string)this.npcsNpcsAddDroneFormationCombobox.SelectedValue,
                LaserId = (string)this.npcsNpcsAddLaserCombobox.SelectedValue,
                Priority = (int)this.npcsNpcsAddPrioritySlider.Value,
                Radius = (int)this.npcsNpcsAddRadiusSlider.Value,
                RocketId = (string)this.npcsNpcsAddRocketsCombobox.SelectedValue,
                RocketLauncherId = (string)this.npcsNpcsAddRocketLauncherCombobox.SelectedValue,
                UseRockets = this.npcsNpcsAddRocketCheckbox.IsChecked!.Value,
                UseRocketLauncher = this.npcsNpcsAddRocketLauncherCheckbox.IsChecked!.Value,
                UseRSB = this.npcsNpcsAddRSBCheckbox.IsChecked!.Value,
                UseSAB = this.npcsNpcsAddSABCheckbox.IsChecked!.Value,
                RepairAt = (int)this.npcsNpcsAddRepairAtSlider.Value,
                UseRebuyLaser = this.npcsNpcsAddRebuyLaserTypeCheckbox.IsChecked!.Value,
                UseRebuyRocket = this.npcsNpcsAddRebuyRocketTypeCheckbox.IsChecked.Value,
                UseRebuyRocketLauncher = this.npcsNpcsAddRebuyRocketLauncherTypeCheckbox.IsChecked.Value,
                UseCircle = this.npcsNpcsAddCircleCheckbox.IsChecked.Value,
                UseAbility = this.npcsNpcsAddUseAbilityCheckbox.IsChecked.Value,
                Finish = this.npcsNpcsAddFinishNpcCheckbox.IsChecked.Value,
                OnlyFree = this.npcsNpcsAddOnlyFreeCheckbox.IsChecked.Value,
                UsePetMode = this.npcsNpcsAddUsePetModeCheckbox.IsChecked.Value,
                PetMode = (int)petMode,
                ShieldBelowFifteenFormation = (string)this.npcsNpcsAddDroneFormationBelowFifteenCombobox.SelectedValue,
                UseShieldBelowFifteenFormation = this.npcsNpcsAddDroneFormationBelowFifteenCheckbox.IsChecked!.Value,
            };
            if (this.Api.Settings!.NpcSettings.TryGetValue(npcSetting.Name, out NpcSetting? npc))
            {
                this.Api.Settings.NpcSettings[npc.Name].Config = npcSetting.Config;
                this.Api.Settings.NpcSettings[npc.Name].DroneFormation = npcSetting.DroneFormation;
                this.Api.Settings.NpcSettings[npc.Name].LaserId = npcSetting.LaserId;
                this.Api.Settings.NpcSettings[npc.Name].Priority = npcSetting.Priority;
                this.Api.Settings.NpcSettings[npc.Name].Radius = npcSetting.Radius;
                this.Api.Settings.NpcSettings[npc.Name].RocketId = npcSetting.RocketId;
                this.Api.Settings.NpcSettings[npc.Name].RocketLauncherId = npcSetting.RocketLauncherId;
                this.Api.Settings.NpcSettings[npc.Name].UseRockets = npcSetting.UseRockets;
                this.Api.Settings.NpcSettings[npc.Name].UseRocketLauncher = npcSetting.UseRocketLauncher;
                this.Api.Settings.NpcSettings[npc.Name].UseRSB = npcSetting.UseRSB;
                this.Api.Settings.NpcSettings[npc.Name].UseSAB = npcSetting.UseSAB;
                this.Api.Settings.NpcSettings[npc.Name].RepairAt = npcSetting.RepairAt;
                this.Api.Settings.NpcSettings[npc.Name].UseRebuyLaser = npcSetting.UseRebuyLaser;
                this.Api.Settings.NpcSettings[npc.Name].UseRebuyRocket = npcSetting.UseRebuyRocket;
                this.Api.Settings.NpcSettings[npc.Name].UseRebuyRocketLauncher = npcSetting.UseRebuyRocketLauncher;
                this.Api.Settings.NpcSettings[npc.Name].UseCircle = npcSetting.UseCircle;
                this.Api.Settings.NpcSettings[npc.Name].UseAbility = npcSetting.UseAbility;
                this.Api.Settings.NpcSettings[npc.Name].OnlyFree = npcSetting.OnlyFree;
                this.Api.Settings.NpcSettings[npc.Name].Finish = npcSetting.Finish;
                this.Api.Settings.NpcSettings[npc.Name].UsePetMode = npcSetting.UsePetMode;
                this.Api.Settings.NpcSettings[npc.Name].PetMode = npcSetting.PetMode;
                this.Api.Settings.NpcSettings[npc.Name].ShieldBelowFifteenFormation = npcSetting.ShieldBelowFifteenFormation;
                this.Api.Settings.NpcSettings[npc.Name].UseShieldBelowFifteenFormation = npcSetting.UseShieldBelowFifteenFormation;
            }
            else
            {
                this.Api.Settings.NpcSettings.Add(npcSetting);
            }
            this.ResetNpcInput();
        }

        private void ResetNpcInput()
        {
            this.npcsNpcsAddPrioritySlider.Value = 0;
            this.npcsNpcsAddRocketCheckbox.IsChecked = false;
            this.npcsNpcsAddRocketLauncherCheckbox.IsChecked = false;
            this.npcsNpcsAddRSBCheckbox.IsChecked = false;
            this.npcsNpcsAddSABCheckbox.IsChecked = false;
            this.npcsNpcsAddRebuyLaserTypeCheckbox.IsChecked = false;
            this.npcsNpcsAddRebuyRocketTypeCheckbox.IsChecked = false;
            this.npcsNpcsAddRebuyRocketLauncherTypeCheckbox.IsChecked = false;
            this.npcsNpcsAddCircleCheckbox.IsChecked = true;
            this.npcsNpcsAddOnlyFreeCheckbox.IsChecked = true;
            this.npcsNpcsAddFinishNpcCheckbox.IsChecked = true;
            this.npcsNpcsAddUsePetModeCheckbox.IsChecked = false;
            //npcsNpcsDataGrid.Items.Refresh();
            CollectionViewSource.GetDefaultView(this.npcsNpcsDataGrid.ItemsSource).Refresh();
            CollectionViewSource.GetDefaultView(this.npcsNpcsAddNameCombobox.ItemsSource).Refresh();
        }

        private void settingsTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Api.Statistics == null || !this.Ready)
            {
                return;
            }
            if (this.settingsTabControl.SelectedIndex == this.selectedTabIndex)
            {
                return;
            }
            if (this.settingsTabControl.SelectedIndex != 10 && this.settingsTabControl.SelectedIndex != 11)
            {
                this.selectedTabIndex = this.settingsTabControl.SelectedIndex;
            }
            if (this.settingsTabControl.SelectedIndex == 10)
            {
                this.settingsTabControl.SelectedIndex = this.selectedTabIndex;
                this.ImportSettingsHeader();
            }
            if (this.settingsTabControl.SelectedIndex == 11)
            {
                this.settingsTabControl.SelectedIndex = this.selectedTabIndex;
                this.ExportSettingsHeader();
            }
            App.SaveSettings();
        }

        private void npcsNpcsDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (this.npcsNpcsDataGrid.SelectedItem == null
                || this.npcsNpcsDataGrid.SelectedItem is not NpcSetting
                || this.npcsNpcsDataGrid.CurrentCell == null
                || this.npcsNpcsDataGrid.CurrentCell.Column == null
                || !this.npcsNpcsDataGrid.CurrentCell.IsValid)
            {
                return;
            }
            if (this.npcsNpcsDataGrid.CurrentCell.Column.DisplayIndex <= 0)
            {
                return;
            }
            var npcSetting = (NpcSetting)this.npcsNpcsDataGrid.SelectedItem;
            if (npcSetting == null)
            {
                return;
            }

            this.npcsNpcsAddNameCombobox.Text = npcSetting.Name;
            this.npcsNpcsAddConfigBtn.Content = npcSetting.Config;
            this.npcsNpcsAddDroneFormationCombobox.SelectedValue = npcSetting.DroneFormation;
            this.npcsNpcsAddLaserCombobox.SelectedValue = npcSetting.LaserId;
            this.npcsNpcsAddPrioritySlider.Value = npcSetting.Priority;
            this.npcsNpcsAddRadiusSlider.Value = (double)npcSetting.Radius;
            this.npcsNpcsAddRocketsCombobox.SelectedValue = npcSetting.RocketId;
            this.npcsNpcsAddRocketLauncherCombobox.SelectedValue = npcSetting.RocketLauncherId;
            this.npcsNpcsAddRocketCheckbox.IsChecked = npcSetting.UseRockets;
            this.npcsNpcsAddRocketLauncherCheckbox.IsChecked = npcSetting.UseRocketLauncher;
            this.npcsNpcsAddRSBCheckbox.IsChecked = npcSetting.UseRSB;
            this.npcsNpcsAddSABCheckbox.IsChecked = npcSetting.UseSAB;
            this.npcsNpcsAddRepairAtSlider.Value = npcSetting.RepairAt;
            this.npcsNpcsAddRebuyLaserTypeCheckbox.IsChecked = npcSetting.UseRebuyLaser;
            this.npcsNpcsAddRebuyRocketTypeCheckbox.IsChecked = npcSetting.UseRebuyRocket;
            this.npcsNpcsAddRebuyRocketLauncherTypeCheckbox.IsChecked = npcSetting.UseRebuyRocketLauncher;
            this.npcsNpcsAddCircleCheckbox.IsChecked = npcSetting.UseCircle;
            this.npcsNpcsAddUseAbilityCheckbox.IsChecked = npcSetting.UseAbility;
            this.npcsNpcsAddFinishNpcCheckbox.IsChecked = npcSetting.Finish;
            this.npcsNpcsAddOnlyFreeCheckbox.IsChecked = npcSetting.OnlyFree;
            this.npcsNpcsAddUsePetModeCheckbox.IsChecked = npcSetting.UsePetMode;
            this.npcsNpcsAddPetModeCombobox.SelectedValue = npcSetting.PetMode;
            this.npcsNpcsAddDroneFormationBelowFifteenCombobox.SelectedValue = npcSetting.ShieldBelowFifteenFormation;
            this.npcsNpcsAddDroneFormationBelowFifteenCheckbox.IsChecked = npcSetting.UseShieldBelowFifteenFormation;
        }

        public void SetLoginButtonEnabledState(bool isEnabled)
        {
            if (!this.Dispatcher.CheckAccess())
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.SetLoginButtonEnabledState(isEnabled);
                });
                return;
            }
            this.loginLoginBtn.IsEnabled = isEnabled;
        }

        private void loginLoginBtn_Click(object sender, RoutedEventArgs e)
        {
            this.SetLoginButtonEnabledState(false);
            this.Api.GameClient.Login();
            this.Api.Settings.DisplayName = (this.Api.Settings.DisplayName == "New Account" ? this.Api.Settings.Username : this.Api.Settings.DisplayName);
            OnNewAccountAdded?.Invoke(this, this.ApiControl);
        }

        private void loginPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            this.Api.Settings!.Password = this.loginPasswordBox.Password;
        }

        private void labLabUpgradeAddBtn_Click(object sender, RoutedEventArgs e)
        {
            var labType = this.labLabUpgradeTypeCombobox.SelectedIndex;
            var labUsingType = (string)this.labLabUpgradeUsingCombobox.SelectedValue;
            if (!CommandConstants.ORE_LOOKUP.TryGetValue(labUsingType, out var oreId))
            {
                return;
            }
            LabSetting labSetting = new()
            {
                IsEnabled = true,
                Type = labType,
                UsingOreType = oreId,
            };
            this.Api.Settings.LabSettings.Add(labSetting);
        }

        private void labCargoBtn_Click(object sender, RoutedEventArgs e)
        {
            var cargoUpgradeType = (string)this.labCargoCombobox.SelectedValue;
            if (!CommandConstants.ORE_LOOKUP.TryGetValue(cargoUpgradeType, out var oreId))
            {
                return;
            }
            CargoSettings cargoSettings = new()
            {
                IsEnabled = true,
                Type = oreId,
            };
            this.Api.Settings.CargoUpgradeSettings.Add(cargoSettings);
        }

        private void auctionCreditsTxtBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void auctionAddItemBtn_Click(object sender, RoutedEventArgs e)
        {
            var auctionItem = (string)this.auctionItemsCombobox.SelectedValue;
            var creditsAmount = long.Parse(this.auctionCreditsTxtBox.Text);
            if (!CommandConstants.AUCTION_NAMES.TryGetValue(auctionItem, out var auctionId))
            {
                Debug.WriteLine("DID NOT FIND " + auctionItem + " in AUCTION_NAMES!");
                return;
            }
            AuctionSetting auctionSetting = new()
            {
                CreditsAmount = creditsAmount,
                ItemName = auctionItem,
                ItemLoodId = auctionId,
                IsEnabled = true,
            };

            if (this.Api.Settings.AuctionSettings.Contains(auctionId))
            {
                this.Api.Settings.AuctionSettings[auctionId].ItemLoodId = auctionId;
                this.Api.Settings.AuctionSettings[auctionId].CreditsAmount = creditsAmount;
            }
            else
            {
                this.Api.Settings.AuctionSettings.Add(auctionSetting);
            }

            this.auctionCreditsTxtBox.Text = "10000";
            CollectionViewSource.GetDefaultView(this.auctionItemsDataGrid.ItemsSource).Refresh();
        }

        private void auctionItemsDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (this.auctionItemsDataGrid.SelectedItem == null || !(this.auctionItemsDataGrid.SelectedItem is AuctionSetting))
            {
                return;
            }
            if (this.auctionItemsDataGrid.CurrentCell.Column.DisplayIndex <= 0)
            {
                return;
            }
            var auctionSetting = (AuctionSetting)this.auctionItemsDataGrid.SelectedItem;
            if (auctionSetting == null)
            {
                return;
            }

            this.auctionItemsCombobox.SelectedValue = auctionSetting.ItemName;
            this.auctionCreditsTxtBox.Text = auctionSetting.CreditsAmount.ToString();
        }

        private void mapMapAreaStayCloseRangeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.mapMapAreaStayCloseRangeSliderLabel == null || this.mapMapAreaStayCloseRangeSlider == null)
            {
                return;
            }
            this.mapMapAreaStayCloseRangeSliderLabel.Content = Math.Round(this.mapMapAreaStayCloseRangeSlider.Value, 0);
        }

        private void mapMapAreaMapSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var mapId = (int)this.mapMapAreaMapSelectCombobox.SelectedValue;
            if (!Api.StarSystem.TryGetValue(mapId, out DarksideApi.DarkOrbit.Objects.StarSystem.StarMap? starMap))
            {
                return;
            }
            List<DarksideApi.DarkOrbit.Objects.StarSystem.StarPortal> dic = starMap.Portals;

            if (dic.Count <= 0)
            {
                this.SetMapAreaUiEnabled(false);
                return;
            }

            this.mapMapAreaStayCloseToCombobox.ItemsSource = dic;
            this.mapMapAreaStayCloseToCombobox.DisplayMemberPath = "DisplayName";
            this.mapMapAreaStayCloseToCombobox.SelectedValuePath = "GateId";
            this.SetMapAreaUiEnabled(true);
        }

        private void SetMapAreaUiEnabled(bool enabled)
        {
            this.mapMapAreaStayCloseToCombobox.IsEnabled = enabled;
            this.mapMapAreaStayCloseToLabel.IsEnabled = enabled;
            this.mapMapAreaStayCloseToRangeLabel.IsEnabled = enabled;
            this.mapMapAreaStayCloseRangeSlider.IsEnabled = enabled;
            this.mapMapAreaStayCloseRangeSliderLabel.IsEnabled = enabled;
            this.mapMapAreaAddBtn.IsEnabled = enabled;
        }

        private void mapMapAreaAddBtn_Click(object sender, RoutedEventArgs e)
        {
            var mapId = (int)this.mapMapAreaMapSelectCombobox.SelectedValue;
            var range = (double)this.mapMapAreaStayCloseRangeSlider.Value;
            var typeId = (int)this.mapMapAreaStayCloseToCombobox.SelectedValue;
            if (!Api.StarSystem.TryGetValue(mapId, out DarksideApi.DarkOrbit.Objects.StarSystem.StarMap? starMap))
            {
                Debug.WriteLine("DID NOT FIND MAPID IN STAR SYTEM!");
                return;
            }
            if (!starMap.Portals.Exists(p => p.GateId == typeId))
            {
                Debug.WriteLine("DID NOT FIND TYPE" + typeId + " IN GATE JUMPS!");
                return;
            }
            if (!CommandConstants.MAP_NAMES.TryGetValue(mapId, out var mapName))
            {
                Debug.WriteLine("DID NOT FIND " + mapId + " in AUCTION_NAMES!");
                return;
            }
            DarksideApi.DarkOrbit.Objects.StarSystem.StarPortal? type = starMap.Portals.FirstOrDefault(p => p.GateId == typeId);
            MapAreaSetting setting = new()
            {
                IsEnabled = true,
                AreaType = MapAreaSetting.MapAreaType.Portal,
                MapId = mapId,
                Range = range,
                GateId = type.GateId,
                TypeName = type.DisplayName,
            };

            //api.Settings.MapAreaSettings.Add(setting);
        }

        private void generalGeneralRoamConfig_Click(object sender, RoutedEventArgs e)
        {
            this.Api.Settings!.RoamConfig = HelpTools.GetConfig(this.Api.Settings!.RoamConfig);
        }

        private void generalGeneralRepairConfig_Click(object sender, RoutedEventArgs e)
        {
            this.Api.Settings!.RepairConfig = HelpTools.GetConfig(this.Api.Settings!.RepairConfig);
        }

        private void onAttackedFleeConfigBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Api.Settings!.OnAttackedFleeConfig = HelpTools.GetConfig(this.Api.Settings!.OnAttackedFleeConfig);
        }

        private void npcsNpcsAddConfigBtn_Click(object sender, RoutedEventArgs e)
        {
            this.npcsNpcsAddConfigBtn.Content = HelpTools.GetConfig(int.Parse(this.npcsNpcsAddConfigBtn.Content.ToString()!));
        }

        private void ggConfigBtn_Click(object sender, RoutedEventArgs e)
        {
            this.ggConfigBtn.Content = HelpTools.GetConfig(int.Parse(this.ggConfigBtn.Content.ToString()!));
        }

        private void abilityAbilityAddBtn_Click(object sender, RoutedEventArgs e)
        {
            var abilityName = (string)this.abilityAbilitesComboBox.SelectedValue;
            var abilityData = (int)this.abilityAbilitiesTypeDataSlider.Value;
            var abilityUsage = (string)this.abilityAbilitesTypeComboBox.SelectedValue;

            if (!AbilitySetting.ABILITY_USAGES.Contains(abilityUsage))
            {
                Debug.WriteLine("Ability Usage not found: " + abilityUsage);
                return;
            }
            if (!CommandConstants.SHIP_ABILITIES.TryGetValue(abilityName, out var abilityType))
            {
                Debug.WriteLine("Ability " + abilityName + " not found in Abilities!");
                return;
            }

            AbilitySetting setting = new()
            {
                IsEnabled = true,
                AbilityName = abilityName,
                AbilityType = abilityType,
                AbilityUsageData = abilityData,
                AbilityUsageType = abilityUsage,
            };

            this.Api.Settings.AbilitySettings.Add(setting);
        }

        private void abilityAbilitiesTypeDataSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.abilityAbilitiesTypeDataLabel == null || this.abilityAbilitiesTypeDataSlider == null)
            {
                return;
            }
            this.abilityAbilitiesTypeDataLabel.Content = this.abilityAbilitiesTypeDataSlider.Value;
        }

        private void abilityAbilitesTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.abilityAbilitiesTypeDataLabel == null || this.abilityAbilitiesTypeDataSlider == null || this.abilityAbilitesTypeComboBox == null)
            {
                return;
            }
            if (this.abilityAbilitesTypeComboBox.SelectedValue == AbilitySetting.ABILITY_USAGE_ALWAYS)
            {
                this.abilityAbilitiesTypeDataLabel.IsEnabled = false;
                this.abilityAbilitiesTypeDataSlider.IsEnabled = false;
                return;
            }
            this.abilityAbilitiesTypeDataLabel.IsEnabled = true;
            this.abilityAbilitiesTypeDataSlider.IsEnabled = true;
        }

        private void ggRadiusSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.ggRadiusSlider == null || this.ggRadiusSliderLabel == null)
            {
                return;
            }
            this.ggRadiusSliderLabel.Content = this.ggRadiusSlider.Value;
        }

        private void ggGalaxyGateAddBtn_Click(object sender, RoutedEventArgs e)
        {
            var ggId = (int)(this.ggGalaxyGateNameCombobox.SelectedValue ?? 0);
            var ggCfg = int.Parse(this.ggConfigBtn.Content.ToString()!);
            var ggRepCfg = int.Parse(this.ggRepairConfigBtn.Content.ToString()!);
            var ggFormaion = (string)this.ggFormationCombobox.SelectedValue;
            var ggRepFormation = (string)this.ggRepairFormationCombobox.SelectedValue;
            var ggCornerFormation = (string)this.ggCornerFormationCombobox.SelectedValue;
            var ggLaserId = (string)this.ggLaserCombobox.SelectedValue;
            var ggUseRocket = this.ggUseRocketCheckbox.IsChecked.Value;
            var ggRocketId = (string)this.ggRocketCombobox.SelectedValue;
            var ggUseLauncher = this.ggUseLauncherCheckbox.IsChecked.Value;
            var ggLauncherId = (string)this.ggRocketLauncherCombobox.SelectedValue;
            var ggCanCircle = this.ggCanCircleCheckbox.IsChecked.Value;
            var ggCollectBoxes = this.ggCollectBoxesCheckbox.IsChecked.Value;
            var ggRadius = this.ggRadiusSlider.Value;
            var ggUseSpin = this.ggSpinGateCheckbox.IsChecked.Value;
            var ggSpinAmount = (byte)(this.ggSpinGateAmountCombobox.SelectedIndex < 0 ? 0 : CommandConstants.GG_SPINNER_AMOUNT[this.ggSpinGateAmountCombobox.SelectedIndex]);
            var ggFillUri = this.ggSpinGateFillUriCheckbox.IsChecked.Value;
            var ggBuyLaser = this.ggRebuyLaserCheckbox.IsChecked.Value;
            var ggBuyRocket = this.ggRebuyRocketCheckbox.IsChecked.Value;
            var ggBuyLauncher = this.ggRebuyLauncherCheckbox.IsChecked.Value;
            var ggSpinPlace = this.ggSpinPlaceGateCheckbox.IsChecked.Value;
            var ggMaxDeaths = this.ggMaxDeathSlider.Value;
            var ggStopAtWave = this.ggStopAtWaveSlider.Value;
            var ggCircleWhileRepairing = this.ggCircleWhileRepairCheckbox.IsChecked.Value;
            var ggFinishFormation = (string)this.ggFinishFormationCombobox.SelectedValue;
            var ggUsePetMode = this.ggdUsePetModeCheckbox.IsChecked.Value;
            var ggPetMode = (int)this.ggPetModeCombobox.SelectedValue;

            Debug.WriteLine("SPIN AMOUNT: " + ggSpinAmount + ", selected Index: " + this.ggSpinGateAmountCombobox.SelectedIndex);
            if (!CommandConstants.GALAXY_GATES.TryGetValue(ggId, out var ggName))
            {
                Debug.WriteLine("GG Id not found: " + ggId);
                return;
            }
            if (!CommandConstants.GG_SPINNER_NAME_ID.TryGetValue(ggName, out var ggSpinId))
            {
                Debug.WriteLine("GG Spinner Id not found: " + ggName);
            }
            if (ggStopAtWave <= 0)
            {
                ggStopAtWave = -1;
            }

            GalaxyGateSetting setting = new()
            {
                IsEnabled = true,
                CollectBoxes = ggCollectBoxes,
                Config = ggCfg,
                DroneFormation = ggFormaion,
                GalaxyGateId = ggId,
                GalaxyGateName = ggName,
                LaserId = ggLaserId,
                RocketLauncherId = ggLauncherId,
                Radius = ggRadius,
                RepairConfig = ggRepCfg,
                RepairFormation = ggRepFormation,
                CornerFormation = ggCornerFormation,
                RocketId = ggRocketId,
                UseRocketLauncher = ggUseLauncher,
                UseRockets = ggUseRocket,
                UseCircle = ggCanCircle,
                SpinGateId = ggSpinId,
                SpinAmount = ggSpinAmount,
                SpinGenerator = ggUseSpin,
                SpinGateWithUri = ggFillUri,
                UseRebuyLaser = ggBuyLaser,
                UseRebuyRocket = ggBuyRocket,
                UseRebuyRocketLauncher = ggBuyLauncher,
                SpinPlaceGate = ggSpinPlace,
                MaxDeaths = (int)ggMaxDeaths,
                CircleWhileRepairing = ggCircleWhileRepairing,
                StopAtWave = (int)ggStopAtWave,
                FinishFormation = ggFinishFormation,
                UsePetMode = ggUsePetMode,
                PetMode = ggPetMode,
            };

            if (this.Api.Settings.GalaxyGateSettings.Contains(ggId))
            {
                this.Api.Settings.GalaxyGateSettings[ggId].CollectBoxes = setting.CollectBoxes;
                this.Api.Settings.GalaxyGateSettings[ggId].Config = setting.Config;
                this.Api.Settings.GalaxyGateSettings[ggId].DroneFormation = setting.DroneFormation;
                this.Api.Settings.GalaxyGateSettings[ggId].LaserId = setting.LaserId;
                this.Api.Settings.GalaxyGateSettings[ggId].RocketLauncherId = setting.RocketLauncherId;
                this.Api.Settings.GalaxyGateSettings[ggId].Radius = setting.Radius;
                this.Api.Settings.GalaxyGateSettings[ggId].RepairConfig = setting.RepairConfig;
                this.Api.Settings.GalaxyGateSettings[ggId].RepairFormation = setting.RepairFormation;
                this.Api.Settings.GalaxyGateSettings[ggId].CornerFormation = setting.CornerFormation;
                this.Api.Settings.GalaxyGateSettings[ggId].RocketId = setting.RocketId;
                this.Api.Settings.GalaxyGateSettings[ggId].UseRocketLauncher = setting.UseRocketLauncher;
                this.Api.Settings.GalaxyGateSettings[ggId].UseRockets = setting.UseRockets;
                this.Api.Settings.GalaxyGateSettings[ggId].UseCircle = setting.UseCircle;
                this.Api.Settings.GalaxyGateSettings[ggId].SpinGateId = ggSpinId;
                this.Api.Settings.GalaxyGateSettings[ggId].SpinAmount = ggSpinAmount;
                this.Api.Settings.GalaxyGateSettings[ggId].SpinGenerator = ggUseSpin;
                this.Api.Settings.GalaxyGateSettings[ggId].SpinGateWithUri = ggFillUri;
                this.Api.Settings.GalaxyGateSettings[ggId].UseRebuyLaser = ggBuyLaser;
                this.Api.Settings.GalaxyGateSettings[ggId].UseRebuyRocketLauncher = ggBuyLauncher;
                this.Api.Settings.GalaxyGateSettings[ggId].UseRebuyRocket = ggBuyRocket;
                this.Api.Settings.GalaxyGateSettings[ggId].SpinPlaceGate = ggSpinPlace;
                this.Api.Settings.GalaxyGateSettings[ggId].MaxDeaths = (int)ggMaxDeaths;
                this.Api.Settings.GalaxyGateSettings[ggId].CircleWhileRepairing = ggCircleWhileRepairing;
                this.Api.Settings.GalaxyGateSettings[ggId].StopAtWave = (int)ggStopAtWave;
                this.Api.Settings.GalaxyGateSettings[ggId].FinishFormation = ggFinishFormation;
                this.Api.Settings.GalaxyGateSettings[ggId].UsePetMode = ggUsePetMode;
                this.Api.Settings.GalaxyGateSettings[ggId].PetMode = ggPetMode;
            }
            else
            {
                this.Api.Settings.GalaxyGateSettings.Add(setting);
            }
            CollectionViewSource.GetDefaultView(this.ggGalaxyGatesDataGrid.ItemsSource).Refresh();

            this.ggMaxDeathSlider.Value = -1;
        }

        private void ggGalaxyGatesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ggGalaxyGatesDataGrid.SelectedItem == null || this.ggGalaxyGatesDataGrid.SelectedItem is not GalaxyGateSetting)
            {
                return;
            }
            if (this.ggGalaxyGatesDataGrid.CurrentCell.Column.DisplayIndex <= 0)
            {
                return;
            }
            var setting = (GalaxyGateSetting)this.ggGalaxyGatesDataGrid.SelectedItem;
            if (setting == null)
            {
                return;
            }

            this.ggGalaxyGateNameCombobox.SelectedValue = setting.GalaxyGateId;
            this.ggConfigBtn.Content = setting.Config;
            this.ggRepairConfigBtn.Content = setting.RepairConfig;
            this.ggFormationCombobox.SelectedValue = setting.DroneFormation;
            this.ggRepairFormationCombobox.SelectedValue = setting.RepairFormation;
            this.ggCornerFormationCombobox.SelectedValue = setting.CornerFormation;
            this.ggLaserCombobox.SelectedValue = setting.LaserId;
            this.ggUseRocketCheckbox.IsChecked = setting.UseRockets;
            this.ggRocketCombobox.SelectedValue = setting.RocketId;
            this.ggUseLauncherCheckbox.IsChecked = setting.UseRocketLauncher;
            this.ggRocketLauncherCombobox.SelectedValue = setting.RocketLauncherId;
            this.ggCanCircleCheckbox.IsChecked = setting.UseCircle;
            this.ggCollectBoxesCheckbox.IsChecked = setting.CollectBoxes;
            this.ggRadiusSlider.Value = setting.Radius;
            this.ggSpinGateCheckbox.IsChecked = setting.SpinGenerator;
            this.ggSpinGateAmountCombobox.SelectedIndex = CommandConstants.GG_SPINNER_AMOUNT.IndexOf(setting.SpinAmount);
            this.ggSpinGateFillUriCheckbox.IsChecked = setting.SpinGateWithUri;
            this.ggRebuyLaserCheckbox.IsChecked = setting.UseRebuyLaser;
            this.ggRebuyLauncherCheckbox.IsChecked = setting.UseRebuyRocketLauncher;
            this.ggRebuyRocketCheckbox.IsChecked = setting.UseRebuyRocket;
            this.ggSpinPlaceGateCheckbox.IsChecked = setting.SpinPlaceGate;
            this.ggMaxDeathSlider.Value = setting.MaxDeaths;
            this.ggCircleWhileRepairCheckbox.IsChecked = setting.CircleWhileRepairing;
            this.ggStopAtWaveSlider.Value = setting.StopAtWave;
            this.ggFinishFormationCombobox.SelectedValue = setting.FinishFormation;
            this.ggdUsePetModeCheckbox.IsChecked = setting.UsePetMode;
            this.ggPetModeCombobox.SelectedValue = setting.PetMode;
        }

        private void generalGroupInviteAddBtn_Click(object sender, RoutedEventArgs e)
        {
            var username = this.generalGroupInviteTextbox.Text;

            GroupSetting setting = new()
            {
                IsEnabled = true,
                Username = username,
            };

            this.Api.Settings.GroupSettings.Add(setting);

            this.generalGroupInviteTextbox.Text = string.Empty;
        }

        private void ggRepairConfigBtn_Click(object sender, RoutedEventArgs e)
        {
            this.ggRepairConfigBtn.Content = HelpTools.GetConfig(int.Parse(this.ggRepairConfigBtn.Content.ToString()!));
        }

        private void boxesBoxesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.boxesBoxesDataGrid.SelectedItem == null
                || this.boxesBoxesDataGrid.CurrentCell.Column == null
                || this.boxesBoxesDataGrid.SelectedItem is not BoxSetting setting)
            {
                return;
            }
            if (this.boxesBoxesDataGrid.CurrentCell.Column.DisplayIndex <= 0)
            {
                return;
            }
            //BoxSetting setting = (BoxSetting)boxesBoxesDataGrid.SelectedItem;
            if (setting == null)
            {
                return;
            }

            this.boxesBoxesAddCombobox.SelectedValue = setting.Type;
            this.boxesBoxesPrioritySlider.Value = setting.Priority;
        }

        private void loginLoginSessionIdBtn_Click(object sender, RoutedEventArgs e)
        {
            this.SetLoginButtonEnabledState(false);
            this.Api.GameClient.Login(true);
        }

        private void onAttackedShootbackCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (this.onAttackedFleeCheckbox == null || this.onAttackedShootbackCheckbox == null) { return; }
            this.Api.Settings.OnAttackedShootback = this.onAttackedShootbackCheckbox.IsChecked.Value;
            if (this.Api.Settings.OnAttackedShootback)
            {
                this.onAttackedFleeCheckbox.IsChecked = false;
            }
            this.UpdateFleeElements();
            this.UpdateShootbackElements();
        }

        private void onAttackedFleeCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (this.onAttackedFleeCheckbox == null || this.onAttackedShootbackCheckbox == null)
            {
                return;
            }

            this.Api.Settings.OnAttackedFlee = this.onAttackedFleeCheckbox.IsChecked.Value;
            if (this.Api.Settings.OnAttackedFlee)
            {
                this.onAttackedShootbackCheckbox.IsChecked = false;
            }

            this.UpdateFleeElements();
            this.UpdateShootbackElements();
        }

        private void UpdateFleeElements()
        {
            if (this.onAttackedFleeConfigBtn == null
                || this.onAttackedFleeConfigLabel == null
                || this.onAttackedFleeFormationCombobox == null
                || this.onAttackedFleeFormationLabel == null
                || this.onAttackedFleeUseMineCheckbox == null
                || this.onAttackedFleeUseMineCombobox == null
                || this.onAttackedFleeIgnorePvPGatesCheckbox == null)
            {
                return;
            }
            this.onAttackedFleeConfigBtn.IsEnabled = !this.Api.Settings.OnAttackedShootback;
            this.onAttackedFleeConfigLabel.IsEnabled = !this.Api.Settings.OnAttackedShootback;
            this.onAttackedFleeFormationCombobox.IsEnabled = !this.Api.Settings.OnAttackedShootback;
            this.onAttackedFleeFormationLabel.IsEnabled = !this.Api.Settings.OnAttackedShootback;
            this.onAttackedFleeUseMineCheckbox.IsEnabled = !this.Api.Settings.OnAttackedShootback;
            this.onAttackedFleeUseMineCombobox.IsEnabled = !this.Api.Settings.OnAttackedShootback;
            this.onAttackedFleeIgnorePvPGatesCheckbox.IsEnabled = !this.Api.Settings.OnAttackedShootback;
        }

        private void UpdateShootbackElements()
        {
            if (this.onAttackedShootbackConfigBtn == null
                || this.onAttackedShootbackConfigLabel == null
                || this.onAttackedShootbackDroneFormationCombobox == null
                || this.onAttackedShootbackDroneFormationCombobox == null
                || this.onAttackedShootbackDroneFormationLabel == null
                || this.onAttackedShootbackLaserCombobox == null
                || this.onAttackedShootbackLaserLabel == null
                || this.onAttackedShootbackRadiusLabel == null
                || this.onAttackedShootbackRadiusSlider == null
                || this.onAttackedShootbackRocketCheckbox == null
                || this.onAttackedShootbackRocketCheckbox == null
                || this.onAttackedShootbackRocketLauncherCheckbox == null
                || this.onAttackedShootbackRocketLauncherCombobox == null
                || this.onAttackedShootbackRocketsCombobox == null
                || this.onAttackedShootbackRSBCheckbox == null
                || this.onAttackedShootbackSABCheckbox == null
                || this.onAttackedShootbackAutoAttackClanlessCheckbox == null
                || this.onAttackedShootbackAutoAttackEnemiesCheckbox == null
                || this.onAttackedShootbackASupportAlliesCheckbox == null
                || this.onAttackedShootbackPetKillersCheckbox == null)
            {
                return;
            }
            this.onAttackedShootbackConfigBtn.IsEnabled = !this.Api.Settings.OnAttackedFlee;
            this.onAttackedShootbackConfigLabel.IsEnabled = !this.Api.Settings.OnAttackedFlee;
            this.onAttackedShootbackDroneFormationCombobox.IsEnabled = !this.Api.Settings.OnAttackedFlee;
            this.onAttackedShootbackDroneFormationLabel.IsEnabled = !this.Api.Settings.OnAttackedFlee;
            this.onAttackedShootbackLaserCombobox.IsEnabled = !this.Api.Settings.OnAttackedFlee;
            this.onAttackedShootbackLaserLabel.IsEnabled = !this.Api.Settings.OnAttackedFlee;
            this.onAttackedShootbackRadiusLabel.IsEnabled = !this.Api.Settings.OnAttackedFlee;
            this.onAttackedShootbackRadiusSlider.IsEnabled = !this.Api.Settings.OnAttackedFlee;
            this.onAttackedShootbackRocketCheckbox.IsEnabled = !this.Api.Settings.OnAttackedFlee;
            this.onAttackedShootbackRocketLauncherCheckbox.IsEnabled = !this.Api.Settings.OnAttackedFlee;
            this.onAttackedShootbackRocketLauncherCombobox.IsEnabled = !this.Api.Settings.OnAttackedFlee;
            this.onAttackedShootbackRocketsCombobox.IsEnabled = !this.Api.Settings.OnAttackedFlee;
            this.onAttackedShootbackRSBCheckbox.IsEnabled = !this.Api.Settings.OnAttackedFlee;
            this.onAttackedShootbackSABCheckbox.IsEnabled = !this.Api.Settings.OnAttackedFlee;
            this.onAttackedShootbackAutoAttackClanlessCheckbox.IsEnabled = !this.Api.Settings.OnAttackedFlee;
            this.onAttackedShootbackAutoAttackEnemiesCheckbox.IsEnabled = !this.Api.Settings.OnAttackedFlee;
            this.onAttackedShootbackASupportAlliesCheckbox.IsEnabled = !this.Api.Settings.OnAttackedFlee;
            this.onAttackedShootbackPetKillersCheckbox.IsEnabled = !this.Api.Settings.OnAttackedFlee;
        }

        private void onAttackedShootbackConfigBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Api.Settings.OnAttackedShootbackConfig = HelpTools.GetConfig(this.Api.Settings.OnAttackedShootbackConfig);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        private void npcsNpcsAddRadiusSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.npcsNpcsAddRadiusSlider == null || this.npcsNpcsAddRadiusSliderLabel == null)
            {
                return;
            }
            this.npcsNpcsAddRadiusSliderLabel.Content = (int)this.npcsNpcsAddRadiusSlider.Value;
        }

        private void MenuItemDropMine_Click(object sender, RoutedEventArgs e)
        {
            this.Api.GameMethods.UseMine(CommandConstants.EQUIPMENT_MINE_ACM);
        }

        private void npcsNpcsAddRepairAtSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.npcsNpcsAddRepairAtSlider == null || this.npcsNpcsAddRepairAtSliderLabel == null)
            {
                return;
            }
            this.npcsNpcsAddRepairAtSliderLabel.Content = (int)this.npcsNpcsAddRepairAtSlider.Value;
        }

        private void dispatchDispatcherAddBtn_Click(object sender, RoutedEventArgs e)
        {
            DispatcherObj dispatcherObj = ConstantCollection.DISPATCHER_DEFAULT_LIST.ElementAt(this.dispatchDispatcherCombobox.SelectedIndex);
            if (dispatcherObj == null)
            {
                return;
            }
            DispatcherSetting setting = new()
            {
                IsEnabled = true,
                Duration = dispatcherObj.Duration,
                Id = dispatcherObj.Id,
                Name = dispatcherObj.Name,
            };
            if (this.Api.Settings.DispatcherSettings.Contains(setting.Id))
            {
                this.Api.Settings.DispatcherSettings.Remove(setting.Id);
            }
            this.Api.Settings.DispatcherSettings.Add(setting);
        }

        private void boxesOresSellHangarCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (this.boxesOresSellHangarCombobox == null || this.boxesOresSellHangarCheckbox == null || this.boxesOresSellEverythingCheckbox == null)
            {
                return;
            }
            this.boxesOresSellHangarCombobox.IsEnabled = this.boxesOresSellHangarCheckbox.IsChecked.Value;
            this.boxesOresSellEverythingCheckbox.IsEnabled = this.boxesOresSellHangarCheckbox.IsChecked.Value;
        }

        private void mapMapUseMaxDeathCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (this.mapMapMaxDeathsSlider == null || this.mapMapUseMaxDeathCheckbox == null)
            {
                return;
            }
            this.mapMapMaxDeathsSlider.IsEnabled = this.mapMapUseMaxDeathCheckbox.IsChecked.Value;
            this.mapMapMaxDeathsSliderLabel.IsEnabled = this.mapMapUseMaxDeathCheckbox.IsChecked.Value;
        }

        private void ImportSettingsHeader()
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog openFileDialog = new()
            {
                // Set filter for file extension and default file extension 
                CheckFileExists = true,
                Title = "Import Darkside Settings File",
                DefaultExt = ".dss",
                Filter = "Darkside Settings File (*.dss)|*.dss",
            };

            // Display OpenFileDialog by calling ShowDialog method 
            var result = openFileDialog.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result != true)
            {
                return;
            }
            // Open document 
            var filename = openFileDialog.FileName;
            DarkOrbotSettings? settings = JsonConvert.DeserializeObject<DarkOrbotSettings>(GZipUtilities.UnzipFromString(System.IO.File.ReadAllText(filename)));
            this.Api.LoadSettings(settings);

            if (!App.Apis.Contains(this.Api.Settings.ApiKey))
            {
                App.Apis.Add(this.ApiControl);
            }
            this.settingsTabControl.SelectedIndex = this.selectedTabIndex;
        }

        private void ExportSettingsHeader()
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new()
            {
                Title = "Export Darkside Settings File",
                DefaultExt = ".dss",
                Filter = "Darkside Settings File (*.dss)|*.dss",
            };

            var result = saveFileDialog.ShowDialog();

            if (result != true)
            {
                return;
            }
            DarkOrbotSettings settingsObj = this.Api.Settings;
            settingsObj.IsRunning = false;

            var settings = JsonConvert.SerializeObject(settingsObj);
            File.WriteAllText(saveFileDialog.FileName, GZipUtilities.ZipToString(settings));

            this.settingsTabControl.SelectedIndex = this.selectedTabIndex;
        }

        private void loginSolveCaptchaCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (this.loginSolveCaptchaCheckbox == null || this.loginSolveCaptchaApiKeyTextbox == null)
            {
                return;
            }
            this.loginSolveCaptchaApiKeyTextbox.IsEnabled = this.loginSolveCaptchaCheckbox.IsChecked.Value;
        }

        private void ggMaxDeathSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.ggMaxDeathSliderLabel == null || this.ggMaxDeathSlider == null)
            {
                return;
            }
            this.ggMaxDeathSliderLabel.Content = this.ggMaxDeathSlider.Value.ToString();
        }

        private void ggStopAtWaveSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.ggStopAtWaveSliderLabel == null || this.ggStopAtWaveSlider == null)
            {
                return;
            }
            this.ggStopAtWaveSliderLabel.Content = ((int)this.ggStopAtWaveSlider.Value <= 0 ? "Never" : ((int)this.ggStopAtWaveSlider.Value).ToString());
        }
        private void npcsNpcsAddNameCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.npcsNpcsDataGrid == null || this.npcsNpcsAddNameCombobox == null || this.npcsNpcsAddRadiusSlider == null)
            {
                return;
            }
            if (this.npcsNpcsDataGrid.IsFocused)
                return;

            var npcName = this.npcsNpcsAddNameCombobox.SelectedValue;
            if (npcName == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(npcName.ToString()) || !CommandConstants.NPC_DEFAULT_RANGES.TryGetValue(npcName.ToString(), out var range))
            {
                Debug.WriteLine("Npc not found in default ranges!");
                return;
            }

            this.npcsNpcsAddRadiusSlider.Value = range;
        }

        private void generalPetModeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.generalPetModeOptionsCombobox == null || this.generalPetModeOptionsLabel == null)
            {
                return;
            }
            this.generalPetModeOptionsCombobox.IsEnabled = (int)this.generalPetModeCombobox.SelectedValue == CommandConstants.PET_GEAR_ENEMY_LOCATOR;
            this.generalPetModeOptionsLabel.IsEnabled = (int)this.generalPetModeCombobox.SelectedValue == CommandConstants.PET_GEAR_ENEMY_LOCATOR;
        }

        private void tasksAddTaskTypesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.tasksAddTaskTypesComboBox == null
                || this.tasksAddTaskDurationSlider == null
                || this.tasksAddMapSelectCombobox == null
                || this.tasksAddMapSelectLabel == null)
            {
                return;
            }
            if (((TaskType)this.tasksAddTaskTypesComboBox.SelectedValue) != TaskType.ChangeWorkingMap)
            {
                this.tasksAddMapSelectCombobox.IsEnabled = false;
                this.tasksAddMapSelectLabel.IsEnabled = false;
                return;
            }
            this.tasksAddMapSelectCombobox.IsEnabled = true;
            this.tasksAddMapSelectLabel.IsEnabled = true;
        }

        private void tasksAddTaskDurationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.tasksAddTaskDurationValueLabel == null || this.tasksAddTaskDurationSlider == null)
            {
                return;
            }
            this.tasksAddTaskDurationValueLabel.Content = $"{Math.Round(this.tasksAddTaskDurationSlider.Value, 0)}Min";
        }

        private void tasksTasksDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.tasksTasksDataGrid.SelectedItem == null || this.tasksTasksDataGrid.SelectedItem is not TaskSetting)
            {
                return;
            }
            if (this.tasksAddTaskTypesComboBox == null || this.tasksAddTaskDurationSlider == null || this.tasksAddMapSelectCombobox == null)
            {
                return;
            }
            if (this.tasksTasksDataGrid.CurrentCell.Column.DisplayIndex <= 0)
            {
                return;
            }
            var setting = (TaskSetting)this.tasksTasksDataGrid.SelectedItem;
            if (setting == null)
            {
                return;
            }

            this.tasksAddTaskTypesComboBox.SelectedValue = setting.TaskType;
            this.tasksAddTaskDurationSlider.Value = setting.Duration;
            this.tasksAddMapSelectCombobox.SelectedValue = setting.Option;
        }

        private void tasksAddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var taskItem = (TaskType)this.tasksAddTaskTypesComboBox.SelectedValue;
            var taskDuration = this.tasksAddTaskDurationSlider.Value;
            var option = this.tasksAddMapSelectCombobox.SelectedValue ?? -1;
            if (!ConstantCollection.TASK_TYPES.ContainsKey(taskItem))
            {
                Debug.WriteLine("DID NOT FIND " + taskItem + " in TASK_TYPES!");
                return;
            }
            if (taskItem == TaskType.ChangeWorkingMap)
            {
                if ((int)option <= 0)
                {
                    Debug.WriteLine("INVALID OPTION FOR TASK TYPE ChangeWorkingMap: " + option);
                    return;
                }
                taskDuration = 1;
            }
            TaskSetting taskSetting = new()
            {
                TaskType = taskItem,
                Duration = taskDuration,
                Option = (int)option,
                IsEnabled = true,
            };

            this.Api.Settings.TaskSettings.Add(taskSetting);

            CollectionViewSource.GetDefaultView(this.auctionItemsDataGrid.ItemsSource).Refresh();
        }

        private void tasksUseTaskSystemCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (this.tasksUseTaskSystemCheckbox == null)
            {
                return;
            }
            if (this.Api.Settings.UseTaskSystem)
            {
                return;
            }
            this.Api.GameObservers.TaskManager.StopTasks();
        }

        private void loginLoggingCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (this.loginLoggingCheckbox == null)
                return;

            var state = this.loginLoggingCheckbox.IsChecked.Value;

            if (state)
                this.Api.Logging.Open();
            else
                this.Api.Logging.Close();
        }

        private void mapMapTimeTableSelectCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void mapMapTimeTableSelectTimeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void mapMapTimeTableSelectAddBtn_Click(object sender, RoutedEventArgs e)
        {
            var mapIdSelected = this.mapMapTimeTableSelectCombobox.SelectedValue;
            var timeSelected = this.mapMapTimeTableSelectTimeCombobox.SelectedValue.ToString();

            if (mapIdSelected == null || timeSelected == null)
            {
                Debug.WriteLine("Invalid input for Time Table!");
                return;
            }

            if (!DateTime.TryParse($"01.01.1998 {timeSelected}", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
            {
                Debug.WriteLine("Could not parse to Date!");
                return;
            }

            WorkMapSetting setting = new()
            {
                IsEnabled = true,
                MapId = (int)mapIdSelected,
                StartTimeUTC = parsed,
                StartTimeUTCValue = timeSelected,
            };

            if (this.Api.Settings.WorkMapSettings.Contains(setting.StartTimeUTCValue))
            {
                this.Api.Settings.WorkMapSettings[setting.StartTimeUTCValue].MapId = (int)mapIdSelected;
                this.Api.Settings.WorkMapSettings[setting.StartTimeUTCValue].StartTimeUTCValue = timeSelected;
                this.Api.Settings.WorkMapSettings[setting.StartTimeUTCValue].StartTimeUTC = parsed;
            }
            else
            {
                this.Api.Settings.WorkMapSettings.Add(setting);
            }

            CollectionViewSource.GetDefaultView(this.mapMapTimeTableDataGrid.ItemsSource).Refresh();
        }

        private void mapMapTimeTableDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.mapMapTimeTableDataGrid.SelectedItem == null || this.mapMapTimeTableDataGrid.SelectedItem is not WorkMapSetting workMapSetting)
            {
                return;
            }
            if (this.mapMapTimeTableSelectCombobox == null || this.mapMapTimeTableSelectTimeCombobox == null)
            {
                return;
            }
            if (this.mapMapTimeTableDataGrid.CurrentCell.Column.DisplayIndex <= 0)
            {
                return;
            }
            if (workMapSetting == null)
            {
                return;
            }

            this.mapMapTimeTableSelectCombobox.SelectedValue = workMapSetting.MapId;
            this.mapMapTimeTableSelectTimeCombobox.SelectedIndex = CommandConstants.TIME_TABLE.IndexOf(workMapSetting.StartTimeUTCValue);
        }

        private void MapMapUseAdvancedJumpCpuCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (this.mapMapUseAdvancedJumpCpuCheckbox == null || this.mapMapUseAdvancedJumpCpuMinPathLengthBtn == null)
            {
                return;
            }
            this.mapMapUseAdvancedJumpCpuMinPathLengthBtn.IsEnabled = this.mapMapUseAdvancedJumpCpuCheckbox.IsChecked.Value;
        }

        private void MapMapUseAdvancedJumpCpuMinPathLengthBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Api.Settings.MapUseAdvancedJumpCpuMinPathLength = (Api.Settings.MapUseAdvancedJumpCpuMinPathLength + 1) % 7;
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.snapUtil == null)
            {
                return;
            }

            if ((Visibility)e.NewValue == Visibility.Visible)
            {
                this.snapUtil.Attach();
                return;
            }
            this.snapUtil.Detach();
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            App.SaveSettings();
            this.Hide();
        }
    }
}
