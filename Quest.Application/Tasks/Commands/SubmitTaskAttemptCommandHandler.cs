using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quest.Application.DTOs;
using Quest.Application.Services;
using Quest.DAL.Data;
using Quest.Domain.Enums;
using Quest.Domain.Interfaces;
using Quest.Domain.Models;

namespace Quest.Application.Tasks.Commands
{
    public class SubmitTaskAttemptCommandHandler : IRequestHandler<SubmitTaskAttemptCommand, BaseResponse<TaskAndHintsDTO>>
    {
        private readonly Db _context;
        private readonly IMediator _mediator;
        private readonly ICacheService _cache;
        private readonly UserManager<ApplicationUser> _userManager;

        public SubmitTaskAttemptCommandHandler(Db context, IMediator mediator, ICacheService cache, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mediator = mediator;
            _cache = cache;
            _userManager = userManager;
        }

        private static string Normalize(string x) => x.Trim().ToLowerInvariant();

        private async Task ProcessAttempt(TaskEntity task, string attemptText,
            Participant participant, int usedHintsCount, CancellationToken cancellationToken)
        {
            var taskAttempt =
                await _context.TaskAttempts
                    .Include(x => x.Participant)
                    .ThenInclude(x => x.Moderator)
                    .FirstOrDefaultAsync(x => x.TaskId == task.Id && x.ParticipantId == participant.Id, cancellationToken);
            
            if (taskAttempt != null)
            {
                // if this task is already solved or on manual review, exit
                if (taskAttempt.Status == TaskAttemptStatus.Accepted || 
                    task.VerificationType == VerificationType.Manual && taskAttempt.Status == TaskAttemptStatus.OnReview )
                {
                    return;
                }

                taskAttempt.Text = attemptText;
                taskAttempt.Status = TaskAttemptStatus.OnReview;
                taskAttempt.AdminComment = null;
                taskAttempt.UsedHintsCount = usedHintsCount;
                taskAttempt.SubmitTime = DateTime.Now.ToUniversalTime();
            }
            else
            {
                taskAttempt = new TaskAttempt
                {
                    TaskEntity = task,
                    ParticipantId = participant.Id,
                    Participant = participant,
                    Text = attemptText,
                    UsedHintsCount = usedHintsCount,
                    Status = TaskAttemptStatus.OnReview,
                    SubmitTime = DateTime.Now.ToUniversalTime()
                };
                await _context.AddAsync(taskAttempt, cancellationToken);
            }

            if (task.VerificationType == VerificationType.Automatic)
            {
                // do auto verification
                if (task.CorrectAnswers.Any(x => Normalize(x) == Normalize(taskAttempt.Text)))
                {
                    taskAttempt.Status = TaskAttemptStatus.Accepted;
                    // invalidate scoreboard caches
                    _cache.Invalidate(CacheName.ProgressBoardMain, task.QuestId.ToString());
                    _cache.Invalidate(CacheName.ProgressBoardSingleEntry, participant.Id.ToString());
                    _cache.Invalidate(CacheName.ScoreBoard, task.QuestId.ToString());
                    _cache.Invalidate(CacheName.ScoreBoardSingleEntry, participant.Id.ToString());

                }
                else
                    taskAttempt.Status = TaskAttemptStatus.Error;

                await _context.SaveChangesAsync(cancellationToken);

                return;
            }
            await _context.SaveChangesAsync(cancellationToken);

            await _context.Entry(participant).Reference(x => x.Moderator).LoadAsync(cancellationToken);
            
            var response = await _mediator.Send(new SendAttemptToHubCommand(taskAttempt), cancellationToken);
            if (!response.Result)
            {
                taskAttempt.AdminComment = response.Message;
                taskAttempt.Status = TaskAttemptStatus.Error;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        
        
        public async Task<BaseResponse<TaskAndHintsDTO>> Handle(SubmitTaskAttemptCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId,
                cancellationToken: cancellationToken);

            if (user == null)
                return BaseResponse.Failure<TaskAndHintsDTO>("Internal: user was not found");
            
            var task = await _context.Tasks.Where(x => x.Id == request.TaskId)
                .Include(x => x.Quest)
                .ThenInclude(x => x.Participants)
                    .ThenInclude(x => (x as Team).Members)
                            .ThenInclude(x => x.User)
                .Include(x => x.Quest)
                    .ThenInclude(x => x.Participants)
                        .ThenInclude(x => x.UsedHints)
                            .ThenInclude(x => x.Hint)
                .FirstOrDefaultAsync(cancellationToken);

            if (task == null)
                return BaseResponse.Failure<TaskAndHintsDTO>("Task was not found");

            if (task.Quest.IsHidden)
            {
                var userIsAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                if (!userIsAdmin)
                    return BaseResponse.Failure<TaskAndHintsDTO>("Task was not found");
            }

            var participant = task.Quest.FindParticipant(request.UserId);
            
            if (participant == null)
                return BaseResponse.Failure<TaskAndHintsDTO>("User is not participating in this quest");

            if (!task.Quest.IsReadyToReceiveTaskAttempts())
                return BaseResponse.Failure<TaskAndHintsDTO>("Quest is not in active state yet.");
            
            if (participant is Team team && team.GetDeadline() <= DateTime.Now)
            {
                return BaseResponse.Failure<TaskAndHintsDTO>("Can't receive any task attempts after the deadline.");
            }

            var usedHints = participant.UsedHints
                .Where(x => x.Hint.TaskId == task.Id)
                .Select(x => x.Hint)
                .ToList();
            
            await ProcessAttempt(task, request.AttemptText, participant, usedHints.Count, cancellationToken);

            var taskAttempt = await _context.TaskAttempts
                .Include(x => x.TaskEntity)
                .ThenInclude(x => x.Hints)
                .FirstOrDefaultAsync(x => x.TaskId == request.TaskId && x.ParticipantId == participant.Id,
                    cancellationToken);
            
            return BaseResponse.Success(new TaskAndHintsDTO(taskAttempt.TaskEntity,taskAttempt, usedHints), "Success");
        }
    }
}