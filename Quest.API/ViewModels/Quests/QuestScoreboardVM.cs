using System.Collections.Generic;
using System.Linq;
using Quest.API.ViewModels.Teams;
using Quest.Application.DTOs;

namespace Quest.API.ViewModels.Quests
{
    public class QuestScoreboardVM
    {
        public QuestScoreboardVM(QuestScoreboardDTO dto)
        {
            var winnerScore = dto.TeamResults.FirstOrDefault(x => x.Place == 0)?.Score ?? 0;
            TeamResults = dto.TeamResults
                .Select(x => new TeamResultVM(x, winnerScore))
                .ToList();
        }
        
        public List<TeamResultVM> TeamResults { get; set; } 
    }
}