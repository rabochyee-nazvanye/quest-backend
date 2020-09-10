using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Quest.Domain.Enums;

namespace Quest.Domain.Models
{
    public class TaskAttempt
    {
        public int ParticipantId { get; set; }
        [ForeignKey("ParticipantId")]
        public Participant Participant { get; set; }
        public int TaskId { get; set; }
        [ForeignKey("TaskId")]
        public TaskEntity TaskEntity { get; set; } 
        public string Text { get; set; }
        public string PhotoUrl { get; set; }
        public int UsedHintsCount { get; set; }
        public TaskAttemptStatus Status { get; set; }
        public string AdminComment { get; set; }
        public DateTime SubmitTime { get; set; }
    }
}