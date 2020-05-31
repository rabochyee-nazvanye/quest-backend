using MediatR;
using Quest.Application.Enums;
using Quest.Domain.Models;

namespace Quest.Application.ExternalAuth.Commands
{
    public class AuthenticateCommand : IRequest<BaseResponse<ApplicationUser>>
    {
        public AuthenticateCommand(string accessToken, OAuthProvider provider)
        {
            AccessToken = accessToken;
            AuthProvider = provider;
        }
        public string AccessToken { get; set; }
        public OAuthProvider AuthProvider { get; set; }
    }
}