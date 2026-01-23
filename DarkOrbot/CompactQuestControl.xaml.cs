using DarkOrbot.Util;
using DarksideApi;
using DarksideApi.DarkOrbit;
using DarksideApi.DarkOrbit.Objects.Group;
using DarksideApi.DarkOrbit.Objects.Quest;
using DarksideApi.Util.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace DarkOrbot
{
    /// <summary>
    /// Interaktionslogik für CompactQuestControl.xaml
    /// </summary>
    public partial class CompactQuestControl : Window
    {
        public Api Api { get; private set; }
        public CompactApiControl ApiControl { get; private set; }
        private WindowSnapUtil? snapUtil;

        public bool Ready { get; set; }
        public string ApiKey => this.Api.Settings.ApiKey;
        public DarkOrbotSettings Settings => this.Api.Settings;

        public CompactQuestControl(CompactApiControl owner)
        {
            this.ApiControl = owner;
            this.Api = owner.Api;
            this.InitializeComponent();
            this.Init();
        }

        private CompactQuestControl() { }

        public static CompactQuestControl Empty()
        {
            return new CompactQuestControl();
        }

        private void Init()
        {
            this.Api.OnAddQuest += (s, e) =>
            {
                if (!this.Dispatcher.CheckAccess())
                {
                    this.Dispatcher.Invoke(() => this.AddQuest(e));
                    return;
                }
                this.AddQuest(e);
            };
            //this.snapUtil = new WindowSnapUtil(this, this.ApiControl.mainWindow);

            //this.listBox.ItemsSource = null;
            //this.listBox.ItemsSource = Api.Hero.ActiveBoosters;

            this.Ready = true;
        }

        private void AddQuest(List<Quest> quests)
        {

            // Ensure ListBox is bound to the group collection
            if (this.questControl.ItemsSource == null)
            {
                this.questControl.ItemsSource = this.Api.Quests;
            }

            // Add or update members
            foreach (var q in quests)
            {
                if (!q.IsActive && q.TimeToQuestEnd < 0)
                {
                    this.Api.Quests.Remove(q.Id);
                    continue; 
                }
                if (this.Api.Quests.Contains(q.Id))
                {
                    this.Api.Quests[q.Id].Targets = q.Targets;
                    this.Api.Quests[q.Id].Rewards = q.Rewards;
                    this.Api.Quests[q.Id].TimeToQuestEnd = q.TimeToQuestEnd;
                    this.Api.Quests[q.Id].IsActive = q.IsActive;
                }
                else
                {
                    this.Api.Quests.TryAdd(q.Id, q);
                }
            }

            var view = CollectionViewSource.GetDefaultView(this.questControl.ItemsSource);
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

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
