using System.Collections.Generic;
using Quest.API.ViewModels.Teams;
using Quest.API.ViewModels.Users;
using Quest.Domain.Interfaces;

namespace Quest.API.ViewModels.Quests
{
    public class SoloInfiniteQuestDetailedDTO : SoloInfiniteQuestDTO
    {
        public SoloInfiniteQuestDetailedDTO(IInfiniteQuest row) : base(row)
        {
        }
        public UserDTO Author { get; set; }
        public List<ParticipantDTO> Participants { get; set; }
    }
}