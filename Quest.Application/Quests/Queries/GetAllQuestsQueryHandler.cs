﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quest.DAL.Data;
using Quest.Domain.Models;

namespace Quest.Application.Quests.Queries
{
    public class GetAllQuestsQueryHandler : IRequestHandler<GetAllQuestsQuery, List<QuestEntity>>
    {
        private readonly Db _context;

        public GetAllQuestsQueryHandler(Db context)
        {
            _context = context;
        }

        public async Task<List<QuestEntity>> Handle(GetAllQuestsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Quests
                .Include(x => x.Author)
                .Include(x => x.Participants)
                .ThenInclude(x => (x as Team).Members)
                .ThenInclude(x => x.User)
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
