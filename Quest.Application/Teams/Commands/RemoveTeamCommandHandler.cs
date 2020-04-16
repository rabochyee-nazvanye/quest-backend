using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quest.DAL.Data;

namespace Quest.Application.Teams.Commands
{
    public class RemoveTeamCommandHandler : IRequestHandler<RemoveTeamCommand, BaseResponse<bool>>
    {
        private readonly Db _context;

        public RemoveTeamCommandHandler(Db context)
        {
            _context = context;
        }

        public async Task<BaseResponse<bool>> Handle(RemoveTeamCommand request, CancellationToken cancellationToken)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == request.TeamId,
                cancellationToken: cancellationToken);

            var user = await _context.Users.FindAsync(request.UserId);

            if (team.CaptainUserId != request.UserId)
                return BaseResponse.Failure<bool>("Only team captain can remove the team.");

            if (user == null)
                return BaseResponse.Failure<bool>("User not found.");

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync(cancellationToken);

            return BaseResponse.Success(true, "Successfully removed team");
        }
    }
}
