using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Quest.Application.DTOs;
using Quest.Domain.Models;

namespace Quest.Application.Tasks.Queries
{
    public class GetQuestTasksQuery : IRequest<BaseResponse<List<TeamTaskStatusDTO>>>
    {
        public GetQuestTasksQuery(string userId,int questId)
        {
            QuestId = questId;
            UserId = userId;
        }
        
        public string UserId { get; set; }
        public int QuestId { get; set; }
    }
}
