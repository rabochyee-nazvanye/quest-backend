using System.ComponentModel.DataAnnotations;

namespace Quest.API.BindingModels.Teams
{
    public class AddUserToTeamBM
    {
        [Required] public string RequestSecret { get; set; }
        [Required] public string UserId { get; set; }
    }
}