using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Quest.Domain.Enums;

namespace Quest.Domain.Models
{
    public class TaskEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Reward { get; set; }
        public int QuestId { get; set; }
        [ForeignKey("QuestId")]
        public QuestEntity Quest { get; set; }
        public VerificationType VerificationType { get; set; }
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }
        public string Group { get; set; }
        public List<Hint> Hints { get; set; }
        public List<TaskAttempt> TaskAttempts { get; set; }
        
        //quick and dirty multiple answers support implementation
        public List<string> CorrectAnswers => CorrectAnswer.Split(";").ToList();
    }
}