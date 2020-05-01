using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quest.DAL.Data;
using Quest.DAL.Migrations;

namespace Quest.Application.Teams.Commands
{
    public class RemoveUserFromTeamCommandHandler : IRequestHandler<RemoveUserFromTeamCommand, BaseResponse<bool>>
    {
        private readonly Db _context;

        public RemoveUserFromTeamCommandHandler(Db context)
        {
            _context = context;
        }

        public async Task<BaseResponse<bool>> Handle(RemoveUserFromTeamCommand request, CancellationToken cancellationToken)
        {
            var team = await _context.Teams
                .Where(x => x.Id == request.TeamId)
                .Include(x => x.Members)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (team == null)
                return BaseResponse.Failure<bool>("Team not found!");

            if (team.PrincipalUserId != request.UserId && request.UserId != request.UserToKickId)
                return BaseResponse.Failure<bool>("You need to be a principal to kick this user from team");
            
            var userToKick = team.Members.FirstOrDefault(x => x.UserId == request.UserToKickId);

            if (userToKick == null)
                return BaseResponse.Failure<bool>("This user does not belong to this team.");

            if (request.UserToKickId == team.PrincipalUserId)
            {
                if (team.Members.Count <= 1)
                {
                    _context.Teams.Remove(team);
                }
                else
                {
                    team.PrincipalUserId = team.Members.First(x => x.UserId != request.UserToKickId).UserId;
                    team.Members.Remove(userToKick);
                }
            }
            else
            {
                team.Members.Remove(userToKick);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return BaseResponse.Success(true, "Successfully removed user from the team.");
        }
    }
}
