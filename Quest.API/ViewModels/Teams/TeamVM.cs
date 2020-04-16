using System.Collections.Generic;
using System.Linq;
using Quest.API.ViewModels.Users;
using Quest.Domain.Models;

namespace Quest.API.ViewModels.Teams
{
    public class TeamVM
    {
        public TeamVM(Team row)
        {
            Id = row.Id;
            Name = row.Name;
            Members = row.Members.Select(x => new UserVM(x.User)).ToList();
            Captain = new UserVM(row.Captain);
        }

        public int Id { get; set; }
        public UserVM Captain { get; set; }
        public string Name { get; set; }
        public List<UserVM> Members { get; set; }
    }
}
