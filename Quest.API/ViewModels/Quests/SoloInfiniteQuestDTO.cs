using Quest.Domain.Interfaces;

namespace Quest.API.ViewModels.Quests
{
    public class SoloInfiniteQuestDTO : QuestDTO
    {
        public SoloInfiniteQuestDTO(IQuest row) : base(row)
        {
            IsInfinite = true;
        }

        public bool IsInfinite { get; set; }
    }
}