using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Quest.Domain.Models;

namespace Quest.Application.Teams.Commands
{
    public class RemoveTeamCommand : IRequest<BaseResponse<bool>>
    {
        public RemoveTeamCommand(string userId, int teamId)
        {
            UserId = userId;
            TeamId = teamId;
        }

        public string UserId { get; set; }
        public int TeamId { get; set; }
    }
}
