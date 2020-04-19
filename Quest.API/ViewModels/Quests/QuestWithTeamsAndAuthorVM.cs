using System;
using System.Collections.Generic;
using System.Linq;
using Quest.API.ViewModels.Teams;
using Quest.API.ViewModels.Users;
using Quest.Domain.Models;

namespace Quest.API.ViewModels.Quests
{
    public class QuestWithTeamsAndAuthorVM : QuestVM
    {
        public QuestWithTeamsAndAuthorVM(QuestEntity row) : base(row)
        {
            Author = new UserVM(row.Author);
            Teams = row.Teams.Select(x => new TeamWithCaptainAndMembersVM(x)).ToList();
        }
        
        public UserVM Author { get; set; }
        public List<TeamWithCaptainAndMembersVM> Teams { get; set; }
    }
}
