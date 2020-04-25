using System.Collections.Generic;
using Quest.Domain.Models;

namespace Quest.Application.DTOs
{
    public class TeamProgressDTO
    {
        public TeamProgressDTO(Team team, Dictionary<TaskEntity, int> scores)
        {
            Team = team;
            Scores = scores;
        }
        public Team Team { get; set; }
        public Dictionary<TaskEntity, int> Scores { get; set; }
    }
}