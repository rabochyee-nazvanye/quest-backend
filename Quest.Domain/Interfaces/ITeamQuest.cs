using System.Collections.Generic;
using Quest.Domain.Models;

namespace Quest.Domain.Interfaces
{
    public interface ITeamQuest : IQuest
    {
        public int MaxTeamSize { get; set; }
        public List<Team> GetTeams();
    }
}