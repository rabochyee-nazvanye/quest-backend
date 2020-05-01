using System.Linq;
using Quest.Domain.Interfaces;
using Quest.Domain.Models;

namespace Quest.Application.Services
{
    public class SoloPlayerConstructorArgs : IParticipantConstructorArgs
    {
        public string Name { get; set; }
        public int QuestId { get; set; }
        public string PrincipalUserId { get; set; }
        public BaseResponse<bool> IsValidFor(IQuest questEntity)
        {
            if (!(questEntity is ISoloQuest soloQuest))
            {
                return BaseResponse.Failure<bool>("Solo player can be registered only in solo quests.");
            }
            
            if (soloQuest.GetPlayers().Any(x => x.PrincipalUserId == PrincipalUserId))
                return BaseResponse.Failure<bool>("User already have associated participant in this quest.");
            
            if (soloQuest.GetPlayers().Any(x => x.Name == Name))
                return BaseResponse.Failure<bool>("The player with that name already exists");
            
            return BaseResponse.Success(true,"test");
        }
    }
}