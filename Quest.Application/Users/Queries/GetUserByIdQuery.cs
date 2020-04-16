using MediatR;
using Quest.Domain.Models;

namespace Quest.Application.Users.Queries
{
    public class GetUserByIdQuery : IRequest<ApplicationUser>
    {
        public GetUserByIdQuery(string userId)
        {
            UserId = userId;
        }
        public string UserId { get; set; }
    }
}
