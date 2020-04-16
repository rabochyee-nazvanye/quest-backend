using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quest.Application.Accounts.Queries;
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
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken: cancellationToken);
            if (user == null)
                return BaseResponse.Failure<bool>("Couldn't find quest with that ID");

            var team = await _context.Teams
                .Where(x => x.Id == request.TeamId)
                .Include(x => x.Quest)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (team == null)
                return BaseResponse.Failure<bool>("Couldn't find team with that ID");

            if (team.Quest == null)
                return BaseResponse.Failure<bool>("The team is not linked to any quest");

            if (!team.ValidateSecret(request.TeamSecret))
                return BaseResponse.Failure<bool>("Bad secret");

            if (team.Members.Count >= team.Quest.MaxTeamSize)
                return BaseResponse.Failure<bool>("You couldn't add more people to the team!");

            team.Members.Add(new TeamUser
            {
                User = user,
                Team = team,
            });

            await _context.SaveChangesAsync(cancellationToken);
            return BaseResponse.Success(true, "User was successfully added");
        }
    }
}
