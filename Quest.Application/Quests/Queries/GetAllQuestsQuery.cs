using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Quest.Domain.Models;

namespace Quest.Application.Quests.Queries
{
    public class GetAllQuestsQuery : IRequest<List<QuestEntity>>
    {
        public GetAllQuestsQuery(string userId)
        {
            UserId = userId;
        }
        
        public string UserId { get; set; }
    }
}
