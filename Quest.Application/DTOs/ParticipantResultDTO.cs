using System;

namespace Quest.Application.DTOs
{
    public class ParticipantResultDTO
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public int Place { get; set; } 
        public DateTime LastSuccessfulSubmitTime { get; set; }
    }
}