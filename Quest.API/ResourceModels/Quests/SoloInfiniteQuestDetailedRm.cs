using System.Collections.Generic;
using Quest.API.ResourceModels.Participants;
using Quest.API.ResourceModels.Users;
using Quest.Domain.Interfaces;

namespace Quest.API.ResourceModels.Quests
{
    public class SoloInfiniteQuestDetailedRm : SoloInfiniteQuestRM
    {
        public SoloInfiniteQuestDetailedRm(IInfiniteQuest row) : base(row)
        {
        }
        public UserRM Author { get; set; }
        public List<ParticipantRM> Participants { get; set; }
    }
}