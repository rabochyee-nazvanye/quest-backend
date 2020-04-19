using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quest.API.ViewModels.Quests;
using Quest.Domain.Models;

namespace Quest.API.ViewModels.Teams
{
    public class TeamWithQuestVM : TeamWithCaptainAndMembersVM
    {
        public TeamWithQuestVM(Team row) : base(row)
        {
            Quest = new QuestVM(row.Quest);
        }

        public QuestVM Quest { get; set; }
    }
}
