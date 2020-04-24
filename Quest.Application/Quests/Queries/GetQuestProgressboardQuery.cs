using MediatR;
using Quest.Application.DTOs;

namespace Quest.Application.Quests.Queries
{
    public class GetQuestProgressboardQuery : IRequest<BaseResponse<QuestProgressboardDTO>>
    {
        public GetQuestProgressboardQuery(string userId, int questId)
        {
            UserId = userId;
            QuestId = questId;
        }
        public string UserId { get; set; }
        public int QuestId { get; set; }
    }
}