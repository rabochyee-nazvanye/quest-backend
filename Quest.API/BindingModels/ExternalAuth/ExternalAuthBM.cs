using System.ComponentModel.DataAnnotations;
using Quest.Application.Enums;

namespace Quest.API.BindingModels.ExternalAuth
{
    public class ExternalAuthBM
    {
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public OAuthProvider OAuthProvider { get; set; }
    }
}