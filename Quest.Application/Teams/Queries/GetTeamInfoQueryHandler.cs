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

namespace Quest.Application.Teams.Queries
{
    public class GetTeamInfoQueryHandler : IRequestHandler<GetTeamInfoQuery, Team>
    {
        private readonly Db _context;

        public GetTeamInfoQueryHandler(Db context)
        {
            _context = context;
        }

        public async Task<Team> Handle(GetTeamInfoQuery request, CancellationToken cancellationToken)
        {
            return await _context.Teams.Where(x => x.Id == request.TeamId)
                    .Include(x => x.Members)
                    .ThenInclude(x => x.User)
                    .Include(x => x.Captain)
                    .Include(x => x.Quest)
                    .Include(x => x.Moderator)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
