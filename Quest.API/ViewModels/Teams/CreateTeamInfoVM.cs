using System.ComponentModel.DataAnnotations;

namespace Quest.API.Models.ViewModels.Teams
{
    public class CreateTeamInfoVM
    {
        [Required]
        public int QuestId { get; set; }
        [Required]
        [MaxLength(26)]
        [MinLength(3)]
        public string Name { get; set; }
    }
}