using System;
using System.ComponentModel.DataAnnotations;

namespace Quest.API.ViewModels.Quests
{
    public class CreateTeamScheduledQuestDTO
    { 
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public DateTime RegistrationDeadline { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Range(1, int.MaxValue)]
        public int MaxTeamSize { get; set; }
    }
}
