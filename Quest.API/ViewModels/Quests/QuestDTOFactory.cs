using System;
using Quest.Domain.Interfaces;
using Quest.Domain.Models;

namespace Quest.API.ViewModels.Quests
{
    public static class QuestDTOFactory
    {
        public static QuestDTO Create(IQuest questEntity)
        {
            switch (questEntity)
            {
                case SoloInfiniteQuest soloInfiniteQuest:
                    return new SoloInfiniteQuestDTO(questEntity);
                case TeamScheduledQuest teamScheduledQuest:
                    return new TeamScheduledQuestDTO(teamScheduledQuest);
                default:
                    throw new ArgumentOutOfRangeException(nameof(questEntity));
            }
        }
    }
}