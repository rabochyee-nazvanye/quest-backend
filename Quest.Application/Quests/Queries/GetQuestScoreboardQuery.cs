using MediatR;
using Quest.Application.DTOs;

namespace Quest.Application.Quests.Queries
{
    public class GetQuestScoreboardQuery : IRequest<BaseResponse<QuestScoreboardDTO>>
    {
        public GetQuestScoreboardQuery(int questId)
        {
            QuestId = questId;
        }
        public int QuestId { get; set; }
    }
}