using System.Collections.Generic;
using System.Linq;
using Quest.Domain.Interfaces;

namespace Quest.Domain.Models
{
    public class SoloInfiniteQuest : QuestEntity, IInfiniteQuest, ISoloQuest
    {
        public List<SoloPlayer> GetPlayers() => Participants.Select(x => x as SoloPlayer).ToList();
        public override bool IsReadyToShowResults() => true;

        public override bool IsReadyToReceiveTaskAttempts() => true;
        public override bool RegistrationIsAvailable() => true;

        public override Participant FindParticipant(string userId) => 
            GetPlayers().FirstOrDefault(x => x.PrincipalUserId == userId);
    }
}