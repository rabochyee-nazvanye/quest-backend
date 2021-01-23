using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Quest.Application.DTOs;
using Quest.DAL.Data;
using Quest.Domain.Interfaces;
using Quest.Domain.Models;

namespace Quest.Application.Tasks.Queries
{
    public class GetQuestTasksQueryHandler : IRequestHandler<GetQuestTasksQuery, BaseResponse<List<TaskAndHintsDTO>>>
    {
        private readonly Db _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetQuestTasksQueryHandler(Db context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<BaseResponse<List<TaskAndHintsDTO>>> Handle(GetQuestTasksQuery request,
            CancellationToken cancellationToken)
        {
            var quest = await _context.Quests
                .Include(x => x.Participants)
                    .ThenInclude(x => (x as Team).Members)
                        .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == request.QuestId,
                cancellationToken: cancellationToken);

            if (quest == null)
                return BaseResponse.Failure<List<TaskAndHintsDTO>>("Could not find quest with provided id.");

            if (!quest.IsReadyToReceiveTaskAttempts())
                return BaseResponse.Failure<List<TaskAndHintsDTO>>("Quest is not in active state yet.");

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId,
                cancellationToken: cancellationToken);

            if (user == null)
                return BaseResponse.Failure<List<TaskAndHintsDTO>>("Internal: request user does not exist.");

            if (quest.IsHidden)
            {
                var userIsAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                if (!userIsAdmin)
                    return BaseResponse.Failure<List<TaskAndHintsDTO>>("Could not find quest with provided id.");
            }
            
            var participant = quest.FindParticipant(request.UserId);
            
            if (participant == null)
                return BaseResponse.Failure<List<TaskAndHintsDTO>>("Could not find participant of this user.");
            
            var allTasks = await _context.Tasks
                .Where(x => x.QuestId == request.QuestId)
                .Include(x => x.Hints)
                .ToListAsync(cancellationToken);

            await _context.Entry(participant)
                .Collection(x => x.UsedHints)
                .Query()
                .Include(x => x.Hint)
                .LoadAsync(cancellationToken);

            await _context.Entry(participant)
                .Collection(x => x.TaskAttempts)
                .LoadAsync(cancellationToken);

            var usedHintsByTaskIds = participant.UsedHints
                .ToLookup(x => x.Hint.TaskId, x => x.Hint);
            
            var taskAttemptByTaskIds = participant
                .TaskAttempts.ToDictionary(x => x.TaskId, x => x);

            var participantTaskStatuses = allTasks
                .Select(x =>
                {
                    var usedHints = usedHintsByTaskIds[x.Id].ToList();
                    taskAttemptByTaskIds.TryGetValue(x.Id, out var taskAttempt);
                    return new TaskAndHintsDTO(x, taskAttempt, usedHints);
                })
                .ToList();


            return BaseResponse.Success(participantTaskStatuses, "Success");
        }
    }
}