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
            if (request.TaskAttempt.Participant.Moderator == null)
                return BaseResponse.Failure<bool>( $"{request.TaskAttempt.SubmitTime:MM/dd/yyyy H:mm} UTC: No moderator assigned");
            
            if (request.TaskAttempt.Participant.Moderator.TelegramId == 0)
                return BaseResponse.Failure<bool>( 
                    $"{request.TaskAttempt.SubmitTime:MM/dd/yyyy H:mm} UTC: Moderator doesn't have telegram id assigned");
            
            await _hubContext.Clients.All.SendAsync("Send",
                new TaskAttemptDTO(request.TaskAttempt), cancellationToken: cancellationToken);
            
            return BaseResponse.Success(true, "Message sent");
        }
    }
}