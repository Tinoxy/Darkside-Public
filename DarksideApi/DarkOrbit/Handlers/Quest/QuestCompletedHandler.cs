using DarksideApi.DarkOrbit.Commands.Quests;

namespace DarksideApi.DarkOrbit.Handlers.Quest
{
    internal class QuestCompletedHandler : ICommandHandler
    {
        private readonly Api api;

        public QuestCompletedHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            QuestCompletedCommand questCompleted = new();
            questCompleted.Read(@byte);
            
            if (!this.api.Quests.TryGetValue(questCompleted.questId, out var quest))
            {
                this.api.Logging.Log($"Quest {questCompleted.questId} not found in active quests! Rating: {questCompleted.rating}");
                return;
            }
            //!q.IsActive && q.TimeToQuestEnd < 0
            quest.IsActive = false;
            quest.TimeToQuestEnd = -1000;
            this.api.AddQuest([quest]);

            this.api.Logging.Log("Quest completed successfully.");
            this.api.AddLootResource("Quest", 1, 0);
        }
    }
}
