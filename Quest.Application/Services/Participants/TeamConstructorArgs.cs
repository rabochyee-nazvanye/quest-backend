using System.Linq;
using Quest.Domain.Interfaces;
using Quest.Domain.Models;

namespace Quest.Application.Services
{
    public class TeamConstructorArgs : IParticipantConstructorArgs
    {
        public string Name { get; set; }
        public string PrincipalUserId { get; set; }
        public BaseResponse<bool> IsValidFor(IQuest quest)
        {
            if (!(quest is ITeamQuest teamQuest))
            {
                return BaseResponse.Failure<bool>("Team can be registered only for team quests");
            }
            
            var captainTeams = teamQuest.GetTeams()
                .Where(x => x.Members.Any(y => y.UserId == PrincipalUserId) 
                            || x.PrincipalUserId == PrincipalUserId);
            if (captainTeams.Any())
                return BaseResponse.Failure<bool>("Team principal is currently member of another team");

            if (teamQuest.GetTeams().Any(x => x.Name == Name))
                return BaseResponse.Failure<bool>("The team with that name already exists");

            return BaseResponse.Success(true,"Arguments are valid");
        }

        public int QuestId { get; set; }
    }
}