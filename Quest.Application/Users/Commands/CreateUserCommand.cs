using MediatR;
using Quest.Domain.Models;

namespace Quest.Application.Users.Commands
{
    public class CreateUserCommand : IRequest<BaseResponse<ApplicationUser>>
    {
        public CreateUserCommand(string username, string password)
        {
            Username = username;
            Password = password;
        }
        public CreateUserCommand(string username, string password, string avatarUrl) : this(username, password)
        {
            AvatarUrl = avatarUrl;
        }


        public string Username { get; set; }
        public string Password { get; set; }
        public string AvatarUrl { get; set; }
    }
}
