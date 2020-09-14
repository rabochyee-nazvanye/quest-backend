using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quest.Application.Services;
using Quest.Application.Teams.Commands;
using Quest.DAL.Data;
using Quest.Domain.Interfaces;
using Quest.Domain.Models;
using Quest.Domain.Services;

namespace Quest.Application.Participants.Commands
{
    public class CreateParticipantCommandHandler : IRequestHandler<CreateParticipantCommand, BaseResponse<Participant>>
    {
        private readonly Db _context;
        private readonly ParticipantFactory _participantFactory;

        public CreateParticipantCommandHandler(Db context, ITeamService teamService)
        {
            _context = context;
            _participantFactory = new ParticipantFactory(teamService, _context);
        }

        public async Task<BaseResponse<Participant>> Handle(CreateParticipantCommand request, CancellationToken cancellationToken)
        {
            var captainUser = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.ParticipantConstructorArgs.PrincipalUserId,
                    cancellationToken: cancellationToken);
            if (captainUser == null)
                return BaseResponse.Failure<Participant>("Could not find principal user for participant");

            var quest = await _context.Quests.Where(x => x.Id == request.ParticipantConstructorArgs.QuestId)
                .Include(x => x.Participants)
                    .ThenInclude(x => (x as Team).Members)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (quest == null)
                return BaseResponse.Failure<Participant>("Couldn't find quest with that ID");

            var validationResult = request.ParticipantConstructorArgs.IsValidFor(quest);

            if (!validationResult.Result)
                return BaseResponse.Failure<Participant>(validationResult.Message);
            
            if (!quest.RegistrationIsAvailable())
                return BaseResponse.Failure<Participant>("You can't register to that quest no more");
            
            var participant = _participantFactory.Create(request.ParticipantConstructorArgs);
            
            await _context.Participants.AddAsync(participant, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return BaseResponse.Success(participant, "Successfully created a new participant");
        }
    }
}
