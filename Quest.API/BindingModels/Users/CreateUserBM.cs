using System.ComponentModel.DataAnnotations;

namespace Quest.API.BindingModels.Users
{
    public class CreateUserBM
    {
        [Required] public string Username { get; set; }
        [Required] public string Password { get; set; }
    }
}
