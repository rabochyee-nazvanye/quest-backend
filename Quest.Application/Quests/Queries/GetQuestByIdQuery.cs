using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Quest.Domain.Models;

namespace Quest.Application.Quests.Queries
{
    public class GetQuestByIdQuery : IRequest<QuestEntity>
    {
        public GetQuestByIdQuery(int questId, string userId)
        {
            QuestId = questId;
            UserId = userId;
        }
        public int QuestId { get; set; }
        public string UserId { get; set; }
    }
}
