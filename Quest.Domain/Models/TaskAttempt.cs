using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Quest.Domain.Enums;

namespace Quest.Domain.Models
{
    public class TaskAttempt
    {
        [Key]
        public int Id { get; set; }

        public string TaskId { get; set; }
        [ForeignKey("TaskId")]
        public Task Task { get; set; }
        
        public string TeamId { get; set; }
        [ForeignKey("TeamId")]
        public Team Team { get; set; }

        public string Text { get; set; }
        public string PhotoUrl { get; set; }
        
        public TaskAttemptStatus Status { get; set; }
        
        public string AdminComment { get; set; }
    }
}