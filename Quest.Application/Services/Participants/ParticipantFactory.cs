using System;
using System.Collections.Generic;
using Quest.DAL.Data;
using Quest.Domain.Models;
using Quest.Domain.Services;

namespace Quest.Application.Services
{
    public class ParticipantFactory
    {
        private readonly ITeamService _teamService;
        private readonly Db _context;
        public ParticipantFactory(ITeamService teamService, Db context)
        {
            _teamService = teamService;
            _context = context;
        }
        
        public Participant Create(IParticipantConstructorArgs args)
        {
            switch (args)
            {
                case SoloPlayerConstructorArgs playerConstructorArgs:
                    return new SoloPlayer
                    {
                        Name = playerConstructorArgs.Name,
                        PrincipalUserId = playerConstructorArgs.PrincipalUserId,
                        QuestId = playerConstructorArgs.QuestId
                    };
                case TeamConstructorArgs teamConstructorArgs:
                    var team = new Team
                    {
                        Name = teamConstructorArgs.Name,
                        PrincipalUserId = teamConstructorArgs.PrincipalUserId,
                        QuestId = teamConstructorArgs.QuestId,
                        InviteTokenSecret = _teamService.GenerateTeamToken(6)
                    };
                    team.Members = new List<TeamUser>{new TeamUser
                    {
                        Team = team,
                        UserId = teamConstructorArgs.PrincipalUserId
                    }};
                    return team;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(args));
            }
        }
    }
}