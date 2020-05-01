using System.ComponentModel.DataAnnotations;

namespace Quest.API.BindingModels.Teams
{
    public class CreateTeamBM
    {
        [Required]
        public int QuestId { get; set; }
        [Required]
        [MaxLength(26)]
        [MinLength(3)]
        public string Name { get; set; }
    }
}