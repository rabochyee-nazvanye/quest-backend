using System.ComponentModel.DataAnnotations;

namespace Quest.API.ViewModels.Sessions
{
    public class LoginVM
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}