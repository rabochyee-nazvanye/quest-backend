using System;
using Quest.Domain.Interfaces;
using Quest.Domain.Models;

namespace Quest.API.ViewModels.Quests
{
    public static class QuestDTOFactory
    {
        public static QuestDTO CreateBasic(IQuest questEntity)
        {
            switch (questEntity)
            {
                case SoloInfiniteQuest soloInfiniteQuest:
                    return new SoloInfiniteQuestDTO(soloInfiniteQuest);
                case TeamScheduledQuest teamScheduledQuest:
                    return new TeamScheduledQuestDTO(teamScheduledQuest);
                default:
                    throw new ArgumentOutOfRangeException(nameof(questEntity));
            }
        }
        
        public static QuestDTO CreateDetailed(IQuest questEntity)
        {
            switch (questEntity)
            {
                case SoloInfiniteQuest soloInfiniteQuest:
                    return new SoloInfiniteQuestDetailedDTO(soloInfiniteQuest);
                case TeamScheduledQuest teamScheduledQuest:
                    return new TeamScheduledQuestDetailedDTO(teamScheduledQuest);
                default:
                    throw new ArgumentOutOfRangeException(nameof(questEntity));
            }
        }
    }
}