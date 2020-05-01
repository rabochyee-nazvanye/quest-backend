using System.Collections.Generic;
using System.Linq;
using Quest.API.ResourceModels.Participants;
using Quest.Application.DTOs;

namespace Quest.API.ResourceModels.Quests.Results
{
    public class QuestScoreboardInvertedScoreRM
    {
        public QuestScoreboardInvertedScoreRM(QuestScoreboardDTO dto)
        {
            var winnerScore = dto.ParticipantResults.FirstOrDefault(x => x.Place == 0)?.Score ?? 0;
            TeamResults = dto.ParticipantResults
                .Select(x => new ParticipantResultRM(x, winnerScore))
                .ToList();
        }
        
        public List<ParticipantResultRM> TeamResults { get; set; } 
    }
}