using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DarkOrbot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<TabItem> tabItems;

        public MainWindow()
        {
            this.InitializeComponent();
            this.tabItems = [];
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Init();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.SaveSettings();
            foreach (CompactApiControl api in App.Apis)
            {
                api.Api.GameObservers?.Stop();
                api.Api.Logic?.StopLogic();
                api.Api.GameMethods?.GameLogout();
                api.Api.GameClient?.Disconnect();
            }
            Process.GetCurrentProcess().Kill();
        }

        private void Init()
        {
            foreach (CompactApiControl api in App.Apis)
            {
                api.mainWindow = this;
                api.VerticalAlignment = VerticalAlignment.Stretch;
                api.HorizontalAlignment = HorizontalAlignment.Stretch;
                api.OnAccountRemove += this.RemoveAccountTab;
                var item = new TabItem
                {
                    Header = api.Settings.DisplayName,
                    Content = api,
                };
                item.SetBinding(TabItem.HeaderProperty, new Binding("Api.Settings.DisplayName") { Source = item.Content, });
                //AddContextMenu(item);
                this.tabItems.Add(item);
            }

            this.AddNewAccountTab();

            this.mainTabControl.ItemsSource = this.tabItems;
            this.mainTabControl.SelectedIndex = 0;
        }

        private void AddNewAccountTab()
        {
            var control = new CompactApiControl(new DarksideApi.DarkOrbotSettings(), App.LicenseManager?.InjectionKey, this)
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            var binding = new Binding("Api.Settings.DisplayName")
            {
                Source = control,
            };
            var newApiItem = new TabItem
            {
                Content = control,
            };
            //AddContextMenu(newApiItem);
            newApiItem.SetBinding(TabItem.HeaderProperty, binding);
            control.OnNewAccountAdded += this.NewAccountTab;
            control.OnAccountRemove += this.RemoveAccountTab;
            this.tabItems.Add(newApiItem);
        }

        private void NewAccountTab(object? sender, CompactApiControl apiControl)
        {
            if (App.Apis.Contains(apiControl.ApiKey))
            {
                App.Apis.Remove(apiControl);
            }
            App.Apis.Add(apiControl);
            this.AddNewAccountTab();
        }

        private void RemoveAccountTab(object? sender, CompactApiControl apiControl)
        {
            DependencyObject parent = apiControl.Parent;
            while (parent is not TabItem)
            {
                parent = LogicalTreeHelper.GetParent(parent);
            }
            this.tabItems.Remove((TabItem)parent);
            App.Apis.Remove(apiControl);
            this.mainTabControl.SelectedIndex = 0;
        }

        private void darkOrbotMainWindow_LocationChanged(object sender, EventArgs e)
        {

        }

        private void darkOrbotMainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void mainTabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            App.SaveSettings();
        }
    }
}