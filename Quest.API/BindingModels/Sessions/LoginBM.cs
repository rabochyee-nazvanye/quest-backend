using System.ComponentModel.DataAnnotations;

namespace Quest.API.BindingModels.Sessions
{
    public class LoginBM
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}