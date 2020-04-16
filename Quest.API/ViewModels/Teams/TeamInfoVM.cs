using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Quest.API.Models.ViewModels.Users;
using Quest.Domain.Models;

namespace Quest.API.Models.ViewModels.Teams
{
    public class TeamInfoVM
    {
        public TeamInfoVM(Team row)
        {
            Id = row.Id;
            Name = row.Name;
            Users = row.Members.Select(x => new UserBasicInfoVM(x.User)).ToList();
        }
        public int Id { get; set; }
        
        public string Name { get; set; }
        public List<UserBasicInfoVM> Users { get; set; }
    }
}
