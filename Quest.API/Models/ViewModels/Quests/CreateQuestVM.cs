using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Quest.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace Quest.API.Models.ViewModels.Quests
{
    public class CreateQuestVM
    { 
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public DateTime RegistrationDeadline { get; set; }
        public DateTime StartDate { get; set; }
    }
}
