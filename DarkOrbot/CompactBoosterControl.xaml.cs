using DarkOrbot.Util;
using DarksideApi;
using DarksideApi.DarkOrbit;
using DarksideApi.DarkOrbit.Commands.Booster;
using DarksideApi.DarkOrbit.Objects.Boosters;
using DarksideApi.DarkOrbit.Objects.Client;
using DarksideApi.Util.Extensions;
using System.Windows;
using System.Windows.Data;

namespace DarkOrbot
{
    /// <summary>
    /// Interaktionslogik für CompactBoosterControl.xaml
    /// </summary>
    public partial class CompactBoosterControl : Window, IControl
    {
        public Api Api { get; private set; }
        public CompactApiControl ApiControl { get; private set; }
        private WindowSnapUtil? snapUtil;

        public bool Ready { get; set; }
        public string ApiKey => this.Api.Settings.ApiKey;
        public DarkOrbotSettings Settings => this.Api.Settings;

        public CompactBoosterControl(CompactApiControl owner)
        {
            this.ApiControl = owner;
            this.Api = owner.Api;
            this.InitializeComponent();
            this.Init();
        }

        private CompactBoosterControl() { }

        public static CompactBoosterControl Empty()
        {
            return new CompactBoosterControl();
        }

        private void Init()
        {
            this.Api.OnAddBoosters += (s, e) =>
            {
                if (!this.Dispatcher.CheckAccess())
                {
                    this.Dispatcher.Invoke(() => this.AddBooster(e));
                    return;
                }
                this.AddBooster(e);
            };
            //this.snapUtil = new WindowSnapUtil(this, this.ApiControl.mainWindow);

            //this.listBox.ItemsSource = null;
            //this.listBox.ItemsSource = Api.Hero.ActiveBoosters;

            this.Ready = true;
        }

        private void AddBooster(List<Booster> boosters)
        {
            //this.Api.Hero.ActiveBoosters.Clear();
            if (boosters.Count <= 0)
            {
                this.Api.Hero.ActiveBoosters.Clear();
                return;
            }

            boosters.ForEach(b => {

                if (this.Api.Hero.ActiveBoosters.TryGetValue(b.BoosterId, out var booster))
                {
                    switch (b)
                    {
                        case SharedBooster sb when booster is SharedBooster existingSb:
                            existingSb.Amount = sb.Amount;
                            break;
                        case PlayerBooster pb when booster is PlayerBooster existingPb:
                            existingPb.Duration = pb.Duration;
                            break;
                        case EternalBlacklightBooster eb when booster is EternalBlacklightBooster existingEb:
                            existingEb.Amount = eb.Amount;
                            break;
                    }
                }
                else
                    this.Api.Hero.ActiveBoosters.TryAdd(b.BoosterId, b);
            });
            CollectionViewSource.GetDefaultView(this.boosterListBox.ItemsSource).Refresh();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //this.listBox.ItemsSource = null;
            //this.listBox.ItemsSource = Api.Hero.ActiveBoosters;
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //this.listBox.ItemsSource = null;
            //this.listBox.ItemsSource = Api.Hero.ActiveBoosters;
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
