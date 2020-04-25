using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Quest.Application.DTOs;

namespace Quest.Application.Tasks.Commands
{
    public class SendAttemptToHubCommandHandler : IRequestHandler<SendAttemptToHubCommand, BaseResponse<bool>>
    {
        private readonly IHubContext<AttemptsHub> _hubContext;

        public SendAttemptToHubCommandHandler(IHubContext<AttemptsHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task<BaseResponse<bool>> Handle(SendAttemptToHubCommand request, CancellationToken cancellationToken)
        {
            await _hubContext.Clients.All.SendAsync("Send",
                new TaskAttemptDTO(request.TaskAttempt), cancellationToken: cancellationToken);
            
            return BaseResponse.Success(true, "Message sent");
        }
    }
}