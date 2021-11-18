using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Quest.API.Enums;

namespace Quest.API.Interfaces
{
    public interface IQuestBM
    {
        public string Name { get; set; } 
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime RegistrationDeadline { get; set; }
        public bool IsRegistrationLimited { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxTeamSize { get; set; }

        public QuestParticipantType ParticipantType { get; set; }
        public bool IsInfinite { get; set; }
        public int TimeToCompleteInMinutes { get; set; }
    }
}