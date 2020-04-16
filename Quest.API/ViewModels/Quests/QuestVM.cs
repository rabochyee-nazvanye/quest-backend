using System;
using System.Collections.Generic;
using System.Linq;
using Quest.API.ViewModels.Teams;
using Quest.API.ViewModels.Users;
using Quest.Domain.Models;

namespace Quest.API.ViewModels.Quests
{
    public class QuestVM
    {
        public QuestVM(QuestEntity row)
        {
            Id = row.Id;
            Name = row.Name;
            Description = row.Description;
            ImageUrl = row.ImageUrl;
            RegistrationDeadline = row.RegistrationDeadline;
            StartDate = row.StartDate;
            EndDate = row.EndDate;
            Status = row.GetQuestStatus().ToString().ToLowerInvariant();
            Author = new UserVM(row.Author);
            Teams = row.Teams.Select(x => new TeamVM(x)).ToList();
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public DateTime RegistrationDeadline { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public UserVM Author { get; set; }
        public List<TeamVM> Teams { get; set; }
    }
}
