using System.ComponentModel.DataAnnotations;

namespace Quest.API.BindingModels.Quests
{
    public class CreateSoloInfiniteQuestBM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ImageUrl { get; set; }
    }
}