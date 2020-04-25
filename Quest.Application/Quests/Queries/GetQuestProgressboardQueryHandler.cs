using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quest.Application.DTOs;
using Quest.DAL.Data;
using Quest.Domain.Enums;
using Quest.Domain.Models;

namespace Quest.Application.Quests.Queries
{
    public class GetQuestProgressboardQueryHandler : IRequestHandler<GetQuestProgressboardQuery, BaseResponse<QuestProgressboardDTO>>
    {
        private readonly Db _context;
        private const int UsedHintPenaltyPercent = 20;

        public GetQuestProgressboardQueryHandler(Db context)
        {
            _context = context;
        }

        public async Task<BaseResponse<QuestProgressboardDTO>> Handle(GetQuestProgressboardQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Where(x => x.Id == request.UserId)
                .Include(x => x.ModeratedTeams)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                return BaseResponse.Failure<QuestProgressboardDTO>("Internal: user not found");

            if (user.ModeratedTeams.All(x => x.QuestId != request.QuestId))
                return BaseResponse.Failure<QuestProgressboardDTO>("User is not a moderator of this quest");
            
            var quest = await _context.Quests
                .Where(x => x.Id == request.QuestId)
                .Include(x => x.Tasks)
                .Include(x => x.Teams)
                .ThenInclude(x => x.TaskAttempts)
                .ThenInclude(x => x.TaskEntity)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (quest == null)
                return BaseResponse.Failure<QuestProgressboardDTO>("Quest not found.");

            var allQuestTasks = quest.Tasks.ToList();
            
            var teamsResults = quest.Teams.Select(team =>
            {
                var successfulAttempts = team.TaskAttempts
                    .Where(x => x.Status == TaskAttemptStatus.Accepted)
                    .ToLookup(x => x.TaskId);
                
                //todo optimizations, 12hrs before release...
                var attemptedTasksScores = successfulAttempts
                    .Select(x =>
                    {
                        var bestAttempt = x.OrderBy(attempt => attempt.UsedHintsCount).First();
                        return (taskId: bestAttempt.TaskId, attempt:bestAttempt);
                    })
                    .ToDictionary(x => x.taskId, x => x.attempt);

                var allTaskScores = allQuestTasks
                    .Select(x =>
                    {
                        if (attemptedTasksScores.TryGetValue(x.Id, out var attempt))
                        {
                            var taskReward = attempt.TaskEntity.Reward;
                            var totalPenaltyPercent = attempt.UsedHintsCount * UsedHintPenaltyPercent;
                            var taskScore = totalPenaltyPercent >= 100
                                ? 0
                                : (taskReward * (100 - totalPenaltyPercent)) / 100;

                            return (task: attempt.TaskEntity, score: taskScore);
                        }

                        return (task: x, score: 0);
                    })
                    .ToDictionary(x => x.task, x => x.score);

                return new TeamProgressDTO(team, allTaskScores);
            }).ToList();

            var allTasksLookupByGroup = allQuestTasks.ToLookup(x => x.Group, x => x);

            return BaseResponse.Success(new QuestProgressboardDTO(teamsResults, allTasksLookupByGroup));
        }
    }
}