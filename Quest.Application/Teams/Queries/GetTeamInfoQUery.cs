using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Quest.Domain.Models;

namespace Quest.Application.Teams.Queries
{
    public class GetTeamInfoQuery : IRequest<Team>
    {
        public GetTeamInfoQuery(int teamId)
        {
            TeamId = teamId;
        }
        public int TeamId { get; set; }
    }
}
