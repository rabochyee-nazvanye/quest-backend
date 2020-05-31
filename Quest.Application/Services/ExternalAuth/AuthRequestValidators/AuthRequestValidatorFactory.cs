using System;
using Quest.Application.Enums;

namespace Quest.Application.Services.ExternalAuth.AuthRequestValidators
{
    public static class AuthRequestValidatorFactory
    {
        public static IAuthRequestValidator Create(OAuthProvider authProvider)
        {
            switch (authProvider)
            {
                case OAuthProvider.Google:
                    return new GoogleAuthRequestValidator();
                case OAuthProvider.GitHub:
                    throw new NotImplementedException();
                case OAuthProvider.Facebook:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(authProvider), authProvider, null);
            }
        }
    }
}