using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
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
        public DateTime EndDate { get; set; }

        public string AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public ApplicationUser Author { get; set; }

        public List<TaskEntity> Tasks { get; set; }
        
        public int MaxTeamSize { get; set; }

        public List<Team> Teams { get; set; }

        public QuestStatus GetQuestStatus()
        {
            var timeNow = DateTime.Now;
            if (StartDate > timeNow && timeNow > EndDate)
                return QuestStatus.InProgress;

            if (EndDate < timeNow)
                return QuestStatus.Finished;

            return QuestStatus.Scheduled;
        }


        public enum QuestStatus
        {
            Scheduled = 0,
            InProgress = 1,
            Finished = 2
        }
    }
}
