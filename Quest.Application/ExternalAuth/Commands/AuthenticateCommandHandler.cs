using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Quest.Application.Services.ExternalAuth;
using Quest.Application.Services.ExternalAuth.AuthRequestValidators;
using Quest.Domain.Models;

namespace Quest.Application.ExternalAuth.Commands
{
    public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, BaseResponse<ApplicationUser>>
    {
        private readonly IExternalAuthService _externalAuthService;

        public AuthenticateCommandHandler(IExternalAuthService externalAuthService)
        {
            _externalAuthService = externalAuthService;
        }

        public async Task<BaseResponse<ApplicationUser>> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            var authRequestValidator = AuthRequestValidatorFactory.Create(request.AuthProvider);
            var authCredentials = await authRequestValidator.ValidateAuthRequestAsync(request.AccessToken);
            if (authCredentials == null)
            {
                return BaseResponse.Failure<ApplicationUser>("Failed to verify provided auth token.");
            }

            return await _externalAuthService.GetUserAsync(authCredentials);
        }
    }
}