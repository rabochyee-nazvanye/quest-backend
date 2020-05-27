using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;
using Quest.Domain.Interfaces;

namespace Quest.Domain.Models
{
    public abstract class QuestEntity : IQuest
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public ApplicationUser Author { get; set; }
        public List<TaskEntity> Tasks { get; set; }
        public abstract bool IsReadyToShowResults();
        public abstract bool IsReadyToReceiveTaskAttempts();
        public abstract bool RegistrationIsAvailable();
        public List<Participant> Participants { get; set; }
        public abstract Participant FindParticipant(string userId);
    }
}
