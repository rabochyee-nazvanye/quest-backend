using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quest.Application.DTOs;
using Quest.Application.Services;
using Quest.DAL.Data;
using Quest.Domain.Enums;
using Quest.Domain.Models;

namespace Quest.Application.Quests.Queries
{
    public class GetQuestScoreboardQueryHandler : IRequestHandler<GetQuestScoreboardQuery, BaseResponse<QuestScoreboardDTO>>
    {
        private readonly Db _context;
        private const int UsedHintPenaltyPercent = 20;
        private readonly ICacheService _cache;

        public GetQuestScoreboardQueryHandler(Db context, ICacheService cache)
        {
            _context = context;
            _cache = cache;
        }

        private async Task<QuestScoreboardDTO> CreateScoreboardAsync(int questId, CancellationToken cancellationToken)
        {
            var quest = await _context.Quests
                .Where(x => x.Id == questId)
                .Include(x => x.Tasks)
                .Include(x => x.Participants)
                .ThenInclude(x => x.TaskAttempts)
                .ThenInclude(x => x.TaskEntity)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            
            var teamsResults = quest.Participants.Select(team =>
            {
                var successfulAttempts = team.TaskAttempts
                    .Where(x => x.Status == TaskAttemptStatus.Accepted)
                    .ToLookup(x => x.TaskId);

                var bestAttempts = successfulAttempts
                    .Select(x => x.OrderBy(attempt => attempt.UsedHintsCount).First())
                    .ToList();

                var bestAttemptScores = bestAttempts.Select(attempt =>
                {
                    var taskReward = attempt.TaskEntity.Reward;
                    var totalPenaltyPercent = attempt.UsedHintsCount * UsedHintPenaltyPercent;
                    if (totalPenaltyPercent >= 100)
                        return 0;

                    return (taskReward * (100 - totalPenaltyPercent)) / 100;
                }).ToList();

                return new ParticipantResultDTO
                {
                    Name = team.Name,
                    Score = bestAttemptScores.Sum()
                };
            }).ToList();

            var sortedResults = teamsResults
                .OrderByDescending(x => x.Score)
                .Select((x, idx) =>
                {
                    x.Place = idx;
                    return x;
                })
                .ToList();

            return new QuestScoreboardDTO(sortedResults);
        }
        
        public async Task<BaseResponse<QuestScoreboardDTO>> Handle(GetQuestScoreboardQuery request, CancellationToken cancellationToken)
        {
            var quest = await _context.Quests.FindAsync(request.QuestId);

            if (quest == null)
                return BaseResponse.Failure<QuestScoreboardDTO>("Quest not found.");

            Task<QuestScoreboardDTO> ScoreBoardGetter() =>
                CreateScoreboardAsync(request.QuestId, CancellationToken.None);
            
            var scoreBoard = await _cache.GetOrAddAsync(CacheName.ScoreBoard, 
                request.QuestId.ToString(), ScoreBoardGetter);
            
            return new BaseResponse<QuestScoreboardDTO>(scoreBoard, "Success");
        }
    }
}