using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Quest.Application.DTOs;
using Quest.DAL.Data;
using Quest.Domain.Interfaces;
using Quest.Domain.Models;

namespace Quest.Application.Tasks.Queries
{
    public class GetQuestTasksQueryHandler : IRequestHandler<GetQuestTasksQuery, BaseResponse<List<TaskStatusDTO>>>
    {
        private readonly Db _context;

        public GetQuestTasksQueryHandler(Db context)
        {
            _context = context;
        }

        public async Task<BaseResponse<List<TaskStatusDTO>>> Handle(GetQuestTasksQuery request,
            CancellationToken cancellationToken)
        {
            var quest = await _context.Quests
                .Include(x => x.Participants)
                    .ThenInclude(x => (x as Team).Members)
                        .ThenInclude(x => x.User)
                .Include(x => x.Participants)
                    .ThenInclude(x => x.TaskAttempts)
                .Include(x => x.Participants)
                    .ThenInclude(x => x.UsedHints)
                        .ThenInclude(x => x.Hint)
                .FirstOrDefaultAsync(x => x.Id == request.QuestId,
                cancellationToken: cancellationToken);

            if (quest == null)
                return BaseResponse.Failure<List<TaskStatusDTO>>("Could not find quest with provided id.");

            if (!quest.IsReadyToReceiveTaskAttempts())
                return BaseResponse.Failure<List<TaskStatusDTO>>("Quest is not in active state yet.");

            var userExists = await _context.Users.AnyAsync(x => x.Id == request.UserId,
                cancellationToken: cancellationToken);

            if (!userExists)
                return BaseResponse.Failure<List<TaskStatusDTO>>("Internal: request user does not exist.");

            var participant = quest.FindParticipant(request.UserId);
            
            if (participant == null)
                return BaseResponse.Failure<List<TaskStatusDTO>>("Could not find participant of this user.");

            var tasks = await _context.Tasks
                .Where(x => x.QuestId == request.QuestId)
                .Include(x => x.TaskAttempts)
                .Include(x => x.Hints)
                .ToListAsync(cancellationToken);

            var participantTaskStatuses = tasks
                .Select(x =>
                {
                    x.TaskAttempts = x.TaskAttempts.Where(x => x.ParticipantId == participant.Id).ToList();
                    return x;
                })
                .Select(x =>
                {
                    var usedHints = participant.UsedHints
                        .Where(h => h.Hint.TaskId == x.Id)
                        .Select(participantHint => participantHint.Hint)
                        .ToList();
                    return new TaskStatusDTO(x, usedHints);
                })
                .ToList();


            return BaseResponse.Success(participantTaskStatuses, "Success");
        }
    }
}