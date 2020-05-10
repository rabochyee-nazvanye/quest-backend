using MediatR;
using Quest.Application.Services;
using Quest.Domain.Models;

namespace Quest.Application.Participants.Commands
{
    public class FinishQuestCommand : IRequest<BaseResponse<Participant>>
    {
        public FinishQuestCommand(string userId, int participantId)
        {
            UserId = userId;
            ParticipantId = participantId;
        }

        public int ParticipantId { get; }
        public string UserId { get; }
    }
}