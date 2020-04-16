using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Quest.Application.Teams.Commands
{
    public class RemoveUserFromTeamCommand : IRequest<BaseResponse<bool>>
    {
        public RemoveUserFromTeamCommand(int teamId, string userId, string userTokickId)
        {
            TeamId = teamId;
            UserId = userId;
            UserToKickId = userTokickId;
        }

        public int TeamId { get; set; }
        public string UserId { get; set; }
        public string UserToKickId { get; set; }
    }
}
