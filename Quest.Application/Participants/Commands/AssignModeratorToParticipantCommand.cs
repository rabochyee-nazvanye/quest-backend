using MediatR;

namespace Quest.Application.Participants.Commands
{
    public class AssignModeratorToParticipantCommand : IRequest<BaseResponse<bool>>
    {
        public AssignModeratorToParticipantCommand(int participantId, string userId, string moderatorId)
        {
            ParticipantId = participantId;
            UserId = userId;
            ModeratorId = moderatorId;
        }
        public int ParticipantId { get; set; }
        public string UserId { get; set; }
        public string ModeratorId { get; set; }
    }
}
