using System.Threading.Tasks;
using Google.Apis.Auth;
using Quest.Application.DTOs;

namespace Quest.Application.Services.ExternalAuth.AuthRequestValidators
{
    public class GoogleAuthRequestValidator : IAuthRequestValidator
    {
        public async Task<ExternalAuthCredentials> ValidateAuthRequestAsync(string token)
        {
            try
            {
                var payload = await GoogleJsonWebSignature
                    .ValidateAsync(token, new GoogleJsonWebSignature.ValidationSettings());
                return new ExternalAuthCredentials
                {
                    Username = payload.Email
                };
            }
            catch (InvalidJwtException)
            {
                return null;
            }
        }
    }
}