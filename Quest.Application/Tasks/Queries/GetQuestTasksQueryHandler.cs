using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Quest.Application.DTOs;
using Quest.DAL.Data;
using Quest.Domain.Models;

namespace Quest.Application.Tasks.Queries
{
    public class GetQuestTasksQueryHandler : IRequestHandler<GetQuestTasksQuery, BaseResponse<List<TeamTaskStatusDTO>>>
    {
        private readonly Db _context;

        public GetQuestTasksQueryHandler(Db context)
        {
            _context = context;
        }

        public async Task<BaseResponse<List<TeamTaskStatusDTO>>> Handle(GetQuestTasksQuery request,
            CancellationToken cancellationToken)
        {
            var quest = await _context.Quests.FirstOrDefaultAsync(x => x.Id == request.QuestId,
                cancellationToken: cancellationToken);

            if (quest == null)
                return BaseResponse.Failure<List<TeamTaskStatusDTO>>("Could not find quest with provided id.");

            if (quest.GetQuestStatus() != QuestEntity.QuestStatus.InProgress)
                return BaseResponse.Failure<List<TeamTaskStatusDTO>>("Quest is not in active state yet.");

            var userExists = await _context.Users.AnyAsync(x => x.Id == request.UserId,
                cancellationToken: cancellationToken);

            if (!userExists)
                return BaseResponse.Failure<List<TeamTaskStatusDTO>>("Internal: request user does not exist.");

            var team = await _context.Teams
                .Where(x => x.QuestId == request.QuestId
                            && x.Members.Any(m => m.UserId == request.UserId))
                .Include(x => x.UsedHints)
                .ThenInclude(x => x.Hint)
                .FirstOrDefaultAsync(cancellationToken);

            if (team == null)
                return BaseResponse.Failure<List<TeamTaskStatusDTO>>("User do not belong to any team.");

            var tasks = await _context.Tasks
                .Where(x => x.QuestId == request.QuestId)
                .Include(x => x.TaskAttempts)
                .Include(x => x.Hints)
                .ToListAsync(cancellationToken);

            var teamTaskStatuses = tasks
                .Select(x =>
                {
                    x.TaskAttempts = x.TaskAttempts.Where(x => x.TeamId == team.Id).ToList();
                    return x;
                })
                .Select(x =>
                {
                    var usedHints = team.UsedHints
                        .Where(h => h.Hint.TaskId == x.Id)
                        .Select(x => x.Hint)
                        .ToList();
                    return new TeamTaskStatusDTO(x, usedHints);
                })
                .ToList();


            return BaseResponse.Success(teamTaskStatuses, "Success");
        }
    }
}