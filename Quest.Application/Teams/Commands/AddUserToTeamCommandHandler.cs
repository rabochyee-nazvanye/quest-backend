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

namespace Quest.Application.Teams.Commands
{
    public class AddUserToTeamCommandHandler : IRequestHandler<AddUserToTeamCommand, BaseResponse<bool>>
    {
        private readonly IMediator _mediator;
        private readonly Db _context;

        public AddUserToTeamCommandHandler(IMediator mediator, Db context)
        {
            _mediator = mediator;
            _context = context;
        }

        public async Task<BaseResponse<bool>> Handle(AddUserToTeamCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId != request.UserToAddId)
                return BaseResponse.Failure<bool>("Request user and new team user do not match.");

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserToAddId, cancellationToken: cancellationToken);
            if (user == null)
                return BaseResponse.Failure<bool>("Couldn't find quest with that ID");

            var team = await _context.Teams
                .Where(x => x.Id == request.TeamId)
                .Include(x => x.Quest)
                .ThenInclude(x => x.Teams)
                .ThenInclude(x => x.Members)
                .Include(x => x.Members)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (team == null)
                return BaseResponse.Failure<bool>("Couldn't find team with that ID");

            if (team.Quest == null)
                return BaseResponse.Failure<bool>("The team is not linked to any quest");

            if (!team.ValidateSecret(request.TeamSecret))
                return BaseResponse.Failure<bool>("Bad secret");

            if (team.Members.Count >= team.Quest.MaxTeamSize)
                return BaseResponse.Failure<bool>("You couldn't add more people to the team!");

            if (team.Members.Any(x => x.UserId == user.Id))
                return BaseResponse.Failure<bool>("User is already in that team!");

            var teamMemberToRemove =
                team.Quest.Teams
                    .Where(x => x.Id != team.Id && x.Members.Any(m => m.UserId == user.Id))
                    .Select(x => x.Members.FirstOrDefault(m => m.UserId == user.Id))
                    .ToList();

            _context.TeamUsers.RemoveRange(teamMemberToRemove);

            team.Members.Add(new TeamUser
            {
                User = user,
                Team = team,
            });

            await _context.SaveChangesAsync(cancellationToken);
            return BaseResponse.Success(true, "User was successfully added to the team");
        }
    }
}
