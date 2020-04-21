using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quest.DAL.Data;
using Quest.Domain.Models;

namespace Quest.Application.Teams.Queries
{
    public class GetTeamByUserAndQuestQueryHandler : IRequestHandler<GetTeamByUserAndQuestQuery, List<Team>>
    {
        private readonly Db _context;

        public GetTeamByUserAndQuestQueryHandler(Db context)
        {
            _context = context;
        }

        public async Task<List<Team>> Handle(GetTeamByUserAndQuestQuery request, CancellationToken cancellationToken)
        {
            if (!request.MemberIds.Any())
                return null;
            
            var quest = await _context.Quests
                .Where(x => x.Id == request.QuestId)
                .Include(x => x.Teams)
                .ThenInclude(x => x.Captain)
                .Include(x => x.Teams)
                .ThenInclude(x => x.Members)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(cancellationToken);

            return quest?.Teams
                .Where(x => x.Members.Any(x => request.MemberIds.Contains(x.UserId)))
                .ToList();
        }
    }
}