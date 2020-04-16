using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Quest.Application.Teams.Commands
{
    public class RemoveUserFromTeamCommand : IRequest<BaseResponse<bool>>
    {
        public RemoveUserFromTeamCommand(int teamId, string userId)
        {
            TeamId = teamId;
            UserId = userId;
        }

        public int TeamId { get; set; }
        public string UserId { get; set; }
    }
}
