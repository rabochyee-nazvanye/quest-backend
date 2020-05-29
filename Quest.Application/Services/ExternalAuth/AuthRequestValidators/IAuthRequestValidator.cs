using System.Threading.Tasks;
using Quest.Application.DTOs;

namespace Quest.Application.Services.ExternalAuth.AuthRequestValidators
{
    public interface IAuthRequestValidator
    {
        Task<ExternalAuthCredentials> ValidateAuthRequestAsync(string token);
    }
}