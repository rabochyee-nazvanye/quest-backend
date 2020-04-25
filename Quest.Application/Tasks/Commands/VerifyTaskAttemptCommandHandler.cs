using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quest.DAL.Data;
using Quest.Domain.Enums;

namespace Quest.Application.Tasks.Commands
{
    public class VerifyTaskAttemptCommandHandler : IRequestHandler<VerifyTaskAttemptCommand, BaseResponse<bool>>
    {
        private readonly Db _context;

        public VerifyTaskAttemptCommandHandler(Db context)
        {
            _context = context;
        }

        public async Task<BaseResponse<bool>> Handle(VerifyTaskAttemptCommand request, CancellationToken cancellationToken)
        {
            var taskAttempt = await _context.TaskAttempts.Where(x => x.Id == request.AttemptId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (taskAttempt == null)
                return BaseResponse.Failure<bool>("Task attempt was not found");

            if (taskAttempt.Status == TaskAttemptStatus.OnReview)
            {
                taskAttempt.Status = request.IsCorrect ? TaskAttemptStatus.Accepted : TaskAttemptStatus.Error;
                taskAttempt.AdminComment = (string.IsNullOrEmpty(request.Message)) ? null : request.Message;
                await _context.SaveChangesAsync(cancellationToken);
            }
            
            return BaseResponse.Success(true, "Success");
        }
    }
}