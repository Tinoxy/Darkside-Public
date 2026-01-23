using DarkOrbot.Util;
using DarksideApi;
using DarksideApi.DarkOrbit;
using DarksideApi.Statistics;
using System.Windows;

namespace DarkOrbot
{
    /// <summary>
    /// Interaktionslogik für CompactStatisticsControl.xaml
    /// </summary>
    public partial class CompactStatisticsControl : Window, IControl
    {
        public Api Api { get; private set; }
        public CompactApiControl ApiControl { get; private set; }
        private WindowSnapUtil? snapUtil;

        public bool Ready { get; set; }
        public string ApiKey => this.Api.Settings.ApiKey;
        public DarkOrbotSettings Settings => this.Api.Settings;

        public CompactStatisticsControl(CompactApiControl owner)
        {
            this.ApiControl = owner;
            this.Api = owner.Api;
            this.InitializeComponent();
            this.Init();
        }

        private CompactStatisticsControl() { }

        public static CompactStatisticsControl Empty()
        {
            return new CompactStatisticsControl();
        }

        private void Init()
        {
            //this.snapUtil = new WindowSnapUtil(this, this.ApiControl.mainWindow);
            this.Api.OnStatisticsItem += (s, e) => this.AddStatisticsItem(e); 
            this.Ready = true;
        }

        private void AddStatisticsItem(StatisticsItem statisticsItem)
        {
            if (!this.Dispatcher.CheckAccess())
            {
                this.Dispatcher.Invoke(() => this.AddStatisticsItem(statisticsItem));
                return;
            }
            this.Api.Statistics.AddLoot(statisticsItem);
        }

        private void generalResetStatsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.generalResetStatsBtn == null)
            {
                return;
            }
            if (this.Api == null || this.Api.Statistics == null)
            {
                return;
            }
            try
            {
                this.Api.Statistics.Reset();
                this.Api.Logging.Log("Statistics Reset!");
            }
            catch (Exception ex)
            {
                this.Api.Logging.Log("Reset Stats Error: " + ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {

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
            this.Hide();
        }
    }
}
