using System;
using System.ComponentModel.DataAnnotations;
using Quest.API.Enums;

namespace Quest.API.BindingModels.Quests
{
    public class CreateQuestBM
    {
        [Required] public string Name { get; set; }
        [Required] public string Description { get; set; }
        [Required] public string ImageUrl { get; set; }

        public DateTime RegistrationDeadline { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxTeamSize { get; set; }

        public QuestParticipantType ParticipantType { get; set; }
        public bool IsInfinite { get; set; }
    }
}
