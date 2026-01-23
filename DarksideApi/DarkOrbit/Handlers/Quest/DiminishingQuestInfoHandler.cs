using DarksideApi.DarkOrbit.Commands.Quests;
using DarksideApi.DarkOrbit.Objects.Quest;
using DarksideApi.Util.Extensions;

namespace DarksideApi.DarkOrbit.Handlers.Quest
{
    internal class DiminishingQuestInfoHandler : ICommandHandler
    {
        private readonly Api api;

        public DiminishingQuestInfoHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            DiminishingQuestInitializationCommand diminishingQuestInfo = new();
            diminishingQuestInfo.Read(@byte);

            QuestTypeModule type = diminishingQuestInfo.quest.types.FirstOrDefault() ?? new QuestTypeModule();
            var rewards = new List<QuestRewardObj>();
            var targets = new List<QuestTargetObj>();

            //https://darkorbit-22.bpsecure.com/spacemap/templates/de/resource_quest.xml

            diminishingQuestInfo.quest.rewards.ForEach(reward => rewards.Add(new QuestRewardObj(reward.lootId.Split('_').Last().ToUpper(), reward.amount)));
            foreach (QuestElementModule mod in diminishingQuestInfo.quest.rootCase.modifier)
            {
                if (mod.condition.matches.Count <= 0)
                {
                    this.api.Logging.Log($"Diminishing-Quest {diminishingQuestInfo.quest.questId} has no matches in condition, skipping target creation.");
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

            var quest = new Objects.Quest.Quest(diminishingQuestInfo.quest.questId, (QuestTypes)type.type, targets, rewards, diminishingQuestInfo.timeToQuestEnd);

            this.api.AddQuest([quest]);
            /*if (!this.api.Quests.TryAdd(quest.Id, quest))
            {
                this.api.Quests[quest.Id].Targets = quest.Targets;
                this.api.Quests[quest.Id].Rewards = quest.Rewards;
                this.api.Quests[quest.Id].TimeToQuestEnd = quest.TimeToQuestEnd;
                this.api.Quests[quest.Id].IsActive = quest.IsActive;

            }*/

            this.api.DiminishingQuest = quest;

            this.api.Logging.Log("Dminishing-" + quest.ToString());

            if (diminishingQuestInfo.acceptabilityStatus.type == 0
                && diminishingQuestInfo.timeToQuestEnd > 100
                && diminishingQuestInfo.quest.rootCase.active)
            {
                //api.Logic.SetState(this, Logic.LogicState.GotoDiminishingQuest);
            }
        }
    }
}
