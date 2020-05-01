using System.ComponentModel.DataAnnotations;

namespace Quest.API.BindingModels.SoloPlayers
{
    public class CreatePlayerBM
    {
        [Required]
        public int QuestId { get; set; }
    }
}