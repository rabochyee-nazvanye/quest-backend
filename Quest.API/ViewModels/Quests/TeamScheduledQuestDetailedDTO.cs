using System.Collections.Generic;
using System.Linq;
using Quest.API.ViewModels.Teams;
using Quest.API.ViewModels.Users;
using Quest.Domain.Interfaces;
using Quest.Domain.Models;

namespace Quest.API.ViewModels.Quests
{
    public class TeamScheduledQuestDetailedDTO : TeamScheduledQuestDTO
    {
        public TeamScheduledQuestDetailedDTO(IScheduledQuest row) : base(row)
        {
            Author = new UserDTO(row.Author);
            Teams = row.Participants.Select(x => new TeamWithCaptainAndMembersDTO(x as Team)).ToList();
        }
        public UserDTO Author { get; set; }
        public List<TeamWithCaptainAndMembersDTO> Teams { get; set; }
    }
}