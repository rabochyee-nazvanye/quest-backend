using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quest.API.ViewModels.Quests;
using Quest.API.ViewModels.Users;
using Quest.Domain.Models;

namespace Quest.API.ViewModels.Teams
{
    public class ParticipantWithQuestAndModeratorDTO : ParticipantDTO
    {
        public ParticipantWithQuestAndModeratorDTO(Participant row) : base(row)
        {
            Quest = QuestDTOFactory.CreateBasic(row.Quest);
            if (row.Moderator != null)
                Moderator = new ModeratorDTO(row.Moderator);
        }
        
        public QuestDTO Quest { get; set; }
        public ModeratorDTO Moderator { get; set; }
    }
}
