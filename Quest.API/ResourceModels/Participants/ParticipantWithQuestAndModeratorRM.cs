using Quest.API.ResourceModels.Quests;
using Quest.API.ResourceModels.Users;
using Quest.Domain.Models;

namespace Quest.API.ResourceModels.Participants
{
    public class ParticipantWithQuestAndModeratorRM : ParticipantRM
    {
        public ParticipantWithQuestAndModeratorRM(Participant row) : base(row)
        {
            Quest = QuestRMFactory.CreateBasic(row.Quest);
            if (row.Moderator != null)
                Moderator = new ModeratorRm(row.Moderator);
        }
        
        public QuestRM Quest { get; set; }
        public ModeratorRm Moderator { get; set; }
    }
}
