using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Quest.DAL.Data;
using Quest.Domain.Models;

namespace Quest.Application.Tasks.Queries
{
    public class GetQuestTasksQueryHandler : IRequestHandler<GetQuestTasksQuery, BaseResponse<List<TaskEntity>>>
    {
        private readonly Db _context;

        public GetQuestTasksQueryHandler(Db context)
        {
            _context = context;
        }

        public async Task<BaseResponse<List<TaskEntity>>> Handle(GetQuestTasksQuery request, CancellationToken cancellationToken)
        {
            var questExists = await _context.Quests.AnyAsync(x => x.Id == request.QuestId,
                cancellationToken: cancellationToken);

            if (!questExists)
                return BaseResponse.Failure<List<TaskEntity>>("Could not find quest with provided id.");
            
            var userExists = await _context.Users.AnyAsync(x => x.Id == request.UserId,
                cancellationToken: cancellationToken);
            
            if (!userExists)
                return BaseResponse.Failure<List<TaskEntity>>("Internal: request user does not exist.");

            var team = await _context.Teams
                .Where(x => x.QuestId == request.QuestId
                            && x.Members.Any(m => m.UserId == request.UserId))
                .FirstOrDefaultAsync(cancellationToken);
            
            if (team == null)
                return BaseResponse.Failure<List<TaskEntity>>("User do not belong to any team.");

            var tasks  = await _context.Tasks
                .Where(x => x.QuestId == request.QuestId)
                .Include(x => x.TaskAttempts)
                .Include(x => x.Hints)
                .ToListAsync(cancellationToken);

            var teamTasks = tasks.Select(x =>
            {
                x.TaskAttempts = x.TaskAttempts.Where(x => x.TeamId == team.Id).ToList();
                return x;
            }).ToList();

            return BaseResponse.Success(teamTasks, "Success");
        }
    }
}