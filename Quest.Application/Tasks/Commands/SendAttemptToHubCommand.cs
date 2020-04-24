using MediatR;
using Quest.Domain.Models;

namespace Quest.Application.Tasks.Commands
{
    public class SendAttemptToHubCommand : IRequest<BaseResponse<bool>>
    {
        public SendAttemptToHubCommand(TaskAttempt taskAttempt)
        {
            TaskAttempt = taskAttempt;
        }
        public TaskAttempt TaskAttempt { get; set; }
    }
}