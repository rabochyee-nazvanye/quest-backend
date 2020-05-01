using System.Collections.Generic;
using System.Linq;

namespace Quest.Application.DTOs
{
    public class QuestScoreboardDTO
    {
        public QuestScoreboardDTO(IEnumerable<ParticipantResultDTO> teamResults)
        {
            ParticipantResults = teamResults.OrderBy(x => x.Place).ToList();
        }
        public List<ParticipantResultDTO> ParticipantResults { get; set; }
    }
}