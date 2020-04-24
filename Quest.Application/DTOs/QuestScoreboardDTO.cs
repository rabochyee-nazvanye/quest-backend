using System.Collections.Generic;
using System.Linq;

namespace Quest.Application.DTOs
{
    public class QuestScoreboardDTO
    {
        public QuestScoreboardDTO(IEnumerable<TeamResultDTO> teamResults)
        {
            TeamResults = teamResults.OrderBy(x => x.Place).ToList();
        }
        public List<TeamResultDTO> TeamResults { get; set; }
    }
}