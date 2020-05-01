using System.Collections.Generic;
using System.Linq;
using Quest.API.ResourceModels.Participants;
using Quest.API.ResourceModels.Users;
using Quest.Domain.Models;

namespace Quest.API.ResourceModels.Teams
{
    public class TeamWithCaptainAndMembersRM : ParticipantRM
    {
        public TeamWithCaptainAndMembersRM(Team row) : base(row)
        {
            Members = row.Members.Select(x => new UserRM(x.User)).ToList();
            Captain = new UserRM(row.Principal);
        }

  
        public UserRM Captain { get; set; }
        public List<UserRM> Members { get; set; }
    }
}
