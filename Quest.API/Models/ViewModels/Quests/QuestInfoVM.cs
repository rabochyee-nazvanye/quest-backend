using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Quest.API.Models.ViewModels.Teams;
using Quest.Domain.Models;

namespace Quest.API.Models.ViewModels.Quests
{
    public class QuestInfoVM
    {
        public QuestInfoVM(QuestEntity row)
        {
            Id = row.Id;
            Name = row.Name;
            Description = row.Description;
            ImageUrl = row.ImageUrl;
            RegistrationDeadline = row.RegistrationDeadline.ToString("dd.MM.yyyy HH:mm");
            StartDate = row.StartDate.ToString("dd.MM.yyyy HH:mm");
            AuthorName = row.Author.UserName;
            Teams = row.Teams.Select(x => new TeamBasicInfoVM(x)).ToList();
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        public string RegistrationDeadline { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        public string StartDate { get; set; }

        public string AuthorName { get; set; }
        public List<TeamBasicInfoVM> Teams { get; set; }
    }
}
