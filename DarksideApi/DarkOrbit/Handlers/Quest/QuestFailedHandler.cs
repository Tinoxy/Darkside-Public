using DarksideApi.DarkOrbit.Commands.Quests;

namespace DarksideApi.DarkOrbit.Handlers.Quest
{
    internal class QuestFailedHandler : ICommandHandler
    {
        private readonly Api api;

        public QuestFailedHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            QuestFailedCommand questFailed = new();
            questFailed.Read(@byte);

            if (!this.api.Quests.TryGetValue(questFailed.questId, out var quest))
            {
                this.api.Logging.Log($"Quest {questFailed.questId} not found in active quests.");
                return;
            }
            quest.IsActive = false;
            quest.TimeToQuestEnd = -1;
            this.api.AddQuest([quest]);
            this.api.Logging.Log("Quest Failed!");
        }
    }
}
