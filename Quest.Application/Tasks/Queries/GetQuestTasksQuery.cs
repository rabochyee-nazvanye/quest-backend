using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Quest.Domain.Models;

namespace Quest.Application.Tasks.Queries
{
    public class GetQuestTasksQuery : IRequest<BaseResponse<List<TaskEntity>>>
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
