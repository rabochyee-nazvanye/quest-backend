using MediatR;
using Quest.Domain.Models;

namespace Quest.Application.Tasks.Commands
{
    public class SubmitTaskAttemptCommand : IRequest<BaseResponse<TaskEntity>>
    {
        public string AttemptText { get; set; }
        public string UserId { get; set; }
        public int TaskId { get; set; }
    }
}