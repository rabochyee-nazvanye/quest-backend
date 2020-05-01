using System.Collections.Generic;
using System.Linq;
using Quest.API.ViewModels.Teams;
using Quest.Application.DTOs;

namespace Quest.API.ViewModels.Quests
{
    public class QuestScoreboardInvertedScoreDTO
    {
        public QuestScoreboardInvertedScoreDTO(QuestScoreboardDTO dto)
        {
            var winnerScore = dto.ParticipantResults.FirstOrDefault(x => x.Place == 0)?.Score ?? 0;
            TeamResults = dto.ParticipantResults
                .Select(x => new ParticipantResultInvertedScoreDTO(x, winnerScore))
                .ToList();
        }
        
        public List<ParticipantResultInvertedScoreDTO> TeamResults { get; set; } 
    }
}