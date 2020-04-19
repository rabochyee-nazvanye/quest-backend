using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quest.Domain.Models;

namespace Quest.API.ViewModels.Teams
{
    public class TeamVM
    {
        public TeamVM(Team row)
        {
            Id = row.Id;
            Name = row.Name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
