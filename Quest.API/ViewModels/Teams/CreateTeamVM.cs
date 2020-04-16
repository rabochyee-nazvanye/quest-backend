using System.ComponentModel.DataAnnotations;

namespace Quest.API.ViewModels.Teams
{
    public class CreateTeamVM
    {
        [Required]
        public int QuestId { get; set; }
        [Required]
        [MaxLength(26)]
        [MinLength(3)]
        public string Name { get; set; }
    }
}