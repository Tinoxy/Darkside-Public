using DarkOrbot.Util;
using DarksideApi;
using DarksideApi.DarkOrbit;
using System.Windows;

namespace DarkOrbot
{
    /// <summary>
    /// Interaktionslogik für CompactShipControl.xaml
    /// </summary>
    public partial class CompactShipControl : Window
    {
        public Api Api { get; private set; }
        public CompactApiControl ApiControl { get; private set; }
        private WindowSnapUtil? snapUtil;

        public bool Ready { get; set; }
        public string ApiKey => this.Api.Settings.ApiKey;
        public DarkOrbotSettings Settings => this.Api.Settings;

        public CompactShipControl(CompactApiControl owner)
        {
            this.ApiControl = owner;
            this.Api = owner.Api;
            this.InitializeComponent();
            this.Init();
        }

        private CompactShipControl() { }

        public static CompactShipControl Empty()
        {
            return new CompactShipControl();
        }

        private void Init()
        {
            //this.snapUtil = new WindowSnapUtil(this, this.ApiControl.mainWindow);

            this.Ready = true;
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.Api.Hero.Ship != this.Api.Statistics.LoadedShip)
            {
                this.Api.Statistics.LoadedShip = this.Api.Hero.Ship;
                this.shipImage.Source = Util.HelpTools.GetShipImage(this.Api.Hero.Ship);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Api.Hero.Ship != this.Api.Statistics.LoadedShip)
            {
                this.Api.Statistics.LoadedShip = this.Api.Hero.Ship;
                this.shipImage.Source = Util.HelpTools.GetShipImage(this.Api.Hero.Ship);
            }
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();    
        }
    }
}
