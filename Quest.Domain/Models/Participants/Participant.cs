using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Quest.Domain.Interfaces;

namespace Quest.Domain.Models
{
    public abstract class Participant
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int QuestId { get; set; }
        
        [ForeignKey("QuestId")]
        public QuestEntity Quest { get; set; }
        public List<TaskAttempt> TaskAttempts { get; set; }
        public List<ParticipantHint> UsedHints { get; set; }
        public ApplicationUser Moderator { get; set; }
        
        public string PrincipalUserId { get; set; }
        [ForeignKey("PrincipalUserId")]
        public ApplicationUser Principal { get; set; }
    }
}