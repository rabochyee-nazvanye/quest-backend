using Quest.API.Enums;
using Quest.Domain.Interfaces;

namespace Quest.API.ResourceModels.Quests
{
    public class SoloInfiniteQuestRM : QuestRM
    {
        public SoloInfiniteQuestRM(IInfiniteQuest row) : base(row)
        {
        }
        
        protected override string GetQuestType() => QuestParticipantType.Solo.ToString().ToLowerInvariant();
        protected override bool CheckIsInfinite() => true;
    }
}