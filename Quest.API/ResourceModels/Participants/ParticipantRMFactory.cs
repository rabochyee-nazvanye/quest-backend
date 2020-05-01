using System;
using Quest.API.ResourceModels.Teams;
using Quest.Domain.Models;

namespace Quest.API.ResourceModels.Participants
{
    public static class ParticipantRMFactory
    {
        public static ParticipantRM Create(Participant participant)
        {
            switch (participant)
            {
                case SoloPlayer soloPlayer:
                    return new ParticipantRM(soloPlayer);
                case Team team:
                    return new TeamWithCaptainAndMembersRM(team);
                default:
                    throw new ArgumentOutOfRangeException(nameof(participant));
            }
        }
    }
}