using System.Collections.Generic;
using System.Linq;
using Quest.API.ViewModels.Users;
using Quest.Domain.Models;

namespace Quest.API.ViewModels.Teams
{
    public class TeamWithCaptainAndMembersDTO : TeamVM
    {
        public TeamWithCaptainAndMembersDTO(Team row) : base(row)
        {
            Members = row.Members.Select(x => new UserDTO(x.User)).ToList();
            Captain = new UserDTO(row.Principal);
        }

  
        public UserDTO Captain { get; set; }
        public List<UserDTO> Members { get; set; }
    }
}
