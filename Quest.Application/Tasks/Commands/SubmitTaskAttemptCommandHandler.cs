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

namespace Quest.Application.Tasks.Commands
{
    public class SubmitTaskAttemptCommandHandler : IRequestHandler<SubmitTaskAttemptCommand, BaseResponse<TeamTaskStatusDTO>>
    {
        private readonly Db _context;

        public SubmitTaskAttemptCommandHandler(Db context)
        {
            _context = context;
        }

        private async Task ProcessAttempt(TaskEntity task, string attemptText,
            int teamId, int usedHintsCount, CancellationToken cancellationToken)
        {
            static string Normalize(string x) => x.Trim().ToLowerInvariant();
            var taskAttempt = new TaskAttempt
            {
                TaskEntity = task,
                TeamId = teamId,
                Text = Normalize(attemptText),
                UsedHintsCount = usedHintsCount,
                Status = TaskAttemptStatus.OnReview,
                SubmitTime = DateTime.Now.ToUniversalTime()
            };

            if (task.VerificationType == VerificationType.Automatic)
            {
                // do auto verification
                if (task.CorrectAnswers.Any(x => Normalize(x) == taskAttempt.Text))
                    taskAttempt.Status = TaskAttemptStatus.Accepted;
                else
                    taskAttempt.Status = TaskAttemptStatus.Error;

                await _context.TaskAttempts.AddAsync(taskAttempt, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return;
            }
            
            // todo manual verification logic goes here
            throw new NotImplementedException();
        }
        
        
        public async Task<BaseResponse<TeamTaskStatusDTO>> Handle(SubmitTaskAttemptCommand request, CancellationToken cancellationToken)
        {
            var userExists = await _context.Users.AnyAsync(x => x.Id == request.UserId,
                cancellationToken: cancellationToken);

            if (!userExists)
                return BaseResponse.Failure<TeamTaskStatusDTO>("Internal: user not found");
            
            var task = await _context.Tasks.Where(x => x.Id == request.TaskId)
                .Include(x => x.TaskAttempts)
                .Include(x => x.Quest)
                    .ThenInclude(x => x.Teams)
                        .ThenInclude(x => x.Members)
                .Include(x => x.Quest)
                    .ThenInclude(x => x.Teams)
                        .ThenInclude(x => x.UsedHints)
                            .ThenInclude(x => x.Hint)
                .FirstOrDefaultAsync(cancellationToken);

            if (task == null)
                return BaseResponse.Failure<TeamTaskStatusDTO>("Task was not found");

            var team = task.Quest.Teams
                .FirstOrDefault(x => x.Members.Any(m => m.UserId == request.UserId));
            
            if (team == null)
                return BaseResponse.Failure<TeamTaskStatusDTO>("Team was not found");

            if (team.Quest.GetQuestStatus() != QuestEntity.QuestStatus.InProgress)
                return BaseResponse.Failure<TeamTaskStatusDTO>("Quest is not in active state yet.");

            var usedHints = team.UsedHints
                .Where(x => x.Hint.TaskId == task.Id)
                .Select(x => x.Hint)
                .ToList();
            
            await ProcessAttempt(task, request.AttemptText, team.Id, usedHints.Count, cancellationToken);
            
            // quick and dirty task re-fetch
            var updatedTask = await _context.Tasks.Where(x => x.Id == request.TaskId)
                .Include(x => x.TaskAttempts)
                .Include(x => x.Hints)
                .FirstOrDefaultAsync(cancellationToken);

            
            return BaseResponse.Success(new TeamTaskStatusDTO(updatedTask, usedHints), "Success");
        }
    }
}