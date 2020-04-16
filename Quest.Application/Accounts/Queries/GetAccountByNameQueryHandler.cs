using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quest.DAL.Data;
using Quest.Domain.Models;

namespace Quest.Application.Accounts.Queries
{
    public class GetAccountByNameQueryHandler : IRequestHandler<GetAccountByNameQuery, ApplicationUser>
    {
        private readonly Db _context;

        public GetAccountByNameQueryHandler(Db context)
        {
            _context = context;
        }

        public async Task<ApplicationUser> Handle(GetAccountByNameQuery request, CancellationToken cancellationToken)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == request.Username, cancellationToken: cancellationToken);
        }
    }
}
