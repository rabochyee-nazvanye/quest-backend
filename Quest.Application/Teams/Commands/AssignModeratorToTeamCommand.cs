using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Quest.Application.Teams.Commands
{
    public class AssignModeratorToTeamCommand : IRequest<BaseResponse<bool>>
    {
        public AssignModeratorToTeamCommand(int teamId, string userId, string moderatorId)
        {
            TeamId = teamId;
            UserId = userId;
            ModeratorId = moderatorId;
        }
        public int TeamId { get; set; }
        public string UserId { get; set; }
        public string ModeratorId { get; set; }
    }
}
