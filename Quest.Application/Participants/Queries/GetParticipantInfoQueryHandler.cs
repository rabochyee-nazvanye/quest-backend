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
    public class GetParticipantInfoQueryHandler : IRequestHandler<GetParticipantInfoQuery, Participant>
    {
        private readonly Db _context;

        public GetParticipantInfoQueryHandler(Db context)
        {
            _context = context;
        }

        public async Task<Participant> Handle(GetParticipantInfoQuery request, CancellationToken cancellationToken)
        {
            return await _context.Participants.Where(x => x.Id == request.TeamId)
                    .Include(x => (x as Team).Members)
                    .ThenInclude(x => x.User)
                    .Include(x => x.Principal)
                    .Include(x => x.Quest)
                    .Include(x => x.Moderator)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
