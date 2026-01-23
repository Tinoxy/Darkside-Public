using DarksideApi.DarkOrbit.Commands.Quests;
using DarksideApi.DarkOrbit.Objects.Quest;
using DarksideApi.Util.Extensions;

namespace DarksideApi.DarkOrbit.Handlers.Quest
{
    internal class QuestInitalizationHandler : ICommandHandler
    {
        private readonly Api api;

        public QuestInitalizationHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            QuestInitializationCommand questInitialization = new();
            questInitialization.Read(@byte);

            QuestTypeModule type = questInitialization.quest.types.FirstOrDefault() ?? new QuestTypeModule();
            var rewards = new List<QuestRewardObj>();
            var targets = new List<QuestTargetObj>();

            //https://darkorbit-22.bpsecure.com/spacemap/templates/de/resource_quest.xml
            //Debug.WriteLine(questInitialization.ToString());

            questInitialization.quest.rewards.ForEach(reward => rewards.Add(new QuestRewardObj(reward.lootId.Split('_').Last().ToUpper(), reward.amount)));
            foreach (QuestElementModule mod in questInitialization.quest.rootCase.modifier)
            {
                if (mod.condition.matches.Count <= 0)
                {
                    this.api.Logging.Log($"Quest {questInitialization.quest.questId} has no matches in condition, skipping target creation.");
                    continue;
                }
                QuestTargetObj? subTarget = null;
                foreach (QuestConditionModule condition in mod.condition.subConditions)
                {
                    subTarget = new QuestTargetObj(condition.matches, condition.questId, condition.type, (long)condition.targetValue, (long)condition.state.currentValue, null);
                }
                var target = new QuestTargetObj(mod.condition.matches, mod.condition.questId, mod.condition.type, (long)mod.condition.targetValue, (long)mod.condition.state.currentValue, subTarget);

                targets.Add(target);
            }

            var quest = new Objects.Quest.Quest(questInitialization.quest.questId, (QuestTypes)type.type, targets, rewards);

            this.api.AddQuest([quest]);
            /*if (!this.api.Quests.TryAdd(quest.Id, quest))
            {
                this.api.Quests[quest.Id].Targets = quest.Targets;
                this.api.Quests[quest.Id].Rewards = quest.Rewards;
                this.api.Quests[quest.Id].TimeToQuestEnd = quest.TimeToQuestEnd;
                this.api.Quests[quest.Id].IsActive = quest.IsActive;
            }*/

            //api.Logging.Log(quest.ToString());
        }
    }
}
