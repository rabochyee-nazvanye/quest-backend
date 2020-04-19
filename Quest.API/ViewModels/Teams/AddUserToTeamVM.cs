using System.ComponentModel.DataAnnotations;

namespace Quest.API.ViewModels.Teams
{
    public class AddUserToTeamVM
    {
        [Required] public string RequestSecret;
    }
}