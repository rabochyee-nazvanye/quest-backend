using Quest.Application.DTOs;

namespace Quest.API.ViewModels.Teams
{
    public class TeamResultVM
    {
        public TeamResultVM(TeamResultDTO dto, int winnerScore)
        {
            Name = dto.Name;
            Score = dto.Place == 0 ? 0 : winnerScore - dto.Score;
            Place = dto.Place;
        }
        
        public string Name { get; set; }
        public int Score { get; set; }
        public int Place { get; set; } 
    }
}