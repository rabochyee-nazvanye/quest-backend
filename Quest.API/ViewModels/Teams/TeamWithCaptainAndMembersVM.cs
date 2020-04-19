using System.Collections.Generic;
using System.Linq;
using Quest.API.ViewModels.Users;
using Quest.Domain.Models;

namespace Quest.API.ViewModels.Teams
{
    public class TeamWithCaptainAndMembersVM : TeamVM
    {
        public TeamWithCaptainAndMembersVM(Team row) : base(row)
        {
            Members = row.Members.Select(x => new UserVM(x.User)).ToList();
            Captain = new UserVM(row.Captain);
        }

  
        public UserVM Captain { get; set; }
        public List<UserVM> Members { get; set; }
    }
}
