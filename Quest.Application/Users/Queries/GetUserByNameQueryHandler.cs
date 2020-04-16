using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quest.DAL.Data;
using Quest.Domain.Models;

namespace Quest.Application.Users.Queries
{
    public class GetUserByNameQueryHandler : IRequestHandler<GetUserByNameQuery, ApplicationUser>
    {
        private readonly Db _context;

        public GetUserByNameQueryHandler(Db context)
        {
            _context = context;
        }

        public async Task<ApplicationUser> Handle(GetUserByNameQuery request, CancellationToken cancellationToken)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == request.Username, cancellationToken: cancellationToken);
        }
    }
}
