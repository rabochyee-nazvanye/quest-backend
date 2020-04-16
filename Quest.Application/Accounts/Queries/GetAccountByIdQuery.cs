using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Quest.Domain.Models;

namespace Quest.Application.Accounts.Queries
{
    public class GetAccountByIdQuery : IRequest<ApplicationUser>
    {
        public GetAccountByIdQuery(string userId)
        {
            UserId = userId;
        }
        public string UserId { get; set; }
    }
}
