using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quest.Domain.Models
{
    public class Hint
    {
        [Key] public int Id { get; set; }
        
        public int TaskId { get; set; }
        [ForeignKey("TaskId")]
        public TaskEntity TaskEntity { get; set; }
        
        public string Name { get; set; }
        public string Secret { get; set; }
        public int Sorting { get; set; }
        
        public ICollection<TeamHint> UsedTeamHints { get; set; } 
    }
}