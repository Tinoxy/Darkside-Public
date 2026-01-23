using DarkOrbot.Util;
using DarksideApi;
using DarksideApi.DarkOrbit;
using DarksideApi.DarkOrbit.Objects.Boosters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DarkOrbot
{
    /// <summary>
    /// Interaktionslogik für CompactLogControl.xaml
    /// </summary>
    public partial class CompactLogControl : Window
    {
        public Api Api { get; private set; }
        public CompactApiControl ApiControl { get; private set; }
        private WindowSnapUtil? snapUtil;

        public bool Ready { get; set; }
        public string ApiKey => this.Api.Settings.ApiKey;
        public DarkOrbotSettings Settings => this.Api.Settings;

        public CompactLogControl(CompactApiControl owner)
        {
            this.ApiControl = owner;
            this.Api = owner.Api;
            this.InitializeComponent();
            this.Init();
        }

        private CompactLogControl() { }

        public static CompactLogControl Empty()
        {
            return new CompactLogControl();
        }

        private void Init()
        {
            this.Api.OnLogMessage += (s, e) =>
            {
                if (!this.Dispatcher.CheckAccess())
                {
                    this.Dispatcher.Invoke(() => this.AddLogMessage(e));
                    return;
                }
                this.AddLogMessage(e);
            };

            this.Ready = true;
        }

        private void AddLogMessage(string logMessage)
        {
            var range = new TextRange(this.generalLogTextbox.Document.ContentEnd, this.generalLogTextbox.Document.ContentEnd)
            {
                Text = $"[{DateTime.Now:T}] " + logMessage + "\n",
            };
            this.generalLogTextbox.ScrollToEnd();
        }

        private void generalLogTextbox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TextRange MyText = new(this.generalLogTextbox.Document.ContentStart, this.generalLogTextbox.Document.ContentEnd);

            var splittedLines = MyText.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            if (splittedLines.Length > 55)
            {
                this.generalLogTextbox.Document.Blocks.Remove(this.generalLogTextbox.Document.Blocks.FirstBlock);
            }
        }

        private void generalLogTextbox_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (this.logBoxLogDamageCheckbox == null || this.generalLogTextbox == null)
            {
                return;
            }
            this.logBoxLogDamageCheckbox.Visibility = Visibility.Visible;
        }

        private void generalLogTextbox_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (this.logBoxLogDamageCheckbox == null || this.generalLogTextbox == null)
            {
                return;
            }
            this.logBoxLogDamageCheckbox.Visibility = Visibility.Hidden;
        }

        private void logBoxLogDamageCheckbox_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (this.logBoxLogDamageCheckbox == null)
            {
                return;
            }
            this.logBoxLogDamageCheckbox.Visibility = Visibility.Visible;
        }

        private void logBoxLogDamageCheckbox_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (this.logBoxLogDamageCheckbox == null)
            {
                return;
            }
            this.logBoxLogDamageCheckbox.Visibility = Visibility.Hidden;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
