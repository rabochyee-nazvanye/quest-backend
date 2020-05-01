using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quest.DAL.Data;
using Quest.Domain.Models;

namespace Quest.Application.Quests.Queries
{
    public class GetQuestByIdQueryHandler : IRequestHandler<GetQuestByIdQuery, QuestEntity>
    {
        private readonly Db _context;

        public GetQuestByIdQueryHandler(Db context)
        {
            _context = context;
        }

        public async Task<QuestEntity> Handle(GetQuestByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Quests
                .Where(x => x.Id == request.QuestId)
                .Include(x => x.Author)
                .Include(x => x.Participants)
                .ThenInclude(x => (x as Team).Members)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
