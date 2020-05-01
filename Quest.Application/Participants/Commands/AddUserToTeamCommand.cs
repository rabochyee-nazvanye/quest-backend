using MediatR;

namespace Quest.Application.Participants.Commands
{
    public class AddUserToTeamCommand : IRequest<BaseResponse<bool>>
    {
        public AddUserToTeamCommand(string userId, string userToAddId, string teamSecret, int teamId)
        {
            UserId = userId;
            UserToAddId = userToAddId;
            TeamSecret = teamSecret;
            TeamId = teamId;
        }
        public string UserId { get; set; }
        public string UserToAddId { get; set; }
        public string TeamSecret { get; set; }
        public int TeamId { get; set; }
    }
}
