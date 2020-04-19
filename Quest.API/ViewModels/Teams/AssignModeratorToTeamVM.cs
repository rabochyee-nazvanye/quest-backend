using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Quest.API.ViewModels.Teams
{
    public class AssignModeratorToTeamVM
    {
        [Required] public string ModeratorId { get; set; }
    }
}
