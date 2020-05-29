using Quest.Application.Enums;

namespace Quest.API.BindingModels.ExternalAuth
{
    public class ExternalAuthBM
    {
        public string AccessToken { get; set; }
        public OAuthProvider OAuthProvider { get; set; }
    }
}