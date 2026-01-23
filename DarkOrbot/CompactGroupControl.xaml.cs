using DarkOrbot.Util;
using DarksideApi;
using DarksideApi.DarkOrbit;
using DarksideApi.DarkOrbit.Objects.Group;
using DarksideApi.Util.Extensions;
using System.Windows;
using System.Windows.Data;

namespace DarkOrbot
{
    /// <summary>
    /// Interaktionslogik für CompactGroupControl.xaml
    /// </summary>
    public partial class CompactGroupControl : Window
    {
        public Api Api { get; private set; }
        public CompactApiControl ApiControl { get; private set; }
        private WindowSnapUtil? snapUtil;

        public bool Ready { get; set; }
        public string ApiKey => this.Api.Settings.ApiKey;
        public DarkOrbotSettings Settings => this.Api.Settings;

        public CompactGroupControl(CompactApiControl owner)
        {
            this.ApiControl = owner;
            this.Api = owner.Api;
            this.InitializeComponent();
            this.Init();
        }

        private CompactGroupControl() { }

        public static CompactGroupControl Empty()
        {
            return new CompactGroupControl();
        }

        private void Init()
        {
            this.Api.OnAddGroupMembers += (s, e) =>
            {
                if (!this.Dispatcher.CheckAccess())
                {
                    this.Dispatcher.Invoke(() => this.AddGroupMember(e));
                    return;
                }
                this.AddGroupMember(e);
            };
            //this.snapUtil = new WindowSnapUtil(this, this.ApiControl.mainWindow);

            //this.listBox.ItemsSource = null;
            //this.listBox.ItemsSource = Api.Hero.ActiveBoosters;

            this.Ready = true;
        }

        private void AddGroupMember(List<GroupMemberObj> groupMembers)
        {
            // Ensure Group object exists
            if (this.Api.Group == null)
            {
                this.Api.Group = new(this.Api, this.Api.Hero.Username, this.Api.Hero.UserId);
            }

            // Ensure ListBox is bound to the group collection
            if (this.boosterListBox.ItemsSource == null)
            {
                this.boosterListBox.ItemsSource = this.Api.Group.Members;
            }

            // Add or update members
            foreach (var g in groupMembers)
            {
                if (this.Api.Group.Members.TryGetValue(g.Id, out var member))
                {
                    member.Player = g.Player;
                    member.IsInRaidGate = g.IsInRaidGate;
                    member.MapId = g.MapId;
                    member.RaidGateStage = g.RaidGateStage;
                    member.Target = g.Target;
                }
                else
                {
                    this.Api.Group.Members.TryAdd(g.Id, g);
                }
            }

            var view = CollectionViewSource.GetDefaultView(this.boosterListBox.ItemsSource);
            view?.Refresh();
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

        private void GoToBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not FrameworkElement fe)
                return;

            if (fe.DataContext is not GroupMemberObj member)
                return;

            // Sicherstellen, dass Player/Position vorhanden sind
            if (member.Player == null)
                return;

            try
            {
                // Move akzeptiert Vector2
                this.Api.GameMethods.Move(member.Player.Position);
            }
            catch (Exception ex)
            {
                this.Api.Logging.Log($"GoTo failed for {member.DisplayName}: {ex.Message}");
            }
        }
    }
}
