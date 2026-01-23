using DarksideApi.DarkOrbit.Commands.Quests;

namespace DarksideApi.DarkOrbit.Handlers.Quest
{
    internal class QuestConditionUpdateHandler : ICommandHandler
    {
        private readonly Api api;
        public QuestConditionUpdateHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            QuestConditionUpdateCommand questConditionUpdate = new();
            questConditionUpdate.Read(@byte);

            //Debug.WriteLine("QuestConditionUpdate: " + questConditionUpdate.questConditionId + ": " + questConditionUpdate.questCondition.currentValue);

            for (var i = 0; i < this.api.Quests.Count; i++)
            {
                Objects.Quest.Quest quest = this.api.Quests.ElementAt(i);
                if (!quest.Targets.Any())
                {
                    continue;
                }
                foreach (Objects.Quest.QuestTargetObj target in quest.Targets)
                {
                    if (target.ConditionId == questConditionUpdate.questConditionId)
                    {
                        target.CurrentAmount = (long)questConditionUpdate.questCondition.currentValue;
                        //api.Logging.Log($"Quest condition updated: {quest.Id} - {target.ConditionId} - {target.CurrentAmount} - {questConditionUpdate.questCondition.active}({questConditionUpdate.questCondition.completed})");
                        /*lock (this.api.ObjectLocker)
                        {
                            foreach (Objects.Quest.Quest q in this.api.Quests)
                                q.IsActive = false;
                        }
                        quest.IsActive = true;*/
                        api.AddQuest([quest]);
                        return;
                    }
                }
            }
        }
    }
}
