using Quest.Domain.Interfaces;

namespace Quest.API.ViewModels.Quests
{
    public class SoloInfiniteQuestDTO : QuestDTO
    {
        public SoloInfiniteQuestDTO(IInfiniteQuest row) : base(row)
        {
        }
        
        protected override string GetQuestType() => "solo";
        protected override bool CheckIsInfinite() => true;
    }
}