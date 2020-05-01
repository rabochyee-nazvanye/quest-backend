using System.Collections.Generic;
using System.Linq;
using Quest.API.ResourceModels.Teams;
using Quest.API.ResourceModels.Users;
using Quest.Domain.Interfaces;
using Quest.Domain.Models;

namespace Quest.API.ResourceModels.Quests
{
    public class TeamScheduledQuestDetailedRm : TeamScheduledQuestRM
    {
        public TeamScheduledQuestDetailedRm(IScheduledQuest row) : base(row)
        {
            Author = new UserRM(row.Author);
            Teams = row.Participants.Select(x => new TeamWithCaptainAndMembersRM(x as Team)).ToList();
        }
        public UserRM Author { get; set; }
        public List<TeamWithCaptainAndMembersRM> Teams { get; set; }
    }
}