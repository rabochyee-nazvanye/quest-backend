using Quest.Domain.Interfaces;
using Quest.Domain.Models;

namespace Quest.Application.Services
{
    public interface IParticipantConstructorArgs
    {
        public string Name { get; set; }
        public int QuestId { get; set; }
        public string PrincipalUserId { get; set; }

        public BaseResponse<bool> IsValidFor(IQuest questEntity);
    }
}