using System.ComponentModel.DataAnnotations.Schema;

namespace Quest.Domain.Models
{
    public class ParticipantHint
    {
        public int HintId { get; set; }
        [ForeignKey("HintId")]
        public Hint Hint { get; set; }
        public int ParticipantId { get; set; }
        [ForeignKey("ParticipantId")]
        public Participant Participant { get; set; }
    }
}