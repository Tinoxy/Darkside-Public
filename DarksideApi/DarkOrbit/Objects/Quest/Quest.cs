using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DarksideApi.DarkOrbit.Objects.Quest
{
    public class Quest
    {
        public int Id { get; private set; }
        public ObservableCollection<QuestTargetObj> Targets { get; set; }
        public List<QuestRewardObj> Rewards { get; set; }
        public QuestTypes QuestType { get; private set; }
        public double TimeToQuestEnd { get; set; }
        public bool IsActive { get; set; }
        public string DisplayName => "Quest";

        public Quest(int Id, QuestTypes questType, List<QuestTargetObj> targets, List<QuestRewardObj> rewards, double timeToQuestEnd = -1.0)
        {
            this.Id = Id;
            this.QuestType = questType;
            this.TimeToQuestEnd = timeToQuestEnd;

            this.Targets = [];
            this.Rewards = [];

            rewards.ForEach(reward => this.Rewards.Add(reward));
            targets.ForEach(target => this.Targets.Add(target));

            this.IsActive = true;   //Default to display atleast one Quest in UI, will be set to false when Quest Update is received from server

            Debug.WriteLine(this.ToString());
        }

        public override string ToString()
        {
            return $"Quest Id: {this.Id}, Type: {this.QuestType}, Time To Quest End: {this.TimeToQuestEnd}, Targets: [{string.Join(", ", this.Targets)}], Rewards: [{string.Join(", ", this.Rewards)}]";
        }
    }
}
