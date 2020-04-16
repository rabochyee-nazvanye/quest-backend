using MediatR;
using Quest.Domain.Models;

namespace Quest.Application.Users.Queries
{
    public class GetUserByNameQuery : IRequest<ApplicationUser>
    {
        public GetUserByNameQuery(string username)
        {
            Username = username;
        }
        public string Username { get; set; }
    }
}
