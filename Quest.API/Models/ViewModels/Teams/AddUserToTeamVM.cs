using System.ComponentModel.DataAnnotations;

namespace Quest.API.Models.ViewModels.Teams
{
    public class AddUserToTeamVM
    {
        [Required] public int TeamID;
    }
}