using MediatR;

namespace Quest.Application.Tasks.Commands
{
    public class VerifyTaskAttemptCommand : IRequest<BaseResponse<bool>>
    {
        public int AttemptId { get; set; }
        public string Message { get; set; }
        public bool IsCorrect { get; set; }
    }
}