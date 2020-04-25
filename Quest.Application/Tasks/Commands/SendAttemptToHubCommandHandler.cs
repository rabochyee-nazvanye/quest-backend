using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Quest.Application.Tasks.Commands
{
    public class SendAttemptToHubCommandHandler : IRequestHandler<SendAttemptToHubCommand, BaseResponse<bool>>
    {
        private IHubContext<AttemptsHub> _hubContext;

        public SendAttemptToHubCommandHandler(IHubContext<AttemptsHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task<BaseResponse<bool>> Handle(SendAttemptToHubCommand request, CancellationToken cancellationToken)
        {
            await _hubContext.Clients.All.SendAsync("Send", request.TaskAttempt, cancellationToken: cancellationToken);
            throw new System.NotImplementedException();
        }
    }
}