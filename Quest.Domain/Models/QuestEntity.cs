using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Quest.Domain.Models
{
    public class QuestEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime RegistrationDeadline { get; set; }
        public DateTime StartDate { get; set; }
        
        public string AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public ApplicationUser Author { get; set; }
        
        public List<Task> Tasks { get; set; }
        
        public ICollection<AppUserQuest> AppUserQuests { get; set; }

        public string InviteTokenSecret { get; set; }

        public List<Team> Teams { get; set; }
    }
}
