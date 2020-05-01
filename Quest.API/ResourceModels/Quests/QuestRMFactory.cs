using System;
using Quest.Domain.Interfaces;
using Quest.Domain.Models;

namespace Quest.API.ResourceModels.Quests
{
    public static class QuestRMFactory
    {
        public static QuestRM CreateBasic(IQuest questEntity)
        {
            switch (questEntity)
            {
                case SoloInfiniteQuest soloInfiniteQuest:
                    return new SoloInfiniteQuestRM(soloInfiniteQuest);
                case TeamScheduledQuest teamScheduledQuest:
                    return new TeamScheduledQuestRM(teamScheduledQuest);
                default:
                    throw new ArgumentOutOfRangeException(nameof(questEntity));
            }
        }
        
        public static QuestRM CreateDetailed(IQuest questEntity)
        {
            switch (questEntity)
            {
                case SoloInfiniteQuest soloInfiniteQuest:
                    return new SoloInfiniteQuestDetailedRm(soloInfiniteQuest);
                case TeamScheduledQuest teamScheduledQuest:
                    return new TeamScheduledQuestDetailedRm(teamScheduledQuest);
                default:
                    throw new ArgumentOutOfRangeException(nameof(questEntity));
            }
        }
    }
}