using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Quest.Domain.Models;

namespace Quest.Application.Teams.Queries
{
    public class GetTeamBySecretQuery : IRequest<Team>
    {
        public GetTeamBySecretQuery(string secret)
        {
            Secret = secret;
        }
        public string Secret { get; set; }
    }
}
