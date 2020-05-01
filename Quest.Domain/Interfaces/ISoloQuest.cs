using System.Collections.Generic;
using Quest.Domain.Models;

namespace Quest.Domain.Interfaces
{
    public interface ISoloQuest : IQuest
    {
        public List<SoloPlayer> GetPlayers();
    }
}