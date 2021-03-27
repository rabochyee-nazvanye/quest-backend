using MediatR;
using Quest.Application.DTOs;

namespace Quest.Application.Quests.Queries
{
    public class GetQuestStatusQuery : IRequest<BaseResponse<QuestStatusDto>>
    {
        public GetQuestStatusQuery(int questId, string userId)
        {
            QuestId = questId;
            UserId = userId;
        }
        public int QuestId { get; set; }
        public string UserId { get; set; }
    }
}