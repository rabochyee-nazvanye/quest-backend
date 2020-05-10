using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quest.Application.Teams.Commands;
using Quest.DAL.Data;
using Quest.Domain.Models;

namespace Quest.Application.Participants.Commands
{
    public class FinishQuestCommandHandler : IRequestHandler<RemoveParticipantCommand, BaseResponse<bool>>
    {
        private readonly Db _context;

        public FinishQuestCommandHandler(Db context)
        {
            _context = context;
        }

        public async Task<BaseResponse<bool>> Handle(RemoveParticipantCommand request, CancellationToken cancellationToken)
        {
            var participant = await _context.Participants
                .FirstOrDefaultAsync(x => x.Id == request.ParticipantId,
                    cancellationToken: cancellationToken);
            if (participant == null)
                return BaseResponse.Failure<bool>("Participant not found.");

            var user = await _context.Users.FindAsync(request.UserId);

            if (participant.PrincipalUserId != request.UserId)
                return BaseResponse.Failure<bool>("Only participant principal can finish the quest.");

            if (user == null)
                return BaseResponse.Failure<bool>("User not found.");
            
            participant.isFinished = true;
            await _context.SaveChangesAsync(cancellationToken);

            return BaseResponse.Success(true, "Successfully finished the quest");
        }
    }
}