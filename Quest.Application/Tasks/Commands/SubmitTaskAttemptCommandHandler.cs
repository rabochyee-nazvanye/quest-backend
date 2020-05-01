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
using Quest.Domain.Interfaces;
using Quest.Domain.Models;

namespace Quest.Application.Tasks.Commands
{
    public class SubmitTaskAttemptCommandHandler : IRequestHandler<SubmitTaskAttemptCommand, BaseResponse<TaskStatusDTO>>
    {
        private readonly Db _context;
        private readonly IMediator _mediator;

        public SubmitTaskAttemptCommandHandler(Db context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        private async Task ProcessAttempt(TaskEntity task, string attemptText,
            int participantId, int usedHintsCount, CancellationToken cancellationToken)
        {
            static string Normalize(string x) => x.Trim().ToLowerInvariant();
            var taskAttempt = new TaskAttempt
            {
                TaskEntity = task,
                ParticipantId = participantId,
                Text = attemptText,
                UsedHintsCount = usedHintsCount,
                Status = TaskAttemptStatus.OnReview,
                SubmitTime = DateTime.Now.ToUniversalTime()
            };

            if (task.VerificationType == VerificationType.Automatic)
            {
                // do auto verification
                if (task.CorrectAnswers.Any(x => Normalize(x) == Normalize(taskAttempt.Text)))
                    taskAttempt.Status = TaskAttemptStatus.Accepted;
                else
                    taskAttempt.Status = TaskAttemptStatus.Error;

                await _context.TaskAttempts.AddAsync(taskAttempt, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return;
            }
            
            await _context.TaskAttempts.AddAsync(taskAttempt, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            var response = await _mediator.Send(new SendAttemptToHubCommand(taskAttempt), cancellationToken);
            if (!response.Result)
            {
                taskAttempt.AdminComment = response.Message;
                taskAttempt.Status = TaskAttemptStatus.Error;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        
        
        public async Task<BaseResponse<TaskStatusDTO>> Handle(SubmitTaskAttemptCommand request, CancellationToken cancellationToken)
        {
            var userExists = await _context.Users.AnyAsync(x => x.Id == request.UserId,
                cancellationToken: cancellationToken);

            if (!userExists)
                return BaseResponse.Failure<TaskStatusDTO>("Internal: user not found");
            
            var task = await _context.Tasks.Where(x => x.Id == request.TaskId)
                .Include(x => x.TaskAttempts)
                .Include(x => x.Quest)
                .ThenInclude(x => x.Participants)
                    .ThenInclude(x => (x as Team).Members)
                            .ThenInclude(x => x.User)
                .Include(x => x.Quest)
                    .ThenInclude(x => x.Participants)
                        .ThenInclude(x => x.Moderator)
                .Include(x => x.Quest)
                    .ThenInclude(x => x.Participants)
                        .ThenInclude(x => x.UsedHints)
                            .ThenInclude(x => x.Hint)
                .FirstOrDefaultAsync(cancellationToken);

            if (task == null)
                return BaseResponse.Failure<TaskStatusDTO>("Task was not found");

            var participant = task.Quest.FindParticipant(request.UserId);
            
            if (participant == null)
                return BaseResponse.Failure<TaskStatusDTO>("User is not participating in this quest");

            if (!task.Quest.IsReadyToReceiveTaskAttempts())
                return BaseResponse.Failure<TaskStatusDTO>("Quest is not in active state yet.");

            var usedHints = participant.UsedHints
                .Where(x => x.Hint.TaskId == task.Id)
                .Select(x => x.Hint)
                .ToList();
            
            await ProcessAttempt(task, request.AttemptText, participant.Id, usedHints.Count, cancellationToken);
            
            // quick and dirty task re-fetch
            var updatedTask = await _context.Tasks.Where(x => x.Id == request.TaskId)
                .Include(x => x.TaskAttempts)
                .Include(x => x.Hints)
                .FirstOrDefaultAsync(cancellationToken);

            
            return BaseResponse.Success(new TaskStatusDTO(updatedTask, usedHints), "Success");
        }
    }
}