using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quest.Application.Services;
using Quest.DAL.Data;
using Quest.Domain.Enums;

namespace Quest.Application.Tasks.Commands
{
    public class VerifyTaskAttemptCommandHandler : IRequestHandler<VerifyTaskAttemptCommand, BaseResponse<bool>>
    {
        private readonly Db _context;
        private readonly ICacheService _cache;

        public VerifyTaskAttemptCommandHandler(Db context, ICacheService cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<BaseResponse<bool>> Handle(VerifyTaskAttemptCommand request, CancellationToken cancellationToken)
        {
            var taskAttempt = await _context.TaskAttempts
                .Include(x => x.TaskEntity)
                .FirstOrDefaultAsync(
                x => x.TaskId == request.TaskId && x.ParticipantId == request.ParticipantId, cancellationToken);

            if (taskAttempt == null)
                return BaseResponse.Failure<bool>("Task attempt was not found");

            if (taskAttempt.Status != TaskAttemptStatus.Accepted)
            {
                var currentTime = $"{DateTime.Now:MM/dd/yyyy H:mm} UTC";
                taskAttempt.Status = request.IsCorrect ? TaskAttemptStatus.Accepted : TaskAttemptStatus.Error;
                taskAttempt.AdminComment = (string.IsNullOrEmpty(request.Message))
                    ? $"{currentTime}: no message"
                    : $"{currentTime}: {request.Message}";
                await _context.SaveChangesAsync(cancellationToken);
                
                if (request.IsCorrect)
                {
                    // invalidate scoreboard caches
                    _cache.Invalidate(CacheName.ProgressBoardMain, taskAttempt.TaskEntity.QuestId.ToString());
                    _cache.Invalidate(CacheName.ProgressBoardSingleEntry, taskAttempt.ParticipantId.ToString());
                }
            }
            
            return BaseResponse.Success(true, "Success");
        }
    }
}