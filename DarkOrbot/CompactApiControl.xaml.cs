using DarkOrbot.Render;
using DarkOrbot.Util.Extensions;
using DarksideApi;
using DarksideApi.DarkOrbit;
using DarksideApi.DarkOrbit.Settings;
using DarksideApi.Util;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace DarkOrbot
{
    /// <summary>
    /// Interaktionslogik für CompactApiControl.xaml
    /// </summary>
    public partial class CompactApiControl : UserControl, IControl
    {
        public Api Api { get; private set; }
        public MapRenderer? PrototypRender { get; private set; }
        public MainWindow? mainWindow;
        private CompactSettingsControl? settingsControl;
        private CompactStatisticsControl? statisticsControl;
        private CompactBoosterControl? boosterControl;
        private CompactQuestControl? questControl;
        private CompactShipControl? shipControl;
        private CompactGroupControl? groupControl;
        private CompactLogControl? logControl;

        private DispatcherTimer? dispatcherTimer;
        private bool middeMouseDown;

        public event EventHandler<CompactApiControl>? OnNewAccountAdded;
        public event EventHandler<CompactApiControl>? OnAccountRemove;

        public bool Ready { get; set; }
        public string ApiKey => this.Api.Settings.ApiKey;
        public DarkOrbotSettings Settings => this.Api.Settings;
        public bool HasMainWindow => this.mainWindow != null;

        // Speichert die Offsets (Top, Left) relativ zum mainWindow für jedes gezeigte Fenster
        private readonly Dictionary<Window, (double topOffset, double leftOffset)> windowOffsets = new();

        public CompactApiControl(DarkOrbotSettings settings, string injectionKey, MainWindow? mainWindow)
        {
            this.Api = new(settings, injectionKey);
            this.mainWindow = mainWindow;
            this.InitializeComponent();
            this.Init();
        }

#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Fügen Sie ggf. den „erforderlichen“ Modifizierer hinzu, oder deklarieren Sie den Modifizierer als NULL-Werte zulassend.
        private CompactApiControl() { }
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Fügen Sie ggf. den „erforderlichen“ Modifizierer hinzu, oder deklarieren Sie den Modifizierer als NULL-Werte zulassend.

        public static CompactApiControl Empty()
        {
            return new CompactApiControl();
        }

        private void Init()
        {
            this.dispatcherTimer = new DispatcherTimer(DispatcherPriority.Background)
            {
                Interval = TimeSpan.FromMilliseconds(1000)
            };
            this.PrototypRender = new MapRenderer(this.Api, this.generalGameCanvas.Width, this.generalGameCanvas.Height);
            this.generalGameCanvas.Children.Add(this.PrototypRender);

            this.settingsControl = new CompactSettingsControl(this);
            this.statisticsControl = new CompactStatisticsControl(this);
            this.boosterControl = new CompactBoosterControl(this);
            this.questControl = new CompactQuestControl(this);
            this.shipControl = new CompactShipControl(this);
            this.groupControl = new CompactGroupControl(this);
            this.logControl = new CompactLogControl(this);

            // MainWindow-Events abonnieren, damit geöffnete Controls folgen
            if (this.mainWindow != null)
            {
                this.mainWindow.LocationChanged += MainWindow_LocationChanged;
                this.mainWindow.SizeChanged += MainWindow_SizeChanged;
                this.mainWindow.StateChanged += MainWindow_StateChanged;
            }

            this.Api.GameClient.GameStateChanged += (s, e) =>
            {
                Settings.IsLoggingIn = false;
                Settings.IsInGame = false;
                if (e == GameState.Ready)
                {
                    Settings.IsInGame = true;
                    return;
                }
                if (e == GameState.LoggingIn || e == GameState.CollectingLoginBonus || e == GameState.Loading)
                {
                    Settings.IsLoggingIn = true;
                    return;
                }
            };
            this.Api.OnLastPosition += (s, e) => this.PrototypRender.AddLastPosition(e);
            this.Api.OnAddMenuItems += (s, e) =>
            {
                if (!this.Dispatcher.CheckAccess())
                {
                    this.Dispatcher.Invoke(() => this.AddMenuItems(e));
                    return;
                }
                this.AddMenuItems(e);
            };

            if (this.Api.Settings.AutoLogin)
            {
                this.Api.GameClient?.Login();
            }

            this.Api.WriteLog($"Darkside {Api.Version}\n");
            this.Api.WriteLog($"Support the project: https://shorturl.at/DMQys");
            this.dispatcherTimer.Start();

            foreach (var licenseStateText in App.LicenseManager!.LicenseStateTexts)
            {
                this.Api.WriteLog(licenseStateText);
            }

            this.Ready = true;
        }

        private void AddMenuItems(List<DarksideApi.DarkOrbit.Objects.Hero.MenuItem> menuItems)
        {
            menuItems.ForEach(i => { this.Api.Menu.Add(i); this.Api.Menu.AddAvailableMenuItem(i); });
            CollectionViewSource.GetDefaultView(this.generalGameCanvasContextMenu.Items).Refresh();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.PrototypRender?.Start();
        }

        private void generalHideGameBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Api.Settings!.IsMinimapShown = !this.Api.Settings.IsMinimapShown;
            if (this.Api.Settings.IsMinimapShown)
            {
                this.generalGameCanvas.Visibility = Visibility.Visible;
                return;
            }
            this.generalGameCanvas.Visibility = Visibility.Hidden;
        }

        private void generalStartBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.Api.Settings!.IsRunning)
            {
                this.Api.Logic.StopLogic();
                return;
            }
            this.Api.Logic.StartLogic();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.SaveSettings();
            //MapRenderer?.Stop();
            this.PrototypRender?.Stop();
            this.Api.GameMethods?.GameLogout();
            this.Api.Logic?.StopLogic();
            this.Api.BackpageApi?.ShutDown();
            this.Api.GameClient?.Disconnect();
            this.Api.Logging?.Close();
            Application.Current.Shutdown();
        }

        private void generalCollectBoxesBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Api.Settings!.GeneralCollectBoxes = !this.Api.Settings.GeneralCollectBoxes;
        }

        private void generalShootNpcsBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Api.Settings!.GeneralShootNpcs = !this.Api.Settings.GeneralShootNpcs;
        }

        private void generalPassiveShootNpcsBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Api.Settings!.GeneralPassiveShootNpcs = !this.Api.Settings.GeneralPassiveShootNpcs;
        }

        private void darkOrbotMainWindow_LocationChanged(object sender, EventArgs e)
        {
            //MapRenderer?.UpdateCanvasSize((float)this.generalGameCanvas.Width, (float)this.generalGameCanvas.Height);
            this.PrototypRender?.UpdateCanvasSize(this.generalGameCanvas.Width, this.generalGameCanvas.Height);
        }

        private void darkOrbotMainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //MapRenderer?.UpdateCanvasSize((float)this.generalGameCanvas.Width, (float)this.generalGameCanvas.Height);
            this.PrototypRender?.UpdateCanvasSize(this.generalGameCanvas.Width, this.generalGameCanvas.Height);
        }

        private void generalOpenHomepageBtn_Click(object sender, RoutedEventArgs e)
        {
            var url = $"https://www.buymeacoffee.com/delemojo";
            Process.Start(url);
        }

        private void generalGameCanvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this.Api.GameClient.GameState != GameState.Ready)
            {
                return;
            }
            if (e.ChangedButton != System.Windows.Input.MouseButton.Left
                && e.ChangedButton != System.Windows.Input.MouseButton.Middle)
            {
                return;
            }
            Point screenMapPosition = e.GetPosition(this.generalGameCanvas);
            var gameMapPosition = this.PrototypRender!.ReverseScale((int)screenMapPosition.X, (int)screenMapPosition.Y).ToVector2();
            if (e.ChangedButton == System.Windows.Input.MouseButton.Middle)
            {
                switch (e.ButtonState)
                {
                    case System.Windows.Input.MouseButtonState.Pressed:
                        this.PrototypRender?.SetCurrentMousePosition(gameMapPosition.ToDrawingPoint());
                        this.middeMouseDown = true;
                        this.Api.Settings.WorkAreaSettings.Remove(this.Api.Map.Id);
                        var workAreaSetting = new WorkAreaSetting()
                        {
                            MapId = this.Api.Map.Id,
                            CornerTopLeft = gameMapPosition,
                            CornerBottomRight = default,
                            IsEnabled = true,
                        };
                        this.Api.Settings.WorkAreaSettings.Add(workAreaSetting);
                        Debug.WriteLine("Added Work Area Position: " + gameMapPosition);
                        break;
                    default:
                        Debug.WriteLine("Unknown button state: " + e.ButtonState);
                        break;
                }
                return;
            }
            this.Api.GameMethods.Move(gameMapPosition);
        }

        private void generalGameCanvas_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this.Api.GameClient.GameState != GameState.Ready)
            {
                return;
            }
            if (e.ChangedButton != System.Windows.Input.MouseButton.Middle)
            {
                return;
            }
            if (!this.Api.Settings.WorkAreaSettings.TryGetValue(this.Api.Map.Id, out _))
            {
                return;
            }
            Point screenMapPosition = e.GetPosition(this.generalGameCanvas);
            var gameMapPosition = this.PrototypRender!.ReverseScale((int)screenMapPosition.X, (int)screenMapPosition.Y).ToVector2();
            switch (e.ButtonState)
            {
                case System.Windows.Input.MouseButtonState.Released:

                    if (!this.Api.Settings.WorkAreaSettings.TryGetValue(this.Api.Map.Id, out WorkAreaSetting? workArea))
                    {
                        Debug.WriteLine("Work Area entry missing!");
                        this.middeMouseDown = false;
                        return;
                    }

                    workArea.CornerBottomRight = gameMapPosition;
                    Debug.WriteLine("Closed Work Area at: " + gameMapPosition);
                    this.middeMouseDown = false;

                    if (HelpTools.CalculateDistance(workArea.CornerTopLeft, workArea.CornerBottomRight) < 1000)
                    {
                        this.Api.Settings.WorkAreaSettings.Remove(this.Api.Map.Id);
                        Debug.WriteLine("Removed Workarea -> too small!");
                    }

                    workArea.CheckWorkArea();

                    break;
                default:
                    Debug.WriteLine("Unknown button state: " + e.ButtonState);
                    break;
            }
        }

        private void generalGameCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (this.Api.GameClient is null)
            {
                return;
            }
            if (this.Api.GameClient.GameState != GameState.Ready)
            {
                return;
            }
            if (!this.middeMouseDown)
            {
                return;
            }
            Point screenMapPosition = e.GetPosition(this.generalGameCanvas);
            Point gameMapPosition = this.PrototypRender!.ReverseScale((int)screenMapPosition.X, (int)screenMapPosition.Y);
            this.PrototypRender?.SetCurrentMousePosition(gameMapPosition);
        }

        private void generalGameCanvas_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (!this.Api.Settings.IsMinimapPlayerView)
            {
                return;
            }
            if (e.Delta > 0)
            {
                this.PrototypRender?.IncreaseZoom();
            }
            if (e.Delta < 0)
            {
                this.PrototypRender?.DecreaseZoom();
            }
        }

        private void generalDoGalaxyGatesBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Api.Settings.GeneralDoGalaxyGates = !this.Api.Settings.GeneralDoGalaxyGates;
        }

        private void generalShowFullMapBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Api.Settings.IsMinimapPlayerView = !this.Api.Settings.IsMinimapPlayerView;
        }

        private void generalSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!this.Ready || this.settingsControl == null)
                return;

            if (this.settingsControl.Visibility == Visibility.Visible)
            {
                Debug.WriteLine("Hiding Settings Control");
                this.settingsControl.Hide();
                App.SaveSettings();
                return;
            }
            this.ShowWindow(this.settingsControl, 0, this.Width + 5);
        }

        private void generalShipInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!this.Ready || this.shipControl == null)
                return;

            if (this.shipControl.Visibility == Visibility.Visible)
            {
                Debug.WriteLine("Hiding Ship Control");
                this.shipControl.Hide();
                return;
            }
            this.ShowWindow(this.shipControl, 0, -this.shipControl.Width);
        }

        private void generalQuestInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!this.Ready || this.questControl == null)
                return;

            if (this.questControl.Visibility == Visibility.Visible)
            {
                Debug.WriteLine("Hiding Quest Control");
                this.questControl.Hide();
                return;
            }
            this.ShowWindow(this.questControl, 0 + (this.Height - this.questControl.Height) / 2, this.Width + 5);
        }

        private void generalGroupInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!this.Ready || this.groupControl == null)
                return;

            if (this.Api.Group == null || this.Api.Group.MemberCount <= 0)
                return;

            if (this.groupControl.Visibility == Visibility.Visible)
            {
                Debug.WriteLine("Hiding Group Control");
                this.groupControl.Hide();
                return;
            }
            this.ShowWindow(this.groupControl, 0 + (this.Height - this.groupControl.Height) / 2 - 5, this.Width + 5);
        }

        private void generalBoosterInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!this.Ready || this.boosterControl == null)
                return;

            if (this.Api.Hero.ActiveBoosters.Count <= 0)
                return;

            if (this.boosterControl.Visibility == Visibility.Visible)
            {
                Debug.WriteLine("Hiding Booster Control");
                this.boosterControl.Hide();
                return;
            }
            this.ShowWindow(this.boosterControl, 0 + (this.Height - this.boosterControl.Height) / 2 + 5, this.Width + 5);
        }

        private void generalStatisticsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!this.Ready || this.statisticsControl == null)
                return;

            if (this.statisticsControl.Visibility == Visibility.Visible)
            {
                Debug.WriteLine("Hiding Statistics Control");
                this.statisticsControl.Hide();
                return;
            }
            this.ShowWindow(this.statisticsControl, 0, this.Width + 5);
        }

        private void generalLogBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!this.Ready || this.logControl == null)
                return;

            if (this.logControl.Visibility == Visibility.Visible)
            {
                Debug.WriteLine("Hiding Log Control");
                this.logControl.Hide();
                return;
            }
            this.ShowWindow(this.logControl, this.Height + 82, 10);
        }

        private void ShowWindow(Window window, double top = 0, double left = 5)
        {
            if (mainWindow == null)
                return;

            window.Owner = mainWindow;
            window.Top = mainWindow.Top + top;
            window.Left = mainWindow.Left + left;
            window.WindowState = WindowState.Normal;
            window.Show();

            // Offset speichern, damit Fenster dem mainWindow später folgen kann
            windowOffsets[window] = (top, left);

            Debug.WriteLine("Window activated: " + window.Activate());
        }

        // Repositioniert geöffnete Fenster basierend auf gespeicherten Offsets
        private void UpdateChildWindowsPositions()
        {
            if (this.mainWindow == null)
                return;

            foreach (var kv in windowOffsets)
            {
                var window = kv.Key;
                var offsets = kv.Value;
                if (window == null)
                    continue;
                if (window.Visibility == Visibility.Visible)
                {
                    window.Top = this.mainWindow.Top + offsets.topOffset;
                    window.Left = this.mainWindow.Left + offsets.leftOffset;
                }
            }
        }

        private void MainWindow_LocationChanged(object? sender, EventArgs e)
        {
            UpdateChildWindowsPositions();
        }

        private void MainWindow_SizeChanged(object? sender, SizeChangedEventArgs e)
        {
            UpdateChildWindowsPositions();
        }

        private void MainWindow_StateChanged(object? sender, EventArgs e)
        {
            // Bei Zustandwechsel (z. B. Maximieren/Minimieren) ebenfalls Positionen anpassen
            UpdateChildWindowsPositions();
        }

        private void generalLogoutApiBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!Settings.IsInGame && !Settings.IsLoggingIn)
            {
                this.Api.GameClient.Login();
                this.Api.Settings.DisplayName = (this.Api.Settings.DisplayName == "New Account" ? this.Api.Settings.Username : this.Api.Settings.DisplayName);
                OnNewAccountAdded?.Invoke(this, this);
                this.PrototypRender?.UpdateInterval(100);
                return;
            }
            if (Settings.IsInGame)
            {
                this.Api.Logic.StopLogic();
                this.Api.GameObservers.SessionObserver.StopObserver();
                this.Api.GameObservers.GameTaskObserver.StopObserver();
                this.Api.GameMethods.GameLogout();
                this.Api.Logic.SetState(this, DarksideApi.Logic.LogicState.Pausing);
                this.Api.GameClient.CanDisconnect = false;
                this.Api.GameClient.StopTimer();
                return;
            }
            Api.Logging.Log("LogoutApiBtn state: InGame=" + Settings.IsInGame + ", IsLoggingIn=" + Settings.IsLoggingIn + ", state=" + Api.GameClient.GameState);
        }

        private void MenuItemJump_Click(object sender, RoutedEventArgs e)
        {
            /*var gate = api.Logic.GetClosestGateWithoutArea();
            if (gate.CurrentDistance >= 100)
            {
                return;
            }*/
            this.Api.GameMethods.Jump();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            this.PrototypRender?.Stop();

            // Events abmelden
            if (this.mainWindow != null)
            {
                this.mainWindow.LocationChanged -= MainWindow_LocationChanged;
                this.mainWindow.SizeChanged -= MainWindow_SizeChanged;
                this.mainWindow.StateChanged -= MainWindow_StateChanged;
            }
        }

        private void generalRemoveApiBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you really want to remove " + this.Api.Settings.DisplayName + "?", "Remove Account", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }
            OnAccountRemove?.Invoke(this, this);
        }

        private void generalCopySidApiBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.Api == null || this.Api.Hero == null || string.IsNullOrEmpty(this.Api.Hero.ServerTag) || string.IsNullOrEmpty(this.Api.Hero.SessionId))
            {
                return;
            }
            Clipboard.SetText(this.Api.BackpageApi.GetSessionIdUrl(), TextDataFormat.UnicodeText);
            this.Api.WriteLog("Session Url copied to Clipboard!");
        }

        private void generalBuyMeCoffeeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.generalBuyMeCoffeeBtn == null)
            {
                return;
            }
            var url = "https://buymeacoffee.com/delemojo";
            if (MessageBox.Show("If you want to support the project via Paypal, press OK", "Support the Project", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
            {
                url = "https://shorturl.at/DMQys";
            }
            Process.Start(new ProcessStartInfo(url)
            {
                UseShellExecute = true,
            });
        }

        private void MenuItemUse_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is not MenuItem item)
                return;
            if (item is null)
                return;
            if (item.Header is not DarksideApi.DarkOrbit.Objects.Hero.MenuItem possibleItem)
                return;
            if (possibleItem == null)
                return;

            this.Api.GameMethods.MenuItemUse(possibleItem.Id);
            this.Api.WriteLog("Using " + possibleItem.DisplayName);
        }

        private void MenuItemValidateBoxes_Click(object sender, RoutedEventArgs e)
        {
            this.Api.InvalidCollectibles.Clear();
        }
    }
}
