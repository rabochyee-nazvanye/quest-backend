using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Quest.Application.Teams.Commands
{
    public class AddUserToTeamCommand : IRequest<BaseResponse<bool>>
    {
        public AddUserToTeamCommand(string userId, string teamSecret, int teamId)
        {
            UserId = userId;
            TeamSecret = teamSecret;
            TeamId = teamId;
        }

        public string UserId { get; set; }
        public string TeamSecret { get; set; }
        public int TeamId { get; set; }
    }
}
