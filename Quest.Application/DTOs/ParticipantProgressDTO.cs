using System;
using System.Collections.Generic;
using Quest.Domain.Models;

namespace Quest.Application.DTOs
{
    public class ParticipantProgressDTO
    {
        public ParticipantProgressDTO(Participant participant, Dictionary<TaskEntity, int> scores, DateTime lastSubmitDateTime)
        {
            Participant = participant;
            Scores = scores;
            LastAccSubmitTime = lastSubmitDateTime;
        }
        public Participant Participant { get; set; }
        public Dictionary<TaskEntity, int> Scores { get; set; }
        public DateTime LastAccSubmitTime { get; set; }
    }
}