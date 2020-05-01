using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quest.API.ViewModels.Quests;
using Quest.API.ViewModels.Users;
using Quest.Domain.Models;

namespace Quest.API.ViewModels.Teams
{
    public class TeamWithQuestAndModeratorVM : TeamVM
    {
        public TeamWithQuestAndModeratorVM(Team row) : base(row)
        {
            Quest = new QuestDTO(row.Quest);
            if (row.Moderator != null)
                Moderator = new ModeratorDTO(row.Moderator);
        }
        
        public QuestDTO Quest { get; set; }
        public ModeratorDTO Moderator { get; set; }
    }
}
