using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Quest.Domain.Models;

namespace Quest.Application.Accounts.Queries
{
    public class GetAccountByNameQuery : IRequest<ApplicationUser>
    {
        public GetAccountByNameQuery(string username)
        {
            Username = username;
        }
        public string Username { get; set; }
    }
}
