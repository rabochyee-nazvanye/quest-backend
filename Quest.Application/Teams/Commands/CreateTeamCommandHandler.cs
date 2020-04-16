using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quest.Application.Quests.Queries;
using Quest.DAL.Data;
using Quest.Domain.Models;
using Quest.Domain.Services;

namespace Quest.Application.Teams.Commands
{
    public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, BaseResponse<Team>>
    {
        private readonly Db _context;
        private readonly IMediator _mediator;
        private readonly ITeamService _teamService;

        public CreateTeamCommandHandler(Db context, IMediator mediator, ITeamService teamService)
        {
            _context = context;
            _mediator = mediator;
            _teamService = teamService;
        }

        public async Task<BaseResponse<Team>> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            var captainUser = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken: cancellationToken);
            if (captainUser == null)
                return BaseResponse.Failure<Team>("Could not find captain user for the team");

            var quest = await _context.Quests.Where(x => x.Id == request.QuestId)
                .Include(x => x.Teams)
                    .ThenInclude(x => x.Members)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (quest == null)
                return BaseResponse.Failure<Team>("Couldn't find quest with that ID");

            var captainTeams = quest.Teams
                .Where(x => x.Members.Any(y => y.UserId == captainUser.Id) 
                            || x.CaptainUserId == captainUser.Id);
            if (captainTeams.Any())
                return BaseResponse.Failure<Team>("This user is currently member of another team");

           
            if (quest.RegistrationDeadline < DateTime.Now)
                return BaseResponse.Failure<Team>("You can't register to that event no more");

            if (await _context.Teams.AnyAsync(x => x.Name == request.Name, cancellationToken: cancellationToken))
                return BaseResponse.Failure<Team>("The team with that name already exists");

            var team = new Team(request.Name, captainUser.Id, quest.Id, _teamService.GenerateTeamToken(6));
            await _context.Teams.AddAsync(team, cancellationToken);

            await _context.TeamUsers.AddAsync(new TeamUser
            {
                Team = team,
                User = captainUser
            }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return BaseResponse.Success(team, "Successfully created a new team");
        }
    }
}
