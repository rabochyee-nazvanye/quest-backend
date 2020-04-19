using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quest.DAL.Data;

namespace Quest.Application.Teams.Commands
{
    public class AssignModeratorToTeamCommandHandler : IRequestHandler<AssignModeratorToTeamCommand, BaseResponse<bool>>
    {
        private readonly Db _context;

        public AssignModeratorToTeamCommandHandler(Db context)
        {
            _context = context;
        }

        public async Task<BaseResponse<bool>> Handle(AssignModeratorToTeamCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId,
                cancellationToken: cancellationToken);

            if (user == null)
                return BaseResponse.Failure<bool>("User does not exist");

            var team = await _context.Teams.Where(x => x.Id == request.TeamId)
                .Include(x => x.Moderator)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (team == null)
                return BaseResponse.Failure<bool>("Specified team does not exist.");

            var moderator = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.ModeratorId,
                cancellationToken: cancellationToken);

            if (moderator == null)
                return BaseResponse.Failure<bool>("Specified moderator does not exist.");

            team.Moderator = moderator;

            await _context.SaveChangesAsync(cancellationToken);

            return BaseResponse.Success(true, "Successfully assigned moderator to team");
        }
    }
}
