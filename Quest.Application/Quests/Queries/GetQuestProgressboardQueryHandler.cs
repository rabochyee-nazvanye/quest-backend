using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Quest.Application.DTOs;
using Quest.Application.Services;
using Quest.DAL.Data;
using Quest.Domain.Enums;
using Quest.Domain.Models;

namespace Quest.Application.Quests.Queries
{
    public class GetQuestProgressboardQueryHandler : IRequestHandler<GetQuestProgressboardQuery,
        BaseResponse<QuestParticipantProgressAndTasksDTO>>
    {
        private readonly Db _context;
        private const int UsedHintPenaltyPercent = 20;
        private readonly ICacheService _cache; 
        
        public GetQuestProgressboardQueryHandler(Db context, ICacheService cache)
        {
            _context = context;
            _cache = cache;
        }

        private async Task<QuestParticipantProgressAndTasksDTO> CreateProgressBoardAsync(int questId,
            CancellationToken cancellationToken)
        {
            var quest = await _context.Quests
                .Where(x => x.Id == questId)
                .Include(x => x.Tasks)
                .Include(x => x.Participants)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (quest == null)
                return null;

            var allQuestTasks = quest.Tasks.ToList();

            var teamsResults = new List<ParticipantProgressDTO>();
            foreach (var participant in quest.Participants)
            {
                Task<ParticipantProgressDTO> ParticipantProgressGetter() =>
                    CalcParticipantProgressAsync(participant.Id, allQuestTasks, CancellationToken.None);

                var teamResult = await _cache.GetOrAddAsync(CacheName.ProgressBoardSingleEntry, participant.Id.ToString(),
                    ParticipantProgressGetter);
                teamsResults.Add(teamResult);
            }
            
            var allTasksLookupByGroup = allQuestTasks.ToLookup(x => x.Group, x => x);
            return new QuestParticipantProgressAndTasksDTO(teamsResults, allTasksLookupByGroup);
        }

        private async Task<ParticipantProgressDTO> CalcParticipantProgressAsync(int participantId, IEnumerable<TaskEntity> allQuestTasks,
            CancellationToken cancellationToken)
        {
            var participant = await _context.Participants
                .Include(x => x.TaskAttempts)
                .ThenInclude(x => x.TaskEntity)
                .FirstOrDefaultAsync(x => x.Id == participantId, cancellationToken: cancellationToken);
            
            var acceptedAttempts = participant.TaskAttempts
                .Where(x => x.Status == TaskAttemptStatus.Accepted)
                .ToList();
            
            var successfulAttempts = acceptedAttempts
                .ToLookup(x => x.TaskId);

            //todo optimizations, 12hrs before release...
            var attemptedTasksScores = successfulAttempts
                .Select(x =>
                {
                    var bestAttempt = x.OrderBy(attempt => attempt.UsedHintsCount).First();
                    return (taskId: bestAttempt.TaskId, attempt: bestAttempt);
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

            var lastSuccessAttemptSubmitDate = DateTime.MinValue;

            if (successfulAttempts.Any())
            {
                var lastAcceptedAttempt = acceptedAttempts.OrderByDescending(x => x.SubmitTime).FirstOrDefault();
                if (lastAcceptedAttempt != null)
                {
                    lastSuccessAttemptSubmitDate = lastAcceptedAttempt.SubmitTime;
                }
            }

            return new ParticipantProgressDTO(participant, allTaskScores, lastSuccessAttemptSubmitDate);
        }

        public async Task<BaseResponse<QuestParticipantProgressAndTasksDTO>> Handle(GetQuestProgressboardQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Where(x => x.Id == request.UserId)
                .Include(x => x.ModeratedParticipants)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                return BaseResponse.Failure<QuestParticipantProgressAndTasksDTO>("Internal: user not found");

            if (user.ModeratedParticipants.All(x => x.QuestId != request.QuestId))
                return BaseResponse.Failure<QuestParticipantProgressAndTasksDTO>(
                    "User is not a moderator of this quest");

            Task<QuestParticipantProgressAndTasksDTO> ProgressBoardGetter() =>
                CreateProgressBoardAsync(request.QuestId, CancellationToken.None);

            var progressBoard = await _cache.GetOrAddAsync(CacheName.ProgressBoardMain, 
                request.QuestId.ToString(), ProgressBoardGetter);
            
            if (progressBoard == null)
            {
                return BaseResponse.Failure<QuestParticipantProgressAndTasksDTO>(
                    "Failed to create progress board.");
            }

            return BaseResponse.Success(progressBoard);
        }
    }
}