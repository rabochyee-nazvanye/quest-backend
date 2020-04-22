using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quest.DAL.Data;
using Quest.Domain.Enums;
using Quest.Domain.Models;

namespace Quest.Application.Tasks.Commands
{
    public class SubmitTaskAttemptCommandHandler : IRequestHandler<SubmitTaskAttemptCommand, BaseResponse<TaskEntity>>
    {
        private readonly Db _context;

        public SubmitTaskAttemptCommandHandler(Db context)
        {
            _context = context;
        }

        private async Task ProcessAttempt(TaskEntity task, string attemptText,
            int teamId, CancellationToken cancellationToken)
        {
            static string Normalize(string x) => x.Trim().ToLowerInvariant();
            
            var taskAttempt = new TaskAttempt
            {
                TaskEntity = task,
                TeamId = teamId,
                Text = Normalize(attemptText),
                Status = TaskAttemptStatus.OnReview
            };

            if (task.VerificationType == VerificationType.Manual)
            {
                // do auto verification
                var normalizedAnswer = Normalize(task.CorrectAnswer);

                if (attemptText == normalizedAnswer)
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
        
        
        public async Task<BaseResponse<TaskEntity>> Handle(SubmitTaskAttemptCommand request, CancellationToken cancellationToken)
        {
            var userExists = await _context.Users.AnyAsync(x => x.Id == request.UserId,
                cancellationToken: cancellationToken);

            if (!userExists)
                return BaseResponse.Failure<TaskEntity>("Internal: user not found");

            var team = await _context.Teams.Where(x => x.Id == request.TeamId)
                .Include(x => x.Members)
                .FirstOrDefaultAsync(cancellationToken);

            if (team == null)
                return BaseResponse.Failure<TaskEntity>("Team was not found");

            if (team.Members.All(x => x.UserId != request.UserId))
                return BaseResponse.Failure<TaskEntity>("User do not belong to specified team");

            var task = await _context.Tasks.Where(x => x.Id == request.TaskId)
                .Include(x => x.TaskAttempts)
                .Include(x => x.Quest)
                .FirstOrDefaultAsync(cancellationToken);

            if (task == null)
                return BaseResponse.Failure<TaskEntity>("Task was not found");

            if (task.QuestId != team.QuestId)
                return BaseResponse.Failure<TaskEntity>("Task quest does not match team quest");
            
            if (team.Quest.GetQuestStatus() != QuestEntity.QuestStatus.InProgress)
                return BaseResponse.Failure<TaskEntity>("Quest is not in active state yet.");

            
            await ProcessAttempt(task, request.AttemptText, team.Id, cancellationToken);
            
            // quick and dirty task re-fetch
            var taskUpdated = await _context.Tasks.Where(x => x.Id == request.TaskId)
                .Include(x => x.TaskAttempts)
                .Include(x => x.Hints)
                .FirstOrDefaultAsync(cancellationToken);

            return BaseResponse.Success(taskUpdated, "Success");
        }
    }
}