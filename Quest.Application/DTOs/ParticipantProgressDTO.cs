using System.Collections.Generic;
using Quest.Domain.Models;

namespace Quest.Application.DTOs
{
    public class ParticipantProgressDTO
    {
        public ParticipantProgressDTO(Participant participant, Dictionary<TaskEntity, int> scores)
        {
            Participant = participant;
            Scores = scores;
        }
        public Participant Participant { get; set; }
        public Dictionary<TaskEntity, int> Scores { get; set; }
    }
}