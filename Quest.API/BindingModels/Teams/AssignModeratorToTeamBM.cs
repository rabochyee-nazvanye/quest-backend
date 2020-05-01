using System.ComponentModel.DataAnnotations;

namespace Quest.API.BindingModels.Teams
{
    public class AssignModeratorToTeamBM
    {
        [Required] public string ModeratorId { get; set; }
    }
}
