using System.Collections.Generic;
using MediatR;
using Quest.Domain.Models;

namespace Quest.Application.Teams.Queries
{
    public class GetParticipantsByUserAndQuestQuery : IRequest<BaseResponse<List<Participant>>>
    {
        public GetParticipantsByUserAndQuestQuery(int questId, List<string> memberIds)
        {
            QuestId = questId;
            MemberIds = memberIds;
        }
        
        public int QuestId { get; set; }
        public List<string> MemberIds { get; set; }
    }
}