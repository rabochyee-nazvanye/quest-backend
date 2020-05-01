using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quest.DAL.Data;
using Quest.Domain.Interfaces;
using Quest.Domain.Models;

namespace Quest.Application.Teams.Queries
{
    public class GetTeamByUserAndQuestQueryHandler : IRequestHandler<GetTeamByUserAndQuestQuery, BaseResponse<List<Team>>>
    {
        private readonly Db _context;

        public GetTeamByUserAndQuestQueryHandler(Db context)
        {
            _context = context;
        }

        public async Task<BaseResponse<List<Team>>> Handle(GetTeamByUserAndQuestQuery request, CancellationToken cancellationToken)
        {
            if (!request.MemberIds.Any())
                return null;
            
            var quest = await _context.Quests
                .Where(x => x.Id == request.QuestId)
                .FirstOrDefaultAsync(cancellationToken);

            if (quest == null)
                return BaseResponse.Failure<List<Team>>("Quest not found");
            
            if (!(quest is ITeamQuest teamQuest))
                return BaseResponse.Failure<List<Team>>("Quest with specified id is not a team quest");

            var teams = await _context.Teams
                .Where(x => x.QuestId == quest.Id &&
                                                        x.Members.Any(teamUser =>
                                                            request.MemberIds.Contains(teamUser.UserId)))
                .Include(x => x.Principal)
                .Include(x => x.Members)
                .ThenInclude(x => x.User)
                .ToListAsync(cancellationToken: cancellationToken);
            
            return BaseResponse.Success(teams, "Success");
        }
    }
}