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
    class GetTeamBySecretQueryHandler : IRequestHandler<GetTeamBySecretQuery, Team>
    {
        private readonly Db _context;

        public GetTeamBySecretQueryHandler(Db context)
        {
            _context = context;
        }

        public async Task<Team> Handle(GetTeamBySecretQuery request, CancellationToken cancellationToken)
        {
            return await _context.Teams.Where(x => x.InviteTokenSecret == request.Secret)
                .Include(x => x.Members)
                .ThenInclude(x => x.User)
                .Include(x => x.Captain)
                .Include(x => x.Quest)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
