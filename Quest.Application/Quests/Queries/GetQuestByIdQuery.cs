using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Quest.Domain.Models;

namespace Quest.Application.Quests.Queries
{
    public class GetQuestByIdQuery : IRequest<QuestEntity>
    {
        public GetQuestByIdQuery(int questId)
        {
            QuestId = questId;
        }
        public int QuestId { get; set; }
    }
}
