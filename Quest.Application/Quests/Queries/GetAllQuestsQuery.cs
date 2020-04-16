using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Quest.Domain.Models;

namespace Quest.Application.Quests.Queries
{
    public class GetAllQuestsQuery : IRequest<List<QuestEntity>>
    {
    }
}
