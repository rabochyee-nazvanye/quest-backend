using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Quest.Domain.Models;

namespace Quest.Application.Teams.Queries
{
    public class GetParticipantInfoQuery : IRequest<Participant>
    {
        public GetParticipantInfoQuery(int teamId)
        {
            TeamId = teamId;
        }
        public int TeamId { get; set; }
    }
}
