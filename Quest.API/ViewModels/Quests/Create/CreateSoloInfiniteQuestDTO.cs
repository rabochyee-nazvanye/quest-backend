using System;
using System.ComponentModel.DataAnnotations;

namespace Quest.API.ViewModels.Quests
{
    public class CreateSoloInfiniteQuestDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ImageUrl { get; set; }
    }
}